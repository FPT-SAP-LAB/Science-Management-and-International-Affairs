using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    public class ResearcherCandidate
    {
        public int rowNum { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public int title_id { get; set; }
        public int paperNumber { get; set; }
        public int office_id { get; set; }
        public string office_name { get; set; }
        public int people_id { get; set; }
    }
}
