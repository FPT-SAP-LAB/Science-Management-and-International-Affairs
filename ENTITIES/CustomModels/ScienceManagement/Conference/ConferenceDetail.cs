using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Conference
{
    public class ConferenceDetail
    {
        public string ConferenceName { get; set; }
        public string Website { get; set; }
        public string KeynoteSpeaker { get; set; }
        public string QsUniversity { get; set; }
        public string Co_organizedUnit { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime AttendanceStart { get; set; }
        public DateTime AttendanceEnd { get; set; }
        public int ConferenceID { get; set; }
        public bool EditAble { get; set; }
        public string InvitationLink { get; set; }
        public string PaperLink { get; set; }
        public string PaperName { get; set; }
        public int RequestID { get; set; }
        public string CountryName { get; set; }
        public string StatusName { get; set; }
        public string FormalityName { get; set; }
    }
}
