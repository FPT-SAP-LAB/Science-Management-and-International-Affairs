using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class ArticlesInoutCountryReports
    {
        public int rownum { get; set; }
        public string decision_number { get; set; }
        public List<String> authors { get; set; }
        public List<String> titles { get; set; }
        public List<String> offices { get; set; }
        public string paper_name { get; set; }
        public string journal_name { get; set; }
        public string valid_date_string { get; set; }
        public DateTime valid_date { get; set; }
        public Int64? total_reward { get; set; }
        public String specialization { get; set; }
        public List<PaperCriteriaCustom> criterias { get; set; }
        public String co_author { get; set; }
        
    }
    public class PaperCriteriaCustom
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
