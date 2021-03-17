using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA
{
    public class MOAAdd
    {
        public MOAAdd() { }
        public MOABasicInfo MOABasicInfo { get; set; }
        public List<MOAPartnerInfo> MOAPartnerInfo { get; set; }
    }
}
