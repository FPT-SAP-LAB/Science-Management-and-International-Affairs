using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using static ENTITIES.CustomModels.ScienceManagement.Report.TotalBonusByYearItem;

namespace BLL.ScienceManagement.Report
{
    public class ReportRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public ReportRepo()
        {
            db = new ScienceAndInternationalAffairsEntities();
        }

        public ReportRepo(ScienceAndInternationalAffairsEntities db)
        {
            this.db = db;
        }

        public Dictionary<int, TotalBonusByYearItem> GetBonusByYearItems(int? year)
        {
            year = year == null ? DateTime.Now.Year : year;
            try
            {
                var temp = (from a in db.Decisions
                            join b in db.RequestDecisions on a.decision_id equals b.decision_id
                            join c in db.BaseRequests on b.request_id equals c.request_id
                            where a.valid_date.Year == year
                            select new TotalBonusByYearItem
                            {
                                Month = a.valid_date.Month,
                                PaperRewards = (from d in db.RequestPapers
                                                where d.status_id == 2 && d.request_id == c.request_id
                                                select new PaperReward
                                                {
                                                    Vietnam = (from e in db.AuthorPapers
                                                               join f in db.Authors on e.people_id equals f.people_id
                                                               where d.type == 1 && f.is_reseacher == false
                                                               && e.money_reward != null && e.paper_id == d.paper_id
                                                               select e.money_reward.Value).ToList(),
                                                    International = (from e in db.AuthorPapers
                                                                     join f in db.Authors on e.people_id equals f.people_id
                                                                     where d.type == 2 && f.is_reseacher == false
                                                                     && e.money_reward != null && e.paper_id == d.paper_id
                                                                     select e.money_reward.Value).ToList(),
                                                    FromResearchers = (from e in db.AuthorPapers
                                                                       join f in db.Authors on e.people_id equals f.people_id
                                                                       where f.is_reseacher == true && e.money_reward != null
                                                                       && e.paper_id == d.paper_id
                                                                       select e.money_reward.Value).ToList(),
                                                }).FirstOrDefault(),
                                CitationRewards = (from d in db.RequestCitations
                                                   where d.citation_status_id == 2 && d.total_reward != null && d.request_id == c.request_id
                                                   select d.total_reward.Value).ToList(),
                                InventionRewards = (from d in db.RequestInventions
                                                    where d.status_id == 2 && d.total_reward != null && d.request_id == c.request_id
                                                    select d.total_reward.Value).ToList(),
                            }).ToList();
                Dictionary<int, TotalBonusByYearItem> monthRewardsPairs = new Dictionary<int, TotalBonusByYearItem>();
                foreach (var item in temp)
                {
                    if (monthRewardsPairs.TryGetValue(item.Month, out TotalBonusByYearItem value))
                    {
                        value.CitationRewards.AddRange(item.CitationRewards);
                        value.InventionRewards.AddRange(item.InventionRewards);
                        if (value.PaperRewards != null && item.PaperRewards != null)
                        {
                            value.PaperRewards.Vietnam.AddRange(item.PaperRewards.Vietnam);
                            value.PaperRewards.International.AddRange(item.PaperRewards.International);
                            value.PaperRewards.FromResearchers.AddRange(item.PaperRewards.FromResearchers);
                        }
                        else if (value.PaperRewards == null)
                        {
                            value.PaperRewards = item.PaperRewards;
                        }
                    }
                    else
                    {
                        monthRewardsPairs.Add(item.Month, item);
                    }
                }
                return monthRewardsPairs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return null;
        }
    }
}
