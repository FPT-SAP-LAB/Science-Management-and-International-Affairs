using System;

namespace ENTITIES.CustomModels.ScienceManagement.Conference
{
    public class ConferenceDetail
    {
        public string ConferenceName { get; set; }
        public string Website { get; set; }
        public string KeynoteSpeaker { get; set; }
        public string QsUniversity { get; set; }
        public string OrganizedUnit { get; set; }
        public string Co_organizedUnit { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? FinishedDate { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime AttendanceStart { get; set; }
        public DateTime AttendanceEnd { get; set; }
        public int ConferenceID { get; set; }
        public bool EditAble { get; set; }
        public string InvitationLink { get; set; }
        public string InvitationFileName { get; set; }
        public string PaperLink { get; set; }
        public string PaperFileName { get; set; }
        public string PaperName { get; set; }
        public string FolderLink { get; set; }
        public int RequestID { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string StatusName { get; set; }
        public int StatusID { get; set; }
        public int FormalityID { get; set; }
        public string FormalityName { get; set; }
        public int Reimbursement { get; set; }
        public int SpecializationID { get; set; }
        public string SpecializationName { get; set; }
        public int AccountID { get; set; }
    }
}
