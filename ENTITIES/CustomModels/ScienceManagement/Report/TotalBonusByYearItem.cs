using System.Collections.Generic;

namespace ENTITIES.CustomModels.ScienceManagement.Report
{
    public class TotalBonusByYearItem
    {
        public int Month { get; set; }
        public PaperReward PaperRewards { get; set; }
        public List<int> CitationRewards { get; set; }
        public List<int> InventionRewards { get; set; }
        public long Total { get; set; }
    }
    public partial class PaperReward
    {
        public int MyProperty { get; set; }
        public List<int> Vietnam { get; set; }
        public List<int> International { get; set; }
        public List<int> FromResearchers { get; set; }
    }
}
