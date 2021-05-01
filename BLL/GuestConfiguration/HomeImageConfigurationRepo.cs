using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BLL.GuestConfiguration
{
    public class HomeImageConfigurationRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db;
        private int account_id;

        public HomeImageConfigurationRepo(ScienceAndInternationalAffairsEntities db)
        {
            this.db = db;
        }

        public HomeImageConfigurationRepo()
        {
            db = new ScienceAndInternationalAffairsEntities();
        }

        public AlertModal<string> Update(HttpPostedFileBase wallpaper, List<HttpPostedFileBase> banner, int account_id, List<int> keeps)
        {
            this.account_id = account_id;
            bool is_uploaded = false;

            string HomePageDrive = GoogleDriveService.HomePageDrive;
            List<ImageHomePage> activated = db.ImageHomePages.Where(x => x.is_active).ToList();
            if (wallpaper != null)
            {
                try
                {
                    Google.Apis.Drive.v3.Data.File uploadedWallpaper =
                        GoogleDriveService.UploadFile(wallpaper.FileName, wallpaper.InputStream, wallpaper.ContentType, HomePageDrive);
                    GoogleDriveService.ShareWithAnyone(uploadedWallpaper.Id);

                    bool uploaded = AddImageHomePage(uploadedWallpaper, true, wallpaper.FileName);
                    if (!uploaded)
                        GoogleDriveService.DeleteFile(uploadedWallpaper.Id);

                    is_uploaded = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            List<ImageHomePage> deactive = activated.Where(x => !keeps.Contains(x.file_id)).ToList();
            deactive.ForEach(x => x.is_active = false);

            foreach (var item in banner)
            {
                try
                {
                    Google.Apis.Drive.v3.Data.File uploadedBanner =
                        GoogleDriveService.UploadFile(item.FileName, item.InputStream, item.ContentType, HomePageDrive);
                    GoogleDriveService.ShareWithAnyone(uploadedBanner.Id);

                    bool uploaded = AddImageHomePage(uploadedBanner, false, item.FileName);
                    if (!uploaded)
                        GoogleDriveService.DeleteFile(uploadedBanner.Id);

                    is_uploaded = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            if (is_uploaded)
                return new AlertModal<string>(true);
            else
                return new AlertModal<string>(false);
        }

        private bool AddImageHomePage(Google.Apis.Drive.v3.Data.File uploadedBanner, bool is_wallpaper, string FileName)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (is_wallpaper)
                    {
                        var wallpaper = db.ImageHomePages.Where(x => x.is_wallpaper && x.is_active).FirstOrDefault();
                        if (wallpaper != null)
                            wallpaper.is_active = false;
                    }

                    //File file = new File
                    //{
                    //    file_drive_id = uploadedBanner.Id,
                    //    link = uploadedBanner.WebViewLink,
                    //    name = FileName
                    //};
                    //db.Files.Add(file);
                    //db.SaveChanges();

                    var newBanner = new ImageHomePage
                    {
                        File = new File
                        {
                            file_drive_id = uploadedBanner.Id,
                            link = uploadedBanner.WebViewLink,
                            name = FileName
                        },
                        //file_id = file.file_id,
                        account_id = account_id,
                        is_active = true,
                        is_wallpaper = is_wallpaper
                    };
                    db.ImageHomePages.Add(newBanner);
                    db.SaveChanges();
                    trans.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return false;
                }
            }
        }
    }
}
