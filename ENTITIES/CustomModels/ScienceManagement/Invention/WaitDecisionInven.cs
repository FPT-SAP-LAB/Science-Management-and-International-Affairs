using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Invention
{
    public class WaitDecisionInven
    {
        public string name { get; set; }
        public string type_name { get; set; }
        public string author_name { get; set; }
        public string mssv_msnv { get; set; }
        public string office_abbreviation { get; set; }
        public int note { get; set; }
        public int request_id { get; set; }
        public int invention_id { get; set; }
    }
}
