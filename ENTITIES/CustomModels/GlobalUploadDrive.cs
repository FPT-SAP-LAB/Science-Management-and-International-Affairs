using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Drive.v3.FilesResource;

namespace ENTITIES.CustomModels
{
    public static class GlobalUploadDrive
    {
        public static UserCredential credential;
        public static string ParentDrive;
        public static string UploadFile(string FileName, Stream InputStream, string ContentType, string SharedTo)
        {
            var driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });

            Google.Apis.Drive.v3.Data.File folderMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = "SonNT69",
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string>
                {
                    ParentDrive,
                }
            };

            CreateRequest CreateFolderRequest = driveService.Files.Create(folderMetadata);
            CreateFolderRequest.Fields = "id";
            CreateFolderRequest.SupportsAllDrives = true;

            Google.Apis.Drive.v3.Data.File folder = CreateFolderRequest.Execute();

            // ID thư mục file, các bạn thay bằng ID của các bạn khi chạy
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                // Tên file sẽ lưu trên Google Drive
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
            Google.Apis.Drive.v3.Data.File file = request.ResponseBody;

            if (SharedTo != null)
            {
                Permission userPermission = new Permission
                {
                    Type = "user",
                    Role = "reader",
                    EmailAddress = SharedTo
                };

                PermissionsResource.CreateRequest createRequest = driveService.Permissions.Create(userPermission, file.Id);
                createRequest.SupportsAllDrives = true;
                createRequest.Execute();
            }

            return file.Id;
        }
        public static bool ChangeParentDrive(string ParentID)
        {
            ParentDrive = ParentID;
            return true;
        }
    }
}
