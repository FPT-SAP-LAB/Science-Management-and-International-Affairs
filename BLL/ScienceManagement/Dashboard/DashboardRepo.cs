using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ScienceManagement.Dashboard
{
    public class DashboardRepo
    {
        readonly private ScienceAndInternationalAffairsEntities db;

        public DashboardRepo()
        {
            db = new ScienceAndInternationalAffairsEntities();
        }

        public DashboardRepo(ScienceAndInternationalAffairsEntities db)
        {
            this.db = db;
        }

        public DashboardNumber GetHomeData(int year)
        {
            try
            {
                int invention = (from a in db.Inventions
                                 join b in db.RequestInventions on a.invention_id equals b.invention_id
                                 join c in db.RequestDecisions on b.request_id equals c.request_id
                                 join d in db.Decisions on c.decision_id equals d.decision_id
                                 where b.status_id == 2 && d.valid_date.Year == year
                                 select a.invention_id).Count();
                int scopusISI = (from a in db.Papers
                                 join b in db.RequestPapers on a.paper_id equals b.paper_id
                                 join c in db.RequestDecisions on b.request_id equals c.request_id
                                 join d in db.Decisions on c.decision_id equals d.decision_id
                                 where b.status_id == 2 && d.valid_date.Year == year
                                 select a.paper_id).Count();
                int researcher = db.Profiles.Where(x => x.profile_page_active).Count();

                DashboardNumber item = new DashboardNumber();
                item.Invention = invention;
                item.ScopusISI = scopusISI;
                item.Researcher = researcher;

                return item;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        public PaperByOffice GetPaperByOffices(string[] criterias, int? year)
        {
            year = year == null ? DateTime.Now.Year : year;
            PaperByOffice paperByUnit = new PaperByOffice(criterias);
            var temp = (from a in db.InternalUnits
                        join b in db.Offices on a.unit_id equals b.unit_id
                        where a.unit_id == 1
                        select new PaperByOfficeItem
                        {
                            Office = b.office_abbreviation,
                            PaperByCriterias = (from i in db.PaperCriterias
                                                where criterias.Contains(i.name)
                                                select new PaperByCriteria
                                                {
                                                    Criteria = i.name,
                                                    Papers = (from c in db.Authors
                                                              join d in db.AuthorPapers on c.people_id equals d.people_id
                                                              join e in db.RequestPapers on d.paper_id equals e.paper_id
                                                              join f in db.RequestDecisions on e.request_id equals f.request_id
                                                              join g in db.Decisions on f.decision_id equals g.decision_id
                                                              join h in db.PaperWithCriterias on d.paper_id equals h.paper_id
                                                              join j in db.PaperCriterias on h.criteria_id equals j.criteria_id
                                                              where e.status_id == 2 && g.valid_date.Year == year
                                                              && b.office_id == c.office_id && j.name == i.name
                                                              select d.paper_id).Distinct().Count()
                                                }).Distinct().ToList()
                        }).ToList();
            int numOffice = temp.Count;
            foreach (var item in criterias)
            {
                paperByUnit.CriteriaValuePairs.Add(item, new List<int>());
                paperByUnit.Offices = new string[numOffice];
            }
            for (int i = 0; i < temp.Count; i++)
            {
                paperByUnit.Offices[i] = temp[i].Office;
                foreach (var item in temp[i].PaperByCriterias)
                {
                    paperByUnit.CriteriaValuePairs[item.Criteria].Add(item.Papers);
                }
            }
            return paperByUnit;
        }

        public Dictionary<string, int> GetPaperBySpecializations(int? year)
        {
            year = year == null ? DateTime.Now.Year : year;
            var temp = (from d in db.SpecializationLanguages
                        join e in db.RequestPapers on d.specialization_id equals e.specialization_id
                        join f in db.RequestDecisions on e.request_id equals f.request_id
                        join g in db.Decisions on f.decision_id equals g.decision_id
                        where e.status_id == 2 && g.valid_date.Year == year && d.language_id == 1
                        select new
                        {
                            d.name,
                            e.paper_id
                        }).ToList();
            Dictionary<string, int> specialiNumPaper = new Dictionary<string, int>();
            foreach (var item in temp)
            {
                if (specialiNumPaper.TryGetValue(item.name, out int papers))
                    specialiNumPaper[item.name] = papers + 1;
                else
                    specialiNumPaper.Add(item.name, 1);
            }
            return specialiNumPaper;
        }

        private class PaperByOfficeItem
        {
            public string Office { get; set; }
            public List<PaperByCriteria> PaperByCriterias { get; set; }
        }

        public class PaperByCriteria
        {
            public string Criteria { get; set; }
            public int Papers { get; set; }
        }
    }
}
