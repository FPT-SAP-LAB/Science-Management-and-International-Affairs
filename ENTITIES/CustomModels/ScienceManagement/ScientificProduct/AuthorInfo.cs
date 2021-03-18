using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Paper
{
    public class AuthorInfo : Person
    {
        public string title_name { get; set; }
        public string contract_name { get; set; }
        public string money_reward { get; set; }
        public string office_abbreviation { get; set; }
        public string link { get; set; }
        public int area_id { get; set; }
        public int contract_id { get; set; }
        public int title_id { get; set; }
    }
}
