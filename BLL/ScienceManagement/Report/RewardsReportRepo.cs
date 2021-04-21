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
        public Tuple<BaseServerSideData<ArticlesInoutCountryReports>, String> getAriticlesByAreaReports(BaseDatatable baseDatatable, SearchFilter search, int? paperType, int account_id = 0, int language_id = 1)
        {
            var data = (from a in db.Decisions
                        join b in db.RequestDecisions on a.decision_id equals b.decision_id
                        join c in db.BaseRequests on b.request_id equals c.request_id
                        join d in db.RequestPapers on c.request_id equals d.request_id
                        join e in db.Papers on d.paper_id equals e.paper_id
                        where d.type == paperType //SAU THÊM ĐIỀU KIỆN CỦA TRƯỜNG is_verified VÀO ĐÂY
                        select new ArticlesInoutCountryReports
                        {
                            decision_number = a.decision_number,
                            authors = (from a1 in db.Authors
                                       join b1 in db.AuthorPapers on a1.people_id equals b1.people_id
                                       where b1.paper_id == e.paper_id
                                       select a1.name).ToList(),
                            offices = (from a2 in db.Authors
                                       join b2 in db.AuthorPapers on a2.people_id equals b2.people_id
                                       join c2 in db.Offices on a2.office_id equals c2.office_id
                                       where b2.paper_id == e.paper_id
                                       select c2.office_name).Distinct().ToList(),
                            titles = (from a3 in db.Authors
                                      join b3 in db.AuthorPapers on a3.people_id equals b3.people_id
                                      join c3 in db.Titles on a3.title_id equals c3.title_id
                                      join d3 in db.TitleLanguages on c3.title_id equals d3.title_id
                                      where b3.paper_id == e.paper_id && d3.language_id == 1
                                      select d3.name).Distinct().ToList(),
                            paper_name = e.name,
                            journal_name = e.journal_name,
                            specialization = (from a4 in db.RequestPapers
                                              join b4 in db.Specializations on a4.specialization_id equals b4.specialization_id
                                              join c4 in db.SpecializationLanguages on b4.specialization_id equals c4.specialization_id
                                              where c4.language_id == 1 && a4.paper_id == e.paper_id
                                              select c4.name).FirstOrDefault(),
                            criterias = (from a5 in db.Papers
                                         join b5 in db.PaperWithCriterias on a5.paper_id equals b5.paper_id
                                         join c5 in db.PaperCriterias on b5.criteria_id equals c5.criteria_id
                                         where a5.paper_id == e.paper_id
                                         && (c5.criteria_id == 1 || c5.criteria_id == 2 || c5.criteria_id == 3 || c5.criteria_id == 4)
                                         select new PaperCriteriaCustom
                                         {
                                             id = c5.criteria_id,
                                             name = c5.name
                                         }).ToList(),
                            co_author = (from a5 in db.Papers
                                         join b5 in db.PaperWithCriterias on a5.paper_id equals b5.paper_id
                                         join c5 in db.PaperCriterias on b5.criteria_id equals c5.criteria_id
                                         where a5.paper_id == e.paper_id
                                         && (c5.criteria_id == 6)
                                         select c5.name).FirstOrDefault(),
                            valid_date = a.valid_date,
                            valid_date_string = "",
                            total_reward = d.total_reward
                        });
            if (search.name != null && search.name.Trim() != "")
            {
                data = data.Where(x => x.paper_name.Contains(search.name));
            }
            if (search.year != null && search.year.Trim() != "")
            {
                data = data.Where(x => x.valid_date.Year.ToString() == search.year);
            }
            if (search.hang != null)
            {
                data = data.Where(x => x.criterias.Select(a => a.id).ToList().Contains(search.hang.Value));
            }
            var res = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
            .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
            String totalAmount = data.Select(x => x.total_reward).Sum().ToString();
            int recordsTotal = data.Count();
            return new Tuple<BaseServerSideData<ArticlesInoutCountryReports>, String>(new BaseServerSideData<ArticlesInoutCountryReports>(res, recordsTotal), totalAmount);
        }
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
                            msnv_mssv = b.mssv_msnv,
                            title = d.name,
                            office = e.office_name,
                            office_id = e.office_id,
                            paperAward = (from m in db.RequestPapers
                                          join n in db.Papers on m.paper_id equals n.paper_id
                                          join h in db.AuthorPapers on n.paper_id equals h.paper_id
                                          join j in db.Authors on h.people_id equals j.people_id
                                          where m.status_id == 2
                                          && n.paper_type_id == 1
                                          && j.mssv_msnv == b.mssv_msnv
                                          select h.money_reward).Distinct().Sum().ToString(),
                            conferenceAward = (from m in db.RequestPapers
                                               join n in db.Papers on m.paper_id equals n.paper_id
                                               join h in db.AuthorPapers on n.paper_id equals h.paper_id
                                               join j in db.Authors on h.people_id equals j.people_id
                                               where m.status_id == 2
                                               && n.paper_type_id == 2
                                               && j.mssv_msnv == b.mssv_msnv
                                               select h.money_reward).Distinct().Sum().ToString(),
                            CitationAward = "0",
                            PublicYear = f.publish_date.Value.Year.ToString()
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
                data = data.Where(x => x.PublicYear == search.year);
            }
            var result = data.Distinct().OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
                .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = data.Count();
            return new BaseServerSideData<ReportByAuthorAward>(result, recordsTotal);
        }
        public Tuple<BaseServerSideData<IntellectualPropertyReport>, String> getIntellectualPropertyReport(BaseDatatable baseDatatable, SearchFilter search, int account_id = 0, int language_id = 1)
        {
            var data = (from a in db.Decisions
                        join b in db.RequestDecisions on a.decision_id equals b.decision_id
                        join c in db.BaseRequests on b.request_id equals c.request_id
                        join d in db.RequestInventions on c.request_id equals d.request_id
                        join e in db.Inventions on d.invention_id equals e.invention_id //SAU THÊM ĐIỀU KIỆN CỦA TRƯỜNG is_verified VÀO ĐÂY
                        select new IntellectualPropertyReport
                        {
                            authors = (from a1 in db.Authors
                                       join b1 in db.AuthorInventions on a1.people_id equals b1.people_id
                                       where b1.invention_id == e.invention_id
                                       select new CustomAuthor
                                       {
                                           id = a1.people_id,
                                           name = a1.name,
                                           msnv = a1.mssv_msnv,
                                           title = (from m in db.Titles
                                                    join n in db.TitleLanguages on m.title_id equals n.title_id
                                                    where m.title_id == a1.title_id && n.language_id == 1
                                                    select n.name).FirstOrDefault(),
                                           office = (from m in db.Offices
                                                     where m.office_id == a1.office_id
                                                     select m.office_name).FirstOrDefault(),
                                           office_id = (from m in db.Offices
                                                        where m.office_id == a1.office_id
                                                        select m.office_id).FirstOrDefault()
                                       }).ToList(),
                            invention_number = e.no,
                            total_reward = d.total_reward,
                            kind = (from m in db.InventionTypes
                                    where e.type_id == m.invention_type_id
                                    select m.name).FirstOrDefault(),
                            date = e.date,
                        });
            if (search.name != null && search.name.Trim() != "")
            {
                data = data.Where(x => x.invention_name.Contains(search.name));
            }
            if (search.year != null && search.year.Trim() != "")
            {
                data = data.Where(x => x.date.Value.Year.ToString() == search.year);
            }
            if (search.office_id != null)
            {
                data = data.Where(x => x.authors.Select(a => a.office_id).ToList().Contains(search.office_id.Value));
            }
            var res = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
            .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
            String totalAmount = "";
            Int64 total = 0;
            foreach (var i in res)
            {
                if (i.total_reward != null && i.total_reward.Trim() != "")
                {
                    total += Int64.Parse(i.total_reward);
                }
            }
            totalAmount = total.ToString();
            int recordsTotal = data.Count();
            return new Tuple<BaseServerSideData<IntellectualPropertyReport>,
                String>(new BaseServerSideData<IntellectualPropertyReport>(res, recordsTotal), totalAmount);
        }
        public List<String> getListYearPaper()
        {
            var data = (from a in db.BaseRequests select a.created_date.Value.Year.ToString()).Distinct().ToList();
            return data;
        }
        public List<String> getListYear(int gap)
        {
            int end = DateTime.Now.Year;
            int start = end - gap;
            List<String> data = Enumerable.Range(start, gap + 1).Select(x => x.ToString()).OrderByDescending(x => x).ToList();
            return data;
        }
        public List<PaperCriteria> getListCriteria()
        {
            List<int> criteria = new List<int>() { 1, 2, 3, 4, 5 };
            var data = (from a in db.PaperCriterias where criteria.Contains(a.criteria_id) select a).ToList();
            return data;
        }
    }
}
