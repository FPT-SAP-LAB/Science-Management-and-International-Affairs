using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;

namespace BLL.ModelDAL
{
    public class ConferenceCriteriaLanguageRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<string> GetCurrentList(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            var ConferenceCriteriaLanguages = (from a in db.RequestConferencePolicies
                                               join b in db.Criteria on a.policy_id equals b.policy_id
                                               join c in db.ConferenceCriteriaLanguages on b.criteria_id equals c.criteria_id
                                               where a.expired_date == null && c.language_id == language_id
                                               select c.name).ToList();
            return ConferenceCriteriaLanguages;
        }
    }
}
