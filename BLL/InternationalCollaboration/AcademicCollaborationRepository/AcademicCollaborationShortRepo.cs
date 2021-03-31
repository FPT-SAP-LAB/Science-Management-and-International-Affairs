using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.InternationalCollaboration.AcademicCollaborationRepository
{
    public class AcademicCollaborationShortRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public BaseServerSideData<ProcedureInfoManager> GetListProcedure(BaseDatatable baseDatatable, string title, int direction, int language)
        {
            try
            {
                string sql = @"select ROW_NUMBER() OVER (ORDER BY a.procedure_id DESC) AS 'no', 
                        a.procedure_id, atv.version_title 'procedure_name',
                        a.full_name, a.language_id, a.article_id from
                        (select pr.procedure_id, ac.full_name, min(av.language_id) as 'language_id', pr.article_id
                        from IA_AcademicCollaboration.[Procedure] pr
                        join IA_Article.Article ar on ar.article_id = pr.article_id
                        join IA_Article.ArticleVersion av on av.article_id = pr.article_id
                        join General.Account ac on ar.account_id = ac.account_id
                        where pr.direction_id = @direction	
                        group by pr.procedure_id, ac.full_name,  pr.article_id) as a 
                        join IA_Article.ArticleVersion atv on a.article_id = atv.article_id 
                        and a.language_id = atv.language_id";
                if (!String.IsNullOrEmpty(title))
                {
                    sql += " where atv.version_title LIKE @title";
                }
                string paging = @" ORDER BY " + baseDatatable.SortColumnName + " "
                            + baseDatatable.SortDirection +
                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT "
                            + baseDatatable.Length + " ROWS ONLY";

                List<ProcedureInfoManager> obj = db.Database.SqlQuery<ProcedureInfoManager>(sql + paging,
                    new SqlParameter("direction", direction), new SqlParameter("title", "%" + title + "%")).ToList();

                int totalRecord = db.Database.SqlQuery<ProcedureInfoManager>(sql,
                    new SqlParameter("direction", direction), new SqlParameter("title", "%" + title + "%")).Count();

                return new BaseServerSideData<ProcedureInfoManager>(obj, totalRecord);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<string> AddProcedure(List<HttpPostedFileBase> files_request, string procedure_title, int direction,
            string content, int number_of_image, int partner_language_type, int account_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    List<string> image_drive_id = new List<string>();
                    List<string> image_drive_data_link = new List<string>();
                    List<Google.Apis.Drive.v3.Data.File> files_upload = new List<Google.Apis.Drive.v3.Data.File>();
                    if (files_request.Count != 0)
                    {
                        if (GoogleDriveService.credential == null && GoogleDriveService.driveService == null)
                        {
                            return new AlertModal<string>(false, "Vui lòng liên hệ với quản trị hệ thống để được cấp quyền");
                        }
                        else
                        {
                            files_upload = GoogleDriveService.UploadIAFile(files_request, procedure_title, 4, false);
                            for (int i = 0; i < number_of_image; i++)
                            {
                                image_drive_id.Add(files_upload[i].Id);
                                image_drive_data_link.Add(files_upload[i].WebViewLink);
                            }
                            for (int i = 0; i < number_of_image; i++)
                            {
                                content = content.Replace("image_" + i, "https://drive.google.com/uc?id=" + image_drive_id[i]);
                            }
                        }
                    }
                    Article article = new Article
                    {
                        need_approved = false,
                        article_status_id = 2,
                        account_id = account_id
                    };
                    db.Articles.Add(article);
                    db.SaveChanges();

                    ArticleVersion articleVersion = new ArticleVersion
                    {
                        article_content = content,
                        publish_time = DateTime.Today,
                        version_title = procedure_title,
                        article_id = article.article_id,
                        language_id = partner_language_type
                    };
                    db.ArticleVersions.Add(articleVersion);

                    Procedure procedure = new Procedure
                    {
                        direction_id = direction,
                        article_id = article.article_id,
                    };
                    db.Procedures.Add(procedure);
                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
        }

        public AlertModal<string> DeleteProcedure(int procedure_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    Procedure procedure = db.Procedures.Find(procedure_id);
                    Article article = db.Articles.Find(procedure.article_id);
                    db.Articles.Remove(article);
                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true, "Xóa thành công");
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
        }

        public ProcedureInfoManager LoadEditProcedure(int procedure_id)
        {
            try
            {
                Procedure procedure = db.Procedures.Find(procedure_id);

                ArticleVersion articleVersion = db.ArticleVersions.
                    Where(x => x.article_id == procedure.article_id).OrderBy(x => x.language_id).FirstOrDefault();
                if (articleVersion != null)
                {
                    ProcedureInfoManager procedureInfoManager = new ProcedureInfoManager
                    {
                        procedure_name = articleVersion.version_title,
                        content = articleVersion.article_content,
                        language_id = articleVersion.language_id
                    };
                    return procedureInfoManager;
                }
                else
                {
                    return new ProcedureInfoManager();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetContentLanguage(int procedure_id, int partner_language_type)
        {
            try
            {
                Procedure procedure = db.Procedures.Where(x => x.procedure_id == procedure_id).FirstOrDefault();
                ArticleVersion articleVersion = db.ArticleVersions.
                    Where(x => x.article_id == procedure.article_id && x.language_id == partner_language_type).FirstOrDefault();
                return articleVersion?.article_content;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<string> SaveEditProcedure(List<HttpPostedFileBase> files_request,
            ProcedureInfoManager procedureInfoManager, int number_of_image, int account_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    List<string> image_drive_id = new List<string>();
                    List<string> image_drive_data_link = new List<string>();
                    List<Google.Apis.Drive.v3.Data.File> files_upload = new List<Google.Apis.Drive.v3.Data.File>();
                    if (files_request.Count != 0)
                    {
                        if (GoogleDriveService.credential == null && GoogleDriveService.driveService == null)
                        {
                            return new AlertModal<string>(false, "Vui lòng liên hệ với quản trị hệ thống để được cấp quyền");
                        }
                        else
                        {
                            files_upload = GoogleDriveService.UploadIAFile(files_request, procedureInfoManager.procedure_name, 4, false);
                            for (int i = 0; i < number_of_image; i++)
                            {
                                image_drive_id.Add(files_upload[i].Id);
                                image_drive_data_link.Add(files_upload[i].WebViewLink);
                            }
                            for (int i = 0; i < number_of_image; i++)
                            {
                                procedureInfoManager.content = procedureInfoManager.content.Replace("image_" + i, "https://drive.google.com/uc?id=" + image_drive_id[i]);
                            }
                        }
                    }
                    Procedure procedure = db.Procedures.Find(procedureInfoManager.procedure_id);

                    Article article = db.Articles.Find(procedure.article_id);
                    if (article != null)
                    {
                        article.account_id = account_id;
                    }
                    ArticleVersion articleVersion = db.ArticleVersions.Where(x => x.article_id == procedure.article_id && x.language_id == procedureInfoManager.language_id).FirstOrDefault();
                    if (articleVersion != null)
                    {
                        articleVersion.article_content = procedureInfoManager.content;
                        articleVersion.version_title = procedureInfoManager.procedure_name;
                    }
                    else
                    {
                        articleVersion = new ArticleVersion
                        {
                            article_id = article.article_id,
                            article_content = procedureInfoManager.content,
                            language_id = procedureInfoManager.language_id,
                            publish_time = DateTime.Today,
                            version_title = procedureInfoManager.procedure_name,
                        };
                        db.ArticleVersions.Add(articleVersion);
                    }
                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
        }
    }
}
