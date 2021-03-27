using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity
{
    public class PartnerArticle : Partner
    {
        public string partner_content { get; set; }
        public int partner_language_type { get; set; }
    }
}
