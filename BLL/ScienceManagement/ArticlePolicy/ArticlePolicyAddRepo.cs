using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyAddRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public AlertModal<string> Add(List<ArticleVersion> articleVersions, List<int> types, int account_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;

                    List<PolicyType> policyTypes = db.PolicyTypes.Where(x => types.Contains(x.policy_type_id)).ToList();

                    Article article = new Article
                    {
                        need_approved = false,
                        category_id = 1,
                        article_status_id = 2,
                        account_id = account_id
                    };

                    foreach (var item in articleVersions)
                    {
                        item.publish_time = now;
                        article.ArticleVersions.Add(item);
                    }

                    policyTypes.ForEach(x => article.PolicyTypes.Add(x));

                    db.Articles.Add(article);
                    db.SaveChanges();

                    trans.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                }
            }
            return new AlertModal<string>(false);
        }
    }
}
