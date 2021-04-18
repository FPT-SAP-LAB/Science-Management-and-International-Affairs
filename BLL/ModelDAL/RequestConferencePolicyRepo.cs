using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;

namespace BLL.ModelDAL
{
    public class RequestConferencePolicyRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public string GetCurrentLink()
        {
            db = new ScienceAndInternationalAffairsEntities();
            Policy policy = GetCurrentPolicy(db);
            if (policy == null)
                return null;
            else
                return policy.File.link;
        }
        public Policy GetCurrentPolicy(ScienceAndInternationalAffairsEntities db = null)
        {
            if (db == null)
                this.db = new ScienceAndInternationalAffairsEntities();
            else
                this.db = db;
            return this.db.Policies.Where(x => x.expired_date == null && x.policy_type_id == 1).FirstOrDefault();
        }
    }
}
