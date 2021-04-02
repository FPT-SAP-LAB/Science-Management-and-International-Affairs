using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOABasicInfo
{
    public class PartnerScopeInfoMOA
    {
        public List<int> scopes_id { get; set; }
        public int partner_id { get; set; }
        public string partner_name { get; set; }
        public string scopes_name { get; set; }
        public List<CollaborationScope> total_scopes { get; set; }
    }
}
