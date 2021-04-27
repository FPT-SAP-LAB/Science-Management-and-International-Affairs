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
            if (article_id <= 0 || article_id == int.MaxValue)
                return null;

            db = db ?? new ScienceAndInternationalAffairsEntities();
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
        public AlertModal<string> Edit(List<ArticleVersion> articleVersions, List<int> types, int article_id, int account_id)
        {
            AlertModal<string> result = ValidateEdit(articleVersions, types, article_id, account_id);
            if (!result.success)
                return result;

            db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime now = DateTime.Now;

                    bool edit = false;

                    Article article = db.Articles.Where(x => x.article_id == article_id && x.PolicyTypes.Count > 0).FirstOrDefault();
                    if (article == null)
                        return new AlertModal<string>(false, "Bài đăng không tồn tại");

                    List<PolicyType> policyTypes = db.PolicyTypes.Where(x => types.Contains(x.policy_type_id)).ToList();

                    ArticlePolicyEdit articleBefore = Detail(article_id);

                    foreach (var item in articleVersions)
                    {
                        var versionBefore = articleBefore.Versions.Where(x => x != null && x.language_id == item.language_id).FirstOrDefault();
                        if (versionBefore == null || !versionBefore.version_title.Equals(item.version_title)
                            || !versionBefore.article_content.Equals(item.article_content))
                        {
                            item.publish_time = now;
                            item.account_id = account_id;
                            article.ArticleVersions.Add(item);
                            edit = true;
                        }
                    }

                    List<int> typesBefore = articleBefore.TypeLanguages.Select(x => x.policy_type_id).ToList();
                    if (!typesBefore.All(types.Contains) || typesBefore.Count != types.Count)
                    {
                        article.PolicyTypes = new List<PolicyType>();
                        policyTypes.ForEach(x => article.PolicyTypes.Add(x));
                        edit = true;
                    }

                    if (!edit)
                        return new AlertModal<string>(false, "Không có nội dung nào thay đổi");
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

        private static AlertModal<string> ValidateEdit(List<ArticleVersion> articleVersions, List<int> types, int article_id, int account_id)
        {
            if (articleVersions == null || articleVersions.Count == 0 ||
                articleVersions.All(x => x.article_content == null) || articleVersions.All(x => x.article_content == "") ||
                articleVersions.All(x => x.version_title == null) || articleVersions.All(x => x.version_title == ""))
                return new AlertModal<string>(false, "Không có nội dung nào thay đổi");

            if (types == null || types.Count == 0 || types.All(x => x <= 0))
                return new AlertModal<string>(false, "Loại chính sách không được bỏ trống");

            if (account_id <= 0 || account_id == int.MaxValue)
                return new AlertModal<string>(false, "Tài khoản không tồn tại");

            if (article_id <= 0 || article_id == int.MaxValue)
                return new AlertModal<string>(false, "Bài đăng không tồn tại");

            return new AlertModal<string>(true);
        }
    }
}
