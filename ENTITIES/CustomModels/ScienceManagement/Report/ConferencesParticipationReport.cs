using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class ConferencesParticipationReport
    {
        public string CreatedDate { get; set; }
        public int RowNumber { get; set; }
        public DateTime? valid_date { get; set; }
        public string valiDateString { get; set; }
        public string decision_number { get; set; }
        public string people_name { get; set; }
        public string title_name { get; set; }
        public string country_name { get; set; }
        public string office_name { get; set; }
        public string conference_name { get; set; }
        public DateTime attendance_date { get; set; }
        public Int64 total { get; set; }
        public float reimbursement { get; set; }
        public string money_total { get; set; }
        public int requestId { get; set; }
        public string dateString { get; set; }
        public int office_id { get; set; }
    }
}
