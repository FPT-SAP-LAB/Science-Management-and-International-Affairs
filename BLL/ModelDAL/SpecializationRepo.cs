using ENTITIES;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ModelDAL
{
    public class SpecializationRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<SpecializationLanguage> GetSpecializations(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.SpecializationLanguages.Where(x => x.language_id == language_id).ToList();
        }
    }
}
