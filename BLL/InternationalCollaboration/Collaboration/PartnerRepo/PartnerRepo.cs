using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.InternationalCollaboration.Collaboration.PartnerRepo
{
    public class PartnerRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public BaseServerSideData<PartnerList> GetListAll(BaseDatatable baseDatatable, SearchPartner searchPartner)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                string sql = @" select * from (select ROW_NUMBER() OVER(ORDER BY a.partner_id ASC) 'no' , partner_name,
                                a.partner_id, a.is_deleted, a.website, a.address, a.is_collab,
                                STRING_AGG(a.[name], ',') 'specialization_name', a.country_name from 
                                (select distinct t1.partner_name, t1.partner_id, t1.is_deleted, t1.website, t1.address,
		                        t4.[name],
		                        t5.country_name,
                                case when t2.partner_id is null     
		                        then 1 else 2 end as 'is_collab'
                                from IA_Collaboration.Partner t1 
                                left join IA_Collaboration.MOUPartner t2 on
                                t2.partner_id = t1.partner_id left join IA_Collaboration.MOU t6
		                        on t6.mou_id = t2.mou_id 
                                left join IA_Collaboration.MOUPartnerSpecialization t3 on
                                t3.mou_partner_id = t2.mou_partner_id
                                left join Localization.SpecializationLanguage t4 on 
                                t4.specialization_id = t3.specialization_id 
		                        left join General.Country t5 on 
		                        t1.country_id = t5.country_id 
                                where ( t1.is_deleted = {0}) and (t4.language_id = {1} or t4.language_id is null)) as a
								group by a.partner_name, a.partner_id, 
		                        a.is_deleted, a.website, a.address, a.partner_id,
		                        a.country_name, a.is_collab
								) as xyz
								where isnull(xyz.partner_name, '') like {2} and
								isnull(xyz.specialization_name, '') like {3} and
								isnull(xyz.country_name, '') like {4} and
								isnull(xyz.is_collab , '') like {5} ";

                bool is_deleted = searchPartner.is_deleted == 0;
                //===========================================================================================================
                var list = (from xyz in (
                                (from a in (
                                ((from t1 in db.Partners
                                  join t2 in db.MOUPartners on t1.partner_id equals t2.partner_id into t2_join
                                  from t2 in t2_join.DefaultIfEmpty()
                                  join t6 in db.MOUs on t2.mou_id equals t6.mou_id into t6_join
                                  from t6 in t6_join.DefaultIfEmpty()
                                  join t3 in db.MOUPartnerSpecializations on t2.mou_partner_id equals t3.mou_partner_id into t3_join
                                  from t3 in t3_join.DefaultIfEmpty()
                                  join t4 in db.SpecializationLanguages on t3.specialization_id equals t4.specialization_id into t4_join
                                  from t4 in t4_join.DefaultIfEmpty()
                                  join t5 in db.Countries on t1.country_id equals t5.country_id into t5_join
                                  from t5 in t5_join.DefaultIfEmpty()
                                  where
                                  t1.is_deleted == false &&
                                  (t4.language_id == 1 ||
                                  t4.language_id == 0)
                                  select new
                                  {
                                      t1.partner_name,
                                      partner_id = (int?)t1.partner_id,
                                      t1.is_deleted,
                                      t1.website,
                                      t1.address,
                                      t4.name,
                                      t5.country_name,
                                      is_collab =
                                  t2.partner_id == 0 ? 1 : 2
                                  }).Distinct()))
                                 select new
                                 {
                                     a.partner_name,
                                     a.partner_id,
                                     a.is_deleted,
                                     a.website,
                                     a.address,
                                     a.country_name,
                                     a.is_collab,
                                     specialization_name = a.name
                                 }))
                            where
                       (xyz.partner_name ?? "").Contains("") &&
                       (xyz.specialization_name ?? "").Contains("") &&
                       (xyz.country_name ?? "").Contains("") &&
                       (Convert.ToString(xyz.is_collab) ?? "").Contains("")
                            select new
                            {
                                xyz.partner_name,
                                xyz.partner_id,
                                xyz.is_deleted,
                                xyz.website,
                                xyz.address,
                                xyz.is_collab,
                                xyz.specialization_name,
                                xyz.country_name
                            });
                var pre_group = list.GroupBy(x => new
                {
                    x.partner_name,
                    x.partner_id,
                    x.is_deleted,
                    x.website,
                    x.address,
                    x.is_collab,
                    x.country_name
                });

                //var result = pre_group.ToList().Select(x => new PartnerList
                //{
                //    partner_name = x.Key.partner_name,
                //    partner_id = (int)x.Key.partner_id,
                //    is_deleted = x.Key.is_deleted,
                //    website = x.Key.website,
                //    address = x.Key.address,
                //    is_collab = x.Key.is_collab,
                //    specialization_name = string.Join(" ,", x.Select(y => y.specialization_name))
                //});

                var temp = (from a in db.Partners
                            join b in db.Countries on a.country_id equals b.country_id
                            where (searchPartner.partner_name == null || a.partner_name.Contains(searchPartner.partner_name))
                            && (searchPartner.nation == null || b.country_name.Contains(searchPartner.nation))
                            && a.is_deleted == (searchPartner.is_deleted == 1)
                            select new PartnerList
                            {
                                partner_name = a.partner_name,
                                partner_id = a.partner_id,
                                is_deleted = a.is_deleted,
                                website = a.website,
                                address = a.address,
                                country_name = b.country_name,
                                is_collab = db.MOUPartners.Any(c => c.partner_id == a.partner_id) ? 2 : 1,
                                SpecializationNames = (from d in db.MOUPartnerSpecializations
                                                       join e in db.SpecializationLanguages on d.specialization_id equals e.specialization_id
                                                       where d.MOUPartner.Partner.partner_id == a.partner_id
                                                       && (e.name.Contains(searchPartner.specialization) || searchPartner.partner_name == null)
                                                       && e.language_id == searchPartner.language
                                                       select e.name).Distinct().ToList()
                            }).Where(x => searchPartner.is_collab == 0 || x.is_collab == searchPartner.is_collab).ToList();
                temp.ForEach(x => x.specialization_name = string.Join(" ,", x.specialization_name));
                int count = temp.Count;
                //===========================================================================================================================
                string paging = @" ORDER BY " + baseDatatable.SortColumnName + " "
                            + baseDatatable.SortDirection +
                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT "
                            + baseDatatable.Length + " ROWS ONLY";

                List<PartnerList> listPartner = db.Database.SqlQuery<PartnerList>(sql + paging,
                    searchPartner.is_deleted, searchPartner.language,
                      searchPartner.partner_name == null ? "%%" : "%" + searchPartner.partner_name + "%",
                    "%" + searchPartner.specialization == null ? "%%" : "%" + searchPartner.specialization + "%",
                    "%" + searchPartner.nation == null ? "%%" : "%" + searchPartner.nation + "%",
                     searchPartner.is_collab == 0 ? "%%" : "%" + searchPartner.is_collab + "%").ToList();

                int totalRecord = db.Database.SqlQuery<PartnerList>(sql, searchPartner.is_deleted, searchPartner.language,
                      searchPartner.partner_name == null ? "%%" : "%" + searchPartner.partner_name + "%",
                    "%" + searchPartner.specialization == null ? "%%" : "%" + searchPartner.specialization + "%",
                    "%" + searchPartner.nation == null ? "%%" : "%" + searchPartner.nation + "%",
                     searchPartner.is_collab == 0 ? "%%" : "%" + searchPartner.is_collab + "%").Count();
                return new BaseServerSideData<PartnerList>(listPartner, totalRecord);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<string> AddPartner(List<HttpPostedFileBase> files_request, HttpPostedFileBase image, string content,
            PartnerArticle partner_article, int number_of_image, int account_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    int partner_check = db.Partners.Where(x => x.partner_name.ToLower().Equals(partner_article.partner_name.ToLower()) && x.is_deleted == false).ToList().Count();
                    if (partner_check > 0)
                    {
                        return new AlertModal<string>(false, "Tên đối tác bị trùng");
                    }
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
                            files_upload = GoogleDriveService.UploadIAFile(files_request, partner_article.partner_name, 1, false);
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
                        version_title = partner_article.partner_name,
                        article_id = article.article_id,
                        language_id = partner_article.partner_language_type
                    };
                    db.ArticleVersions.Add(articleVersion);

                    Partner partner = new Partner
                    {
                        partner_name = partner_article.partner_name,
                        website = partner_article.website,
                        address = partner_article.address,
                        is_deleted = false,
                        country_id = partner_article.country_id,
                        article_id = article.article_id
                    };
                    if (files_upload.Count != 0 && image != null)
                    {
                        partner.avatar = "https://drive.google.com/uc?id=" + files_upload.LastOrDefault().Id;
                    }
                    db.Partners.Add(partner);
                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    trans.Rollback();
                    return new AlertModal<string>(false, "Có lỗi xảy ra");
                }
            }
        }

        public AlertModal<string> DeletePartner(int id)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                Partner partner = new Partner();
                partner = db.Partners.Where(x => x.partner_id == id).FirstOrDefault();
                //if (partner.AcademicPrograms.Count != 0 || partner.MOAPartners.Count != 0 ||
                //    partner.MOUPartners.Count != 0 || partner.PartnerScopes.Count != 0)
                //{
                //    return new AlertModal<string>(false, "Đối tác đang có hoạt động đi kèm không thể xóa");
                //}
                //else
                //{
                partner.is_deleted = true;
                db.SaveChanges();
                return new AlertModal<string>(true, "Xóa thành công");
                //}
            }
            catch (Exception)
            {
                return new AlertModal<string>(false, "Có lỗi xảy ra");
            }
        }

        public PartnerArticle LoadEditPartner(int id)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                Partner partner = db.Partners.Where(x => x.partner_id == id).FirstOrDefault();
                PartnerArticle partnerArticle = new PartnerArticle
                {
                    partner_id = partner.partner_id,
                    partner_name = partner.partner_name,
                    country_id = partner.country_id,
                    address = partner.address,
                    website = partner.website,
                    avatar = partner.avatar,
                };

                ArticleVersion articleVersion = db.ArticleVersions.Where(x => x.article_id == partner.article_id).
                    OrderBy(x => x.language_id).FirstOrDefault();
                partnerArticle.partner_content = articleVersion.article_content;
                partnerArticle.partner_language_type = articleVersion.language_id;
                return partnerArticle;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public PartnerArticle LoadEditPartnerGuest(int id, int language_id)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                Partner partner = db.Partners.Where(x => x.partner_id == id).FirstOrDefault();
                PartnerArticle partnerArticle = new PartnerArticle
                {
                    partner_id = partner.partner_id,
                    partner_name = partner.partner_name,
                    country_id = partner.country_id,
                    address = partner.address,
                    website = partner.website,
                    avatar = partner.avatar,
                };

                ArticleVersion articleVersion = db.ArticleVersions.Where(x => x.article_id == partner.article_id &&
                 x.language_id == language_id).FirstOrDefault();
                if (articleVersion != null)
                {
                    partnerArticle.partner_content = articleVersion.article_content;
                }
                return partnerArticle;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<string> EditPartner(List<HttpPostedFileBase> files_request, HttpPostedFileBase image, string content,
            PartnerArticle partner_article, int number_of_image, int partner_id, int account_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    db = new ScienceAndInternationalAffairsEntities();
                    string partner_name_check = db.Partners.Find(partner_id).partner_name;
                    if (!partner_article.partner_name.ToLower().Equals(partner_name_check.ToLower()))
                    {
                        int partner_check = db.Partners.Where(x => x.partner_name.ToLower().Equals(partner_article.partner_name.ToLower()) && x.is_deleted == false).ToList().Count();
                        if (partner_check > 0)
                        {
                            return new AlertModal<string>(false, "Tên đối tác bị trùng");
                        }
                    }
                    else
                    {
                        int partner_check = db.Partners.Where(x => x.partner_name.ToLower().Equals(partner_article.partner_name.ToLower()) && x.is_deleted == false).ToList().Count();
                        if (partner_check > 1)
                        {
                            return new AlertModal<string>(false, "Tên đối tác bị trùng");
                        }
                    }
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
                            files_upload = GoogleDriveService.UploadIAFile(files_request, partner_article.partner_name, 1, false);
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

                    Partner partner = db.Partners.Where(x => x.partner_id == partner_id).FirstOrDefault();
                    partner.partner_name = partner_article.partner_name;
                    partner.website = partner_article.website;
                    partner.address = partner_article.address;
                    partner.country_id = partner_article.country_id;
                    if (files_upload.Count != 0 && image != null)
                    {
                        partner.avatar = "https://drive.google.com/uc?id=" + files_upload.LastOrDefault().Id;
                    }

                    Article article = db.Articles.Where(x => x.article_id == partner.article_id).FirstOrDefault();
                    article.account_id = account_id;

                    ArticleVersion articleVersion =
                        db.ArticleVersions.Where(x => x.article_id == partner.article_id &&
                        x.language_id == partner_article.partner_language_type).FirstOrDefault();
                    if (articleVersion == null)
                    {
                        articleVersion = new ArticleVersion
                        {
                            article_id = article.article_id,
                            article_content = content,
                            language_id = partner_article.partner_language_type,
                            publish_time = DateTime.Now,
                            version_title = partner_article.partner_name,
                        };
                        db.ArticleVersions.Add(articleVersion);
                    }
                    else
                    {
                        articleVersion.article_content = content;
                        articleVersion.language_id = partner_article.partner_language_type;
                    }
                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    trans.Rollback();
                    return new AlertModal<string>(false, "Có lỗi xảy ra");
                }
            }
        }

        public PartnerHistoryList<PartnerHistory> GetHistory(int id)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                string query = @"WITH b AS(
                                SELECT ps.partner_scope_id, ps.partner_id
                                FROM IA_Collaboration.PartnerScope ps 
                                WHERE ps.partner_id = {0}
                                )
                                SELECT '' as 'code', N'Tổ chức hoạt động học thuật' 'activity', isnull(ap.contact_point_name, '')  'contact_point_name',
								isnull(ap.contact_point_email, '') 'contact_point_email', isnull(ap.contact_point_phone, '')  'contact_point_phone', '' AS 'full_name', aa.activity_date_start, aa.activity_date_end
                                FROM SMIA_AcademicActivity.AcademicActivity aa INNER JOIN SMIA_AcademicActivity.ActivityPartner ap
                                ON aa.activity_id = ap.activity_id INNER JOIN b 
                                ON b.partner_scope_id = ap.partner_scope_id
                                UNION ALL
                                SELECT '' as 'code', N'Hợp tác nghiên cứu' 'activity', '','','','', rc.project_start_date, rc.project_end_date
                                FROM IA_ResearchCollaboration.ResearchCollaboration rc INNER JOIN IA_ResearchCollaboration.ResearchPartner rp
                                ON rc.project_id = rp.project_id INNER JOIN b ON rp.partner_scope_id = b.partner_scope_id
                                UNION ALL
                                SELECT '' as 'code', N'Hợp tác học thuật' 'activity', '','', '','', ac.plan_study_start_date, ac.plan_study_end_date
                                FROM IA_AcademicCollaboration.AcademicCollaboration ac INNER JOIN b 
                                ON ac.partner_scope_id = b.partner_scope_id
                                UNION ALL
                                SELECT DISTINCT mou.mou_code, N'Ký kết biên bản ghi nhớ' 'activity', isnull(mp.contact_point_name, '')  'contact_point_name',
								isnull(mp.contact_point_email, '') 'contact_point_email', isnull(mp.contact_point_phone, '')  'contact_point_phone', isnull(a.full_name, ''),
                                mp.mou_start_date, mou.mou_end_date
                                FROM b INNER JOIN IA_Collaboration.MOUPartner mp ON b.partner_id = mp.partner_id
                                INNER JOIN IA_Collaboration.MOU mou ON mou.mou_id = mp.mou_id INNER JOIN General.[Account] a
                                ON mou.account_id = a.account_id
                                UNION ALL
                                SELECT DISTINCT moa.moa_code, N'Ký kết biên bản thỏa thuận' 'activity','','','', isnull(a.full_name, ''), mp.moa_start_date, moa.moa_end_date
                                FROM b INNER JOIN IA_Collaboration.MOAPartner mp ON b.partner_id = mp.partner_id
                                INNER JOIN IA_Collaboration.MOA moa ON moa.moa_id = mp.moa_id 
                                INNER JOIN General.Account a ON a.account_id = moa.account_id
                                UNION ALL
                                SELECT DISTINCT mb.mou_bonus_code, N'Ký kết biên bản ghi nhớ bổ sung' 'activity','','','', '', mb.mou_bonus_decision_date, mb.mou_bonus_end_date
                                FROM b INNER JOIN IA_Collaboration.MOUPartner mp ON b.partner_id = mp.partner_id
                                INNER JOIN IA_Collaboration.MOUBonus mb ON mp.mou_id = mb.mou_id 
                                LEFT JOIN IA_Collaboration.MOuPartnerScope mps ON mps.mou_bonus_id = mb.mou_bonus_id
                                WHERE (mb.mou_bonus_end_date IS NOT NULL) OR (mps.partner_scope_id IS NOT NULL)
                                UNION ALL
                                SELECT DISTINCT mb.moa_bonus_code, N'Ký kết biên bản thỏa thuận bổ sung' 'activity','','','', '', MB.moa_bonus_decision_date, mb.moa_bonus_end_date
                                FROM b INNER JOIN IA_Collaboration.MOAPartner mp ON b.partner_id = mp.partner_id
                                INNER JOIN IA_Collaboration.MOABonus mb ON mp.moa_id = mb.moa_id 
                                LEFT JOIN IA_Collaboration.MOAPartnerScope mps ON mps.moa_bonus_id = mb.moa_bonus_id
                                WHERE (mb.moa_bonus_end_date IS NOT NULL) OR (mps.partner_scope_id IS NOT NULL)
                                ORDER BY activity_date_start ASC  ";
                List<PartnerHistory> list = db.Database.SqlQuery<PartnerHistory>(query, id).ToList();
                string partner_name = db.Partners.Where(x => x.partner_id == id).Select(x => x.partner_name).FirstOrDefault();
                return new PartnerHistoryList<PartnerHistory>(list, partner_name);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetPartnerDetailSpec(int id)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                string query = @"SELECT s.specialization_name
                        FROM IA_Collaboration.MOUPartnerSpecialization mps
                        INNER JOIN IA_Collaboration.MOUPartner mp ON mp.mou_partner_id = mps.mou_partner_id
                        INNER JOIN General.Specialization s on s.specialization_id = mps.specialization_id
                        WHERE mp.partner_id = {0} ";

                List<string> list = db.Database.SqlQuery<string>(query, id).ToList();
                return String.Join(", ", list.ToArray());
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetContentLanguage(int partner_id, int partner_language_type)
        {
            try
            {
                Partner partner = db.Partners.Where(x => x.partner_id == partner_id).FirstOrDefault();
                ArticleVersion articleVersion = db.ArticleVersions.
                    Where(x => x.article_id == partner.article_id && x.language_id == partner_language_type).FirstOrDefault();
                return articleVersion?.article_content;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GetPreView()
        {
            return;
        }
        public void ExportExcel()
        {
            return;
        }
    }
}
