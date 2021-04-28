using System.Collections.Generic;

namespace ENTITIES.CustomModels.ScienceManagement.Researcher
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class ResearcherView : ResearcherDetail
    {
        public List<BaseRecord<Award>> awards { get; set; }
        public List<AcadBiography> acadBiography { get; set; }
    }
}
