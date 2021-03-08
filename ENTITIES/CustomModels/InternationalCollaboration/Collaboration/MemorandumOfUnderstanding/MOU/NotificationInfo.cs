using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU
{
    public class NotificationInfo
    {
        public NotificationInfo() { }
        public int InactiveNumber { get; set; }
        public List<string> ExpiredMOUCode { get; set; }
    }
}
