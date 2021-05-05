using System.Collections.Generic;

namespace ENTITIES.CustomModels.ScienceManagement.Dashboard
{
    public class PaperByOffice
    {
        public Dictionary<string, List<int>> CriteriaValuePairs { get; set; }
        public string[] Criterias { get; set; }
        public string[] Offices { get; set; }

        public PaperByOffice(string[] criterias)
        {
            Criterias = criterias;
            CriteriaValuePairs = new Dictionary<string, List<int>>();
        }
    }
}
