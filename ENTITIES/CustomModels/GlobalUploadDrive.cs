using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using static Google.Apis.Drive.v3.FilesResource;

namespace ENTITIES.CustomModels
{
    public static class GlobalUploadDrive
    {
        public static UserCredential credential;
        public static string ParentDrive;
        public static DriveService driveService;
        public static void InIt()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            using (var stream =
                new FileStream(filePath + "/credentials.json", FileMode.Open, FileAccess.Read))
            {
                string[] Scopes = { DriveService.Scope.Drive };
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "application",
                    CancellationToken.None).Result;
                driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                });
            }
            using (StreamReader r = new StreamReader(filePath + "/DriveConfig.json"))
            {
                string json = r.ReadToEnd();
                ParentDrive = JObject.Parse(json).Value<string>("ParentDriveID");
            }
        }

        //Thư mục gốc
        //|_____sonnt5
        //|  |_____Khen thưởng bài báo
        //|  |  |_____Bài báo A(file đính kèm 1, file đính kèm 2)
        //|  |  |_____Bài báo B
        //|  |
        //|  |_____Tham dự hội nghị
        //|  |  |_____Hội nghị C(file đính kèm 1, file đính kèm 2)
        //|  |  |_____Hội nghị D
        //|  |
        //|  |_____Học bổng, ...vv
        //|
        //|_____anhnb
        public static string UploadFile(HttpPostedFileBase InputFile, string FolderName, int TypeFolder, string ShareWithEmail = null)
        {
            return UploadFile(InputFile.FileName, FolderName, InputFile.InputStream, InputFile.ContentType, TypeFolder, ShareWithEmail);
        }
        public static string UploadFile(string FileName, string FolderName, Stream InputStream, string ContentType, int TypeFolder, string ShareWithEmail = null)
        {
            string SubFolderName;
            switch (TypeFolder)
            {
                case 1:
                    SubFolderName = "Hội nghị";
                    break;
                case 2:
                    SubFolderName = "Nghiên cứu khoa học";
                    break;
                case 3:
                    SubFolderName = "Bằng sáng chế";
                    break;
                default:
                    throw new ArgumentException("Loại folder không tồn tại");
            }

            string ResearcherFolderName = ShareWithEmail.Split('@')[0];

            var ResearcherFolder = FindFirstFolder(ResearcherFolderName, ParentDrive) ?? CreateFolder(ResearcherFolderName, ParentDrive);

            var SubFolder = FindFirstFolder(SubFolderName, ResearcherFolder.Id) ?? CreateFolder(SubFolderName, ResearcherFolder.Id);

            var folder = FindFirstFolder(FolderName, SubFolder.Id) ?? CreateFolder(FolderName, SubFolder.Id);

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = FileName,
                Parents = new List<string>
                {
                    folder.Id
                }
            };

            FilesResource.CreateMediaUpload request = driveService.Files.Create(fileMetadata, InputStream, ContentType);
            // Cấu hình thông tin lấy về là ID
            request.Fields = "id";
            request.SupportsAllDrives = true;
            request.Upload();

            // Trả về thông tin file đã được upload lên Google Drive
            var file = request.ResponseBody;

            if (ShareWithEmail != null)
            {
                Permission userPermission = new Permission
                {
                    Type = "user",
                    Role = "reader",
                    EmailAddress = ShareWithEmail
                };

                PermissionsResource.CreateRequest createRequest = driveService.Permissions.Create(userPermission, file.Id);
                createRequest.SupportsAllDrives = true;
                createRequest.Execute();
            }

            return file.Id;
        }
        public static Google.Apis.Drive.v3.Data.File CreateFolder(string FolderName, string ParentID = null)
        {
            ParentID = ParentID ?? ParentDrive;
            var folderMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = FolderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string>
                {
                    ParentID,
                }
            };

            CreateRequest CreateFolderRequest = driveService.Files.Create(folderMetadata);
            CreateFolderRequest.Fields = "id";
            CreateFolderRequest.SupportsAllDrives = true;

            var folder = CreateFolderRequest.Execute();
            return folder;
        }
        public static Google.Apis.Drive.v3.Data.File FindFirstFolder(string FolderName, string ParentID = null)
        {
            //ParentID = ParentID ?? ParentDrive;
            //ListRequest listRequest = new ListRequest(driveService)
            //{
            //    Q = "name = '" + FolderName + "' and mimeType = 'application/vnd.google-apps.folder' and '" + ParentID + "' in parents and trashed = false",
            //    Spaces = "drive",
            //    Fields = "files(id)",
            //    SupportsAllDrives = true,
            //    IncludeItemsFromAllDrives = true,
            //};
            //FileList result = listRequest.Execute();
            //if (result.Files.Count == 0)
            //    return null;
            //else
            //    return result.Files[0];
            return null;
        }
        public static bool ChangeParentDrive(string ParentID)
        {
            ParentDrive = ParentID;
            return true;
        }
    }
}
