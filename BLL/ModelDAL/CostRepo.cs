using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;

namespace BLL.ModelDAL
{
    public class CostRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<Cost> GetList(int request_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.Costs.Where(x => x.request_id == request_id).ToList();
        }
    }
}
