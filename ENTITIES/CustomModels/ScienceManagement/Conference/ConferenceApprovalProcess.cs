using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Conference
{
    public class ConferenceApprovalProcess
    {
        public DateTime CreatedDate { get; set; }
        public string PositionName { get; set; }
        public string FullName { get; set; }
        public string Comment { get; set; }
    }
}
