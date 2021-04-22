using System.Collections.Generic;

namespace ENTITIES.CustomModels.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyEdit
    {
        public List<ArticleVersion> Versions { get; set; }
        public List<PolicyTypeLanguage> TypeLanguages { get; set; }
        public ArticlePolicyEdit(List<ArticleVersion> versions, List<PolicyTypeLanguage> typeLanguages)
        {
            Versions = versions;
            TypeLanguages = typeLanguages;
        }
    }
}
