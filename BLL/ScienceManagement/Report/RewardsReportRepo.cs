//using ENTITIES;
//using ENTITIES.CustomModels;
//using ENTITIES.CustomModels.ScienceManagement.Report;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Linq.Dynamic.Core;
//namespace BLL.ScienceManagement.Report
//{
//    public class RewardsReportRepo
//    {
//        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
//        public BaseServerSideData<ArticlesInCountryReport> getAriticlesInCountryReport(BaseDatatable baseDatatable, int account_id = 0, int language_id = 1)
//        {
//            var data = (from a in db.Decisions
//                        join b in db.RequestDecisions on a.decision_id equals b.decision_id
//                        join c in db.BaseRequests on b.request_id equals c.request_id
//                        join d in db.RequestPapers on c.request_id equals d.request_id
//                        join e in db.Papers on d.paper_id equals e.paper_id
//                        join f in db.AuthorPapers on e.paper_id equals f.paper_id
//                        join g in db.People on f.people_id equals g.people_id
//                        join h in db.Profiles on g.people_id equals h.people_id
//                        join i in db.Specializations on d.specialization_id equals i.specialization_id
//                        join k in db.SpecializationLanguages on i.specialization_id equals k.specialization_id
//                        join l in db.Offices on h.office_id equals l.office_id
//                        where k.language_id == 1 && d.type == "trongnuoc"
//                        select new ArticlesInCountryReport
//                        {
//                            decision_number = a.decision_number,
//                            author_name = g.name,
//                            paper_name = e.name,
//                            journal_name = e.journal_name,
//                            valid_date = a.valid_date.ToString(),
//                            total_reward = f.money_reward.ToString()
//                        });
//            var res = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
//            .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
//            int recordsTotal = data.Count();
//            return new BaseServerSideData<ArticlesInCountryReport>(res, recordsTotal);
//        }
//    }
//}
