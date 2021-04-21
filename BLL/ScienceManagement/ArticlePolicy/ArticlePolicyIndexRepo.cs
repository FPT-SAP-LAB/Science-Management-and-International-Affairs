using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyIndexRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<ArticlePolicyIndex> ListManager(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var temp = (from c in db.Articles
                        where c.article_status_id == 2 && c.PolicyTypes.Count > 0
                        select new ArticlePolicyIndex
                        {
                            Version = c.ArticleVersions.Where(x => x.language_id == language_id)
                            .OrderByDescending(x => x.publish_time).FirstOrDefault(),
                            CreatedBy = c.Account.full_name,
                            Types = (from b in db.PolicyTypeLanguages
                                     join d in c.PolicyTypes on b.policy_type_id equals d.policy_type_id
                                     where b.language_id == language_id
                                     select b.policy_type_name).ToList()
                        }).ToList();
            temp.ForEach(x => x.TypeName = string.Join(", ", x.Types));
            return temp;
        }
        public List<ArticlePolicyIndex> ListGuest(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var temp = (from c in db.Articles
                        where c.article_status_id == 2 && c.PolicyTypes.Count > 0
                        select new ArticlePolicyIndex
                        {
                            Version = c.ArticleVersions.Where(x => x.language_id == language_id)
                            .OrderByDescending(x => x.publish_time).FirstOrDefault(),
                            CreatedBy = c.Account.full_name,
                            Types = (from b in db.PolicyTypeLanguages
                                     join d in c.PolicyTypes on b.policy_type_id equals d.policy_type_id
                                     where b.language_id == language_id
                                     select b.policy_type_name).ToList(),
                        }).ToList();
            temp.ForEach(x => x.TypeName = string.Join(", ", x.Types));
            return temp;
        }
    }
}
