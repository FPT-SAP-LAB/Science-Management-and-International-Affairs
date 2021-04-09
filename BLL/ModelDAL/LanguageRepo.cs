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
            List<Language> languages = new List<Language>();
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    languages = db.Languages.ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return languages;
        }
    }
}
