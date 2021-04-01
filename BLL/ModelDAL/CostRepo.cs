using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        public int Update(int request_id, int account_id, List<Cost> costs)
        {
            DataTable dt = new DataTable();
            db = new ScienceAndInternationalAffairsEntities();
            RequestConference request = db.RequestConferences.Where(x => x.request_id == request_id && x.BaseRequest.account_id == account_id).FirstOrDefault();
            if (request != null)
            {
                using (DbContextTransaction trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        int Times = 0;
                        List<Cost> costs1 = request.Costs.Where(x => x.editable == true).ToList();
                        foreach (var x in costs)
                        {
                            foreach (var y in costs1)
                            {
                                if (x.cost_id == y.cost_id)
                                {
                                    int total = int.Parse(dt.Compute(x.detail.Replace(",", ""), "").ToString());
                                    y.detail = x.detail;
                                    y.total = total;
                                    y.editable = false;
                                    Times += 1;
                                }
                            }
                        }
                        db.SaveChanges();
                        trans.Commit();
                        return Times;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        trans.Rollback();
                    }
                }
            }
            return 0;
        }
    }
}
