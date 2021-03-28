using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    public class AcadBiography
    {
        public int people_id { get; set; }
        public int acad_id { get; set; }
        public int rownum { get; set; }
        public string degree { get; set; }
        public string time { get; set; }
        public string place { get; set; }
    }
}
