using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOAPartner
{
    public class ListMOAPartner
    {
        public ListMOAPartner() { }
        public int moa_partner_id { get; set; }
        public int moa_id { get; set; }
        public string moa_code { get; set; }
        public string partner_name { get; set; }
        public string country_name { get; set; }
        public int partner_id { get; set; }
        public string specialization_name { get; set; }
        public string specialization_abbreviation { get; set; }
        public string website { get; set; }
        public string contact_point_name { get; set; }
        public string contact_point_phone { get; set; }
        public string contact_point_email { get; set; }
        public string moa_start_date_string { get; set; }
        public DateTime moa_start_date { get; set; }
        public string scope_abbreviation { get; set; }
    }
}
