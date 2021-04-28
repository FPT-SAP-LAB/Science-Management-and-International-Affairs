using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class ResearcherDetail
    {
        public int id { get; set; }
        public string name { get; set; }
        public System.DateTime? dob { get; set; }
        public string country { get; set; }
        public int country_id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int title_id { get; set; }
        public string title_name { get; set; }
        public string office { get; set; }
        public int position_id { get; set; }
        public string position_name { get; set; }
        public string avatar { get; set; }
        public string website { get; set; }
        public string gscholar { get; set; }
        public string cv { get; set; }
        public List<SelectField> interested_fields { get; set; }
        public List<SelectField> title_fields { get; set; }
        public List<SelectField> position_fields { get; set; }
        public List<SelectField> offices_fields { get; set; }
        public List<SelectField> countries_fields { get; set; }
        public bool profile_page_active { get; set; }
    }
}
