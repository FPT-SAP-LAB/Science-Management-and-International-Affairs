using ENTITIES;

namespace BLL.ModelDAL
{
    public class ConferenceParticipantRepo
    {
        public void AddWithTempData(ScienceAndInternationalAffairsEntities db, ConferenceParticipant participant)
        {
            db.ConferenceParticipants.Add(participant);
        }
    }
}
