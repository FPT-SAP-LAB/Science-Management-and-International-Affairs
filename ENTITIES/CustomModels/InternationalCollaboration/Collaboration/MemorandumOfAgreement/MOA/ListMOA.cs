using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA
{
    public class ListMOA
    {
        public ListMOA() { }
        public string moa_code { get; set; }
        public int moa_partner_id { get; set; }
        public int moa_id { get; set; }
        public string partner_name { get; set; }
        public string evidence { get; set; }
        public DateTime moa_start_date { get; set; }
        public DateTime moa_end_date { get; set; }
        public string moa_start_date_string { get; set; }
        public string moa_end_date_string { get; set; }
        public string office_name { get; set; }
        public string scope_abbreviation { get; set; }
        public string mou_status_name { get; set; }
        public int mou_status_id { get; set; }
    }
}
