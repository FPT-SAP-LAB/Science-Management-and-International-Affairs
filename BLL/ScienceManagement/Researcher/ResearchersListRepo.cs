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
        public BaseServerSideData<ResearcherList> GetList(BaseDatatable baseDatatable, string coso, string name)
        {
            var data = (from a in db.People
                        join b in db.Profiles on a.people_id equals b.people_id
                        join f in db.Offices.DefaultIfEmpty() on a.office_id equals f.office_id
                        where b.profile_page_active == true
                        select new ResearcherList
                        {
                            peopleId = a.people_id,
                            name = a.name,
                            email = a.email,
                            title = (
                            from c in db.AcademicDegreeLanguages.DefaultIfEmpty()
                            where b.current_academic_degree_id == c.academic_degree_id
                            select c.name
                            ).FirstOrDefault(),
                            website = b.website,
                            positions = ((from m in db.People
                                          join n in db.PeoplePositions on m.people_id equals n.people_id
                                          join h in db.PositionLanguages on n.position_id equals h.position_id
                                          where h.language_id == 1 && m.people_id == a.people_id
                                          select h.name
                            ).ToList<String>()),
                            avatar = (
                                    from f in db.Profiles
                                    join ff in db.Files on f.avatar_id equals ff.file_id
                                    where f.people_id == a.people_id
                                    select ff.link
                                      ).FirstOrDefault(),
                            office_id = f.office_id,
                            office_name = f.office_name,
                            googleScholar = b.google_scholar
                        });
            List<ResearcherList> result = null;
            if (coso.Trim() != "")
            {
                int cosoint = Int32.Parse(coso);
                data = data.Where(x => x.office_id == cosoint);
            }
            if (name.Trim() != "")
            {
                data = data.Where(x => x.name.Contains(name));
            }

            result = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
                .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = data.Count();
            return new BaseServerSideData<ResearcherList>(result, recordsTotal);
        }
    }
}
