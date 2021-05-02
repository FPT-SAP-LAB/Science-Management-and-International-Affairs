using System;
using System.Collections.Generic;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class ArticlesInoutCountryReports
    {
        public int rownum { get; set; }
        public string decision_number { get; set; }
        public List<string> authors { get; set; }
        public List<string> titles { get; set; }
        public List<string> offices { get; set; }
        public string paper_name { get; set; }
        public string journal_name { get; set; }
        public string valid_date_string { get; set; }
        public DateTime valid_date { get; set; }
        public long? total_reward { get; set; }
        public string specialization { get; set; }
        public List<PaperCriteriaCustom> criterias { get; set; }
        public string co_author { get; set; }
        public class PaperCriteriaCustom
        {
            public int id { get; set; }
            public string name { get; set; }
        }
    }
}