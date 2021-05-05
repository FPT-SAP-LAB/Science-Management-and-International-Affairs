using ENTITIES;
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
                                                }).ToList()
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

        //public 

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
