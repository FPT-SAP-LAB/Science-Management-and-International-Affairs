using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class IntellectualPropertyReport
    {
        public int rownum { get; set; }
        public List<CustomAuthor> authors { get; set; }
        public DateTime? date { get; set; }
        public string date_string { get; set; }
        public string invention_number { get; set; }
        public string total_reward { get; set; }
        public String kind { get; set; }
        public String invention_name { get; set; }
    }
    public class CustomAuthor
    {
        public int id { get; set; }
        public string name { get; set; }
        public string msnv { get; set; }
        public string title { get; set; }
        public string office { get; set; }
        public int office_id { get; set; }
    }
}
