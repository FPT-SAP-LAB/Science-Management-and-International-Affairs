using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities
{
    public class ProgramInfo
    {
        public int program_id { get; set; }
        public string program_name { get; set; }
        public string avatar { get; set; }
        public string content { get; set; }
        public string registration_deadline { get; set; }
        public string publish_time { get; set; }
        public string country_name { get; set; }
        public string partner_name { get; set; }
        public int direction_id { get; set; }
        public int collab_type_id { get; set; }
        public ProgramInfo() { }
    }
}
