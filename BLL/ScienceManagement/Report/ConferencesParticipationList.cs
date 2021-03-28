using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Report;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Report
{
    public class ConferencesParticipationList
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public BaseServerSideData<ConferencesParticipationReport> ListConferences(BaseDatatable baseDatatable)
        {
            var data = (from d in db.Decisions
                        join rd in db.RequestDecisions on d.decision_id equals rd.decision_id
                        join br in db.BaseRequests on rd.request_id equals br.request_id
                        join rc in db.RequestConferences on br.request_id equals rc.request_id
                        join cp in db.ConferenceParticipants on rc.request_id equals cp.request_id
                        join p in db.Profiles on cp.people_id equals p.people_id
                        join pe in db.People on p.people_id equals pe.people_id
                        join t in db.Titles on cp.title_id equals t.title_id
                        join tl in db.TitleLanguages on t.title_id equals tl.title_id
                        join o in db.Offices on cp.office_id equals o.office_id
                        join cf in db.Conferences on rc.conference_id equals cf.conference_id
                        join c in db.Countries on cf.country_id equals c.country_id
                        join b in (from cost in db.Costs
                                   join rs in db.RequestConferences
            on cost.request_id equals rs.request_id
                                   group cost by rs.request_id into g
                                   select new
                                   {
                                       total = g.Sum(x => x.total),
                                       request_id = g.Key

                                   }) on rc.request_id equals b.request_id
                        where tl.language_id == 1
                        select new ConferencesParticipationReport
                        {
                            valid_date = d.valid_date,
                            decision_number = d.decision_number,
                            people_name = pe.name,
                            title_name = tl.name,
                            office_name = o.office_name,
                            country_name = c.country_name,
                            conference_name = cf.conference_name,
                            attendance_start = rc.attendance_start,
                            total = b.total
                        }).OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
         .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
            data.ForEach(x => x.money_total = x.total.ToString("N0"));
            int recordsTotal = (from d in db.Decisions
                                join rd in db.RequestDecisions on d.decision_id equals rd.decision_id
                                join br in db.BaseRequests on rd.request_id equals br.request_id
                                join rc in db.RequestConferences on br.request_id equals rc.request_id
                                join cp in db.ConferenceParticipants on rc.request_id equals cp.request_id
                                join p in db.Profiles on cp.people_id equals p.people_id
                                join pe in db.People on p.people_id equals pe.people_id
                                join t in db.Titles on cp.title_id equals t.title_id
                                join tl in db.TitleLanguages on t.title_id equals tl.title_id
                                join o in db.Offices on cp.office_id equals o.office_id
                                join cf in db.Conferences on rc.conference_id equals cf.conference_id
                                join c in db.Countries on cf.country_id equals c.country_id
                                join b in (from cost in db.Costs
                                           join rs in db.RequestConferences
                    on cost.request_id equals rs.request_id
                                           group cost by rs.request_id into g
                                           select new
                                           {
                                               total = g.Sum(x => x.total),
                                               request_id = g.Key
                                           }) on rc.request_id equals b.request_id
                                where tl.language_id == 1
                                select new ConferencesParticipationReport
                                {
                                    valid_date = d.valid_date,
                                    decision_number = d.decision_number,
                                    people_name = pe.name,
                                    title_name = tl.name,
                                    office_name = o.office_name,
                                    country_name = c.country_name,
                                    conference_name = cf.conference_name,
                                    attendance_start = rc.attendance_start,
                                    total = b.total
                                }).Count();
            return new BaseServerSideData<ConferencesParticipationReport>(data, recordsTotal);
        }
    }
}
