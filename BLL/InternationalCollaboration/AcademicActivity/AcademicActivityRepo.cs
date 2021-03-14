using ENTITIES;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListAA> listAllAA(int year)
        {
            try
            {
                string sql =
                    @"SELECT al.activity_id,CONCAT(aa.activity_id, '$' ,av.version_title) as 'activity_name', [at].activity_type_name, [as].activity_status_name
                        FROM SMIA_AcademicActivity.AcademicActivity aa inner join SMIA_AcademicActivity.AcademicActivityType [at]
                        on aa.activity_type_id = [at].activity_type_id inner join SMIA_AcademicActivity.AcademicActivityLanguage al 
                        on aa.activity_id = al.activity_id inner join SMIA_AcademicActivity.AcademicActivityStatus [as]
                        on [as].activity_status_id = aa.activity_status_id inner join SMIA_AcademicActivity.ActivityInfo ai
                        on ai.activity_id = aa.activity_id and ai.main_article = 1 inner join IA_Article.Article ar
                        on ar.article_id = ai.article_id inner join IA_Article.ArticleVersion av
                        on av.article_id = ai.article_id and al.language_id = av.language_id
                        WHERE al.language_id = 1 AND YEAR(aa.activity_date_start) = @year";
                List<ListAA> data = db.Database.SqlQuery<ListAA>(sql,
                        new SqlParameter("year", year)).ToList();

                return data;
            }
            catch (Exception)
            {
                return new List<ListAA>();
            }
        }
        public bool AddAA(baseAA obj)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ENTITIES.AcademicActivity aa = db.AcademicActivities.Add(new ENTITIES.AcademicActivity
                    {
                        activity_status_id = 1,
                        activity_type_id = obj.activity_type_id,
                        activity_date_start = DateTime.ParseExact(obj.from, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        activity_date_end = DateTime.ParseExact(obj.to, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    });
                    db.SaveChanges();
                    db.AcademicActivityLanguages.Add(new ENTITIES.AcademicActivityLanguage
                    {
                        language_id = 1,
                        activity_id = aa.activity_id,
                        location = obj.location
                    });
                    db.SaveChanges();
                    ENTITIES.Article ar = db.Articles.Add(new ENTITIES.Article
                    {
                        account_id = 1,
                        article_status_id = 2,
                        need_approved = false
                    });
                    db.SaveChanges();
                    db.ActivityInfoes.Add(new ENTITIES.ActivityInfo
                    {
                        activity_id = aa.activity_id,
                        article_id = ar.article_id,
                        main_article = true
                    });
                    db.SaveChanges();
                    db.ArticleVersions.Add(new ENTITIES.ArticleVersion
                    {
                        article_id = ar.article_id,
                        publish_time = DateTime.Now,
                        version_title = obj.activity_name,
                        language_id = 1,
                        article_content = ""
                    });
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
        public List<ENTITIES.AcademicActivityType> getType()
        {
            try
            {
                List<ENTITIES.AcademicActivityType> data = db.AcademicActivityTypes.ToList();
                return data;
            }
            catch (Exception)
            {
                return new List<ENTITIES.AcademicActivityType>();
            }
        }
        public baseAA GetbaseAA(int id)
        {
            try
            {
                string sql = @"SELECT av.version_title as 'activity_name', [aa].activity_type_id, [al].[location], aa.activity_date_start as 'from', aa.activity_date_end as 'to'
                        FROM SMIA_AcademicActivity.AcademicActivity aa inner join SMIA_AcademicActivity.AcademicActivityLanguage al 
                        on aa.activity_id = al.activity_id inner join SMIA_AcademicActivity.ActivityInfo ai
                        on ai.activity_id = aa.activity_id and ai.main_article = 1 inner join IA_Article.Article ar
                        on ar.article_id = ai.article_id inner join IA_Article.ArticleVersion av
                        on av.article_id = ai.article_id and al.language_id = av.language_id
                        WHERE al.language_id = 1 and aa.activity_id = @id";
                baseAA obj = db.Database.SqlQuery<baseAA>(sql,
                            new SqlParameter("id", id)).FirstOrDefault();
                return obj;
            }
            catch (Exception)
            {
                return new baseAA();
            }
        }
        public bool updateBaseAA(int id, int activity_type_id, string title, string from, string to, string location)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ENTITIES.AcademicActivity aa = db.AcademicActivities.Find(id);
                    aa.activity_date_start = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    aa.activity_date_end = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    aa.activity_type_id = activity_type_id;
                    db.Entry(aa).State = EntityState.Modified;
                    db.SaveChanges();
                    AcademicActivityLanguage al = db.AcademicActivityLanguages.Where(x => x.activity_id == id && x.language_id == 1).FirstOrDefault();
                    al.location = location;
                    db.Entry(al).State = EntityState.Modified;
                    db.SaveChanges();
                    ActivityInfo ai = db.ActivityInfoes.Where(x => x.activity_id == id && x.main_article == true).FirstOrDefault();
                    ArticleVersion av = db.ArticleVersions.Where(x => x.article_id == ai.article_id && x.language_id == 1).FirstOrDefault();
                    av.version_title = title;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool deleteAA(int id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ActivityInfo ai = db.ActivityInfoes.Where(x => x.activity_id == id && x.main_article == true).FirstOrDefault();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public class ListAA
        {
            public int activity_id { get; set; }
            public string activity_name { get; set; }
            public string activity_type_name { get; set; }
            public string activity_status_name { get; set; }
        }
        public class baseAA
        {
            public string activity_name { get; set; }
            public int activity_type_id { get; set; }
            public string location { get; set; }
            public string from { get; set; }
            public string to { get; set; }
        }
    }
}