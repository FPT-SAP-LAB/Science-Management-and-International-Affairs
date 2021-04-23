using ENTITIES;
using System.Linq;

namespace BLL.ModelDAL
{
    public class PositionRepo
    {
        public static int? GetPositionIdByAccountId(int account_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            return GetPositionIdByAccountId(db, account_id);
        }
        public static int? GetPositionIdByAccountId(ScienceAndInternationalAffairsEntities db, int account_id)
        {
            int? position_id = db.Accounts.Find(account_id).position_id;
            if (position_id == null)
            {
                Profile profile = db.Profiles.Where(x => x.account_id == account_id).FirstOrDefault();
                if (profile == null) return null;

                PeoplePosition peoplePosition = profile.PeoplePositions.FirstOrDefault();
                position_id = peoplePosition?.position_id;
            }
            return position_id;
        }
        public static string GetPositionNameByProfileCode(string code, int language_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string PositionName = (from a in db.Profiles
                                   join b in db.PeoplePositions on a.people_id equals b.people_id
                                   join c in db.PositionLanguages on b.position_id equals c.position_id
                                   where a.mssv_msnv == code && c.language_id == language_id
                                   select c.name).FirstOrDefault();
            return PositionName;
        }
    }
}
