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
        public int paperNumber { get; set; }
        public int people_id { get; set; }
    }
}
