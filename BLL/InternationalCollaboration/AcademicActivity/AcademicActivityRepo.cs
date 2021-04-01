using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

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
                    @"SELECT aa.activity_id, CONCAT(aa.activity_id, '$',
                        av.version_title) as 'activity_id_activity_name', 
                        [at].activity_type_name,
                        CASE 
	                        WHEN GETDATE() < aa.activity_date_start THEN N'Chưa hoạt động'
	                        WHEN aa.activity_date_start <= GETDATE() and GETDATE() <= aa.activity_date_end THEN N'Đang hoạt động'
	                        WHEN GETDATE() > aa.activity_date_end THEN N'Đã kết thúc'
                        END as 'academic_status_name'
                        FROM SMIA_AcademicActivity.AcademicActivity aa inner join SMIA_AcademicActivity.AcademicActivityTypeLanguage [at]
                        on aa.activity_type_id = [at].activity_type_id inner join SMIA_AcademicActivity.AcademicActivityLanguage al 
                        on aa.activity_id = al.activity_id inner join SMIA_AcademicActivity.ActivityInfo ai
                        on ai.activity_id = aa.activity_id and ai.main_article = 1 inner join IA_Article.Article ar
                        on ar.article_id = ai.article_id inner join IA_Article.ArticleVersion av
                        on av.article_id = ai.article_id and al.language_id = av.language_id and at.language_id = al.language_id
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
        public bool AddAA(baseAA obj, int language_id, Authen.LoginRepo.User u)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ENTITIES.AcademicActivity aa = db.AcademicActivities.Add(new ENTITIES.AcademicActivity
                    {
                        activity_type_id = obj.activity_type_id,
                        activity_date_start = DateTime.ParseExact(obj.from, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        activity_date_end = DateTime.ParseExact(obj.to, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    });
                    db.SaveChanges();
                    db.AcademicActivityLanguages.Add(new ENTITIES.AcademicActivityLanguage
                    {
                        language_id = language_id,
                        activity_id = aa.activity_id,
                        location = obj.location
                    });
                    ENTITIES.Article ar = db.Articles.Add(new ENTITIES.Article
                    {
                        account_id = u.account.account_id,
                        article_status_id = 1,
                        need_approved = false
                    });
                    db.SaveChanges();
                    db.ActivityInfoes.Add(new ENTITIES.ActivityInfo
                    {
                        activity_id = aa.activity_id,
                        article_id = ar.article_id,
                        main_article = true
                    });
                    db.ArticleVersions.Add(new ENTITIES.ArticleVersion
                    {
                        article_id = ar.article_id,
                        publish_time = DateTime.Now,
                        version_title = obj.activity_name,
                        language_id = language_id,
                        article_content = ""
                    });
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public List<AcademicActivityTypeLanguage> getType(int language_id)
        {
            try
            {
                string sql = @"select al.language_id, al.activity_type_id,al.activity_type_name from SMIA_AcademicActivity.AcademicActivityTypeLanguage al where al.language_id = @language_id";
                List<AcademicActivityTypeLanguage> data = db.Database.SqlQuery<AcademicActivityTypeLanguage>(sql, new SqlParameter("language_id", language_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<AcademicActivityTypeLanguage>();
            }
        }
        public baseAA GetbaseAA(int id)
        {
            try
            {
                string sql = @"SELECT av.version_title as 'activity_name', [aa].activity_type_id, [al].[location], cast(aa.activity_date_start as nvarchar) as 'from', cast(aa.activity_date_end as nvarchar) as 'to'
                        FROM SMIA_AcademicActivity.AcademicActivity aa inner join SMIA_AcademicActivity.AcademicActivityLanguage al 
                        on aa.activity_id = al.activity_id inner join SMIA_AcademicActivity.ActivityInfo ai
                        on ai.activity_id = aa.activity_id and ai.main_article = 1 inner join IA_Article.Article ar
                        on ar.article_id = ai.article_id inner join IA_Article.ArticleVersion av
                        on av.article_id = ai.article_id and al.language_id = av.language_id
                        WHERE al.language_id = 1 and aa.activity_id = @id";
                baseAA obj = db.Database.SqlQuery<baseAA>(sql,
                            new SqlParameter("id", id)).FirstOrDefault();
                if (obj != null)
                {
                    obj.from = changeFormatDate(obj.from);
                    obj.to = changeFormatDate(obj.to);
                }
                return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new baseAA();
            }
        }
        public string changeFormatDate(string date)
        {
            string[] sp = date.Split('-');
            return sp[2] + '/' + sp[1] + '/' + sp[0];
        }
        public bool updateBaseAAA(int id, int activity_type_id, string activity_name, string location, string from, string to, int language_id, HttpPostedFileBase img)
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
                    AcademicActivityLanguage al = db.AcademicActivityLanguages.Where(x => x.activity_id == id && x.language_id == language_id).FirstOrDefault();
                    al.location = location;
                    db.Entry(al).State = EntityState.Modified;
                    db.SaveChanges();
                    ActivityInfo ai = db.ActivityInfoes.Where(x => x.activity_id == id && x.main_article == true).FirstOrDefault();
                    ArticleVersion av = db.ArticleVersions.Where(x => x.article_id == ai.article_id && x.language_id == language_id).FirstOrDefault();
                    av.version_title = activity_name;
                    db.SaveChanges();
                    if (aa.file_id == null)
                    {
                        Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadIAFile(img, "Banner - " + activity_name, 5, false);
                        File file = new File();
                        file.name = img.FileName;
                        file.link = f.WebViewLink;
                        file.file_drive_id = f.Id;
                        File ff = db.Files.Add(file);
                        db.SaveChanges();
                        aa.file_id = ff.file_id;
                        db.Entry(aa).State = EntityState.Modified;
                    }
                    else
                    {
                        Google.Apis.Drive.v3.Data.File fr = GoogleDriveService.UpdateFile(img.FileName, img.InputStream, img.ContentType, aa.File.file_drive_id);
                        File f = db.Files.Find(aa.file_id);
                        f.name = img.FileName;
                        db.Entry(f).State = EntityState.Modified;
                    }
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
        public bool updateBaseAA(int id, int activity_type_id, string activity_name, string location, string from, string to, int language_id)
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
                    AcademicActivityLanguage al = db.AcademicActivityLanguages.Where(x => x.activity_id == id && x.language_id == language_id).FirstOrDefault();
                    al.location = location;
                    db.Entry(al).State = EntityState.Modified;
                    db.SaveChanges();
                    ActivityInfo ai = db.ActivityInfoes.Where(x => x.activity_id == id && x.main_article == true).FirstOrDefault();
                    ArticleVersion av = db.ArticleVersions.Where(x => x.article_id == ai.article_id && x.language_id == language_id).FirstOrDefault();
                    av.version_title = activity_name;
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
                    List<ActivityInfo> ai = db.ActivityInfoes.Where(x => x.activity_id == id).ToList();
                    foreach (ActivityInfo i in ai)
                    {
                        Article a = db.Articles.Find(i.article_id);
                        db.Articles.Remove(a);
                    }
                    db.SaveChanges();
                    ENTITIES.AcademicActivity aa = db.AcademicActivities.Find(id);
                    db.AcademicActivities.Remove(aa);
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool clone(cloneBase obj)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ENTITIES.AcademicActivity aa = db.AcademicActivities.Add(new ENTITIES.AcademicActivity
                    {
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
                    Article ar = db.Articles.Add(new ENTITIES.Article
                    {
                        account_id = 1,
                        article_status_id = 1,
                        need_approved = false
                    });
                    db.SaveChanges();
                    db.ActivityInfoes.Add(new ENTITIES.ActivityInfo
                    {
                        activity_id = aa.activity_id,
                        article_id = ar.article_id,
                        main_article = true
                    });
                    ArticleVersion av_new = db.ArticleVersions.Add(new ENTITIES.ArticleVersion
                    {
                        article_id = ar.article_id,
                        publish_time = DateTime.Now,
                        version_title = obj.activity_name,
                        language_id = 1,
                        article_content = ""
                    });
                    db.SaveChanges();
                    int activity_id = aa.activity_id;
                    cloneContent(obj, av_new, activity_id);
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public void cloneContent(cloneBase obj, ArticleVersion av_new, int activity_id)
        {
            if (obj.content != null)
            {
                cloneKP(obj, obj.content.Contains("KP"), activity_id);
                cloneDTC(obj, obj.content.Contains("DTC"), activity_id);
                cloneND(obj, obj.content.Contains("ND"), activity_id, av_new);
                cloneTD(obj, obj.content.Contains("TD"), activity_id);
            }
        }
        public void cloneKP(cloneBase obj, bool start, int activity_id)
        {
            if (start)
            {
                List<ActivityExpenseCategory> activityExpenses_old = db.ActivityExpenseCategories.Where(x => x.activity_id == obj.id).ToList();
                foreach (ActivityExpenseCategory arc in activityExpenses_old)
                {
                    ActivityExpenseCategory activityExpense_new = db.ActivityExpenseCategories.Add(new ActivityExpenseCategory
                    {
                        activity_id = activity_id,
                        office_id = arc.office_id,
                        expense_category_name = arc.expense_category_name
                    });
                    db.SaveChanges();
                    List<ActivityExpenseDetail> expenseDetails_old = db.ActivityExpenseDetails.Where(x => x.expense_category_id == arc.expense_category_id).ToList();
                    foreach (ActivityExpenseDetail ard in expenseDetails_old)
                    {
                        db.ActivityExpenseDetails.Add(new ActivityExpenseDetail
                        {
                            expense_category_id = activityExpense_new.expense_category_id,
                            expense_price = ard.expense_price,
                            expense_quantity = ard.expense_quantity,
                            expense_type_id = ard.expense_type_id,
                            note = ard.note
                        });
                    }
                    db.SaveChanges();
                }
            }
        }
        public void cloneTD(cloneBase obj, bool start, int activity_id)
        {
            if (start)
            {
                List<AcademicActivityPhase> activityPhaseOld = db.AcademicActivityPhases.Where(x => x.activity_id == obj.id).ToList();
                foreach (AcademicActivityPhase aap in activityPhaseOld)
                {
                    AcademicActivityPhase aap_new = db.AcademicActivityPhases.Add(new AcademicActivityPhase
                    {
                        created_by = 1,
                        activity_id = activity_id
                    });
                    db.SaveChanges();
                    Form f_old = db.Forms.Where(x => x.phase_id == aap.phase_id).FirstOrDefault();
                    Form f_new = db.Forms.Add(new Form
                    {
                        phase_id = aap_new.phase_id,
                        title = f_old.title,
                        title_description = f_old.title_description
                    });
                    db.SaveChanges();
                    List<Question> ques_old = db.Questions.Where(x => x.form_id == f_old.form_id).ToList();
                    foreach (Question q in ques_old)
                    {
                        db.Questions.Add(new Question
                        {
                            answer_type_id = q.answer_type_id,
                            is_compulsory = q.is_compulsory,
                            title = q.title,
                            form_id = f_new.form_id
                        });
                    }
                    db.SaveChanges();
                    List<AcademicActivityPhaseLanguage> activityPhaseLanguages_old = db.AcademicActivityPhaseLanguages.Where(x => x.phase_id == aap.phase_id).ToList();
                    foreach (AcademicActivityPhaseLanguage aapl in activityPhaseLanguages_old)
                    {
                        db.AcademicActivityPhaseLanguages.Add(new AcademicActivityPhaseLanguage
                        {
                            language_id = aapl.language_id,
                            phase_name = aapl.phase_name,
                            phase_id = aap_new.phase_id
                        });
                    }
                    db.SaveChanges();
                }
            }
        }
        public void cloneDTC(cloneBase obj, bool start, int activity_id)
        {
            if (start)
            {
                List<ActivityPartner> partners = db.ActivityPartners.Where(x => x.activity_id == activity_id).ToList();
                foreach (ActivityPartner ap in partners)
                {
                    db.ActivityPartners.Add(new ActivityPartner
                    {
                        activity_id = activity_id,
                        partner_scope_id = ap.partner_scope_id
                    });
                }
                db.SaveChanges();
            }
        }
        public void cloneND(cloneBase obj, bool start, int activity_id, ArticleVersion av_new)
        {
            if (start)
            {
                List<int> ids_article = new List<int>();
                List<ActivityInfo> activityInfos = db.ActivityInfoes.Where(x => x.activity_id == obj.id && x.main_article == false).ToList();
                foreach (ActivityInfo info in activityInfos)
                {
                    Article a = db.Articles.Add(new Article
                    {
                        account_id = 1,
                        article_status_id = 1,
                        need_approved = false
                    });
                    db.SaveChanges();
                    List<ArticleVersion> old = db.ArticleVersions.Where(x => x.article_id == info.article_id).ToList();
                    foreach (ArticleVersion o in old)
                    {
                        db.ArticleVersions.Add(new ArticleVersion
                        {
                            article_id = a.article_id,
                            publish_time = DateTime.Now,
                            version_title = o.version_title,
                            language_id = o.language_id,
                            article_content = o.article_content
                        });
                    }
                    db.SaveChanges();
                    db.ActivityInfoes.Add(new ActivityInfo
                    {
                        article_id = a.article_id,
                        activity_id = activity_id,
                        main_article = info.main_article
                    });
                    db.SaveChanges();
                }
                ActivityInfo ai = db.ActivityInfoes.Where(x => x.activity_id == obj.id && x.main_article == true).FirstOrDefault();
                ArticleVersion av = db.ArticleVersions.Where(x => x.article_id == ai.article_id).FirstOrDefault();
                av_new.article_content = av.article_content;
                db.Entry(av_new).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public class ListAA
        {
            public int activity_id { get; set; }
            public string activity_id_activity_name { get; set; }
            public string activity_type_name { get; set; }
            public string academic_status_name { get; set; }
        }
        public class baseAA
        {
            public string activity_name { get; set; }
            public int activity_type_id { get; set; }
            public string location { get; set; }
            public string from { get; set; }
            public string to { get; set; }
        }
        public class cloneBase : baseAA
        {
            public int id { get; set; }
            public List<string> content { get; set; }
        }
    }
}