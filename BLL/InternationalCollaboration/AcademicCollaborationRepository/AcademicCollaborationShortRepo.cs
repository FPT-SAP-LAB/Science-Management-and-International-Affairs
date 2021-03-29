using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.AcademicCollaborationRepository
{
    public class AcademicCollaborationShortRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public BaseServerSideData<ProcedureInfoManager> GetListProcedure(BaseDatatable baseDatatable, string title, int direction, int language)
        {
            try
            {
                string sql = @"select ROW_NUMBER() OVER (ORDER BY pr.procedure_id ASC) AS 'no', 
                        pr.procedure_id, av.version_title 'procedure_name', 
                        CONVERT(nvarchar,av.publish_time, 20) 'publish_time' , ac.full_name 
		                            from IA_AcademicCollaboration.[Procedure] pr
		                            join IA_Article.Article ar on ar.article_id = pr.article_id
		                            join IA_Article.ArticleVersion av on av.article_id = pr.article_id
			                        join General.Account ac on pr.account_id = ac.account_id
                        where pr.direction_id = @direction AND av.language_id = @language";
                if (!String.IsNullOrEmpty(title))
                {
                    sql += " AND av.version_title LIKE @title";
                }
                string paging = @" ORDER BY " + baseDatatable.SortColumnName + " "
                            + baseDatatable.SortDirection +
                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT "
                            + baseDatatable.Length + " ROWS ONLY";

                List<ProcedureInfoManager> obj = db.Database.SqlQuery<ProcedureInfoManager>(sql + paging, new SqlParameter("language", language),
                    new SqlParameter("direction", direction), new SqlParameter("title", "%" + title + "%")).ToList();

                int totalRecord = db.Database.SqlQuery<ProcedureInfoManager>(sql, new SqlParameter("language", language),
                    new SqlParameter("direction", direction), new SqlParameter("title", "%" + title + "%")).Count();

                return new BaseServerSideData<ProcedureInfoManager>(obj, totalRecord);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<string> AddProcedure() {
            try {

                return new AlertModal<string>(true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
