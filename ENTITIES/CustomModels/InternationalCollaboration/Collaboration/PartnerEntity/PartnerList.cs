using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity
{
    public class PartnerList : Partner
    {
        public long no { get; set; }
        public string country_name { get; set; }
        public string specialization_name { get; set; }
        public int is_collab { get; set; }
        public List<string> SpecializationNames { get; set; }
    }
}
