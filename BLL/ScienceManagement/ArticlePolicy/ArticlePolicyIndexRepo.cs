using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using System.Collections.Generic;
using System.Linq;

namespace BLL.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyIndexRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db;

        public ArticlePolicyIndexRepo()
        {
            db = new ScienceAndInternationalAffairsEntities();
        }

        public ArticlePolicyIndexRepo(ScienceAndInternationalAffairsEntities db)
        {
            this.db = db;
        }

        public List<ArticlePolicyIndex> List(int language_id)
        {
            if (language_id <= 0 || language_id == int.MaxValue)
                return null;

            var temp = (from c in db.Articles
                        where c.article_status_id == 2 && c.PolicyTypes.Count > 0
                        select new ArticlePolicyIndex
                        {
                            ArticleID = c.article_id,
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
    }
}
