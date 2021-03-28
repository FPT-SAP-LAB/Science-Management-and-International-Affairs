using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
namespace BLL.ScienceManagement.Researcher
{
    public class ResearchersBiographyRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<AcadBiography> GetBio(int id)
        {
            var profile = (
                from a in db.Profiles
                join b in db.ProfileAcademicDegrees on a.people_id equals b.people_id
                join c in db.AcademicDegrees on b.academic_degree_id equals c.academic_degree_id
                join d in db.AcademicDegreeLanguages on c.academic_degree_id equals d.academic_degree_id
                where d.language_id == 1 && a.people_id == id
                select new AcadBiography
                {
                    people_id = a.people_id,
                    acad_id = c.academic_degree_id,
                    degree = d.name,
                    time = b.start_year.ToString() + "-" + b.end_year.ToString(),
                    place = b.study_place
                }).AsEnumerable<AcadBiography>().Select((x, index) => new AcadBiography
                {
                    rownum = index + 1,
                    people_id = x.people_id,
                    acad_id = x.acad_id,
                    degree = x.degree,
                    time = x.time,
                    place = x.place
                }).ToList<AcadBiography>();
            return profile;
        }
        public List<BaseRecord<WorkingProcess>> GetHistory(int id)
        {
            var list = (from a in db.WorkingProcesses
                        where a.Profile.people_id == id
                        select new BaseRecord<WorkingProcess>
                        {
                            records = a
                        }).AsEnumerable<BaseRecord<WorkingProcess>>()
                        .Select((x, index) => new BaseRecord<WorkingProcess>
                        {
                            index = index + 1,
                            records = x.records
                        }).ToList<BaseRecord<WorkingProcess>>();
            return list;
        }
    }
}
