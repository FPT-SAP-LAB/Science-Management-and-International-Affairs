using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    class AcademicActivityExpenseTypeRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public HashSet<ENTITIES.ActivityExpenseType> activityExpenseTypes()
        {
            HashSet<ActivityExpenseType> activityExpenseTypes = db.ActivityExpenseTypes.ToHashSet();
            return activityExpenseTypes;
        }
    }
}
