using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaboration
{
    public class StatusType
    {
        public int status_type { get; set; }
        public string status_type_name { get; set; }

        public StatusType(int status_type, string status_type_name)
        {
            this.status_type = status_type;
            this.status_type_name = status_type_name;
        }
    }
}
