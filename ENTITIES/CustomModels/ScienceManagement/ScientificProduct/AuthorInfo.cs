using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Paper
{
    public class AuthorInfo : Profile
    {
        public string title_name { get; set; }
        public string contract_name { get; set; }
        public Nullable<int> money_reward { get; set; }
        public Nullable<int> total_reward { get; set; }
        public string office_abbreviation { get; set; }
        public string link { get; set; }
        public int contract_id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string office_id_string { get; set; }
    }
}
