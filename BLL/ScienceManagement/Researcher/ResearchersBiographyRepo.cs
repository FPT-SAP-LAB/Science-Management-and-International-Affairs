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
        public List<ResearcherPublications> GetPublications(int id)
        {
            var data = (from a in db.Papers
                        join b in db.AuthorPapers on a.paper_id equals b.paper_id
                        where b.people_id == id
                        select new ResearcherPublications
                        {
                            paper_id = a.paper_id,
                            journal_or_cfr_name = b.Paper.journal_name,
                            paper_name = a.name,
                            publish_date = a.publish_date,
                            co_author =
                            (from m in db.Profiles
                             join n in db.AuthorPapers on m.people_id equals n.people_id
                             where n.paper_id == a.paper_id && m.people_id != id
                             select m.Person.name).ToList<string>()
                        }).OrderByDescending(x => x.publish_date).AsEnumerable<ResearcherPublications>().Select((x, index) => new ResearcherPublications
                        {
                            rownum = index + 1,
                            paper_id = x.paper_id,
                            journal_or_cfr_name = x.journal_or_cfr_name,
                            paper_name = x.paper_name,
                            publish_date = x.publish_date,
                            co_author = x.co_author
                        }).ToList<ResearcherPublications>();
            return data;
        }
        public List<ResearcherPublications> GetConferencePublic(int id)
        {
            var data = (from a in db.Papers
                        join b in db.RequestConferences on a.paper_id equals b.paper_id
                        join c in db.Conferences on b.conference_id equals c.conference_id
                        join d in db.AuthorPapers on a.paper_id equals d.paper_id
                        where d.people_id == id
                        select new ResearcherPublications
                        {
                            paper_id = a.paper_id,
                            journal_or_cfr_name = c.conference_name,
                            paper_name = a.name,
                            publish_date = a.publish_date,
                            co_author =
                            (from m in db.Profiles
                             join n in db.AuthorPapers on m.people_id equals n.people_id
                             where n.paper_id == a.paper_id && m.people_id != id
                             select m.Person.name).ToList<string>()
                        }).OrderByDescending(x => x.publish_date).AsEnumerable<ResearcherPublications>().Select((x, index) => new ResearcherPublications
                        {
                            rownum = index + 1,
                            paper_id = x.paper_id,
                            journal_or_cfr_name = x.journal_or_cfr_name,
                            paper_name = x.paper_name,
                            publish_date = x.publish_date,
                            co_author = x.co_author
                        }).ToList<ResearcherPublications>();
            return data;
        }
    }
}
