using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOAPartner
{
    public class PartnerHistory
    {
        public string full_name { get; set; }
        public string content { get; set; }
        public DateTime add_time { get; set; }
        public string add_time_string { get; set; }
    }
}
