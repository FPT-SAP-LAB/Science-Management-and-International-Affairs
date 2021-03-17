using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOUPartner
{
    public class MOUPartnerAdd : ListMOUPartner
    {
        public string address { get; set; }
        public int partner_id { get; set; }
        public List<int> list_spe { get; set; }
        public List<int> list_scope { get; set; }
    }
}
