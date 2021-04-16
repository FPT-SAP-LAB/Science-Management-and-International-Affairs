using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Invention
{
    public class CustomCountry
    {
        public Nullable<int> invention_id { get; set; }
        public int country_id { get; set; }
        public bool selected { get; set; }
        public string country_name { get; set; }
    }
}
