using BLL.InternationalCollaboration.AcademicCollaborationRepository;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicActivity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class DetailOfAcademicActivityRepo
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
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
                if (data != null)
                {
                    data.from = changeFormatDate(data.from);
                    data.to = changeFormatDate(data.to);
                }
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new baseDetail();
            }
        }
        public baseDetail getDetailFirst(int language_id, int activity_id)
        {
            try
            {
                string sql = @"SELECT av.version_title as 'activity_name', [aa].activity_type_id, [al].[location], cast(aa.activity_date_start as nvarchar) as 'from', cast(aa.activity_date_end as nvarchar) as 'to', al.language_id, av.article_content , ar.article_status_id
                                    FROM SMIA_AcademicActivity.AcademicActivity aa inner join SMIA_AcademicActivity.AcademicActivityLanguage al 
                                    on aa.activity_id = al.activity_id inner join SMIA_AcademicActivity.ActivityInfo ai
                                    on ai.activity_id = aa.activity_id and ai.main_article = 1 inner join IA_Article.Article ar
                                    on ar.article_id = ai.article_id inner join (select av1.article_id, av1.language_id, av2.version_title,av2.article_content from 
						(select min(language_id) 'language_id', article_id from IA_Article.ArticleVersion group by article_id) as av1
						inner join
						IA_Article.ArticleVersion av2 on av1.article_id = av2.article_id and av1.language_id = av2.language_id) as av
                        on av.article_id = ai.article_id and al.language_id = av.language_id
                                    WHERE [aa].activity_id = @activity_id and ai.main_article = 1 and av.language_id = @language_id";
                baseDetail data = db.Database.SqlQuery<baseDetail>(sql, new SqlParameter("language_id", language_id),
                                                                        new SqlParameter("activity_id", activity_id)).FirstOrDefault();
                if (data != null)
                {
                    data.from = changeFormatDate(data.from);
                    data.to = changeFormatDate(data.to);
                    return data;
                }
                else
                {
                    int temp_language = language_id == 1 ? 2 : 1;
                    data = db.Database.SqlQuery<baseDetail>(sql, new SqlParameter("language_id", temp_language),
                                                                        new SqlParameter("activity_id", activity_id)).FirstOrDefault();
                    data.from = changeFormatDate(data.from);
                    data.to = changeFormatDate(data.to);
                    return data;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
                                on ai.activity_id = aa.activity_id inner join IA_Article.Article ar
                                on ar.article_id = ai.article_id inner join IA_Article.ArticleVersion av
                                on av.article_id = ai.article_id and al.language_id = av.language_id
                                WHERE al.language_id = @language_id  AND [aa].activity_id = @activity_id and ai.main_article = 0";
                List<subContent> data = db.Database.SqlQuery<subContent>(sql, new SqlParameter("language_id", language_id),
                                                                              new SqlParameter("activity_id", activity_id)).ToList();
                if (data.Count == 0)
                {
                    List<ActivityInfo> listAI = db.ActivityInfoes.Where(x => x.activity_id == activity_id && x.main_article == false).ToList();
                    foreach (ActivityInfo ai in listAI)
                    {
                        db.ArticleVersions.Add(new ArticleVersion
                        {
                            article_id = ai.article_id,
                            language_id = language_id,
                            version_title = String.Empty,
                            article_content = String.Empty,
                            publish_time = DateTime.Now
                        });
                    }
                    db.SaveChanges();
                }
                else
                {
                    int opplanguage_id = language_id == 1 ? 2 : 1;
                    List<subContent> datarv = db.Database.SqlQuery<subContent>(sql, new SqlParameter("language_id", opplanguage_id),
                                                              new SqlParameter("activity_id", activity_id)).ToList();
                    List<int> list_id_rv = datarv.Select(x => x.article_id).ToList();
                    foreach (subContent item in data)
                    {
                        list_id_rv.Remove(item.article_id);
                    }
                    foreach (int item in list_id_rv)
                    {
                        db.ArticleVersions.Add(new ArticleVersion
                        {
                            article_id = item,
                            language_id = language_id,
                            version_title = String.Empty,
                            article_content = String.Empty,
                            publish_time = DateTime.Now
                        });
                    }
                    db.SaveChanges();
                }
                data = db.Database.SqlQuery<subContent>(sql, new SqlParameter("language_id", language_id),
                                                             new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<subContent>();
            }
        }
        public List<AcademicActivityTypeLanguage> getType(int language_id)
        {
            try
            {
                string sql = @"select al.language_id,al.activity_type_id,al.activity_type_name from SMIA_AcademicActivity.AcademicActivityTypeLanguage al where al.language_id = @language_id";
                List<AcademicActivityTypeLanguage> data = db.Database.SqlQuery<AcademicActivityTypeLanguage>(sql, new SqlParameter("language_id", language_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<AcademicActivityTypeLanguage>();
            }
        }
        public bool updateDetail(InfoSumDetail data, Authen.LoginRepo.User u)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    AcademicActivityRepo aaRepo = new AcademicActivityRepo();
                    infoDetail detail = data.infoDetail;
                    string sql = @"select ai.* from SMIA_AcademicActivity.ActivityInfo ai
                                    inner join IA_Article.Article a on ai.article_id = a.article_id
                                    inner join IA_Article.ArticleVersion av on av.article_id = a.article_id
                                    where ai.activity_id = @activity_id and language_id = @language_id";
                    ActivityInfo cons = db.Database.SqlQuery<ActivityInfo>(sql, new SqlParameter("activity_id", detail.activity_id),
                        new SqlParameter("language_id", detail.language_id)).FirstOrDefault();
                    if (cons == null)
                    {
                        db.AcademicActivityLanguages.Add(new AcademicActivityLanguage
                        {
                            activity_id = data.infoDetail.activity_id,
                            language_id = detail.language_id,
                            location = detail.location
                        });
                        db.SaveChanges();
                        ActivityInfo ai = db.ActivityInfoes.Where(x => x.activity_id == detail.activity_id && x.main_article == true).FirstOrDefault();
                        db.ArticleVersions.Add(new ArticleVersion
                        {
                            publish_time = DateTime.Now,
                            version_title = detail.activity_name,
                            article_content = detail.article_content,
                            article_id = ai.article_id,
                            language_id = detail.language_id
                        });
                        db.SaveChanges();
                    }
                    else
                    {
                        bool res = aaRepo.updateBaseAA(detail.activity_id, detail.activity_type_id, detail.activity_name, detail.location, detail.from, detail.to, detail.language_id);
                        ActivityInfo ai = db.ActivityInfoes.Where(x => x.activity_id == detail.activity_id && x.main_article == true).FirstOrDefault();
                        ArticleVersion av = db.ArticleVersions.Where(x => x.article_id == ai.article_id && x.language_id == detail.language_id).FirstOrDefault();
                        av.article_content = detail.article_content;
                        db.Entry(av).State = EntityState.Modified;
                        db.SaveChanges();
                        updateListSubContent(data.subContent, detail.language_id, detail.activity_id, u, detail.article_status_id);
                        db.SaveChanges();
                    }
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
        public void updateListSubContent(List<subContent> data, int language_id, int activity_id, Authen.LoginRepo.User u, int article_status_id)
        {
            List<subContent> old = getSubContents(language_id, activity_id);
            List<int> old_id = old.Select(x => x.article_id).ToList();
            if (data != null)
            {
                foreach (subContent x in data)
                {
                    if (!old_id.Contains(x.article_id))
                    {
                        insertSubContent(x, activity_id, language_id, u, article_status_id);
                    }
                    else
                    {
                        updateSubContent(x, language_id);
                    }
                    old_id.Remove(x.article_id);
                }
                foreach (int i in old_id)
                {
                    ArticleVersion av = db.ArticleVersions.Where(z => z.article_id == i && z.language_id == language_id).FirstOrDefault();
                    db.ArticleVersions.Remove(av);
                    ActivityInfo ai = db.ActivityInfoes.Where(z => z.article_id == i && z.main_article == false).FirstOrDefault();
                    db.ActivityInfoes.Remove(ai);
                    Article a = db.Articles.Find(i);
                    db.Articles.Remove(a);
                }
                db.SaveChanges();
            }
        }
        public void updateSubContent(subContent data, int language_id)
        {
            ArticleVersion av = db.ArticleVersions.Where(x => x.article_id == data.article_id && x.language_id == language_id).FirstOrDefault();
            av.version_title = data.version_title;
            av.article_content = data.article_content;
            db.Entry(av).State = EntityState.Modified;
        }
        public void insertSubContent(subContent data, int activity_id, int language_id, Authen.LoginRepo.User u, int article_status_id)
        {
            Article a = db.Articles.Add(new Article
            {
                account_id = u.account.account_id,
                article_status_id = article_status_id
            });
            db.SaveChanges();
            db.ActivityInfoes.Add(new ActivityInfo
            {
                activity_id = activity_id,
                article_id = a.article_id,
                main_article = false
            });
            db.ArticleVersions.Add(new ArticleVersion
            {
                publish_time = DateTime.Now,
                version_title = data.version_title,
                article_content = data.article_content,
                article_id = a.article_id,
                language_id = language_id
            });
        }
        public bool changeStatusAA(int activity_id, int status)
        {
            try
            {
                List<ActivityInfo> ai = db.ActivityInfoes.Where(x => x.activity_id == activity_id).ToList();
                foreach (ActivityInfo i in ai)
                {
                    Article a = db.Articles.Find(i.article_id);
                    a.article_status_id = status;
                    db.Entry(a).State = EntityState.Modified;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        public List<basePartner> getDatatableDTC(int activity_id)
        {
            try
            {
                string sql = @"SELECT ap.activity_partner_id, p.partner_id ,p.partner_name, 
                                    cast(FORMAT(ap.cooperation_date_start, 'dd/MM/yyyy') as nvarchar) as 'from',
                                    cast(FORMAT(ap.cooperation_date_end, 'dd/MM/yyyy') as nvarchar) as 'to',
                                    CONCAT(ap.activity_partner_id,'$',ap.contact_point_name) as 'contact_point' ,ap.sponsor
                                    FROM SMIA_AcademicActivity.ActivityPartner ap inner join IA_Collaboration.PartnerScope mps
                                    on ap.partner_scope_id = mps.partner_scope_id inner join IA_Collaboration.[Partner] p
                                    on p.partner_id = mps.partner_id where ap.activity_id = @activity_id";
                List<basePartner> data = db.Database.SqlQuery<basePartner>(sql, new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<basePartner>();
            }
        }
        public List<InternalUnit> getUnits()
        {
            try
            {
                List<InternalUnit> data = db.InternalUnits.ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<InternalUnit>();
            }
        }
        public ContactInfo getContact(int activity_partner_id)
        {
            try
            {
                string sql = @"SELECT ap.contact_point_name, ap.contact_point_phone, ap.contact_point_email
                                    FROM SMIA_AcademicActivity.ActivityPartner ap 
                                    WHERE ap.activity_partner_id = @activity_partner_id";
                ContactInfo data = db.Database.SqlQuery<ContactInfo>(sql, new SqlParameter("activity_partner_id", activity_partner_id)).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new ContactInfo();
            }
        }
        public AlertModal<string> saveActivityPartner(HttpPostedFileBase evidence_file, string folder_name, SaveActivityPartner activityPartner)
        {
            using (DbContextTransaction dbContext = db.Database.BeginTransaction())
            {
                try
                {
                    if (checkDuplicatePartnerScope(activityPartner))
                    {
                        AcademicCollaborationRepo academicCollaborationRepo = new AcademicCollaborationRepo();
                        //upload file if exist
                        //upload file
                        Google.Apis.Drive.v3.Data.File f = new Google.Apis.Drive.v3.Data.File();
                        if (evidence_file != null)
                        {
                            f = academicCollaborationRepo.uploadEvidenceFile(evidence_file, "Collab partner - " + folder_name, 5, false);
                        }

                        File file = new File();
                        //save file if null, else just save activityPartner
                        if (f != null)
                        {
                            file = academicCollaborationRepo.saveFile(f, evidence_file);
                        }

                        //update to PartnerScope
                        PartnerScope partnerScope = updatePartnerScope(activityPartner.partner_id, activityPartner.scope_id, academicCollaborationRepo);

                        saveActivityPartner(file, partnerScope, activityPartner);
                        dbContext.Commit();
                        return new AlertModal<string>(null, true, "Thành công", "Thêm đối tác đồng tổ chức thành công.");
                    }
                    else
                    {
                        return new AlertModal<string>(null, false, "Lỗi", "Đối tác đã được thêm vào danh sách trước đó.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    dbContext.Rollback();
                    return new AlertModal<string>(null, false, "Lỗi", "Có lỗi xảy ra.");
                }
            }
        }
        public PartnerScope updatePartnerScope(int partner_id, int scope_id, AcademicCollaborationRepo academicCollaborationRepo)
        {
            PartnerScope partnerScope;
            try
            {
                partnerScope = db.PartnerScopes.Where(x => x.partner_id == partner_id && x.scope_id == scope_id).FirstOrDefault();
                if (partnerScope != null)
                {
                    academicCollaborationRepo.increaseReferenceCountOfPartnerScope(partnerScope);
                }
                else
                {
                    partnerScope = academicCollaborationRepo.savePartnerScope(partner_id, scope_id);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return partnerScope;
        }
        public void saveActivityPartner(File file, PartnerScope partnerScope, SaveActivityPartner activityPartner)
        {
            try
            {
                ActivityPartner ap = new ActivityPartner();
                ap.sponsor = activityPartner.sponsor;
                if (activityPartner.contact_point_name != null) ap.contact_point_name = activityPartner.contact_point_name;
                if (activityPartner.contact_point_email != null) ap.contact_point_email = activityPartner.contact_point_email;
                if (activityPartner.contact_point_phone != null) ap.contact_point_phone = activityPartner.contact_point_phone;
                if (activityPartner.cooperation_date_start != null) ap.cooperation_date_start = activityPartner.cooperation_date_start;
                if (activityPartner.cooperation_date_end != null) ap.cooperation_date_end = activityPartner.cooperation_date_end;
                ap.activity_id = activityPartner.activity_id;
                ap.partner_scope_id = partnerScope.partner_scope_id;
                if (file.file_id != 0) ap.file_id = file.file_id;
                db.ActivityPartners.Add(ap);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public AlertModal<ActivityPartner_Ext> getActivityPartner(int activity_partner_id)
        {
            try
            {
                var sql = @"select 
                            ap.activity_partner_id, p.partner_id , p.partner_name, 
                            ap.sponsor, 
                            cs.scope_id, cs.scope_abbreviation, cs.scope_name,
                            ap.cooperation_date_start, 
                            ap.cooperation_date_end,
                            f.file_id, f.name 'file_name',
                            ap.contact_point_name, ap.contact_point_email, ap.contact_point_phone
                            from 
                            SMIA_AcademicActivity.ActivityPartner ap 
                            left join IA_Collaboration.PartnerScope mps on ap.partner_scope_id = mps.partner_scope_id 
                            left join IA_Collaboration.[Partner] p on p.partner_id = mps.partner_id
                            left join IA_Collaboration.CollaborationScope cs on cs.scope_id = mps.scope_id
                            left join General.[File] f on f.file_id = ap.file_id
                            where ap.activity_partner_id = @activity_partner_id";
                ActivityPartner_Ext activityPartner = db.Database.SqlQuery<ActivityPartner_Ext>(sql,
                    new SqlParameter("activity_partner_id", activity_partner_id)).FirstOrDefault();
                if (activityPartner != null)
                {
                    return new AlertModal<ActivityPartner_Ext>(activityPartner, true, null, null);
                }
                else
                {
                    return new AlertModal<ActivityPartner_Ext>(null, false, "Lỗi", "Có lỗi xảy ra khi lấy thông tin đơn vị đồng tổ chức.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public AlertModal<string> updateActivityPartner(HttpPostedFileBase evidence_file, string folder_name, SaveActivityPartner saveActivityPartner)
        {
            using (DbContextTransaction dbContext = db.Database.BeginTransaction())
            {
                try
                {
                    if (checkDuplicatePartnerScope(saveActivityPartner))
                    {
                        //update file
                        ActivityPartner activityPartner = db.ActivityPartners.Find(saveActivityPartner.activity_partner_id);
                        AcademicCollaborationRepo academicCollaborationRepo = new AcademicCollaborationRepo();
                        Google.Apis.Drive.v3.Data.File f;
                        File old_file = db.Files.Find(activityPartner.file_id);
                        File new_file = new File();
                        if (evidence_file != null)
                        {
                            if (old_file != null)
                            {
                                //update file on Google Drive
                                f = GoogleDriveService.UpdateFile(evidence_file.FileName, evidence_file.InputStream, evidence_file.ContentType, old_file.file_drive_id);
                                new_file = academicCollaborationRepo.saveFile(f, evidence_file);
                            }
                            else
                            {
                                //upload to Goolge Drive
                                f = academicCollaborationRepo.uploadEvidenceFile(evidence_file, "Collab partner - " + folder_name, 5, false);
                                new_file = academicCollaborationRepo.saveFile(f, evidence_file);
                            }
                        }
                        else
                        {
                            if (old_file != null)
                            {
                                //delete corressponding in gg drive
                                GoogleDriveService.DeleteFile(old_file.file_drive_id);
                                //delete corressponding from File
                                new_file = removeFile(old_file);
                            }
                        }
                        //update file_id null in coress ActivityPartner
                        updateActivityPartner(activityPartner, saveActivityPartner, new_file);
                        dbContext.Commit();
                        return new AlertModal<string>(null, true, "Thành công", "Chỉnh sửa thông tin đơn vị đồng tổ chức thành công.");
                    }
                    else
                    {
                        return new AlertModal<string>(null, false, "Lỗi", "Đối tác đã được thêm vào danh sách trước đó.");
                    }
                }
                catch (Exception e)
                {
                    dbContext.Rollback();
                    throw e;
                }
            }
        }
        public File removeFile(File file)
        {
            try
            {
                db.Files.Remove(file);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }
        public void updateActivityPartner(ActivityPartner activityPartner, SaveActivityPartner saveActivityPartner, File file)
        {
            try
            {
                AcademicCollaborationRepo academicCollaborationRepo = new AcademicCollaborationRepo();
                //update PartnerScope
                PartnerScope partnerScope = updatePartnerScope(saveActivityPartner.partner_id, saveActivityPartner.scope_id, academicCollaborationRepo);

                ActivityPartner ap = new ActivityPartner();
                ap.sponsor = saveActivityPartner.sponsor;
                if (saveActivityPartner.contact_point_name != null) ap.contact_point_name = saveActivityPartner.contact_point_name;
                if (saveActivityPartner.contact_point_email != null) ap.contact_point_email = saveActivityPartner.contact_point_email;
                if (saveActivityPartner.contact_point_phone != null) ap.contact_point_phone = saveActivityPartner.contact_point_phone;
                if (saveActivityPartner.cooperation_date_start != null) ap.cooperation_date_start = saveActivityPartner.cooperation_date_start;
                if (saveActivityPartner.cooperation_date_end != null) ap.cooperation_date_end = saveActivityPartner.cooperation_date_end;
                ap.partner_scope_id = partnerScope.partner_scope_id;
                if (file != null) ap.file_id = file.file_id;

                activityPartner.sponsor = ap.sponsor;
                activityPartner.contact_point_name = ap.contact_point_name;
                activityPartner.contact_point_email = ap.contact_point_email;
                activityPartner.contact_point_phone = ap.contact_point_phone;
                activityPartner.cooperation_date_start = ap.cooperation_date_start;
                activityPartner.cooperation_date_end = ap.cooperation_date_end;
                activityPartner.partner_scope_id = ap.partner_scope_id;
                activityPartner.file_id = ap.file_id;
                db.SaveChanges();

                //old partner_scope_id vs new partner_scope_id
                if (activityPartner.partner_scope_id != partnerScope.partner_scope_id)
                {
                    //decrease re_cou of old partner_scope
                    PartnerScope old_partner_scope = db.PartnerScopes.Find(activityPartner.partner_scope_id);
                    academicCollaborationRepo.decreaseReferenceCountOfPartnerScope(old_partner_scope);
                    if (old_partner_scope.reference_count <= 0)
                    {
                        db.PartnerScopes.Remove(old_partner_scope);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public AlertModal<string> deleteActivityPartner(int activity_partner_id)
        {
            using (DbContextTransaction dbContext = db.Database.BeginTransaction())
            {
                try
                {
                    AcademicCollaborationRepo academicCollaborationRepo = new AcademicCollaborationRepo();
                    ActivityPartner activityPartner = db.ActivityPartners.Find(activity_partner_id);
                    //delete corress file in db and gg drive
                    if (activityPartner.file_id != null)
                    {
                        File file = db.Files.Find(activityPartner.file_id);
                        //delete on drive
                        GoogleDriveService.DeleteFile(file.file_drive_id);
                        db.Files.Remove(file);
                        db.SaveChanges();
                    }
                    //decrease ref_cou
                    PartnerScope partnerScope = db.PartnerScopes.Find(activityPartner.partner_scope_id);
                    academicCollaborationRepo.decreaseReferenceCountOfPartnerScope(partnerScope);
                    //delete activi_partner
                    db.ActivityPartners.Remove(activityPartner);
                    db.SaveChanges();
                    //delete partner_scope if ref_cou < =0
                    if (partnerScope.reference_count <= 0)
                    {
                        db.PartnerScopes.Remove(partnerScope);
                        db.SaveChanges();
                    }
                    dbContext.Commit();
                    return new AlertModal<string>(null, true, "Thành công", "Xóa đối tác đồng tổ chức thành công.");
                }
                catch (Exception e)
                {
                    dbContext.Rollback();
                    throw e;
                }
            }
        }
        public bool checkDuplicatePartnerScope(SaveActivityPartner activityPartner)
        {
            try
            {
                var partnerScope = db.PartnerScopes.Where<PartnerScope>(x => x.partner_id == activityPartner.partner_id && x.scope_id == activityPartner.scope_id).FirstOrDefault();
                if (partnerScope == null)
                {
                    return true;
                }
                else
                {
                    if (activityPartner.activity_partner_id == 0)
                    {
                        ActivityPartner ap = db.ActivityPartners.Where<ActivityPartner>(x => x.partner_scope_id == partnerScope.partner_scope_id && x.activity_id == activityPartner.activity_id).FirstOrDefault();
                        if (ap == null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        ActivityPartner ap = db.ActivityPartners.Where<ActivityPartner>(x => x.activity_partner_id == activityPartner.activity_partner_id).FirstOrDefault();
                        if (ap.partner_scope_id != partnerScope.partner_scope_id)
                        {
                            ActivityPartner ap_new = db.ActivityPartners.Where<ActivityPartner>(x => x.partner_scope_id == partnerScope.partner_scope_id && x.activity_id == activityPartner.activity_id).FirstOrDefault();
                            if (ap_new == null)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
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
        public class infoDetail : baseDetail
        {
            public int activity_id { get; set; }
        }
        public class SumDetail
        {
            public baseDetail baseDetail { get; set; }
            public List<subContent> subContent { get; set; }
            public List<AcademicActivityTypeLanguage> types { get; set; }
        }
        public class InfoSumDetail
        {
            public infoDetail infoDetail { get; set; }
            public List<subContent> subContent { get; set; }
        }
        public class basePartner
        {
            public int activity_partner_id { get; set; }
            public int partner_id { get; set; }
            public string partner_name { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public string contact_point { get; set; }
            public Nullable<double> sponsor { get; set; }
        }
        public class ContactInfo
        {
            public string contact_point_name { get; set; }
            public string contact_point_phone { get; set; }
            public string contact_point_email { get; set; }
        }
        public class Ques
        {
            public int question_id { get; set; }
            public string title { get; set; }
            public int answer_type_id { get; set; }
            public int? is_compulsory { get; set; }
            public int? is_changeable { get; set; }
        }
        public class CustomQuestion
        {
            public string title { get; set; }
            public int is_compulsory { get; set; }
        }
        public class QuesOption
        {
            public int question_id { get; set; }
            public string option_title { get; set; }
        }
        public class baseForm
        {
            public Form form { get; set; }
            public List<Ques> ques { get; set; }
            public List<QuesOption> quesOption { get; set; }
        }
    }
}
