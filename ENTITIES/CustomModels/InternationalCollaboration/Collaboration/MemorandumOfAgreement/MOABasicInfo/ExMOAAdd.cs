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
        public int moa_bonus_id { get; set; }
        public string file_drive_id { get; set; }
        public string file_name { get; set; }
        public ExMOABasicInfo ExMOABasicInfo { get; set; }
        public List<PartnerScopeInfoMOA> PartnerScopeInfoMOA { get; set; }
    }
}
