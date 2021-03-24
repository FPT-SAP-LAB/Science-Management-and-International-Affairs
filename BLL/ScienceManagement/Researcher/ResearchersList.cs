using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
namespace BLL.ScienceManagement
{
    class ResearchersList
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ResearcherList> GetList()
        {
            var data = (from a in db.People
                        join b in db.Profiles on a.people_id equals b.people_id
                        join c in db.AcademicDegreeLanguages on b.current_academic_degree_id equals c.academic_degree_id
                        join d in db.Offices on b.office_id equals d.office_id
                        join e in db.PositionLanguages on a.Profile.Positions.Select(x=>x.position_id).Contains(e.position_id)
                        where c.language_id == 1 
                        select new ResearcherList
                        {
                            peopleId=a.people_id,
                            name=a.name,
                            title=c.name,

                        })
            return null;
        }
    }
}
