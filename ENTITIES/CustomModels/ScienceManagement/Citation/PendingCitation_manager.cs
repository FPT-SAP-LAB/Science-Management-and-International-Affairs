using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Citation
{
    public class PendingCitation_manager
    {
        public string email { get; set; }
        public DateTime created_date { get; set; }
        public int request_id { get; set; }
        public int citation_status_id { get; set; }
    }
}
