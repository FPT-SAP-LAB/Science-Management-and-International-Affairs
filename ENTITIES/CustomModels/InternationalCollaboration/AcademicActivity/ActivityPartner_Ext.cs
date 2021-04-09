using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicActivity
{
    public class ActivityPartner_Ext
    {
        public int activity_partner_id { get; set; }
        public int partner_id { get; set; }
        public string partner_name { get; set; }
        public Nullable<double> sponsor { get; set; }
        public int scope_id { get; set; }
        public string scope_abbreviation { get; set; }
        public string scope_name { get; set; }
        public Nullable<DateTime> cooperation_date_start { get; set; }
        public Nullable<DateTime> cooperation_date_end { get; set; }
        public int? file_id { get; set; }
        public string file_name { get; set; }
        public string file_link { get; set; }
        public string contact_point_name { get; set; }
        public string contact_point_email { get; set; }
        public string contact_point_phone { get; set; }
    }
}
