using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.ModelDAL
{
    public class PolicyTypeLanguageRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<PolicyTypeLanguage> PolicyTypeLanguages(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.PolicyTypeLanguages.Where(x => x.language_id == language_id).ToList();
        }
        public AlertModal<string> Edit(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new AlertModal<string>(false, "Không được bỏ trống");
            db = new ScienceAndInternationalAffairsEntities();
            PolicyTypeLanguage typeLanguage = db.PolicyTypeLanguages.Find(id);
            if (typeLanguage == null)
            {
                return new AlertModal<string>(false, "Không tìm thấy");
            }
            else
            {
                typeLanguage.policy_type_name = name.Trim();
                db.SaveChanges();
                return new AlertModal<string>(name.Trim(), true);
            }
        }
        public AlertModal<string> Add(List<PolicyTypeLanguage> types, int account_id)
        {
            if (types == null || types.Count == 0)
                return new AlertModal<string>(false, "Dữ liệu ngôn ngữ chưa đầy đủ");
            types.ForEach(x => x.policy_type_name = x.policy_type_name.Trim());
            if (types.Any(x => string.IsNullOrWhiteSpace(x.policy_type_name)))
                return new AlertModal<string>(false, "Dữ liệu ngôn ngữ chưa đầy đủ");
            db = new ScienceAndInternationalAffairsEntities();
            if (types.Count != db.Languages.Count())
                return new AlertModal<string>(false, "Dữ liệu ngôn ngữ chưa đầy đủ");
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.PolicyTypes.Add(new PolicyType
                    {
                        account_id = account_id,
                        PolicyTypeLanguages = types
                    });
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
