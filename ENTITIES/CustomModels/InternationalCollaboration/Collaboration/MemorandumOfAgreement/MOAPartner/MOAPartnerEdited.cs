using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOAPartner
{
    public class MOAPartnerEdited
    {
        public List<int> scopes { get; set; }
        public int partner_id { get; set; }
        public string sign_date_string { get; set; }
        public int moa_partner_id { get; set; }
    }
}
