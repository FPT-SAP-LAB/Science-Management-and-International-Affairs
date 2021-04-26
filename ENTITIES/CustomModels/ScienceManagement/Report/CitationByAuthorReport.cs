using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class CitationByAuthorReport
    {
        public int rownum { get; set; }
        public string decision_number { get; set; }
        public string author_name { get; set; }
        public string msnv { get; set; }
        public string office { get; set; }
        public int office_id { get; set; }
        public string journal_name { get; set; }
        public string valid_date_string { get; set; }
        public DateTime valid_date { get; set; }
        public Int64? total_reward { get; set; }
        public int? gscholar_citation { get; set; }
        public int? scopus_citation { get; set; }
        public String co_author { get; set; }
    }
    public class SupportClassAuthorCitation
    {
        public int people_id { get; set; }
        public int sum_scopus { get; set; }
        public int sum_scholar { get; set; }
    }
}
