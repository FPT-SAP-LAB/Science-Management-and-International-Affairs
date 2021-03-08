using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU
{
    public class MOUAdd
    {
        public MOUAdd() { }
        public BasicInfo BasicInfo { get; set; }
        public List<PartnerInfo> PartnerInfo { get; set; }
    }
}
