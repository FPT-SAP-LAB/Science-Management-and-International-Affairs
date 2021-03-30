using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    public class ResearcherView : ResearcherDetail
    {
        public List<BaseRecord<Award>> awards { get; set; }
        public List<AcadBiography> acadBiography { get; set; }
    }
}
