using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelDAL
{
    public class SpecializationRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<Specialization> GetSpecializations()
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.Specializations.ToList();
        }
    }
}
