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
        public ExBasicInfo ExBasicInfo = new ExBasicInfo();
        public List<PartnerScopeInfo> PartnerScopeInfo = new List<PartnerScopeInfo>();
    }
}
