using ENTITIES;
using ENTITIES.CustomModels;
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

        public BaseServerSideData<ActivityExpenseType> getListActivityExpenseType(BaseDatatable baseDatatable)
        {
            db.Configuration.LazyLoadingEnabled = false;
            List<ActivityExpenseType> activityExpenseTypes = db.Database.SqlQuery<ActivityExpenseType>("select * from SMIA_AcademicActivity.ActivityExpenseType " +
                                                                    "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                    " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
            int recordsTotal = db.Database.SqlQuery<int>("select count(*) from SMIA_AcademicActivity.ActivityExpenseType").FirstOrDefault();
            return new BaseServerSideData<ActivityExpenseType>(activityExpenseTypes, recordsTotal);
        }
    }
}
