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
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
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
                data.from = changeFormatDate(data.from);
                data.to = changeFormatDate(data.to);
                return data;
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
                                on ai.activity_id = aa.activity_id and ai.main_article = 1 inner join IA_Article.Article ar
                                on ar.article_id = ai.article_id inner join IA_Article.ArticleVersion av
                                on av.article_id = ai.article_id and al.language_id = av.language_id
                                WHERE al.language_id = @language_id  AND [aa].activity_id = @activity_id and ai.main_article = 0";
                List<subContent> data = db.Database.SqlQuery<subContent>(sql, new SqlParameter("language_id", language_id),
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
    }
}
