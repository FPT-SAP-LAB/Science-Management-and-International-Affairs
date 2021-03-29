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
            RequestConferencePolicy policy = db.RequestConferencePolicies.Where(x => x.expired_date == null).FirstOrDefault();
            if (policy == null)
                return null;
            else
                return policy.File.link;
        }
    }
}
