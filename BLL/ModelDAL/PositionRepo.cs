using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return db.Profiles.Where(x => x.account_id == account_id).FirstOrDefault()?.Position.position_id;
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
