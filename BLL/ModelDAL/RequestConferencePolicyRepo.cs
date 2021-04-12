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
            RequestConferencePolicy policy = GetCurrentPolicy(db);
            if (policy == null)
                return null;
            else
                return policy.File.link;
        }
        public RequestConferencePolicy GetCurrentPolicy(ScienceAndInternationalAffairsEntities db = null)
        {
            if (db == null)
                this.db = new ScienceAndInternationalAffairsEntities();
            else
                this.db = db;
            return this.db.RequestConferencePolicies.Where(x => x.expired_date == null).FirstOrDefault();
        }
    }
}
