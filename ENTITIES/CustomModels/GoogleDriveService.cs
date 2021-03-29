using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Requests;
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
    public static class GoogleDriveService
    {
        //Field: Dữ liệu trả về của loại request, Nếu là file nên để là id và webViewLink, xem thêm tại đây https://developers.google.com/drive/api/v3/reference/files
        //Q: query search, xem thêm tại đây https://developers.google.com/drive/api/v3/ref-search-terms
        //SupportsAllDrives: hỗ trợ thêm trên cả drive của người dùng, shared drive
        //IncludeItemsFromAllDrives: hỗ trợ tìm kiếm dữ liệu file/folder trên cả drive của người dùng, shared drive

        public static GoogleCredential credential;
        public static string SMDrive;
        public static string IADrive;
        public static DriveService driveService;
        public static void InIt()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath ?? "");
            using (var stream =
                new FileStream(filePath + "/credentials.json", FileMode.Open, FileAccess.Read))
            {
                string[] Scopes = { DriveService.Scope.Drive };
                credential = GoogleCredential.FromStream(stream)
                                     .CreateScoped(Scopes);
                driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                });
            }
            using (StreamReader r = new StreamReader(filePath + "/DriveConfig.json"))
            {
                string json = r.ReadToEnd();
                SMDrive = JObject.Parse(json).Value<string>("SMDriveID");
                IADrive = JObject.Parse(json).Value<string>("IADriveID");
            }
        }
        public static string CurrentAccount()
        {
            if (driveService == null)
                return null;
            AboutResource.GetRequest request = driveService.About.Get();
            request.Fields = "user";
            var About = request.Execute();
            return About.User.EmailAddress;
        }
        public static void Logout()
        {
            if (driveService == null)
                return;
            //_ = credential.RevokeTokenAsync(CancellationToken.None).Result;
            credential = null;
            driveService = null;
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
        public static Google.Apis.Drive.v3.Data.File UploadResearcherFile(HttpPostedFileBase InputFile, string FolderName, int TypeFolder, string ShareWithEmail)
        {
            return UploadResearcherFile(new List<HttpPostedFileBase> { InputFile }, FolderName, TypeFolder, ShareWithEmail)[0];
        }
        public static List<Google.Apis.Drive.v3.Data.File> UploadResearcherFile(List<HttpPostedFileBase> InputFiles, string FolderName, int TypeFolder, string ShareWithEmail)
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
                case 4:
                    SubFolderName = "Quyết định";
                    break;
                default:
                    throw new ArgumentException("Loại folder không tồn tại");
            }
            string ResearcherFolderName = "";
            if (ShareWithEmail == null) ResearcherFolderName = "Phòng KH";
            else ResearcherFolderName = ShareWithEmail.Split('@')[0];

            var ResearcherFolder = FindFirstFolder(ResearcherFolderName, SMDrive) ?? CreateFolder(ResearcherFolderName, SMDrive);

            var SubFolder = FindFirstFolder(SubFolderName, ResearcherFolder.Id) ?? CreateFolder(SubFolderName, ResearcherFolder.Id);

            var folder = FindFirstFolder(FolderName, SubFolder.Id) ?? CreateFolder(FolderName, SubFolder.Id);

            List<Google.Apis.Drive.v3.Data.File> UploadedFiles = new List<Google.Apis.Drive.v3.Data.File>();

            foreach (HttpPostedFileBase item in InputFiles)
            {
                var file = UploadFile(item.FileName, item.InputStream, item.ContentType, folder.Id);

                UploadedFiles.Add(file);

                if (ShareWithEmail != null) ShareFile(ShareWithEmail, file.Id);
            }

            return UploadedFiles;
        }
        public static Google.Apis.Drive.v3.Data.File CreateFolder(string FolderName, string ParentID)
        {
            var folderMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = FolderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string>
                {
                    ParentID
                }
            };

            CreateRequest CreateFolderRequest = driveService.Files.Create(folderMetadata);
            CreateFolderRequest.Fields = "id,webViewLink";
            CreateFolderRequest.SupportsAllDrives = true;

            var folder = CreateFolderRequest.Execute();
            return folder;
        }
        public static Google.Apis.Drive.v3.Data.File FindFirstFolder(string FolderName, string ParentID)
        {
            ListRequest listRequest = new ListRequest(driveService)
            {
                Q = "name = '" + FolderName + "' and mimeType = 'application/vnd.google-apps.folder' and '" + ParentID + "' in parents and trashed = false",
                Spaces = "drive",
                Fields = "files(id,webViewLink)",
                SupportsAllDrives = true,
                IncludeItemsFromAllDrives = true,
            };
            FileList result = listRequest.Execute();
            if (result.Files.Count == 0)
                return null;
            else
                return result.Files[0];
        }
        public static Google.Apis.Drive.v3.Data.File UploadFile(string FileName, Stream InputStream, string ContentType, string ParentID)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = FileName,
                Parents = new List<string>
                {
                    ParentID
                }
            };

            CreateMediaUpload request = driveService.Files.Create(fileMetadata, InputStream, ContentType);
            request.Fields = "id,webViewLink";
            request.SupportsAllDrives = true;
            request.Upload();

            return request.ResponseBody;
        }

        public static Google.Apis.Drive.v3.Data.File UpdateFile(string FileName, Stream InputStream, string ContentType, string FileID)
        {
            GetRequest RequestGet = driveService.Files.Get(FileID);
            RequestGet.Fields = "id,webViewLink";
            RequestGet.SupportsAllDrives = true;
            Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File();
            file.Name = FileName;

            UpdateMediaUpload RequestPut = driveService.Files.Update(file, FileID, InputStream, ContentType);
            RequestPut.Fields = "id,webViewLink";
            RequestPut.SupportsAllDrives = true;
            RequestPut.Upload();

            return RequestPut.ResponseBody;
        }

        public static void ShareFile(string Email, string FileID)
        {
            Permission userPermission = new Permission
            {
                Type = "user",
                Role = "reader",
                EmailAddress = Email
            };

            PermissionsResource.CreateRequest createRequest = driveService.Permissions.Create(userPermission, FileID);
            createRequest.SupportsAllDrives = true;
            createRequest.Execute();
        }
        public static AlertModal<string> DeleteFile(string FileID)
        {
            try
            {
                DeleteRequest request = new DeleteRequest(driveService, FileID)
                {
                    SupportsAllDrives = true
                };
                request.Execute();
                return new AlertModal<string>(true, "Xóa thành công");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("File not found"))
                    return new AlertModal<string>(false, "File không tồn tại");
                else
                    return new AlertModal<string>(false, "Có lỗi xảy ra");
            }
        }

        //Thư mục gốc
        //|_____Partner (sub folder name)
        //|  |_____Partner A (folder name)
        //|  |  |_____image 1
        //|  |  |_____image 2
        //|  |
        //|  |_____Partner B
        //|     |_____image 1
        //|     |_____image 2
        //|  
        //|_____MOU
        public static Google.Apis.Drive.v3.Data.File UploadIAFile(HttpPostedFileBase InputFile, string FolderName, int TypeFolder, bool isFolder)
        {
            return UploadIAFile(new List<HttpPostedFileBase> { InputFile }, FolderName, TypeFolder, isFolder)[0];
        }

        public static List<Google.Apis.Drive.v3.Data.File> UploadIAFile(List<HttpPostedFileBase> InputFiles, string FolderName, int TypeFolder, bool isFolder)
        {
            string SubFolderName;
            switch (TypeFolder)
            {
                case 1:
                    SubFolderName = "Partner";
                    break;
                case 2:
                    SubFolderName = "MOU";
                    break;
                case 3:
                    SubFolderName = "MOA";
                    break;
                case 4:
                    SubFolderName = "Academic Collaboration";
                    break;
                default:
                    throw new ArgumentException("Loại folder không tồn tại");
            }

            var IAFolder = FindFirstFolder(SubFolderName, IADrive) ?? CreateFolder(SubFolderName, IADrive);

            var folder = FindFirstFolder(FolderName, IAFolder.Id) ?? CreateFolder(FolderName, IAFolder.Id);

            List<Google.Apis.Drive.v3.Data.File> UploadedFiles = new List<Google.Apis.Drive.v3.Data.File>();

            foreach (HttpPostedFileBase item in InputFiles)
            {
                var file = UploadFile(item.FileName, item.InputStream, item.ContentType, folder.Id);

                UploadedFiles.Add(file);

                Permission userPermission = new Permission
                {
                    Type = "anyone",
                    Role = "reader"
                };

                PermissionsResource.CreateRequest createRequest = driveService.Permissions.Create(userPermission, file.Id);
                createRequest.SupportsAllDrives = true;
                createRequest.Execute();
            }

            if (isFolder)
            {
                return new List<Google.Apis.Drive.v3.Data.File>
                {
                    folder //return parent files
                };
            }
            else
            {
                return UploadedFiles;
            }
        }
    }
}
