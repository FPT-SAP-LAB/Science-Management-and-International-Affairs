using System;
using System.Collections.Generic;

namespace ENTITIES.CustomModels.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyIndex
    {
        public int ArticleID { get; set; }
        public string CreatedBy { get; set; }
        public string TypeName { get; set; }
        public List<string> Types { get; set; }
        public ArticleVersion Version { get; set; }
    }
}
