using ENTITIES;
using ENTITIES.CustomModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BLL.ModelDAL
{
    public class ConferenceCriteriaLanguageRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<ConferenceCriteriaLanguage> GetCurrentList(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var temp = db.Criteria.Where(x => x.policy_id == 3).ToList();
            db.Configuration.LazyLoadingEnabled = false;
            var ConferenceCriteriaLanguages = (from a in db.RequestConferencePolicies
                                               join b in db.Criteria on a.policy_id equals b.policy_id
                                               join c in db.ConferenceCriteriaLanguages on b.criteria_id equals c.criteria_id
                                               where a.expired_date == null && c.language_id == language_id
                                               select c).ToList();
            return ConferenceCriteriaLanguages;
        }
        public List<ConferenceCriteriaLanguage> GetAll()
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            var ConferenceCriteriaLanguages = (from a in db.RequestConferencePolicies
                                               join b in db.Criteria on a.policy_id equals b.policy_id
                                               join c in db.ConferenceCriteriaLanguages on b.criteria_id equals c.criteria_id
                                               where a.expired_date == null
                                               select c).ToList();
            return ConferenceCriteriaLanguages;
        }
        public AlertModal<string> Edit(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new AlertModal<string>(false, "Không được bỏ trống");
            db = new ScienceAndInternationalAffairsEntities();
            ConferenceCriteriaLanguage criteriaLanguage = db.ConferenceCriteriaLanguages.Find(id);
            if (criteriaLanguage == null)
            {
                return new AlertModal<string>(false, "Không tìm thấy");
            }
            else
            {
                criteriaLanguage.name = name.Trim();
                db.SaveChanges();
                return new AlertModal<string>(name.Trim(), true);
            }
        }
        public AlertModal<string> Add(HttpPostedFileBase file, string policies)
        {
            string content = "";
            db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                string file_drive_id = null;
                try
                {
                    RequestConferencePolicyRepo policyRepo = new RequestConferencePolicyRepo();

                    RequestConferencePolicy oldPolicy = policyRepo.GetCurrentPolicy(db);
                    oldPolicy.expired_date = DateTime.Now;

                    Google.Apis.Drive.v3.Data.File fileUploaded = GoogleDriveService.UploadPolicyFile(file);
                    file_drive_id = fileUploaded.Id;

                    RequestConferencePolicy newPolicy = new RequestConferencePolicy
                    {
                        File = new File
                        {
                            file_drive_id = file_drive_id,
                            link = fileUploaded.WebViewLink,
                            name = file.FileName
                        },
                        valid_date = DateTime.Now,
                        expired_date = null
                    };
                    db.RequestConferencePolicies.Add(newPolicy);
                    db.SaveChanges();

                    List<Language> languages = LanguageRepo.GetLanguages(db);
                    List<int> keys = languages.Select(x => x.language_id).ToList();

                    JArray _Array = JArray.Parse(policies);

                    bool Added = false;
                    foreach (JObject item in _Array)
                    {
                        Criterion criterion = new Criterion
                        {
                            policy_id = newPolicy.policy_id
                        };

                        foreach (int key in keys)
                        {
                            var name = item[key.ToString()];
                            if (name == null)
                            {
                                content = "Chưa điền đầy đủ ngôn ngữ";
                                throw new Exception();
                            }
                            criterion.ConferenceCriteriaLanguages.Add(new ConferenceCriteriaLanguage
                            {
                                language_id = key,
                                name = name.ToString().Trim()
                            });
                            Added = true;
                        }
                        db.Criteria.Add(criterion);
                    }
                    if (!Added)
                    {
                        content = "Không có điều kiện nào hợp lệ";
                        throw new Exception();
                    }
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
            content = content == "" ? "Có lỗi xảy ra" : content;
            return new AlertModal<string>(false, content);
        }
    }
}
