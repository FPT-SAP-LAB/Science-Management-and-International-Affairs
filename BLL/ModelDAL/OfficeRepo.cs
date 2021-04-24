using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ModelDAL
{
    public class OfficeRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<String> GetOfficeName(string name, int numberOfRow)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.Offices.Select(x => x.office_name).Where(x => x.Contains(name)).Take(numberOfRow).ToList();
        }
        public List<Office> GetList()
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.Offices.ToList();
        }
    }
}
