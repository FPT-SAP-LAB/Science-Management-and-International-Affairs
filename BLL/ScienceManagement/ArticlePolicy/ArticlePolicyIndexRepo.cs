using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;

namespace BLL.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyIndexRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<ArticlePolicyItem> List(int language_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var temp = (from a in db.Policies
                        join b in db.Policy_type on a.policy_type_id equals b.policy_type_id
                        join c in db.Articles on a.article_id equals c.article_id
                        where a.expired_date == null && c.article_status_id == 2
                        select new ArticlePolicyItem
                        {
                            ArticleID = c.article_id,
                            Title = db.ArticleVersions.Where(x => x.language_id == language_id)
                            .OrderByDescending(x => x.publish_time).FirstOrDefault().version_title,
                            TypeName = b.policy_type_name
                        }).ToList();
            return temp;
        }
    }
}
