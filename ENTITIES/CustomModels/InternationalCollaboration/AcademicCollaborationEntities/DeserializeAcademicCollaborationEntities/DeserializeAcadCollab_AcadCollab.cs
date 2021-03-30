using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities.DeserializeAcademicCollaborationEntities
{
    public class DeserializeAcadCollab_AcadCollab
    {
        public string collab_id { get; set; }
        public string status_id { get; set; }
        public DateTime plan_start_date { get; set; }
        public DateTime plan_end_date { get; set; }
        public DateTime? actual_start_date { get; set; }
        public DateTime? actual_end_date { get; set; }
        public bool support { get; set; }
        public string note { get; set; }
        public string evidence_link { get; set; }
        public string status_history_note { get; set; }
    }
}
