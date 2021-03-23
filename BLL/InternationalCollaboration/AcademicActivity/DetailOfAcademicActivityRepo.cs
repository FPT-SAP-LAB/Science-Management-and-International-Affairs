using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class DetailOfAcademicActivityRepo
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public baseDetail getDetail(int language_id, int activity_id)
        {
            try
            {
                string sql = @"SELECT av.version_title as 'activity_name', [aa].activity_type_id, [al].[location], cast(aa.activity_date_start as nvarchar) as 'from', cast(aa.activity_date_end as nvarchar) as 'to', al.language_id, av.article_content , ar.article_status_id
                                    FROM SMIA_AcademicActivity.AcademicActivity aa inner join SMIA_AcademicActivity.AcademicActivityLanguage al 
                                    on aa.activity_id = al.activity_id inner join SMIA_AcademicActivity.ActivityInfo ai
                                    on ai.activity_id = aa.activity_id and ai.main_article = 1 inner join IA_Article.Article ar
                                    on ar.article_id = ai.article_id inner join IA_Article.ArticleVersion av
                                    on av.article_id = ai.article_id and al.language_id = av.language_id
                                    WHERE al.language_id = @language_id AND [aa].activity_id = @activity_id and ai.main_article = 1";
                baseDetail data = db.Database.SqlQuery<baseDetail>(sql, new SqlParameter("language_id", language_id),
                                                                        new SqlParameter("activity_id", activity_id)).FirstOrDefault();
                if (data != null)
                {
                    data.from = changeFormatDate(data.from);
                    data.to = changeFormatDate(data.to);
                    return data;
                }
                else
                {
                    using (DbContextTransaction transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.AcademicActivityLanguages.Add(new AcademicActivityLanguage
                            {
                                activity_id = activity_id,
                                language_id = language_id,
                                location = String.Empty
                            });
                            db.SaveChanges();
                            ActivityInfo ai = db.ActivityInfoes.Where(x => x.activity_id == activity_id && x.main_article == true).FirstOrDefault();
                            db.ArticleVersions.Add(new ArticleVersion
                            {
                                publish_time = DateTime.Now,
                                version_title = String.Empty,
                                article_content = String.Empty,
                                article_id = ai.article_id,
                                language_id = language_id
                            });
                            db.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                        }
                    }
                    data = db.Database.SqlQuery<baseDetail>(sql, new SqlParameter("language_id", language_id),
                                                                        new SqlParameter("activity_id", activity_id)).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception e)
            {
                return new baseDetail();
            }
        }
        public string changeFormatDate(string date)
        {
            string[] sp = date.Split('-');
            return sp[2] + '/' + sp[1] + '/' + sp[0];
        }
        public List<subContent> getSubContents(int language_id, int activity_id)
        {
            try
            {
                string sql = @"SELECT ai.article_id, av.version_title, av.article_content, al.language_id
                                FROM SMIA_AcademicActivity.AcademicActivity aa inner join SMIA_AcademicActivity.AcademicActivityLanguage al 
                                on aa.activity_id = al.activity_id inner join SMIA_AcademicActivity.ActivityInfo ai
                                on ai.activity_id = aa.activity_id inner join IA_Article.Article ar
                                on ar.article_id = ai.article_id inner join IA_Article.ArticleVersion av
                                on av.article_id = ai.article_id and al.language_id = av.language_id
                                WHERE al.language_id = @language_id  AND [aa].activity_id = @activity_id and ai.main_article = 0";
                List<subContent> data = db.Database.SqlQuery<subContent>(sql, new SqlParameter("language_id", language_id),
                                                                              new SqlParameter("activity_id", activity_id)).ToList();
                if (data.Count == 0)
                {
                    List<ActivityInfo> listAI = db.ActivityInfoes.Where(x => x.activity_id == activity_id && x.main_article == false).ToList();
                    foreach (ActivityInfo ai in listAI)
                    {
                        db.ArticleVersions.Add(new ArticleVersion
                        {
                            article_id = ai.article_id,
                            language_id = language_id,
                            version_title = String.Empty,
                            article_content = String.Empty,
                            publish_time = DateTime.Now
                        });
                    }
                    db.SaveChanges();
                }
                data = db.Database.SqlQuery<subContent>(sql, new SqlParameter("language_id", language_id),
                                                                              new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<subContent>();
            }
        }
        public List<AcademicActivityType> getType()
        {
            try
            {
                List<AcademicActivityType> data = db.AcademicActivityTypes.ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<AcademicActivityType>();
            }
        }
        public bool updateDetail(InfoSumDetail data, Authen.LoginRepo.User u)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    AcademicActivityRepo aaRepo = new AcademicActivityRepo();
                    infoDetail detail = data.infoDetail;
                    bool res = aaRepo.updateBaseAA(detail.activity_id, detail.activity_type_id, detail.activity_name, detail.location, detail.from, detail.to, detail.language_id);
                    ActivityInfo ai = db.ActivityInfoes.Where(x => x.activity_id == detail.activity_id && x.main_article == true).FirstOrDefault();
                    ArticleVersion av = db.ArticleVersions.Where(x => x.article_id == ai.article_id && x.language_id == detail.language_id).FirstOrDefault();
                    av.article_content = detail.article_content;
                    db.Entry(av).State = EntityState.Modified;
                    db.SaveChanges();
                    updateListSubContent(data.subContent, detail.language_id, detail.activity_id, u, detail.article_status_id);
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public void updateListSubContent(List<subContent> data, int language_id, int activity_id, Authen.LoginRepo.User u, int article_status_id)
        {
            List<subContent> old = getSubContents(language_id, activity_id);
            List<int> old_id = old.Select(x => x.article_id).ToList();
            foreach (subContent x in data)
            {
                if (!old_id.Contains(x.article_id))
                {
                    insertSubContent(x, activity_id, language_id, u, article_status_id);
                }
                else
                {
                    updateSubContent(x, language_id);
                }
                old_id.Remove(x.article_id);
                foreach (int i in old_id)
                {
                    ArticleVersion av = db.ArticleVersions.Where(z => z.article_id == i && z.language_id == language_id).FirstOrDefault();
                    db.ArticleVersions.Remove(av);
                    ActivityInfo ai = db.ActivityInfoes.Where(z => z.article_id == i && z.main_article == false).FirstOrDefault();
                    db.ActivityInfoes.Remove(ai);
                    Article a = db.Articles.Find(i);
                    db.Articles.Remove(a);
                }
                db.SaveChanges();
            }
        }
        public void updateSubContent(subContent data, int language_id)
        {
            ArticleVersion av = db.ArticleVersions.Where(x => x.article_id == data.article_id && x.language_id == language_id).FirstOrDefault();
            av.version_title = data.version_title;
            av.article_content = data.article_content;
            db.Entry(av).State = EntityState.Modified;
        }
        public void insertSubContent(subContent data, int activity_id, int language_id, Authen.LoginRepo.User u, int article_status_id)
        {
            Article a = db.Articles.Add(new Article
            {
                account_id = u.account.account_id,
                article_status_id = article_status_id
            });
            db.SaveChanges();
            db.ActivityInfoes.Add(new ActivityInfo
            {
                activity_id = activity_id,
                article_id = a.article_id,
                main_article = false
            });
            db.ArticleVersions.Add(new ArticleVersion
            {
                publish_time = DateTime.Now,
                version_title = data.version_title,
                article_content = data.article_content,
                article_id = a.article_id,
                language_id = language_id
            });
        }
        public bool changeStatusAA(int activity_id,int status)
        {
            try
            {
                List<ActivityInfo> ai = db.ActivityInfoes.Where(x => x.activity_id == activity_id).ToList();
                foreach(ActivityInfo i in ai)
                {
                    Article a = db.Articles.Find(i.article_id);
                    a.article_status_id = status;
                    db.Entry(a).State = EntityState.Modified;
                }
                db.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public class baseDetail
        {
            public string activity_name { get; set; }
            public int activity_type_id { get; set; }
            public string location { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public int language_id { get; set; }
            public string article_content { get; set; }
            public int article_status_id { get; set; }
        }
        public class subContent
        {
            public int article_id { get; set; }
            public string version_title { get; set; }
            public string article_content { get; set; }
            public int language_id { get; set; }
        }
        public class infoDetail : baseDetail
        {
            public int activity_id { get; set; }
        }
        public class SumDetail
        {
            public baseDetail baseDetail { get; set; }
            public List<subContent> subContent { get; set; }
        }
        public class InfoSumDetail
        {
            public infoDetail infoDetail { get; set; }
            public List<subContent> subContent { get; set; }
        }
    }
}