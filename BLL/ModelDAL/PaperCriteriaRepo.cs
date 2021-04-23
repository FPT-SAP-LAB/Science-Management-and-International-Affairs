using ENTITIES;
using ENTITIES.CustomModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BLL.ModelDAL
{
    public class PaperCriteriaRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<PaperCriteria> GetCurrentPaperCriterias()
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            var paperCriterias = (from a in db.Policies
                                  join b in db.PaperCriterias on a.policy_id equals b.policy_id
                                  where a.expired_date == null && a.policy_type_id == 2
                                  select b).ToList();
            return paperCriterias;
        }

        public AlertModal<string> Edit(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new AlertModal<string>(false, "Không được bỏ trống");
            db = new ScienceAndInternationalAffairsEntities();
            PaperCriteria paperCriteria = db.PaperCriterias.Find(id);
            if (paperCriteria == null)
            {
                return new AlertModal<string>(false, "Không tìm thấy");
            }
            else
            {
                paperCriteria.name = name.Trim();
                db.SaveChanges();
                return new AlertModal<string>(name.Trim(), true);
            }
        }

        public AlertModal<string> Add(HttpPostedFileBase file, string stringCriterias, int account_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                string file_drive_id = null;
                try
                {
                    List<PaperCriteria> paperCriterias = JsonConvert.DeserializeObject<List<PaperCriteria>>(stringCriterias);
                    if (paperCriterias == null || paperCriterias.Count == 0 || paperCriterias.All(x => string.IsNullOrWhiteSpace(x.name)))
                        return new AlertModal<string>(false, "Không được bỏ trống tiêu chí");

                    paperCriterias.ForEach(x => x.name = x.name.Trim());

                    PolicyRepo policyRepo = new PolicyRepo();

                    Policy oldPolicy = policyRepo.GetCurrentPolicy(2, db);
                    if (oldPolicy != null)
                        oldPolicy.expired_date = DateTime.Now;

                    Google.Apis.Drive.v3.Data.File fileUploaded = GoogleDriveService.UploadPolicyFile(file);
                    file_drive_id = fileUploaded.Id;

                    Policy newPolicy = new Policy
                    {
                        File = new File
                        {
                            file_drive_id = file_drive_id,
                            link = fileUploaded.WebViewLink,
                            name = file.FileName
                        },
                        valid_date = DateTime.Now,
                        expired_date = null,
                        account_id = account_id,
                        policy_type_id = 2,
                        PaperCriterias = paperCriterias
                    };
                    db.Policies.Add(newPolicy);

                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                    if (file_drive_id != null)
                        GoogleDriveService.DeleteFile(file_drive_id);
                }
            }
            return new AlertModal<string>(false);
        }
    }
}
