using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Paper
{
    public class PendingPaper_Manager
    {
        public string name { get; set; }
        public string email { get; set; }
        public DateTime created_date { get; set; }
        public int paper_id { get; set; }
    }
}
