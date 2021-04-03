﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ENTITIES;
using ENTITIES.CustomModels;
using Newtonsoft.Json;

namespace BLL.ScienceManagement.Researcher
{
    public class AdditionalProfile
    {
        ScienceAndInternationalAffairsEntities db;
        public AlertModal<string> Add(HttpPostedFileBase identification, string PersonString, string ProfileString, string username, int account_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    Account account = db.Accounts.Find(account_id);
                    Person person = JsonConvert.DeserializeObject<Person>(PersonString);
                    Profile profile = JsonConvert.DeserializeObject<Profile>(ProfileString);

                    if (account.Profiles.Count > 0)
                        return new AlertModal<string>(false, "Thông tin cá nhân đã được khởi tạo");
                    if (username.Trim() == "")
                        return new AlertModal<string>(false, "Trường tên tài khoản là bắt buộc");
                    if (person.name.Trim() == "")
                        return new AlertModal<string>(false, "Trường họ và tên là bắt buộc");
                    if (profile.mssv_msnv.Trim() == "")
                        return new AlertModal<string>(false, "Trường mã số là bắt buộc");

                    account.full_name = username.Trim();

                    person.email = account.email;
                    db.People.Add(person);
                    db.SaveChanges();

                    profile.account_id = account_id;
                    profile.people_id = person.people_id;
                    db.Profiles.Add(profile);
                    db.SaveChanges();

                    Google.Apis.Drive.v3.Data.File IdentificationFile = GoogleDriveService.UploadProfileMedia(identification, account.email);
                    File file = new File
                    {
                        file_drive_id = IdentificationFile.Id,
                        name = identification.FileName,
                        link = IdentificationFile.WebViewLink
                    };
                    db.Files.Add(file);
                    db.SaveChanges();
                    profile.identification_file_id = file.file_id;

                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                }
            }
            return new AlertModal<string>(false);
        }
    }
}
