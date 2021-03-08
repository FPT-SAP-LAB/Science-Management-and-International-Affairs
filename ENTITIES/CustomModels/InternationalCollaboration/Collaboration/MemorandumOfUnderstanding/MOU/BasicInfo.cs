using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU
{
    public class BasicInfo
    {
        public string mou_code { get; set; }
        public int office_id { get; set; }
        public string mou_end_date { get; set; }
        public int mou_status_id { get; set; }
        public string reason { get; set; }
        public string mou_note { get; set; }
        public string evidence { get; set; }
    }
}
