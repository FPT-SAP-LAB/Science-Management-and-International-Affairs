using ENTITIES;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.Report;
using ENTITIES.CustomModels.ScienceManagement.SearchFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using static ENTITIES.CustomModels.ScienceManagement.Report.ArticlesInoutCountryReports;
using static ENTITIES.CustomModels.ScienceManagement.Report.IntellectualPropertyReport;

namespace BLL.ScienceManagement.Report
{
    public class RewardsReportRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public Tuple<BaseServerSideData<ArticlesInoutCountryReports>, string> GetAriticlesByAreaReports(BaseDatatable baseDatatable, SearchFilter search, int? paperType)
        {
            var data = (from a in db.Decisions
                        join b in db.RequestDecisions on a.decision_id equals b.decision_id
                        join c in db.BaseRequests on b.request_id equals c.request_id
                        join d in db.RequestPapers on c.request_id equals d.request_id
                        join e in db.Papers on d.paper_id equals e.paper_id
                        where d.type == paperType && e.is_verified == true
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
                                      join d3 in db.TitleLanguages on a3.title_id equals d3.title_id
                                      where b3.paper_id == e.paper_id && d3.language_id == 1
                                      select d3.name).Distinct().ToList(),
                            paper_name = e.name,
                            journal_name = e.journal_name,
                            specialization = (from a4 in db.RequestPapers
                                              join b4 in db.Specializations on a4.specialization_id equals b4.specialization_id
                                              join c4 in db.SpecializationLanguages on b4.specialization_id equals c4.specialization_id
                                              where c4.language_id == 1 && a4.paper_id == e.paper_id
                                              select c4.name).FirstOrDefault(),
                            criterias = (from b5 in db.PaperWithCriterias
                                         join c5 in db.PaperCriterias on b5.criteria_id equals c5.criteria_id
                                         where b5.paper_id == e.paper_id
                                         && (c5.criteria_id == 1 || c5.criteria_id == 2 || c5.criteria_id == 3 || c5.criteria_id == 4)
                                         select new PaperCriteriaCustom
                                         {
                                             id = c5.criteria_id,
                                             name = c5.name
                                         }).ToList(),
                            co_author = (from b5 in db.PaperWithCriterias
                                         join c5 in db.PaperCriterias on b5.criteria_id equals c5.criteria_id
                                         where b5.paper_id == e.paper_id
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
            for (int i = 0; i < res.Count; i++)
            {
                res[i].valid_date_string = res[i].valid_date.ToString("dd/MM/yyyy");
                res[i].rownum = baseDatatable.Start + 1 + i;
            }

            string totalAmount = data.Select(x => x.total_reward).Sum().ToString();
            int recordsTotal = data.Count();
            return new Tuple<BaseServerSideData<ArticlesInoutCountryReports>, string>(new BaseServerSideData<ArticlesInoutCountryReports>(res, recordsTotal), totalAmount);
        }
        //public BaseServerSideData<ReportByAuthorAward> GetAwardReportByAuthor(BaseDatatable baseDatatable, SearchFilter search)
        public List<ReportByAuthorAward> GetAwardReportByAuthor()
        {
            db.Configuration.LazyLoadingEnabled = true;

            var paperInvention = db.Authors.Select(x => new ReportByAuthorAward
            {
                msnv_mssv = x.mssv_msnv,
                paperAward = (from m in db.RequestPapers
                              join n in db.Papers on m.paper_id equals n.paper_id
                              join h in x.AuthorPapers on n.paper_id equals h.paper_id
                              where m.status_id == 2 && h.money_reward != null
                              select h.money_reward.Value).ToList(),
                inventionAmount = (from m in db.RequestInventions
                                   join n in db.Inventions on m.invention_id equals n.invention_id
                                   join h in x.AuthorInventions on n.invention_id equals h.invention_id
                                   where m.status_id == 2 && h.money_reward != null
                                   select h.money_reward.Value).ToList()
            }).ToList();

            var citation = (from m in db.RequestCitations
                            join a in db.BaseRequests on m.request_id equals a.request_id
                            join b in db.Profiles on a.account_id equals b.account_id
                            where m.citation_status_id == 2 && m.total_reward != null
                            select new ReportByAuthorAward
                            {
                                msnv_mssv = b.mssv_msnv,
                                CitationAward = m.total_reward.Value
                            }).GroupBy(x => x.msnv_mssv).Select(x => new ReportByAuthorAward
                            {
                                msnv_mssv = x.Key,
                                CitationAward = x.Sum(y => y.CitationAward),
                            }).ToList();

            foreach (var item in citation)
            {
                item.inventionAmount = new List<int>();
                item.paperAward = new List<int>();
            }

            Dictionary<string, ReportByAuthorAward> dict = new Dictionary<string, ReportByAuthorAward>();

            foreach (var item in paperInvention)
            {
                if (dict.TryGetValue(item.msnv_mssv, out ReportByAuthorAward temp))
                {
                    temp.paperAward.AddRange(item.paperAward);
                    temp.inventionAmount.AddRange(item.inventionAmount);
                }
                else
                {
                    dict.Add(item.msnv_mssv, item);
                }
            }

            foreach (var item in citation)
            {
                if (dict.TryGetValue(item.msnv_mssv, out ReportByAuthorAward temp))
                {
                    temp.CitationAward += temp.CitationAward;
                }
                else
                {
                    dict.Add(item.msnv_mssv, item);
                }
            }

            var informations = dict
                .Where(x => x.Value.inventionAmount.Count > 0 || x.Value.paperAward.Count > 0 || x.Value.CitationAward > 0)
                .Select(x => x.Value).ToList();

            foreach (var info in informations)
            {
                info.name = db.People.Where(y => y.Profile.mssv_msnv.Equals(info.msnv_mssv)).Select(y => y.name).FirstOrDefault();
                info.office = db.People.Where(y => y.Profile.mssv_msnv.Equals(info.msnv_mssv)).Select(y => y.Office.office_name).FirstOrDefault();
                info.title = (from a in db.People
                              join b in db.Profiles on a.people_id equals b.people_id
                              join c in db.TitleLanguages on b.title_id equals c.title_id
                              where c.language_id == 1
                              select c.name).FirstOrDefault();
            }

            for (int i = 0; i < informations.Count; i++)
            {
                var info = informations[i];
                if (info.name == null)
                {
                    info.name = db.Authors.Where(y => y.mssv_msnv.Equals(info.msnv_mssv)).Select(y => y.name).FirstOrDefault();
                    info.office = db.Authors.Where(y => y.mssv_msnv.Equals(info.msnv_mssv)).Select(y => y.Office.office_name).FirstOrDefault();
                    info.title = (from b in db.Authors
                                  join c in db.TitleLanguages on b.title_id equals c.title_id
                                  where c.language_id == 1
                                  select c.name).FirstOrDefault();
                }
            }

            //.Union(from d1 in db.Profiles
            //         join a1 in db.Accounts on d1.account_id equals a1.account_id
            //         join e1 in db.People on d1.people_id equals e1.people_id
            //         join f1 in db.TitleLanguages on d1.title_id equals f1.title_id
            //         join g1 in db.Offices on e1.office_id equals g1.office_id
            //         select new ReportByAuthorAward
            //         {
            //             name = e1.name,
            //             msnv_mssv = d1.mssv_msnv,
            //             title = f1.name,
            //             office = g1.office_name,
            //             office_id = g1.office_id,
            //             CitationAward = (from b1 in db.RequestCitations
            //                              join c1 in db.BaseRequests on b1.request_id equals c1.request_id
            //                              where b1.citation_status_id == 2 && c1.account_id == d1.account_id
            //                              select b1.total_reward).Sum().ToString()
            //         });

            //if (search.office_id != null)
            //{
            //    data = data.Where(x => x.office_id == search.office_id);
            //}
            //if (search.name != null && search.name.Trim() != "")
            //{
            //    data = data.Where(x => x.name.Contains(search.name));
            //}
            //if (search.year != null)
            //{
            //    data = data.Where(x => x.PublicYear == search.year);
            //}
            //var result = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
            //    .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
            //for (int i = 0; i < result.Count; i++)
            //{
            //    result[i].inventionAmount = result[i].inventionAwards.Select(x => Convert.ToInt32(x)).ToList().Sum();
            //}

            //for (int i = 0; i < result.Count; i++)
            //{
            //    result[i].rowNum = baseDatatable.Start + 1 + i;
            //}
            //int recordsTotal = data.Count();
            //return new BaseServerSideData<ReportByAuthorAward>(result, recordsTotal);
            return informations;
        }
        public Tuple<BaseServerSideData<IntellectualPropertyReport>, string> GetIntellectualPropertyReport(BaseDatatable baseDatatable, SearchFilter search)
        {
            Support.SupportClass.TrimProperties(search);
            var data = (from a in db.Decisions
                        join b in db.RequestDecisions on a.decision_id equals b.decision_id
                        join c in db.BaseRequests on b.request_id equals c.request_id
                        join d in db.RequestInventions on c.request_id equals d.request_id
                        join e in db.Inventions on d.invention_id equals e.invention_id
                        where d.status_id == 2 && e.is_verified == true
                        select new IntellectualPropertyReport
                        {
                            valid_date = a.valid_date,
                            decision_number = a.decision_number,
                            authors = (from a1 in db.Authors
                                       join b1 in db.AuthorInventions on a1.people_id equals b1.people_id
                                       where b1.invention_id == e.invention_id && (search.office_id == null || a1.office_id == search.office_id)
                                       select a1.name
                                       ).ToList(),
                            invention_name = e.name,
                            date = e.date,
                            kind = (from m in db.InventionTypes
                                    where e.type_id == m.invention_type_id
                                    select m.name).FirstOrDefault(),
                            invention_number = e.no,
                            total_reward = d.total_reward,
                        });
            if (search.name != null)
            {
                data = data.Where(x => x.invention_name.Contains(search.name));
            }
            if (search.year != null)
            {
                data = data.Where(x => x.date.Value.Year.ToString() == search.year);
            }
            //if (search.office_id != null)
            //{
            //    data = data.Where(x => x.authors.Select(a => a.office_id).ToList().Contains(search.office_id.Value));
            //}
            var res = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
            .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
            string totalAmount = "";
            long total = 0;
            for (int i = 0; i < res.Count; i++)
            {
                var item = res[i];
                if (item.total_reward != null && item.total_reward.Trim() != "")
                {
                    total += long.Parse(item.total_reward);
                }
                item.date_string = item.date.Value.ToString("dd/MM/yyyy");
                item.rownum = baseDatatable.Start + 1 + i;
                item.valid_date_string = item.valid_date.ToString("dd/MM/yyyy");
            }
            totalAmount = total.ToString();
            int recordsTotal = data.Count();
            return new Tuple<BaseServerSideData<IntellectualPropertyReport>,
                string>(new BaseServerSideData<IntellectualPropertyReport>(res, recordsTotal), totalAmount);
        }
        public Tuple<BaseServerSideData<CitationByAuthorReport>, long> GetCitationByAuthorReport(BaseDatatable baseDatatable, SearchFilter search)
        {
            db.Configuration.LazyLoadingEnabled = true;

            var data = (from a in db.RequestCitations
                        join b in db.BaseRequests on a.request_id equals b.request_id
                        join c in db.Profiles on b.account_id equals c.account_id
                        join d in db.People on c.people_id equals d.people_id
                        join e in db.RequestDecisions on b.request_id equals e.request_id
                        join f in db.Decisions on e.decision_id equals f.decision_id
                        where a.citation_status_id == 2
                        select new CitationByAuthorReport
                        {
                            valid_date = f.valid_date,
                            decision_number = f.decision_number,
                            author_name = d.name,
                            msnv = c.mssv_msnv,
                            office = d.Office.office_name,
                            scopus_citation = a.Citations.Where(x => x.citation_type_id == 2).Select(x => x.count).Sum(),
                            gscholar_citation = a.Citations.Where(x => x.citation_type_id == 1).Select(x => x.count).Sum(),
                            total_reward = a.total_reward,
                        });
            if (search.msnv != null && search.msnv.Trim() != "")
            {
                data = data.Where(x => x.msnv.Contains(search.msnv));
            }
            var res = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
            .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            for (int i = 0; i < res.Count; i++)
            {
                res[i].scopus_citation = res[i].scopus_citation == null ? 0 : res[i].scopus_citation;
                res[i].gscholar_citation = res[i].gscholar_citation == null ? 0 : res[i].gscholar_citation;
                res[i].total_reward = res[i].total_reward == null ? 0 : res[i].total_reward;
                res[i].valid_date_string = res[i].valid_date.ToString("dd/MM/yyyy");
                res[i].rownum = baseDatatable.Start + 1 + i;
            }

            long total = 0;
            foreach (var i in res)
            {
                if (i.total_reward != null)
                {
                    total += i.total_reward.Value;
                }
            }
            int recordsTotal = data.Count();
            return new Tuple<BaseServerSideData<CitationByAuthorReport>,
                long>(new BaseServerSideData<CitationByAuthorReport>(res, recordsTotal), total);
        }
        public Tuple<BaseServerSideData<ConferencesParticipationReport>, string> GetConferencesReport(BaseDatatable baseDatatable, SearchFilter search)
        {
            var data = (from a in db.BaseRequests
                        join b in db.RequestConferences on a.request_id equals b.request_id
                        join c in db.ConferenceParticipants on b equals c.RequestConference
                        join d in db.RequestDecisions on a.request_id equals d.request_id into decision
                        from e in decision.DefaultIfEmpty()
                        where b.status_id == 5
                        select new ConferencesParticipationReport
                        {
                            valid_date = e.Decision.valid_date,
                            decision_number = e.Decision.decision_number,
                            people_name = c.name,
                            title_name = (
                                    from b1 in db.TitleLanguages
                                    where b1.title_id == c.title_id && b1.language_id == 1
                                    select b1.name
                                ).FirstOrDefault(),
                            office_name = (
                                    from a2 in db.Offices
                                    where a2.office_id == c.office_id
                                    select a2.office_name).FirstOrDefault(),
                            office_id = c.office_id,
                            country_name = b.Conference.Country.country_name,
                            conference_name = b.Conference.conference_name,
                            attendance_date = b.attendance_start,
                            reimbursement = b.reimbursement,
                            total = b.Costs.Sum(x => x.total),
                            requestId = b.request_id
                        });
            if (search.name != null && search.name.Trim() != "")
            {
                data = data.Where(x => x.people_name.Contains(search.name));
            }
            if (search.year != null && search.year.Trim() != "")
            {
                data = data.Where(x => x.valid_date.Value.Year.ToString() == search.year);
            }
            if (search.office_id != null)
            {
                data = data.Where(x => x.office_id == search.office_id);
            }
            var res = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
            .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
            string totalAmount = "";
            long total = 0;
            foreach (var i in res)
            {
                total += i.total;
            }
            totalAmount = total.ToString();
            int recordsTotal = data.Count();
            return new Tuple<BaseServerSideData<ConferencesParticipationReport>,
                string>(new BaseServerSideData<ConferencesParticipationReport>(res, recordsTotal), totalAmount);
        }

        public List<string> GetListYearPaper()
        {
            var data = (from a in db.BaseRequests select a.created_date.Value.Year.ToString()).Distinct().ToList();
            return data;
        }
        public List<string> GetListYear(int gap)
        {
            int end = DateTime.Now.Year;
            int start = end - gap;
            List<string> data = Enumerable.Range(start, gap + 1).Select(x => x.ToString()).OrderByDescending(x => x).ToList();
            return data;
        }
        public List<PaperCriteria> GetListCriteria()
        {
            List<int> criteria = new List<int>() { 1, 2, 3, 4, 5 };
            var data = (from a in db.PaperCriterias where criteria.Contains(a.criteria_id) select a).ToList();
            return data;
        }
    }
}
