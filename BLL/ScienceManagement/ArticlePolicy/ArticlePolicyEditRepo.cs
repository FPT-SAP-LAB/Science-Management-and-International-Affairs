using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyEditRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public ArticlePolicyEdit Detail(int article_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            //db.Configuration.LazyLoadingEnabled = false;
            List<ArticleVersion> Versions = (from a in db.Languages
                                             orderby a.language_id
                                             select (from c in db.Articles
                                                     join b in db.ArticleVersions on c.article_id equals b.article_id
                                                     where c.article_status_id == 2 && c.article_id == article_id
                                                     && b.language_id == a.language_id
                                                     orderby b.publish_time descending
                                                     select b).FirstOrDefault()).ToList();
            List<PolicyTypeLanguage> TypeLanguages = (from a in db.Articles.Find(article_id).PolicyTypes
                                                      join b in db.PolicyTypeLanguages on a.policy_type_id equals b.policy_type_id
                                                      where b.language_id == 1
                                                      select b).ToList()
                                                      .Select(x => new PolicyTypeLanguage
                                                      {
                                                          language_id = x.language_id,
                                                          policy_type_id = x.policy_type_id,
                                                          policy_type_language_id = x.policy_type_language_id,
                                                          policy_type_name = x.policy_type_name
                                                      }).ToList();
            return new ArticlePolicyEdit(Versions, TypeLanguages);
        }
        public AlertModal<string> Edit(List<ArticleVersion> articleVersions, List<int> types, int article_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;

                    Article article = db.Articles.Where(x => x.article_id == article_id && x.PolicyTypes.Count > 0).FirstOrDefault();
                    if (article == null)
                        return new AlertModal<string>(false, "Bài đăng không tồn tại");

                    List<PolicyType> policyTypes = db.PolicyTypes.Where(x => types.Contains(x.policy_type_id)).ToList();

                    foreach (var item in articleVersions)
                    {
                        item.publish_time = now;
                        article.ArticleVersions.Add(item);
                    }

                    //db.PolicyTypes.RemoveRange(article.PolicyTypes);
                    article.PolicyTypes = new List<PolicyType>();
                    policyTypes.ForEach(x => article.PolicyTypes.Add(x));

                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    trans.Rollback();
                }
            }
            return new AlertModal<string>(false);
        }
    }
}
