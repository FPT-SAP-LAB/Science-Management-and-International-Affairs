using System;
using System.Collections.Generic;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class IntellectualPropertyReport
    {
        public int rownum { get; set; }
        public DateTime valid_date { get; set; }
        public string valid_date_string { get; set; }
        public string decision_number { get; set; }
        public List<string> authors { get; set; }
        public string invention_name { get; set; }
        public DateTime? date { get; set; }
        public string date_string { get; set; }
        public string invention_number { get; set; }
        public int total_reward { get; set; }
        public string kind { get; set; }
    }
}
