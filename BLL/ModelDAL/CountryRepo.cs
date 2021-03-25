using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
