using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Invention
{
    public class DetailInvention : ENTITIES.Invention
    {
        public string type_name { get; set; }
        public string reward_type { get; set; }
        public string total_reward { get; set; }
        public int request_id { get; set; }
        public string link_file { get; set; }
    }
}
