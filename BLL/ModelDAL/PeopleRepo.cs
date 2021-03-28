using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelDAL
{
   public class PeopleRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<String> GetPeopleName(string name, int numberOfRows)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.Profiles.Select(x => x.Person.name).Where(x=>x.Contains(name)).Take(numberOfRows).ToList();
        }
    }
}
