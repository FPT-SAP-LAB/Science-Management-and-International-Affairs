using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
                string sql = @"select ROW_NUMBER() OVER (ORDER BY a.program_id DESC) AS 'no',
                            a.program_id, a.registration_deadline, a.article_id, a.duration, t4.partner_name,
                            t5.full_name, CONVERT(varchar, atv.publish_time, 20) as 'publish_time', atv.version_title as 'program_name'
                            from 
                            (select t1.program_id, CONVERT(varchar,t1.program_start_date, 103) + 
                            ' - ' + CONVERT(varchar,t1.program_end_date, 103) as 'registration_deadline',
                            t1.article_id, min(t3.language_id) as 'language_id',
                            case when cast(getdate() as date) <= t1.program_end_date then 1 else 0 end as 'duration' , t1.partner_id, t2.account_id
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

        public AlertModal<string> AddProgram(List<HttpPostedFileBase> files_request, string program_title, int collab_type,
            int direction, string content, int number_of_image, int program_language, int account_id,
            string program_partner, string add_program_start_date, string add_program_end_date, string note)
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
                            files_upload = GoogleDriveService.UploadIAFile(files_request, program_title, 4, false);
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
                        publish_time = DateTime.Now,
                        version_title = program_title,
                        article_id = article.article_id,
                        language_id = program_language
                    };
                    db.ArticleVersions.Add(articleVersion);

                    AcademicProgram academicProgram = new AcademicProgram
                    {
                        article_id = article.article_id,
                        note = note,
                        collab_type_id = collab_type,
                        direction_id = direction,
                        program_start_date = DateTime.ParseExact(add_program_start_date.Trim(), "dd/MM/yyyy", null),
                        program_end_date = DateTime.ParseExact(add_program_end_date.Trim(), "dd/MM/yyyy", null)
                    };

                    if (direction == 1)
                    {
                        int partner_id = Int32.Parse(program_partner.Split('/').LastOrDefault());
                        academicProgram.partner_id = partner_id;
                    }

                    db.AcademicPrograms.Add(academicProgram);
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

        public ProgramInfoManager LoadEditProgram(int program_id)
        {
            try
            {
                AcademicProgram academicProgram = db.AcademicPrograms.Find(program_id);

                ArticleVersion articleVersion = db.ArticleVersions.
                    Where(x => x.article_id == academicProgram.article_id).OrderBy(x => x.language_id).FirstOrDefault();
                if (articleVersion != null)
                {
                    ProgramInfoManager programInfoManager = new ProgramInfoManager
                    {
                        program_name = articleVersion.version_title,
                        language_id = articleVersion.language_id,
                        content = articleVersion.article_content,
                        note = academicProgram.note,
                        registration_deadline = academicProgram.program_start_date.ToString("dd/MM/yyyy") + " - " + academicProgram.program_end_date.ToString("dd/MM/yyyy")
                    };
                    if (academicProgram.partner_id != null)
                    {
                        programInfoManager.partner_id = academicProgram.partner_id;
                        programInfoManager.partner_name = db.Partners.Find(academicProgram.partner_id).partner_name;
                    }
                    return programInfoManager;
                }
                else
                {
                    return new ProgramInfoManager();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<string> DeleteProgram(int article_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.Articles.Remove(db.Articles.Find(article_id));
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

        public AlertModal<string> SaveEditProgram(List<HttpPostedFileBase> files_request, int program_id, string program_title,
            string content, int number_of_image, int program_language, string program_partner, string edit_program_start_date,
            string edit_program_end_date, string note,
            int direction, int account_id)
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
                            files_upload = GoogleDriveService.UploadIAFile(files_request, program_title, 4, false);
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
                    AcademicProgram academicProgram = db.AcademicPrograms.Find(program_id);
                    academicProgram.program_start_date = DateTime.ParseExact(edit_program_start_date.Trim(), "dd/MM/yyyy", null);
                    academicProgram.program_end_date = DateTime.ParseExact(edit_program_end_date.Trim(), "dd/MM/yyyy", null);
                    academicProgram.note = note;
                    if (direction == 1)
                    {
                        int partner_id = Int32.Parse(program_partner.Split('/').LastOrDefault());
                        academicProgram.partner_id = partner_id;
                    }

                    Article article = db.Articles.Find(academicProgram.article_id);
                    if (article != null)
                    {
                        article.account_id = account_id;
                    }
                    ArticleVersion articleVersion = db.ArticleVersions.Where(x => x.article_id ==
                    academicProgram.article_id && x.language_id == program_language).FirstOrDefault();
                    if (articleVersion != null)
                    {
                        articleVersion.article_content = content;
                        articleVersion.version_title = program_title;
                        articleVersion.publish_time = DateTime.Now;
                    }
                    else
                    {
                        articleVersion = new ArticleVersion
                        {
                            article_id = article.article_id,
                            article_content = content,
                            language_id = program_language,
                            publish_time = DateTime.Now,
                            version_title = program_title,
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

        public ArticleVersion LoadProgramDetailLanguage(int program_id, int language_id)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                AcademicProgram academicProgram = db.AcademicPrograms.Where(x => x.program_id == program_id).FirstOrDefault();
                ArticleVersion articleVersion = db.ArticleVersions.
                    Where(x => x.article_id == academicProgram.article_id && x.language_id == language_id).FirstOrDefault();
                return articleVersion ?? new ArticleVersion();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
