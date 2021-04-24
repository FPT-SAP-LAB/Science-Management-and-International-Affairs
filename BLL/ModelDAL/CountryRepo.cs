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
    }
}
