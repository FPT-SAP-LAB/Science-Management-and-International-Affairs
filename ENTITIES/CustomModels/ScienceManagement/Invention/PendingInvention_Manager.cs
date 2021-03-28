using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Invention
{
    public class PendingInvention_Manager
    {
        public string name { get; set; }
        public string email { get; set; }
        public DateTime created_date { get; set; }
        public int invention_id { get; set; }
    }
}
