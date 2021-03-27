using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities
{
    public class AcademicCollaboration_Ext : AcademicCollaboration
    {
        public string people_name { get; set; }
        public string email { get; set; }
        public int office_id { get; set; }
        public string office_name { get; set; }
        public int partner_id { get; set; }
        public string partner_name { get; set; }
        public int country_id { get; set; }
        public string country_name { get; set; }
        public int scope_id { get; set; }
        public string scope_name { get; set; }
        public int collab_status_id { get; set; }
        public string collab_status_name { get; set; }
    }
}
