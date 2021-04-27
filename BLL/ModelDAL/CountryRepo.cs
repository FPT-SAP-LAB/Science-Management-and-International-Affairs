using ENTITIES;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ModelDAL
{
    public class CountryRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<Country> GetCountries()
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.Countries.ToList();
        }

        public List<Country> GetCountries(string country_name)
        {
            country_name = country_name ?? "";
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.Countries.Where(x => x.country_name.Contains(country_name)).ToList();
        }
    }
}
