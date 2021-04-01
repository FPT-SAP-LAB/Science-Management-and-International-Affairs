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
        public void AddWithTempData(ScienceAndInternationalAffairsEntities db, ConferenceParticipant participant)
        {
            db.ConferenceParticipants.Add(participant);
        }
    }
}
