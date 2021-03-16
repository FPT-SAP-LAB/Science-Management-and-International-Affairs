using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOABasicInfo
{
    public class MOABasicInfo
    {

        public MOABasicInfo() { }
        public int moa_id { get; set; }
        public string moa_code { get; set; }
        public string evidence { get; set; }
        public string scope_abbreviation { get; set; }
        public string reason { get; set; }
        public DateTime moa_end_date { get; set; }
        public DateTime moa_start_date { get; set; }
        public string moa_end_date_string { get; set; }
        public string moa_start_date_string { get; set; }
        public string office_abbreviation { get; set; }
        public string mou_status_name { get; set; }
        public int office_id { get; set; }
        public int moa_status_id { get; set; }
        public string moa_note { get; set; }

    }
}
