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
        public Nullable<int> total_reward { get; set; }
        public int? specialization_id { get; set; }
        public int request_id { get; set; }
        public int status_id { get; set; }
        public string date_string { get; set; }
        public string link_file { get; set; }
    }
}
