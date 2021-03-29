using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Paper
{
    public class WaitDecisionPaper
    {
        public string name { get; set; }
        public string company { get; set; }
        public string author_name { get; set; }
        public string mssv_msnv { get; set; }
        public string office_abbreviation { get; set; }
        public int note { get; set; }
        public int request_id { get; set; }
    }
}
