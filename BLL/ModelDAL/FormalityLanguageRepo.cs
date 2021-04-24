using ENTITIES;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ModelDAL
{
    public class FormalityLanguageRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<FormalityLanguage> GetList(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.FormalityLanguages.Where(x => x.language_id == language_id).ToList();
        }
    }
}
