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

        public List<baseAA> getBaseAA(int count, List<int> type)
        {
            try
            {
                StringBuilder typestr = new StringBuilder();

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
                        WHERE al.language_id = 1 AND [aa].activity_type_id IN " + typestr.ToString() + @"
                        ORDER BY [from] DESC
                        OFFSET @count*6 ROWS FETCH NEXT 6 ROWS ONLY";
                List<baseAA> obj = db.Database.SqlQuery<baseAA>(sql, new SqlParameter("count", count)).ToList();
                foreach (baseAA a in obj)
                {
                    a.from = changeFormatDate(a.from);
                    a.to = changeFormatDate(a.to);
                }
                return obj;
            }
            catch (Exception e)
            {
                return new List<baseAA>();
            }
        }

        public string changeFormatDate(string date)
        {
            string[] sp = date.Split('-');
            return sp[2] + '/' + sp[1] + '/' + sp[0];
        }

        public class baseAA
        {
            public string activity_name { get; set; }
            public int activity_id { get; set; }
            public int activity_type_id { get; set; }
            public string location { get; set; }
            public string from { get; set; }
            public string to { get; set; }
        }
    }
}
