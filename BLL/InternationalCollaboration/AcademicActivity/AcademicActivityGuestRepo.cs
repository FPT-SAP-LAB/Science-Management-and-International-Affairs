using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityGuestRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<baseAA> getBaseAA(int count, List<int> type, int language, string search)
        {
            try
            {
                StringBuilder typestr = new StringBuilder();
                List<baseAA> obj;
                if (type is null || type.Count == 0)
                {
                    typestr.Append("(1,2,3,4)");
                }
                else
                {
                    typestr.Append("(");
                    foreach (int i in type)
                    {
                        typestr.Append(i + ",");
                    }
                    typestr.Remove(typestr.Length - 1, 1);
                    typestr.Append(")");
                }
                string sql = @"SELECT aa.activity_id, av.version_title as 'activity_name', [aa].activity_type_id, [al].[location], cast(aa.activity_date_start as nvarchar) as 'from', cast(aa.activity_date_end as nvarchar) as 'to'
                        FROM SMIA_AcademicActivity.AcademicActivity aa inner join SMIA_AcademicActivity.AcademicActivityLanguage al 
                        on aa.activity_id = al.activity_id inner join SMIA_AcademicActivity.ActivityInfo ai
                        on ai.activity_id = aa.activity_id and ai.main_article = 1 inner join IA_Article.Article ar
                        on ar.article_id = ai.article_id inner join IA_Article.ArticleVersion av
                        on av.article_id = ai.article_id and al.language_id = av.language_id
                        WHERE al.language_id = @language AND [aa].activity_type_id IN " + typestr.ToString();
                if (search is null)
                {
                    sql += @" ORDER BY [from] DESC
                           OFFSET @count*6 ROWS FETCH NEXT 6 ROWS ONLY";
                    obj = db.Database.SqlQuery<baseAA>(sql, new SqlParameter("count", count),
                    new SqlParameter("language", language)).ToList();
                }
                else
                {
                    sql += @" AND av.version_title LIKE @search
                           ORDER BY [from] DESC
                           OFFSET @count*6 ROWS FETCH NEXT 6 ROWS ONLY";
                    obj = db.Database.SqlQuery<baseAA>(sql, new SqlParameter("count", count),
                    new SqlParameter("language", language), new SqlParameter("search", "%" + search + "%")).ToList();
                }
                foreach (baseAA a in obj)
                {
                    a.from = changeFormatDate(a.from);
                    a.to = changeFormatDate(a.to);
                }
                return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<baseAA>();
            }
        }

        public List<activityType> getListType(int language)
        {
            try
            {
                string sql = @"SELECT atl.activity_type_id, atl.activity_type_name
                               FROM SMIA_AcademicActivity.AcademicActivityTypeLanguage atl
                               WHERE atl.language_id = @language";
                List<activityType> obj = db.Database.SqlQuery<activityType>(sql, new SqlParameter("language", language)).ToList();
                return obj;
            }
            catch (Exception e)
            {
                return new List<activityType>();
            }
        }

        public string changeFormatDate(string date)
        {
            string[] sp = date.Split('-');
            return sp[2] + '/' + sp[1] + '/' + sp[0];
        }

        public baseAA getBaseAADetail(int id, int language)
        {
            try
            {
                string sql = @"SELECT av.version_title as 'activity_name', [aa].activity_type_id, [al].[location], cast(aa.activity_date_start as nvarchar) as 'from', cast(aa.activity_date_end as nvarchar) as 'to', al.language_id
                        FROM SMIA_AcademicActivity.AcademicActivity aa left join SMIA_AcademicActivity.AcademicActivityLanguage al 
                        on aa.activity_id = al.activity_id left join SMIA_AcademicActivity.ActivityInfo ai
                        on ai.activity_id = aa.activity_id and ai.main_article = 1 left join IA_Article.Article ar
                        on ar.article_id = ai.article_id left join IA_Article.ArticleVersion av
                        on av.article_id = ai.article_id and (al.language_id = av.language_id or al.language_id is null or av.language_id is null)
                        WHERE (al.language_id = @language or av.language_id = @language) AND [aa].activity_id = @id and ai.main_article = 1";
                baseAA detail = db.Database.SqlQuery<baseAA>(sql, new SqlParameter("id", id),
                    new SqlParameter("language", language)).FirstOrDefault();
                detail.from = changeFormatDate(detail.from);
                detail.to = changeFormatDate(detail.to);
                return detail;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new baseAA();
            }
        }
        public List<subContent> GetSubContent(int id, int language)
        {
            try
            {
                string sql = @"SELECT av.article_id 'id', av.version_title as 'name', av.article_content 'content'
                FROM SMIA_AcademicActivity.AcademicActivity aa join SMIA_AcademicActivity.ActivityInfo ai
                on ai.activity_id = aa.activity_id join IA_Article.Article ar
                on ar.article_id = ai.article_id join IA_Article.ArticleVersion av
                on av.article_id = ai.article_id 
                WHERE av.language_id = @language AND [aa].activity_id = @id
                ORDER BY ai.main_article DESC";
                List<subContent> obj = db.Database.SqlQuery<subContent>(sql, new SqlParameter("language", language),
                    new SqlParameter("id", id)).ToList();
                return obj;
            } 
            catch(Exception e)
            {
                return new List<subContent>();
            }
        }
        public fullForm getForm(int phase_id)
        {
            try
            {
                string sql = @"select f.title as 'f_title',f.form_id,f.phase_id,q.question_id,q.title,cast(q.is_compulsory as int) as 'is_compulsory',q.answer_type_id from SMIA_AcademicActivity.AcademicActivityPhase aap
                                inner join SMIA_AcademicActivity.Form f on f.phase_id = aap.phase_id
                                inner join SMIA_AcademicActivity.Question q on f.form_id = q.form_id
                                where f.phase_id = @phase_id";
                List<baseFrom> data = db.Database.SqlQuery<baseFrom>(sql, new SqlParameter("phase_id", phase_id)).ToList();
                List<int> quesOp = data.Where(x => x.answer_type_id == 3 || x.answer_type_id == 5).Select(y => y.question_id).ToList();
                string list_option = "";
                foreach (int i in quesOp)
                {
                    list_option += i + ",";
                }
                list_option = list_option.Remove(list_option.Length - 1);
                List<QuesOption> quesOptions = new List<QuesOption>();
                if (!String.IsNullOrEmpty(list_option))
                {
                    sql = @"select qo.* from SMIA_AcademicActivity.QuestionOption qo where qo.question_id in (" + list_option + ")";
                    quesOptions = db.Database.SqlQuery<QuesOption>(sql).ToList();
                }
                fullForm ff = new fullForm
                {
                    question = data,
                    optins = quesOptions
                };
                return ff;
            }
            catch (Exception e)
            {
                return new fullForm();
            }
        }
        public bool sendForm(int fid, string answer)
        {
            try
            {
                db.Responses.Add(new Response
                {
                    form_id = fid,
                    answer = answer
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public class activityType
        {
            public string activity_type_name { get; set; }
            public int activity_type_id { get; set; }
        }
        public class baseAA
        {
            public string activity_name { get; set; }
            public int activity_id { get; set; }
            public int activity_type_id { get; set; }
            public string location { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public string content { get; set; }
        }
        public class baseFrom
        {
            public string f_title { get; set; }
            public int form_id { get; set; }
            public int phase_id { get; set; }
            public int question_id { get; set; }
            public string title { get; set; }
            public int is_compulsory { get; set; }
            public int answer_type_id { get; set; }
        }
        public class QuesOption
        {
            public int question_id { get; set; }
            public string option_title { get; set; }
        }
        public class fullForm
        {
            public List<baseFrom> question { get; set; }
            public List<QuesOption> optins { get; set; }
        }

        public class subContent
        {
            public int id { get; set; }
            public string name { get; set; }
            public string content { get; set; }
        }
    }
}
