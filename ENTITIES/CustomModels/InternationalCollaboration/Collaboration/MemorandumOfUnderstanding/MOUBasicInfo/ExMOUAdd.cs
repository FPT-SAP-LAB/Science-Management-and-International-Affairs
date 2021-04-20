using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOUBasicInfo
{
    public class ExMOUAdd
    {
        public ExMOUAdd() { }
        public int mou_bonus_id { get; set; }
        public string file_drive_id { get; set; }
        public string file_name { get; set; }
        public ExBasicInfo ExBasicInfo { get; set; }
        public List<PartnerScopeInfo> PartnerScopeInfo { get; set; }
    }
}
