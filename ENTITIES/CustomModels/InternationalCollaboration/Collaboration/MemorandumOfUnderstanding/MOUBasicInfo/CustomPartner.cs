using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOUBasicInfo
{
    public class CustomPartner
    {
        public CustomPartner(int partner_id, string partner_name)
        {
            this.partner_id = partner_id;
            this.partner_name = partner_name;
        }
        public CustomPartner() { }
        public List<CustomScope> ListScopeExMOU { get; set; }
        public int partner_id { get; set; }
        public string partner_name { get; set; }
    }
}
