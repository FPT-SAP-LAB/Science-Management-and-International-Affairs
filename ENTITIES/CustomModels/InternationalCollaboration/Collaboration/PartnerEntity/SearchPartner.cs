using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity
{
    public class SearchPartner
    {
        public string partner_name { get; set; }
        public string nation { get; set; }
        public string specialization { get; set; }
        public int is_collab { get; set; }
    }
}
