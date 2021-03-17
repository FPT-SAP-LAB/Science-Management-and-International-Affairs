using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Conference
{
    public class ConferenceIndex
    {
        public int RequestID { get; set; }
        public int RowNumber { get; set; }
        public string PaperName { get; set; }
        public string ConferenceName { get; set; }
        public DateTime Date { get; set; }
        public string CreatedDate { get; set; }
        public string StatusName { get; set; }
        public int StatusID { get; set; }
        public string FullName { get; set; }
    }
}
