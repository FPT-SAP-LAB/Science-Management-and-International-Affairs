using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU
{
    public class IntersectPeriodMOUDate
    {
        public int num_check { get; set; }
        public DateTime mou_start_date { get; set; }
        public string mou_start_date_string { get; set; }
        public string mou_end_date_string { get; set; }
        public DateTime mou_end_date { get; set; }
        public int mou_id { get; set; }
        public string mou_code { get; set; }
    }
}
