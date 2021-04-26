using ENTITIES;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ModelDAL
{
    public class CitationTypeRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<CitationType> GetCitationTypes()
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.CitationTypes.ToList();
        }
    }
}
