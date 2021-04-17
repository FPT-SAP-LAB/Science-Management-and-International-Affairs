using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Paper
{
    public class CustomPaperPolicy
    {
        public int policy_id { get; set; }
        public int policy_criteria_id { get; set; }
        public string tv { get; set; }
        public string ta { get; set; }
    }
}
