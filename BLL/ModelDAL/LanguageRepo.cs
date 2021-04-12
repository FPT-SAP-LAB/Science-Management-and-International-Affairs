using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ModelDAL
{
    public class LanguageRepo
    {
        public static List<Language> GetLanguages()
        {
            using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
            {
                return GetLanguages(db);
            }
        }
        public static List<Language> GetLanguages(ScienceAndInternationalAffairsEntities db)
        {
            return db.Languages.ToList();
        }
    }
}
