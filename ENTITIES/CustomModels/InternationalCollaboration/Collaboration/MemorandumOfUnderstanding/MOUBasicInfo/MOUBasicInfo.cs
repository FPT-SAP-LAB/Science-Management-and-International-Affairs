using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOUBasicInfo
{
    public class MOUBasicInfo
    {
        public MOUBasicInfo() { }
        public int mou_id { get; set; }
        public string mou_code { get; set; }
        public int? evidence { get; set; }
        public string file_name { get; set; }
        public string file_drive_id { get; set; }
        public string file_link { get; set; }
        public string scopes { get; set; }
        public string reason { get; set; }
        public DateTime mou_end_date { get; set; }
        public DateTime mou_start_date { get; set; }
        public string mou_end_date_string { get; set; }
        public string mou_start_date_string { get; set; }
        public string mou_note { get; set; }
        public string office_abbreviation { get; set; }
        public string mou_status_name { get; set; }
        public int office_id { get; set; }
        public int mou_status_id { get; set; }
    }
}
