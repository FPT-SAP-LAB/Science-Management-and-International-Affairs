using ENTITIES;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ModelDAL
{
    public class TitleLanguageRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<TitleLanguage> GetList(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.TitleLanguages.Where(x => x.language_id == language_id).ToList();
        }
    }
}
