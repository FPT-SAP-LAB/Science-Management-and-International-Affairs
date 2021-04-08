using ENTITIES;
using ENTITIES.CustomModels;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BLL.ModelDAL
{
    public class QsUniversityRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public BaseServerSideData<QsUniversity> List(BaseDatatable baseDatatable)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var data = db.QsUniversities.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
                .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = db.QsUniversities.Count();

            return new BaseServerSideData<QsUniversity>(data, recordsTotal);
        }
    }
}
