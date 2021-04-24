using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.Conference;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorIndexRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public BaseServerSideData<ConferenceIndex> GetIndexPage(BaseDatatable baseDatatable, ConferenceSearch search, int account_id = 0, int language_id = 1)
        {
            search.SearchConference = search.SearchConference ?? "";
            search.SearchPaper = search.SearchPaper ?? "";
            var data = (from a in db.Papers
                        join b in db.RequestConferences on a.paper_id equals b.paper_id
                        join c in db.Conferences on b.conference_id equals c.conference_id
                        join d in db.BaseRequests on b.request_id equals d.request_id
                        join e in db.ConferenceStatusLanguages on b.status_id equals e.status_id
                        join f in db.Accounts on d.account_id equals f.account_id
                        where e.language_id == language_id && (b.status_id != 5 || account_id != 0)
                        && (d.account_id == account_id || account_id == 0)
                        && a.name.Contains(search.SearchPaper) && c.conference_name.Contains(search.SearchConference)
                        && (b.status_id == search.SearchStatus || search.SearchStatus == 0)
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
                                && a.name.Contains(search.SearchPaper) && c.conference_name.Contains(search.SearchConference)
                                && (b.status_id == search.SearchStatus || search.SearchStatus == 0)
                                select b).Count();

            for (int i = 0; i < data.Count; i++)
            {
                data[i].RowNumber = baseDatatable.Start + 1 + i;
                data[i].CreatedDate = data[i].Date.ToString("dd/MM/yyyy");
            }
            return new BaseServerSideData<ConferenceIndex>(data, recordsTotal);
        }
        public BaseServerSideData<ConferenceIndex> GetHistoryPage(BaseDatatable baseDatatable, string search_paper, string search_conference)
        {
            search_paper = search_paper ?? "";
            search_conference = search_conference ?? "";
            var data = (from a in db.Papers
                        join b in db.RequestConferences on a.paper_id equals b.paper_id
                        join c in db.Conferences on b.conference_id equals c.conference_id
                        join d in db.BaseRequests on b.request_id equals d.request_id
                        join e in db.ConferenceStatusLanguages on b.status_id equals e.status_id
                        join f in db.Accounts on d.account_id equals f.account_id
                        where e.language_id == 1 && d.finished_date != null
                        && a.name.Contains(search_paper) && c.conference_name.Contains(search_conference)
                        select new ConferenceIndex
                        {
                            RequestID = d.request_id,
                            PaperName = a.name,
                            ConferenceName = c.conference_name,
                            Date = d.created_date.Value,
                            FullName = f.full_name,
                        })
                             .OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
                             .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = (from a in db.Papers
                                join b in db.RequestConferences on a.paper_id equals b.paper_id
                                join c in db.Conferences on b.conference_id equals c.conference_id
                                join d in db.BaseRequests on b.request_id equals d.request_id
                                where d.finished_date != null
                                && a.name.Contains(search_paper) && c.conference_name.Contains(search_conference)
                                select b).Count();

            for (int i = 0; i < data.Count; i++)
            {
                data[i].RowNumber = baseDatatable.Start + 1 + i;
                data[i].CreatedDate = data[i].Date.ToString("dd/MM/yyyy");
            }
            return new BaseServerSideData<ConferenceIndex>(data, recordsTotal);
        }
    }
}
