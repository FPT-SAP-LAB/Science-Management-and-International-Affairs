using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class ConferencesParticipationReport
    {
        public DateTime valid_date { get; set; }
        public string decision_number { get; set; }
        public string people_name { get; set; }
        public string title_name { get; set; }
        public string country_name { get; set; }
        public string conference_name { get; set; }
        public DateTime attendance_start { get; set; }
        public float total { get; set; }
        public float reimbursement { get; set; }
    }
}
