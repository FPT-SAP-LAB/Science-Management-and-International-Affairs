using ENTITIES;
using ENTITIES.CustomModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorIndexRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public string GetIndexPageGuest(BaseDatatable baseDatatable, int account_id, int language_id)
        {
            var data = (from a in db.Papers
                        join b in db.RequestConferences on a.paper_id equals b.paper_id
                        join c in db.Conferences on b.conference_id equals c.conference_id
                        join d in db.BaseRequests on b.request_id equals d.request_id
                        join e in db.ConferenceStatusLanguages on b.status_id equals e.status_id
                        where e.language_id == language_id && d.account_id == account_id
                        select new
                        {
                            PaperName = a.name,
                            ConferenceName = c.conference_name,
                            Date = d.created_date,
                            StatusName = e.name,
                            StatusID = e.status_id
                        })
                             .OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
                             .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = (from b in db.RequestConferences
                                join d in db.BaseRequests on b.request_id equals d.request_id
                                where d.account_id == account_id
                                select b).Count();

            return JsonConvert.SerializeObject(new { data, recordsTotal });
        }
        public string GetIndexPageManager(BaseDatatable baseDatatable)
        {
            var data = (from a in db.Papers
                        join b in db.RequestConferences on a.paper_id equals b.paper_id
                        join c in db.Conferences on b.conference_id equals c.conference_id
                        join d in db.BaseRequests on b.request_id equals d.request_id
                        join e in db.ConferenceStatusLanguages on b.status_id equals e.status_id
                        join f in db.Accounts on d.account_id equals f.account_id
                        where e.language_id == 1 && b.status_id != 5
                        select new
                        {
                            PaperName = a.name,
                            ConferenceName = c.conference_name,
                            Date = d.created_date,
                            StatusName = e.name,
                            FullName = f.full_name,
                            StatusID = e.status_id
                        })
                             .OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
                             .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = (from b in db.RequestConferences
                                where b.status_id != 5
                                select b).Count();

            return JsonConvert.SerializeObject(new { data, recordsTotal });
        }
    }
}
