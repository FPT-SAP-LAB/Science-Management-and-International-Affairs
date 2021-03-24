using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU
{
    public class PartnerInfo
    {
        public string partnername_add { get; set; }
        public string represent_add { get; set; }
        public List<int> specialization_add { get; set; }
        public string nation_add { get; set; }
        public string website_add { get; set; }
        public string address_add { get; set; }
        public string email_add { get; set; }
        public string sign_date_mou_add { get; set; }
        public string phone_add { get; set; }
        public List<int> coop_scope_add { get; set; }
        public int partner_id { get; set; }
        public int mou_partner_id { get; set; }
    }
}
