using ENTITIES;
using System.Linq;

namespace BLL.ModelDAL
{
    public class PolicyRepo
    {
        public string GetCurrentLink(int policy_type_id, ScienceAndInternationalAffairsEntities db = null)
        {
            db = db ?? new ScienceAndInternationalAffairsEntities();
            string link = (from a in db.Policies
                           join b in db.Files on a.file_id equals b.file_id
                           where a.expired_date == null && a.policy_type_id == policy_type_id
                           select b.link).FirstOrDefault();
            return link;
        }

        public Policy GetCurrentPolicy(int policy_type_id, ScienceAndInternationalAffairsEntities db = null)
        {
            db = db ?? new ScienceAndInternationalAffairsEntities();
            return db.Policies.Where(x => x.expired_date == null && x.policy_type_id == policy_type_id).FirstOrDefault();
        }
    }
}
