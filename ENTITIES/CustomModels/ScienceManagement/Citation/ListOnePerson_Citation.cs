using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement
{
    public class ListOnePerson_Citation
    {
        public string source { get; set; }
        public int count { get; set; }
        public DateTime created_date { get; set; }
        public int status_id { get; set; }
        public int request_id { get; set; }
        public string note { get; set; }
        public List<string> TypeNames { get; set; }
    }
}
