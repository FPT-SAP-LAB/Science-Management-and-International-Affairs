using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using Newtonsoft.Json.Linq;

namespace BLL.ScienceManagement.Researcher
{
    public class ResearchersBiographyRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<AcadBiography> GetAcadHistory(int id)
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
        public List<BaseRecord<WorkingProcess>> GetWorkHistory(int id)
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
        public List<BaseRecord<Award>> GetAwards(int id)
        {
            var list = (from a in db.Awards
                        where a.people_id == id
                        select new BaseRecord<Award>
                        {
                            records = a
                        }).OrderByDescending(x => x.records.award_time).AsEnumerable<BaseRecord<Award>>()
                        .Select((x, index) => new BaseRecord<Award>
                        {
                            index = index + 1,
                            records = x.records
                        }).ToList<BaseRecord<Award>>();
            return list;
        }

        public List<SelectField> getAcadDegrees()
        {
            var data = (from c in db.AcademicDegrees
                        join d in db.AcademicDegreeLanguages on c.academic_degree_id equals d.academic_degree_id
                        where d.language_id == 1
                        select new SelectField
                        {
                            id = c.academic_degree_id,
                            name = d.name,
                            selected = 0
                        }).ToList();
            return data;
        }

        public int AddNewAcadEvent(string data)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    var info = JObject.Parse(data);
                    int people_id = (int)info["data"]["people_id"];
                    int degree = (int)info["data"]["degree"];
                    string location = (string)info["data"]["location"];
                    int start = (int)info["data"]["start"];
                    int end = (int)info["data"]["end"];
                    Profile profile = db.Profiles.Find(people_id);
                    db.ProfileAcademicDegrees.Add(new ProfileAcademicDegree
                    {
                        people_id = people_id,
                        academic_degree_id = degree,
                        start_year = start,
                        end_year = end,
                        study_place = location,
                        Profile = db.Profiles.Find(people_id),
                        AcademicDegree = db.AcademicDegrees.Find(degree)
                    });
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                    return 0;
                }
            }
            return 1;
        }

        public List<SelectField> getTitles()
        {
            var data = (from a in db.Titles
                        join b in db.TitleLanguages on a.title_id equals b.title_id
                        where b.language_id == 1
                        select new SelectField
                        {
                            id = a.title_id,
                            name = b.name,
                            selected = 0
                        }).ToList();
            return data;
        }
        public int AddNewWorkEvent(string data)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    var info = JObject.Parse(data);
                    int people_id = (int)info["data"]["people_id"];
                    string title = (string)info["data"]["title"];
                    string location = (string)info["data"]["location"];
                    int start = (int)info["data"]["start"];
                    int end = (int)info["data"]["end"];
                    db.WorkingProcesses.Add(new WorkingProcess
                    {
                        pepple_id = people_id,
                        work_unit = location,
                        start_year = start,
                        end_year = end,
                        title = title,
                        Profile = db.Profiles.Find(people_id)
                    });
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                    return 0;
                }
                return 1;
            }
        }
    }
}
