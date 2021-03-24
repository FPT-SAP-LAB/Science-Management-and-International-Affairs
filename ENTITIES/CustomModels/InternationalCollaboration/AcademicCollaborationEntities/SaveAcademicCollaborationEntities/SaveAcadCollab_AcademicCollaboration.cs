using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities.SaveAcademicCollaborationEntities
{
    public class SaveAcadCollab_AcademicCollaboration
    {
        public int status_id { get; set; }
        public string plan_start_date { get; set; }
        public string plan_end_date { get; set; }
        public string actual_start_date { get; set; }
        public string actual_end_date { get; set; }
        public bool support { get; set; }
        public string note { get; set; }
    }
}
