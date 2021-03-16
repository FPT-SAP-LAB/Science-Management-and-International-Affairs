using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOABasicInfo
{
    public class ExMOAAdd
    {
        public ExMOAAdd() { }
        public ExMOABasicInfo ExMOABasicInfo { get; set; }
        public List<PartnerScopeInfoMOA> PartnerScopeInfoMOA { get; set; }
    }
}
