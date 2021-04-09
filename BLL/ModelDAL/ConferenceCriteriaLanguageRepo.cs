using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;
using ENTITIES.CustomModels;

namespace BLL.ModelDAL
{
    public class ConferenceCriteriaLanguageRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<ConferenceCriteriaLanguage> GetCurrentList(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            var ConferenceCriteriaLanguages = (from a in db.RequestConferencePolicies
                                               join b in db.Criteria on a.policy_id equals b.policy_id
                                               join c in db.ConferenceCriteriaLanguages on b.criteria_id equals c.criteria_id
                                               where a.expired_date == null && c.language_id == language_id
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
    }
}
