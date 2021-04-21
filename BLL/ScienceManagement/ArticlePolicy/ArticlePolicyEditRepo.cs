using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyEditRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public ArticlePolicyEdit Detail(int article_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            List<ArticleVersion> Versions = (from a in db.Languages
                                             orderby a.language_id
                                             select (from c in db.Articles
                                                     join b in db.ArticleVersions on c.article_id equals b.article_id
                                                     where c.article_status_id == 2 && c.article_id == article_id
                                                     && b.language_id == a.language_id
                                                     orderby b.publish_time descending
                                                     select b).FirstOrDefault()).ToList();
            List<PolicyTypeLanguage> TypeLanguages = (from a in db.PolicyTypeLanguages
                                                      join b in db.PolicyTypes on a.policy_type_id equals b.policy_type_id
                                                      select a).ToList();
            return new ArticlePolicyEdit(Versions, TypeLanguages);
        }
    }
}
