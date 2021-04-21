using System;
using System.Collections.Generic;

namespace ENTITIES.CustomModels.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyIndex
    {
        public int VersionID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public string TypeName { get; set; }
        public List<string> Types { get; set; }
        public DateTime LastEdited { get; set; }
    }
}
