using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities.SaveAcademicCollaborationEntities
{
    public class SaveAcadCollab_Partner
    {
        public bool available_partner { get; set; }
        public string partner_name { get; set; }
        public int partner_id { get; set; }
        public int partner_country_id { get; set; }
        public int collab_scope_id { get; set; }
    }
}
