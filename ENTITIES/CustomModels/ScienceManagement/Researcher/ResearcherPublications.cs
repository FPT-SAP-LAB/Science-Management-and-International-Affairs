using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    public class ResearcherPublications
    {
        public int rownum { get; set; }
        public string journal_or_cfr_name { get; set; }
        public DateTime? publish_date { get; set; }
        public string paper_name { get; set; }
        public int paper_id { get; set; }
        public List<string> co_author { get; set; }
        public string year { get; set; }
        public string link { get; set; }
    }
}
