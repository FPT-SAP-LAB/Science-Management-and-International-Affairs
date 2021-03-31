using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;

namespace BLL.ModelDAL
{
    public class ConferenceParticipantRepo
    {
        public void AddWithTempData(ScienceAndInternationalAffairsEntities db, ConferenceParticipant participant, Person person)
        {
            if (db == null)
                db = new ScienceAndInternationalAffairsEntities();
            Profile profile = db.Profiles.Where(x => x.mssv_msnv == participant.current_mssv_msnv).FirstOrDefault();
            if (profile == null)
            {
                db.People.Add(person);
                db.SaveChanges();

                profile = new Profile()
                {
                    mssv_msnv = participant.current_mssv_msnv,
                    title_id = participant.title_id,
                    people_id = person.people_id,
                };
                db.Profiles.Add(profile);
            }
            else
            {
                participant.people_id = profile.people_id;
                participant.title_id = profile.title_id;
                participant.office_id = profile.Person.office_id.Value;
            }
            db.ConferenceParticipants.Add(participant);
        }
    }
}
