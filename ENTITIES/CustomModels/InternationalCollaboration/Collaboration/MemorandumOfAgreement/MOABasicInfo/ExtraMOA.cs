using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOABasicInfo
{
    public class ExtraMOA
    {
        public int moa_bonus_id { get; set; }
        public string moa_bonus_code { get; set; }
        public string moa_bonus_decision_date_string { get; set; }
        public string moa_bonus_end_date_string { get; set; }
        public DateTime moa_bonus_end_date { get; set; }
        public DateTime moa_bonus_decision_date { get; set; }
        public string partner_name { get; set; }
        public string scope_abbreviation { get; set; }
        public string evidence { get; set; }
        public string link { get; set; }
        public string file_name { get; set; }
        public string file_drive_id { get; set; }
        public int moa_id { get; set; }
        public int partner_id { get; set; }
        public int scope_id { get; set; }
        public List<CustomPartner> ListPartnerExMOA { get; set; }
    }
}
