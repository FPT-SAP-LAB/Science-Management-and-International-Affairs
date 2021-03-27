using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities
{
    public class ProgramDescription
    {
        public int type_id { get; set; }
        public String description { get; set; }
        public ProgramDescription()
        {

        }
        public ProgramDescription(int type_id, String description)
        {
            this.type_id = type_id;
            this.description = description;
        }
    }
}
