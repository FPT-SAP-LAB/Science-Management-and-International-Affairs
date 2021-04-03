using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BLL.ModelDAL
{
    public class ResearcherAreasRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<ResearchAreaLanguage> GetResearchAreasByLanguage(int lang_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.ResearchAreaLanguages.Where(x => x.language_id == lang_id).ToList();
        }
    }
}
