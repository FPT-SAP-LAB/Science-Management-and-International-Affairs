using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities
{
    public class ProcedureInfo
    {
        public long no { get; set; }
        public int procedure_id { get; set; }
        public string procedure_name { get; set; }
        public string publish_time { get; set; }
        public string content { get; set; }
        public int direction_id { get; set; }
        public ProcedureInfo() { }
    }
}
