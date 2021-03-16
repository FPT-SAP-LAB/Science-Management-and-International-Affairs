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
        public string GetIndexPageJson(BaseDatatable baseDatatable, int language_id)
        {
            var data = (from a in db.Papers
                        join b in db.RequestConferences on a.paper_id equals b.paper_id
                        join c in db.Conferences on b.conference_id equals c.conference_id
                        join d in db.BaseRequests on b.request_id equals d.request_id
                        join e in db.ConferenceStatusLanguages on b.status_id equals e.status_id
                        where e.language_id == language_id
                        select new
                        {
                            PaperName = a.name,
                            ConferenceName = c.conference_name,
                            Date = d.created_date,
                            StatusName = e.name
                        })
                             .OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
                             .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = (from a in db.Papers
                                join b in db.RequestConferences on a.paper_id equals b.paper_id
                                join c in db.Conferences on b.conference_id equals c.conference_id
                                join d in db.BaseRequests on b.request_id equals d.request_id
                                join e in db.ConferenceStatusLanguages on b.status_id equals e.status_id
                                where e.language_id == language_id
                                select c).Count();

            return JsonConvert.SerializeObject(new { data, recordsTotal });
        }
    }
}
