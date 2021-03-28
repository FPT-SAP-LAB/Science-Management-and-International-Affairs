using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;

namespace BLL.InternationalCollaboration.AcademicCollaborationRepository
{
    public class AcademicCollaborationGuestRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ProgramInfo> listProgram(int count, int type, int country, string partner, int year, int language)
        {
            try
            {
                string sql = @"SELECT ap.program_id,pa.avatar, av.version_title as 'program_name'
                        FROM IA_AcademicCollaboration.AcademicProgram ap
                        JOIN IA_Article.Article ar ON ap.article_id = ar.article_id
                        JOIN IA_Article.ArticleVersion av ON av.article_id = ap.article_id
                        JOIN Localization.[Language] la ON la.language_id = av.language_id
                        JOIN General.Account ac ON ac.account_id = ar.account_id
                        JOIN IA_Collaboration.[Partner] pa ON pa.partner_id = ap.partner_id
                        WHERE av.language_id = @language AND ap.collab_type_id = @type";
                if (year != 0)
                {
                    sql += @" AND YEAR(ap.program_start_date) = @year";
                }
                if (country != 0)
                {
                    sql += @" AND pa.country_id = @country";
                }
                if (!String.IsNullOrEmpty(partner))
                {
                    sql += @" AND pa.partner_name LIKE @partner";
                }
                sql += @" ORDER BY av.publish_time DESC
                        OFFSET @count*4 ROWS FETCH NEXT 4 ROWS ONLY";
                List<ProgramInfo> obj = db.Database.SqlQuery<ProgramInfo>(sql, new SqlParameter("language", language),
                    new SqlParameter("type", type), new SqlParameter("count", count), new SqlParameter("year", year),
                    new SqlParameter("country", country), new SqlParameter("partner", "%" + partner + "%")).ToList();
                return obj;
            }
            catch (Exception e)
            {
                return new List<ProgramInfo>();
            }
        }

        public List<ProgramDescription> getTypeDescription(int type, int language)
        {
            try
            {
                string sql = @"SELECT 0 AS 'direction_id',[description]
                FROM IA_AcademicCollaboration.AcademicCollaborationTypeLanguage
                WHERE collab_type_id = @type AND language_id = @language
                UNION ALL
                SELECT ctd.direction_id, ctdl.[description]
                FROM IA_AcademicCollaboration.CollaborationTypeDirection ctd
                JOIN IA_AcademicCollaboration.CollaborationTypeDirectionLanguage ctdl 
                ON ctd.collab_type_direction_id = ctdl.collab_type_direction_id
                WHERE ctd.collab_type_id = @type AND ctdl.language_id = @language";
                List<ProgramDescription> obj = db.Database.SqlQuery<ProgramDescription>(sql, new SqlParameter("language", language),
                    new SqlParameter("type", type)).ToList();
                return obj;
            }
            catch (Exception e)
            {
                return new List<ProgramDescription>();
            }
        }

        public List<ProgramInfo> listPartnerProgram(string partner, int year, int country, int language)
        {
            try
            {
                string sql = @"select pa.partner_name, cou.country_name, av.version_title 'program_name', CONVERT(nvarchar ,ap.program_start_date, 103) + ' - ' +CONVERT(nvarchar ,ap.program_end_date, 103) 'registration_deadline', CONVERT(nvarchar ,av.publish_time, 20) 'publish_time'
                from IA_AcademicCollaboration.AcademicProgram ap
                join IA_Article.Article ar on ap.article_id = ar.article_id
                join IA_Article.ArticleVersion av on av.article_id = ap.article_id
                join Localization.[Language] la on la.language_id = av.language_id
                join General.Account ac on ac.account_id = ar.account_id
                join IA_Collaboration.[Partner] pa on pa.partner_id = ap.partner_id
                join General.Country cou on cou.country_id = pa.country_id
                WHERE ap.direction_id = 1 AND av.language_id = @language AND ap.collab_type_id = 2";
                if (country != 0)
                {
                    sql += " AND pa.country_id = @country";
                }
                if (!String.IsNullOrEmpty(partner))
                {
                    sql += " AND pa.partner_id LIKE @partner";
                }
                if (year != 0)
                {
                    sql += " AND YEAR(ap.program_start_date) = @year";
                }
                List<ProgramInfo> obj = db.Database.SqlQuery<ProgramInfo>(sql, new SqlParameter("language", language),
                    new SqlParameter("country", country), new SqlParameter("partner", "%" + partner + "%"), new SqlParameter("year", year)).ToList();
                return obj;
            }
            catch (Exception e)
            {
                return new List<ProgramInfo>();
            }
        }

        public List<ProgramInfo> listFPTProgram(int year, int language)
        {
            try
            {
                string sql = @"select av.version_title as 'program_name', CONVERT(nvarchar ,ap.program_start_date, 103) + ' - ' + CONVERT(nvarchar ,ap.program_end_date, 103) 'registration_deadline', CONVERT(nvarchar ,av.publish_time, 20) 'publish_time'
                from IA_AcademicCollaboration.AcademicProgram ap
                join IA_Article.Article ar on ap.article_id = ar.article_id
                join IA_Article.ArticleVersion av on av.article_id = ap.article_id
                join Localization.[Language] la on la.language_id = av.language_id
                join General.Account ac on ac.account_id = ar.account_id
                where av.language_id = @language and ap.direction_id = 2 AND ap.collab_type_id = 2";
                if (year != 0)
                {
                    sql += " AND YEAR(ap.program_start_date) = @year";
                }
                List<ProgramInfo> obj = db.Database.SqlQuery<ProgramInfo>(sql, new SqlParameter("language", language)).ToList();
                return obj;
            }
            catch (Exception e)
            {
                return new List<ProgramInfo>();
            }
        }

        public List<Country> listCountry()
        {
            try
            {
                string sql = @"SELECT *
                FROM General.Country";
                List<Country> obj = db.Database.SqlQuery<Country>(sql).ToList();
                return obj;
            }
            catch (Exception e)
            {
                return new List<Country>();
            }
        }
        public ProgramInfo programInfo(string id, int language)
        {
            try
            {
                string sql = @"select av.version_title 'program_name', av.publish_time, av.article_content
                from IA_AcademicCollaboration.AcademicProgram ap
                join IA_Article.Article ar on ap.article_id = ar.article_id
                join IA_Article.ArticleVersion av on av.article_id = ap.article_id
                join Localization.[Language] la on la.language_id = av.language_id
                join IA_Collaboration.[Partner] pa on pa.partner_id = ap.partner_id
                where la.language_id = @language and ap.program_id = @id";
                ProgramInfo obj = db.Database.SqlQuery<ProgramInfo>(sql, new SqlParameter("language", language),
                        new SqlParameter("id", id)).FirstOrDefault();
                return obj;
            }
            catch (Exception e)
            {
                return new ProgramInfo();
            }
        }
    }
}
