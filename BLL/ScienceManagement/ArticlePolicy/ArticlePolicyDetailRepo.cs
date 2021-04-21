using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using System.Linq;

namespace BLL.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyDetailRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public ArticlePolicyIndex Detail(int article_version_id, int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var temp = (from a in db.ArticleVersions
                        join c in db.Articles on a.article_id equals c.article_id
                        where c.article_status_id == 2 && c.PolicyTypes.Count > 0
                        && a.language_id == language_id && a.article_version_id == article_version_id
                        orderby a.publish_time descending
                        select new ArticlePolicyIndex
                        {
                            Version = a,
                            Types = (from b in db.PolicyTypeLanguages
                                     join d in c.PolicyTypes on b.policy_type_id equals d.policy_type_id
                                     where b.language_id == language_id
                                     select b.policy_type_name).ToList(),
                        }).FirstOrDefault();
            return temp;
        }
    }
}
