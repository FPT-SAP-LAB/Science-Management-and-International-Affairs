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
            catch (Exception e)
            {
                throw e;
            }
        }
        public class ListAA
        {
            public int activity_id { get; set; }
            public string activity_name { get; set; }
            public string activity_type_name { get; set; }
            public string activity_status_name { get; set; }
        }
    }
}
