using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities.DeserializeAcademicCollaborationEntities
{
    public class DeserializeAcadCollab_Partner
    {
        public bool available_partner { get; set; }
        public string partner_name { get; set; }
        public string partner_id { get; set; }
        public string partner_country_id { get; set; }
        public string collab_scope_id { get; set; }
    }
}
