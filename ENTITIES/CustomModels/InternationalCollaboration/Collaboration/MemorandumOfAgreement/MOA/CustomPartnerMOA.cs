using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA
{
    public class CustomPartnerMOA
    {
        public string website { get; set; }
        public string country_name { get; set; }
        public int partner_id { get; set; }
        public string partner_name { get; set; }
        public string address { get; set; }
        public string contact_point_name { get; set; }
        public string contact_point_phone { get; set; }
        public string contact_point_email { get; set; }
        public string specialization_name { get; set; }
        public int moa_partner_id { get; set; }
        public List<int> scopes { get; set; }
        public DateTime moa_start_date { get; set; }
        public string moa_start_date_string { get; set; }
    }
}
