using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA
{
    public class MOABasicInfo
    {
        public string moa_code { get; set; }
        public string moa_end_date { get; set; }
        public int mou_status_id { get; set; }
        public string reason { get; set; }
        public string moa_note { get; set; }
        public string evidence { get; set; }
    }
}
