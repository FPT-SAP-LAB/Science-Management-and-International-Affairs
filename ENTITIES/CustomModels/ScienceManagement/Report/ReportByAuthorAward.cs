using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class ReportByAuthorAward
    {
        public int rowNum { get; set; }
        public int office_id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string office { get; set; }
        public string paperAward { get; set; }
        public string conferenceAward { get; set; }
        public string CitationAward { get; set; }
        public string PublicYear { get; set; }
    }
}
