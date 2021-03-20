using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity
{
    public class PartnerHistory: Partner
    {
        public string code { get; set; }
        public string activity { get; set; }
        public string full_name { get; set; }
        public DateTime activity_date_start { get; set; }
        public DateTime activity_date_end { get; set; }
    }
}
