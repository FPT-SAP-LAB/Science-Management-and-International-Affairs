using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities.SaveAcademicCollaborationEntities
{
    public class SaveAcadCollab_Person
    {
        public bool available_person { get; set; }
        public string person_name { get; set; }
        public int person_id { get; set; }
        public string person_email { get; set; }
        public int person_profile_office_id { get; set; }
    }
}
