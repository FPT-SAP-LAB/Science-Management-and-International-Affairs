using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.ScienceManagement.DecisionHistory
{
    public class DecisionRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<Decision> GetListDecision(string search)
        {
            if (search == null) search = "";
            search = search.Trim();
            string sql = @"select d.*
                            from SM_Request.Decision d join SM_Request.RequestDecision rd on d.decision_id = rd.decision_id
                            join SM_ScientificProduct.RequestPaper rp on rd.request_id = rp.request_id
                            where decision_number like @search
                            union
                            select d.*
                            from SM_Request.Decision d join SM_Request.RequestDecision rd on d.decision_id = rd.decision_id
                            join SM_ScientificProduct.RequestInvention rp on rd.request_id = rp.request_id
                            where decision_number like @search
                            union
                            select d.*
                            from SM_Request.Decision d join SM_Request.RequestDecision rd on d.decision_id = rd.decision_id
                            join SM_Citation.RequestCitation rp on rd.request_id = rp.request_id
                            where decision_number like @search";
            List<ENTITIES.Decision> list = db.Database.SqlQuery<ENTITIES.Decision>(sql, new SqlParameter("search", "%" + search + "%")).ToList();
            return list;
        }
        public string DeleteDecision(int id)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    List<RequestDecision> list = db.RequestDecisions.Where(x => x.decision_id == id).ToList();
                    RequestDecision rd = list[0];
                    RequestPaper rp = db.RequestPapers.Where(x => x.request_id == rd.request_id).FirstOrDefault();
                    RequestInvention ri = db.RequestInventions.Where(x => x.request_id == rd.request_id).FirstOrDefault();
                    RequestCitation rc = db.RequestCitations.Where(x => x.request_id == rd.request_id).FirstOrDefault();

                    if (rp != null)
                    {
                        foreach (var item in list)
                        {
                            RequestPaper temp = db.RequestPapers.Where(x => x.request_id == item.request_id).FirstOrDefault();
                            temp.status_id = 4;
                            db.Entry(temp).State = EntityState.Modified;
                        }
                    }
                    else if (ri != null)
                    {
                        foreach (var item in list)
                        {
                            RequestInvention temp = db.RequestInventions.Where(x => x.request_id == item.request_id).FirstOrDefault();
                            temp.status_id = 4;
                            db.Entry(temp).State = EntityState.Modified;
                        }
                    }
                    else if (rc != null)
                    {
                        foreach (var item in list)
                        {
                            RequestCitation temp = db.RequestCitations.Where(x => x.request_id == item.request_id).FirstOrDefault();
                            temp.citation_status_id = 4;
                            db.Entry(temp).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();

                    string sql = @"delete from SM_Request.RequestDecision where decision_id = @id
                                delete from SM_Request.Decision where decision_id = @id";
                    db.Database.ExecuteSqlCommand(sql, new SqlParameter("id", id));
                    dbc.Commit();
                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return "ff";
                }
        }
    }
}
