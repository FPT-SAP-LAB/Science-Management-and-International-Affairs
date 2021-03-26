using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    public class ResearcherDetail
    {
        public int id { get; set; }
        public string name { get; set; }
        public System.DateTime? dob { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string title { get; set; }
        public string office { get; set; }
        public string position { get; set; }
        public string avatar { get; set; }
        public string website { get; set; }
        public string gscholar { get; set; }
        public string cv { get; set; }
        public List<InterestedField> interested_fields { get; set; }
    }
}
