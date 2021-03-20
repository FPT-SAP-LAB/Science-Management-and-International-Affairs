using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU
{
    public class ListMOU
    {
        public ListMOU() { }
        public int RowNumber { get; set; }
        public string mou_code { get; set; }
        public int mou_partner_id { get; set; }
        public int mou_id { get; set; }
        public string partner_name { get; set; }
        public string website { get; set; }
        public string country_name { get; set; }
        public string contact_point_name { get; set; }
        public string contact_point_email { get; set; }
        public string contact_point_phone { get; set; }
        public string evidence { get; set; }
        public DateTime mou_start_date { get; set; }
        public DateTime mou_end_date { get; set; }
        public string mou_start_date_string { get; set; }
        public string mou_end_date_string { get; set; }
        public string mou_note { get; set; }
        public string office_abbreviation { get; set; }
        public string scope_abbreviation { get; set; }
        public string specialization_name { get; set; }
        public string mou_status_name { get; set; }
        public int mou_status_id { get; set; }
    }
}
