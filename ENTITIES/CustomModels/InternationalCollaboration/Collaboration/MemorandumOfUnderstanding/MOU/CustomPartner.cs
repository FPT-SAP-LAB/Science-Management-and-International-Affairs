using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU
{
    public class CustomPartner
    {
        public string website { get; set; }
        public int country_id { get; set; }
        public string country_name { get; set; }
        public int partner_id { get; set; }
        public string partner_name { get; set; }
        public string address { get; set; }
    }
}
