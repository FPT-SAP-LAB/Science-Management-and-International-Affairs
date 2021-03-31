using ENTITIES;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Researcher
{
    public class EditResearcherInfoRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public int EditResearcherProfile(string data)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    var editInfo = JObject.Parse(data);
                    int id = (int)editInfo["info"]["people_id"];
                    Person person = db.People.Find(id);
                    person.name = (string)editInfo["info"]["name"];
                    /////////////////////////////////////////////////
                    Profile profile = db.Profiles.Find(id);
                    int nationality = (int)editInfo["info"]["nationality"][0]["id"];
                    profile.country_id = nationality;
                    db.SaveChanges();
                    ///////////////////////////////////////////////////////
                    profile.title_id = (int)editInfo["info"]["title"][0]["id"];
                    db.SaveChanges();
                    ///////////////////////////////////////////////////////
                    List<int> position_ids = new List<int>();
                    foreach (var i in editInfo["info"]["position"])
                    {
                        position_ids.Add((int)i["id"]);
                    }
                    List<Position> positions = db.Positions.Where(x => position_ids.Contains(x.position_id)).ToList();
                    List<PeoplePosition> currentPositions = db.PeoplePositions.Where(x => x.people_id == id).ToList();
                    foreach (PeoplePosition p in currentPositions)
                    {
                        db.PeoplePositions.Remove(p);
                    }
                    db.SaveChanges();
                    foreach (Position p in positions)
                    {
                        profile.PeoplePositions.Add(new PeoplePosition()
                        {
                            people_id = id,
                            position_id = p.position_id,
                            Position = p,
                            Profile = profile
                        });
                    }
                    db.SaveChanges();
                    //////////////////////////////////////////////////////
                    string birthdate = (string)editInfo["info"]["dob"];
                    string phone = (string)editInfo["info"]["phone"];
                    string email = (string)editInfo["info"]["email"];
                    string website = (string)editInfo["info"]["website"];
                    string googlescholar = (string)editInfo["info"]["googlescholar"];
                    string cv = (string)editInfo["info"]["cv"];
                    profile.birth_date = DateTime.ParseExact(birthdate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    person.phone_number = phone;
                    profile.website = website;
                    profile.cv = cv;
                    person.email = email;
                    profile.google_scholar = googlescholar;
                    db.SaveChanges();
                    ///////////////////////////////////////////////////////
                    List<int> field_ids = new List<int>();
                    foreach (var i in editInfo["info"]["fields"])
                    {
                        field_ids.Add((int)i["id"]);
                    }
                    List<ResearchArea> areas = db.ResearchAreas.Where(x => field_ids.Contains(x.research_area_id)).ToList();
                    profile.ResearchAreas.Clear();
                    foreach (ResearchArea p in areas)
                    {
                        profile.ResearchAreas.Add(p);
                    }
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

        public int EditResearcherProfilePicture(Google.Apis.Drive.v3.Data.File file, int people_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    Profile profile = db.Profiles.Find(people_id);
                    File avt = new File()
                    {
                        name = "avatar-" + people_id,
                        file_drive_id = file.Id,
                        link = "https://drive.google.com/uc?export=view&id=" + file.Id
                    };
                    db.Files.Add(avt);
                    db.SaveChanges();
                    profile.avatar_id = avt.file_id;
                    db.SaveChanges();
                    trans.Commit();
                    return 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return 0;
                }
            }
        }
    }
}
