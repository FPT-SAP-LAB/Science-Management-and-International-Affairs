using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class ArticlesInCountryReport
    {
        public int rownum { get; set; }
        public string decision_number { get; set; }
        public string author_name { get; set; }
        public string paper_name { get; set; }
        public string journal_name { get; set; }
        public string valid_date { get; set; }
        public string total_reward { get; set; }
    }
}
