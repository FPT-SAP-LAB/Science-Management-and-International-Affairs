using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Conference
{
    public class ConferenceCriteria
    {
        public int CriteriaID { get; set; }
        public string CriteriaName { get; set; }
        public bool IsAccepted { get; set; }
    }
}
