using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelDAL
{
    public class PositionLanguageRepo
    {
        public static List<PositionLanguage> GetPositionLanguages(int language_id)
        {
            using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.PositionLanguages.Where(x => x.language_id == language_id).ToList();
            }
        }
    }
}
