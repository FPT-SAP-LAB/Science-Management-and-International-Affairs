using ENTITIES;
using System.Linq;

namespace BLL.ModelDAL
{
    public class PolicyRepo
    {
        public string GetCurrentLink(int policy_type_id, ScienceAndInternationalAffairsEntities db = null)
        {
            db = db ?? new ScienceAndInternationalAffairsEntities();
            Policy policy = GetCurrentPolicy(policy_type_id, db);
            if (policy == null)
                return null;
            else
                return policy.File.link;
        }

        public Policy GetCurrentPolicy(int policy_type_id, ScienceAndInternationalAffairsEntities db = null)
        {
            db = db ?? new ScienceAndInternationalAffairsEntities();
            return db.Policies.Where(x => x.expired_date == null && x.policy_type_id == policy_type_id).FirstOrDefault();
        }
    }
}
