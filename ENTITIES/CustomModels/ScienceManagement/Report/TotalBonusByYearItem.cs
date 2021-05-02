using System.Collections.Generic;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class TotalBonusByYearItem
    {
        public int Month { get; set; }
        public PaperReward PaperRewards { get; set; }
        public List<int> CitationRewards { get; set; }
        public List<int> InventionRewards { get; set; }
        public class PaperReward
        {
            public List<int> Vietnam { get; set; }
            public List<int> International { get; set; }
            public List<int> FromResearchers { get; set; }
        }
    }
}
