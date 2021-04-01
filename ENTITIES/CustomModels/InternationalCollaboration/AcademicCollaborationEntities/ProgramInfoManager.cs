using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities
{
    public class ProgramInfoManager : ProgramInfo
    {
        public int duration { get; set; }
        public int article_id { get; set; }
        public int account_id { get; set; }
        public int language_id { get; set; }
        public int? partner_id { get; set; }
        public string full_name { get; set; }
        public string note { get; set; }
    }
}
