using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES.CustomModels.ScienceManagement.Dashboard
{
    public class DashboardNumber
    {
        public int Invention { get; set; }
        public int ScopusISI { get; set; }
        public int Researcher { get; set; }
        public int Conference { get; set; }
        public int PaperRewardPending { get; set; }
        public int ConferenceFundingPending { get; set; }
        public int InventionRewardPending { get; set; }
        public int CitationRewardPending { get; set; }
    }
}
