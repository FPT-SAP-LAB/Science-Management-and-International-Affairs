using System;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class CitationByAuthorReport
    {
        public int rownum { get; set; }
        public string decision_number { get; set; }
        public string author_name { get; set; }
        public string msnv { get; set; }
        public string office { get; set; }
        public string valid_date_string { get; set; }
        public DateTime valid_date { get; set; }
        public int? total_reward { get; set; }
        public int? gscholar_citation { get; set; }
        public int? scopus_citation { get; set; }
    }
}
