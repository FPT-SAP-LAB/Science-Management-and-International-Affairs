using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Paper
{
    public class DetailPaper : ENTITIES.Paper
    {
        public string type { get; set; }
        public string reward_type { get; set; }
        public string total_reward { get; set; }
    }
}
