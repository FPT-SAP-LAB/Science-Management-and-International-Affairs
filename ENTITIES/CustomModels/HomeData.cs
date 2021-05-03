using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using System.Collections.Generic;

namespace ENTITIES.CustomModels
{
    public class HomeData
    {
        public int Partner { get; set; }
        public List<string> Images { get; set; }
        public int Invention { get; set; }
        public int ScopusISI { get; set; }
        public int Researcher { get; set; }
        public List<ArticlePolicyIndex> ArticlePolicies { get; set; }

        public HomeData()
        {
        }

        public HomeData(int partner, List<string> images, int invention, int scopusISI, int researcher, List<ArticlePolicyIndex> articlePolicies)
        {
            Partner = partner;
            Images = images;
            Invention = invention;
            ScopusISI = scopusISI;
            Researcher = researcher;
            ArticlePolicies = articlePolicies;
        }
    }
}
