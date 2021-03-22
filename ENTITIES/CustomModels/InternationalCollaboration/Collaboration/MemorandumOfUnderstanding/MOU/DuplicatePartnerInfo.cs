using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU
{
    public class DuplicatePartnerInfo
    {
        public DuplicatePartnerInfo() { }
        public string partner_name { get; set; }
        public string mou_start_date_string { get; set; }
        public string full_name { get; set; }
        public DateTime mou_start_date { get; set; }
        public string mou_code { get; set; }
    }
}
