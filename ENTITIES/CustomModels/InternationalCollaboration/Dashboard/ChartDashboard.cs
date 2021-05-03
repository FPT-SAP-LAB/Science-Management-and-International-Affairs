using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Dashboard
{
    public class ChartDashboard
    {
        public long year { get; set; }
        public int? signed { get; set; }
        public int? not_sign_yet { get; set; }
        public int? total { get; set; }
    }
}
