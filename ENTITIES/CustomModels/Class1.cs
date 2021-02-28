using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels
{
    public class Class1
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<Country> GetAllCountries()
        {
            var countries = db.Countries.ToList();
            return countries;
        }
    }
}
