using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.AcademicCollaborationRepository
{
    public class AcademicCollaborationProgramRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public BaseServerSideData<ProgramInfoManager> GetListProgram(BaseDatatable baseDatatable, string title,
        int duration, int direction, int collab_type_id)
        {
            try
            {
                string sql = @"select ROW_NUMBER() OVER (ORDER BY atv.publish_time DESC) AS 'no',
                            a.program_id, a.registration_deadline, a.article_id, a.duration, t4.partner_name,
                            t5.full_name, CONVERT(varchar, atv.publish_time, 20) as 'publish_time', atv.version_title as 'program_name'
                            from 
                            (select t1.program_id, CONVERT(varchar,t1.program_start_date, 103) + 
                            ' - ' + CONVERT(varchar,t1.program_end_date, 103) as 'registration_deadline',
                            t1.article_id, min(t3.language_id) as 'language_id',
                            case when getdate() between t1.program_start_date and 
                            t1.program_end_date then 1 else 0 end as 'duration' , t1.partner_id, t2.account_id
                            from 
                            IA_AcademicCollaboration.AcademicProgram t1 left join
                            IA_Article.Article t2 on t1.article_id = t2.article_id left join
                            IA_Article.ArticleVersion t3 on t1.article_id = t3.article_id
                            where t1.direction_id = {0} and t1.collab_type_id = {1}
                            group by t1.program_id, t1.program_start_date, t1.program_end_date,
                            t1.article_id, t1.partner_id, t2.account_id) as a 
                            join IA_Article.ArticleVersion atv on a.article_id = atv.article_id 
                            and a.language_id = atv.language_id 
                            left join IA_Collaboration.Partner t4 on a.partner_id = t4.partner_id
                            left join General.Account t5 on a.account_id = t5.account_id
                            where a.duration = {2} and atv.version_title like {3} ";

                string paging = @" ORDER BY " + baseDatatable.SortColumnName + " "
                            + baseDatatable.SortDirection +
                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT "
                            + baseDatatable.Length + " ROWS ONLY";

                List<ProgramInfoManager> obj = db.Database.SqlQuery<ProgramInfoManager>(sql + paging, direction,
                    collab_type_id, duration, title == null ? "%%" : "%" + title + "%").ToList();

                int totalRecord = db.Database.SqlQuery<ProcedureInfoManager>(sql, direction, collab_type_id, duration, 
                    title == null ? "%%" : "%" + title + "%").Count();

                return new BaseServerSideData<ProgramInfoManager>(obj, totalRecord);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
