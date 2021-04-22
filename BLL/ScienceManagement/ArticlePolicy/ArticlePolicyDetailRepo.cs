using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using System.Linq;

namespace BLL.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyDetailRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public ArticlePolicyIndex Detail(int article_id, int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var temp = (from c in db.Articles
                        where c.article_status_id == 2 && c.PolicyTypes.Count > 0 && c.article_id == article_id
                        select new ArticlePolicyIndex
                        {
                            Version = c.ArticleVersions.Where(x => x.language_id == language_id)
                            .OrderByDescending(x => x.publish_time).FirstOrDefault(),
                            CreatedBy = c.Account.full_name,
                            Types = (from b in db.PolicyTypeLanguages
                                     join d in c.PolicyTypes on b.policy_type_id equals d.policy_type_id
                                     where b.language_id == language_id
                                     select b.policy_type_name).ToList()
                        }).FirstOrDefault();
            if (temp == null)
                return null;
            temp.TypeName = string.Join(", ", temp.Types);
            return temp;
        }
    }
}
