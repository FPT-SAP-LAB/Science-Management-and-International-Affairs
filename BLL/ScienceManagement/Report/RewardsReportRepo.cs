using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Report;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using ENTITIES.CustomModels.ScienceManagement.SearchFilter;

namespace BLL.ScienceManagement.Report
{
    public class RewardsReportRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        //public BaseServerSideData<ArticlesInCountryReport> getAriticlesInCountryReport(BaseDatatable baseDatatable, int account_id = 0, int language_id = 1)
        //{
        //    var data = (from a in db.Decisions
        //                join b in db.RequestDecisions on a.decision_id equals b.decision_id
        //                join c in db.BaseRequests on b.request_id equals c.request_id
        //                join d in db.RequestPapers on c.request_id equals d.request_id
        //                join e in db.Papers on d.paper_id equals e.paper_id
        //                join f in db.AuthorPapers on e.paper_id equals f.paper_id
        //                join g in db.People on f.people_id equals g.people_id
        //                join h in db.Profiles on g.people_id equals h.people_id
        //                join i in db.Specializations on d.specialization_id equals i.specialization_id
        //                join k in db.SpecializationLanguages on i.specialization_id equals k.specialization_id
        //                join l in db.Offices on h.office_id equals l.office_id
        //                where k.language_id == 1 && d.type == "trongnuoc"
        //                select new ArticlesInCountryReport
        //                {
        //                    decision_number = a.decision_number,
        //                    author_name = g.name,
        //                    paper_name = e.name,
        //                    journal_name = e.journal_name,
        //                    valid_date = a.valid_date.ToString(),
        //                    total_reward = f.money_reward.ToString()
        //                });
        //    var res = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
        //    .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
        //    int recordsTotal = data.Count();
        //    return new BaseServerSideData<ArticlesInCountryReport>(res, recordsTotal);
        //}
        public BaseServerSideData<ReportByAuthorAward> getAwardReportByAuthor(BaseDatatable baseDatatable, SearchFilter search, int account_id = 0, int language_id = 1)
        {
            var data = (from a in db.AuthorPapers
                        join b in db.Authors on a.people_id equals b.people_id
                        join c in db.Titles on b.title_id equals c.title_id
                        join d in db.TitleLanguages on c.title_id equals d.title_id
                        join e in db.Offices on b.office_id equals e.office_id
                        join f in db.Papers on a.paper_id equals f.paper_id
                        where d.language_id == 1
                        select new ReportByAuthorAward
                        {
                            name = b.name,
                            title = d.name,
                            office = e.office_name,
                            office_id = e.office_id,
                            paperAward = (from m in db.RequestPapers
                                          join n in db.Papers on m.paper_id equals n.paper_id
                                          join h in db.AuthorPapers on n.paper_id equals h.paper_id
                                          join j in db.Authors on h.people_id equals j.people_id
                                          where m.status_id == 2 
                                          && m.type == 2 
                                          && j.name == b.name 
                                          && j.identification_number == b.identification_number
                                          select h.money_reward).Distinct().Sum().ToString(),
                            conferenceAward = "0",
                            CitationAward = "0",
                            PublicYear=f.publish_date.Value.Year.ToString()
                        });
            if (search.office_id != null)
            {
                data = data.Where(x => x.office_id == search.office_id);
            }
            if (search.name != null)
            {
                data = data.Where(x => x.name.Contains(search.name));
            }
            if (search.year != null)
            {
                data = data.Where(x => x.PublicYear==search.year);
            }
            var result = data.Distinct().OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
                .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = data.Count();
            return new BaseServerSideData<ReportByAuthorAward>(result, recordsTotal);
        }
        
        public List<String> getListYearPaper()
        {
            var data = (from a in db.BaseRequests select a.created_date.Value.Year.ToString()).Distinct().ToList();
            return data;
        }
    }
}
