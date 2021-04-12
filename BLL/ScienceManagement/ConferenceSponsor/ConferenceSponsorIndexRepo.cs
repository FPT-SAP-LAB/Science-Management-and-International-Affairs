using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Conference;
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
        public BaseServerSideData<ConferenceIndex> GetIndexPage(BaseDatatable baseDatatable, string search_paper, string search_conference, int search_status, int account_id = 0, int language_id = 1)
        {
            var data = (from a in db.Papers
                        join b in db.RequestConferences on a.paper_id equals b.paper_id
                        join c in db.Conferences on b.conference_id equals c.conference_id
                        join d in db.BaseRequests on b.request_id equals d.request_id
                        join e in db.ConferenceStatusLanguages on b.status_id equals e.status_id
                        join f in db.Accounts on d.account_id equals f.account_id
                        where e.language_id == language_id && (b.status_id != 5 || account_id != 0) && (d.account_id == account_id || account_id == 0)
                        && a.name.Contains(search_paper) && c.conference_name.Contains(search_conference) && (b.status_id == search_status || search_status == 0)
                        select new ConferenceIndex
                        {
                            RequestID = d.request_id,
                            PaperName = a.name,
                            ConferenceName = c.conference_name,
                            Date = d.created_date.Value,
                            StatusName = e.name,
                            FullName = f.full_name,
                            StatusID = e.status_id,
                            EditAble = b.editable
                        })
                             .OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
                             .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = (from a in db.Papers
                                join b in db.RequestConferences on a.paper_id equals b.paper_id
                                join c in db.Conferences on b.conference_id equals c.conference_id
                                join d in db.BaseRequests on b.request_id equals d.request_id
                                where (b.status_id != 5 || account_id != 0) && (d.account_id == account_id || account_id == 0)
                                && a.name.Contains(search_paper) && c.conference_name.Contains(search_conference) && (b.status_id == search_status || search_status == 0)
                                select b).Count();

            return new BaseServerSideData<ConferenceIndex>(data, recordsTotal);
        }
    }
}
