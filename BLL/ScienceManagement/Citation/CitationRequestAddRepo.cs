using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BLL.ScienceManagement.Citation
{
    public class CitationRequestAddRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public AlertModal<int> AddRequestCitation(List<ENTITIES.Citation> citation, int account_id)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    BaseRequest b = new BaseRequest
                    {
                        account_id = account_id,
                        created_date = DateTime.Today,
                    };
                    db.BaseRequests.Add(b);
                    db.SaveChanges();

                    RequestCitation rc = new RequestCitation
                    {
                        request_id = b.request_id,
                        citation_status_id = 3,
                        Citations = citation
                    };
                    db.RequestCitations.Add(rc);
                    db.SaveChanges();

                    dbc.Commit();

                    return new AlertModal<int>(b.request_id, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return new AlertModal<int>(false);
                }
            }
        }
    }
}
