using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities
{
    public class StatusHistory
    {
        public DateTime change_date { get; set; }
        public int collab_status_id { get; set; }
        public string full_name { get; set; }
        public string file_name { get; set; }
        public string file_link { get; set; }
        public string note { get; set; }
    }
}
