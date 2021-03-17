using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA
{
    public class MOAPartnerInfo
    {
        public string partnername_add { get; set; }
        public string sign_date_moa_add { get; set; }
        public List<int> coop_scope_add { get; set; }
        public int partner_id { get; set; }
    }
}
