using ENTITIES;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ModelDAL
{
    public class PolicyTypeLanguageRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<PolicyTypeLanguage> PolicyTypeLanguages(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.PolicyTypeLanguages.Where(x => x.language_id == language_id).ToList();
        }
    }
}
