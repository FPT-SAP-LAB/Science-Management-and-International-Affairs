using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
namespace BLL.ScienceManagement.ResearcherListRepo
{
    public class ResearchersListRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public BaseServerSideData<ResearcherList> GetList(BaseDatatable baseDatatable, int account_id = 0, int language_id = 1)
        {
            var data = (from a in db.People
                        join b in db.Profiles on a.people_id equals b.people_id
                        join c in db.AcademicDegreeLanguages on b.current_academic_degree_id equals c.academic_degree_id
                        from d in db.Positions.Where(x => a.Profile.Positions.Contains(x))
                        join e in db.PositionLanguages on d.position_id equals e.position_id
                        join f in db.Offices on b.office_id equals f.office_id
                        from g in db.ResearchAreas.Where(x => x.Profiles.Contains(b))
                        join h in db.ResearchAreaLanguages on g.research_area_id equals h.research_area_id
                        where c.language_id == 1 && e.language_id == 1
                        select new ResearcherList
                        {
                            peopleId = a.people_id,
                            name = a.name,
                            title = c.name,
                            position = e.name,
                            workplace = f.office_name,
                            interest = h.name,
                            googleScholar = b.google_scholar
                        });
            var res=data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
            .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
            int recordsTotal = data.Count();
            return new BaseServerSideData<ResearcherList>(res, recordsTotal);
        }
    }
}
