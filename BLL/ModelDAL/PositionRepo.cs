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
        public static int GetPositionIdByAccountId(int account_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            return GetPositionIdByAccountId(db, account_id);
        }
        public static int GetPositionIdByAccountId(ScienceAndInternationalAffairsEntities db, int account_id)
        {
            return db.Profiles.Where(x => x.account_id == account_id)
                .Select(x => x.Positions.FirstOrDefault().position_id).FirstOrDefault();
        }
    }
}
