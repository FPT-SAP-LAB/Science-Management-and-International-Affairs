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
                        join f in db.Offices on b.office_id equals f.office_id
                        join w in db.Files on b.avatar_id equals w.file_id
                        where c.language_id == 1 && b.profile_page_active == true
                        select new ResearcherList
                        {
                            peopleId = a.people_id,
                            name = a.name,
                            email = a.email,
                            title = c.name,
                            website = b.website,
                            positions = ((from m in db.People
                                          from n in db.Positions.Where(x => m.Profile.Positions.Contains(x))
                                          join h in db.PositionLanguages on n.position_id equals h.position_id
                                          where h.language_id == 1 && m.people_id == a.people_id
                                          select h.name
                            ).ToList<String>()),
                            avatar = w.link,
                            workplace = f.office_name,
                            googleScholar = b.google_scholar
                        });
            var res = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
            .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
            int recordsTotal = data.Count();
            return new BaseServerSideData<ResearcherList>(res, recordsTotal);
        }
    }
}
