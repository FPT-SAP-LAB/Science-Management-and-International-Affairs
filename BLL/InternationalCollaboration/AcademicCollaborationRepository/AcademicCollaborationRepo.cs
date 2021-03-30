using BLL.Authen;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities.SaveAcademicCollaborationEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BLL.InternationalCollaboration.AcademicCollaborationRepository
{
    public class AcademicCollaborationRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public BaseServerSideData<AcademicCollaboration_Ext> academicCollaborations(int direction, int collab_type_id, ObjectSearching_AcademicCollaboration obj_searching, BaseDatatable baseDatatable)
        {
            try
            {
                var sql = @"select
                        collab.collab_id, pp.[name] 'people_name', pp.email, offi.office_name, pn.partner_name, c.country_name,
                        collab.plan_study_start_date, collab.plan_study_end_date,
                        acs.collab_status_id, acs.collab_status_id, acs.collab_status_name,
                        collab.is_supported, collab.note
                        from IA_AcademicCollaboration.AcademicCollaboration collab
                        join IA_Collaboration.PartnerScope mpc on collab.partner_scope_id = mpc.partner_scope_id
                        join IA_Collaboration.[Partner] pn on pn.partner_id = mpc.partner_id
                        left join General.Country c on c.country_id = pn.country_id
                        join General.People pp on collab.people_id = pp.people_id
                        left join General.[Profile] pf on pf.people_id = pp.people_id
                        left join General.Office offi on pf.office_id = offi.office_id
                        join (select csh1.collab_id, csh2.collab_status_id, csh1.change_date
		                        from
		                        (select csh1.collab_id, MAX(csh1.change_date) 'change_date'
			                        from IA_AcademicCollaboration.CollaborationStatusHistory csh1
			                        group by csh1.collab_id) as csh1
		                        join
		                        (select csh2.collab_status_id, csh2.collab_id, csh2.change_date
			                        from IA_AcademicCollaboration.CollaborationStatusHistory csh2) as csh2
		                        on csh1.collab_id = csh2.collab_id and csh1.change_date = csh2.change_date) as csh
                        on csh.collab_id = collab.collab_id
                        join IA_AcademicCollaboration.AcademicCollaborationStatus acs on acs.collab_status_id = csh.collab_status_id
                        where collab.direction_id = @direction /*Dài hạn = 2, Ngắn hạn = 1*/ and collab.collab_type_id = @collab_type_id /*Chiều đi = 1, Chiều đến = 2*/
                        and ISNULL(c.country_name, '') like @country_name
                        and ISNULL(pn.partner_name, '') like @partner_name
                        and ISNULL(offi.office_name, '') like @office_name
                        or @year between YEAR(collab.actual_study_start_date) and YEAR(collab.actual_study_end_date)
                        ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                        " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY";

                List<AcademicCollaboration_Ext> academicCollaborations = db.Database.SqlQuery<AcademicCollaboration_Ext>(sql,
                                                    new SqlParameter("direction", direction),
                                                    new SqlParameter("collab_type_id", collab_type_id),
                                                    new SqlParameter("country_name", obj_searching.country_name == null ? "%%" : "%" + obj_searching.country_name + "%"),
                                                    new SqlParameter("partner_name", obj_searching.partner_name == null ? "%%" : "%" + obj_searching.partner_name + "%"),
                                                    new SqlParameter("office_name", obj_searching.office_name == null ? "%%" : "%" + obj_searching.office_name + "%"),
                                                    new SqlParameter("year", obj_searching.year),
                                                    new SqlParameter("sortColumnName", baseDatatable.SortColumnName),
                                                    new SqlParameter("sortDirection", baseDatatable.SortDirection),
                                                    new SqlParameter("start", baseDatatable.Start),
                                                    new SqlParameter("length", baseDatatable.Length)).ToList();

                int recordsTotal = db.Database.SqlQuery<int>(@"select count(*)                                                              
                                                                from IA_AcademicCollaboration.AcademicCollaboration collab
                                                                join IA_Collaboration.PartnerScope mpc on collab.partner_scope_id = mpc.partner_scope_id
                                                                join IA_Collaboration.[Partner] pn on pn.partner_id = mpc.partner_id
                                                                left join General.Country c on c.country_id = pn.country_id
                                                                join General.People pp on collab.people_id = pp.people_id
                                                                left join General.[Profile] pf on pf.people_id = pp.people_id
                                                                left join General.Office offi on pf.office_id = offi.office_id
                                                                join (select csh1.collab_id, csh2.collab_status_id, csh1.change_date
		                                                                from
		                                                                (select csh1.collab_id, MAX(csh1.change_date) 'change_date'
			                                                                from IA_AcademicCollaboration.CollaborationStatusHistory csh1
			                                                                group by csh1.collab_id) as csh1
		                                                                join
		                                                                (select csh2.collab_status_id, csh2.collab_id, csh2.change_date
			                                                                from IA_AcademicCollaboration.CollaborationStatusHistory csh2) as csh2
		                                                                on csh1.collab_id = csh2.collab_id and csh1.change_date = csh2.change_date) as csh
                                                                on csh.collab_id = collab.collab_id
                                                                join IA_AcademicCollaboration.AcademicCollaborationStatus acs on acs.collab_status_id = csh.collab_status_id
                                                                where collab.direction_id = @direction /*Dài hạn = 2, Ngắn hạn = 1*/ and collab.collab_type_id = @collab_type_id /*Chiều đi = 1, Chiều đến = 2*/
                                                                and ISNULL(c.country_name, '') like @country_name
                                                                and ISNULL(pn.partner_name, '') like @partner_name
                                                                and ISNULL(offi.office_name, '') like @office_name
                                                                or @year between YEAR(collab.actual_study_start_date) and YEAR(collab.actual_study_end_date)",
                                                                new SqlParameter("direction", direction),
                                                                new SqlParameter("collab_type_id", collab_type_id),
                                                                new SqlParameter("country_name", obj_searching.country_name == null ? "%%" : "%" + obj_searching.country_name + "%"),
                                                                new SqlParameter("partner_name", obj_searching.partner_name == null ? "%%" : "%" + obj_searching.partner_name + "%"),
                                                                new SqlParameter("office_name", obj_searching.office_name == null ? "%%" : "%" + obj_searching.office_name + "%"),
                                                                new SqlParameter("year", obj_searching.year)).FirstOrDefault();
                return new BaseServerSideData<AcademicCollaboration_Ext>(academicCollaborations, recordsTotal);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<List<Country>> countries(string country_name)
        {
            try
            {
                var sql = @"-----1.1. Danh sách Country
                        select * from General.Country
                        where country_name like @country_name";
                List<Country> countries = db.Database.SqlQuery<Country>(sql,
                    new SqlParameter("country_name", country_name == null ? "%%" : "%" + country_name + "%")).ToList();
                return new AlertModal<List<Country>>(countries, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<YearSearching> yearSearching()
        {
            try
            {
                var sql = @"-----1.2. Danh sách Năm
                    select YEAR(MIN(plan_study_start_date)) as 'year_from', YEAR(GETDATE()) as 'year_to'
                    from IA_AcademicCollaboration.AcademicCollaboration";
                YearSearching yearSearching = db.Database.SqlQuery<YearSearching>(sql).FirstOrDefault();
                return new AlertModal<YearSearching>(yearSearching, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<List<AcademicCollaborationPartner_Ext>> partners(string partner_name)
        {
            try
            {
                var sql = @"-----1.3. Đơn vị đào tạo - chiều đi/chiều đến -> partner/office
                    select par.*, cou.country_name from IA_Collaboration.[Partner] par
                    inner join General.Country cou on cou.country_id = par.country_id
                    where partner_name like @partner_name and is_deleted = 0";
                List<AcademicCollaborationPartner_Ext> partners = db.Database.SqlQuery<AcademicCollaborationPartner_Ext>(sql,
                    new SqlParameter("partner_name", partner_name == null ? "%%" : "%" + partner_name + "%")).ToList();
                return new AlertModal<List<AcademicCollaborationPartner_Ext>>(partners, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<AcademicCollaborationPartner_Ext> partner(int partner_id, string partner_name)
        {
            try
            {
                var sql = @"------1.3.1. Check đơn vị đào tạo
                    select par.*, cou.country_name from IA_Collaboration.[Partner] par
                    inner join General.Country cou on cou.country_id = par.country_id
                    where partner_name = @partner_name and is_deleted = 0
                    or par.partner_id = @partner_id";
                AcademicCollaborationPartner_Ext partner = db.Database.SqlQuery<AcademicCollaborationPartner_Ext>(sql,
                    new SqlParameter("partner_name", partner_name),
                    new SqlParameter("partner_id", partner_id)).FirstOrDefault();
                return new AlertModal<AcademicCollaborationPartner_Ext>(partner, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<List<Office>> offices(string office_name)
        {
            try
            {
                var sql = @"-----1.4. Đơn vị công tác - chiều đi/chiều đến -> office/partner
                    select * from General.Office
                    where office_name like @office_name";
                List<Office> offices = db.Database.SqlQuery<Office>(sql,
                    new SqlParameter("office_name", office_name == null ? "%%" : "%" + office_name + "%")).ToList();
                return new AlertModal<List<Office>>(offices, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<List<AcademicCollaborationPerson_Ext>> people(string person_name)
        {
            try
            {
                var sql = @"-----1.7. Danh sách cán bộ
                    select peo.people_id, name, email, phone_number, pro.mssv_msnv
                    from General.People peo
                    inner join General.Profile pro on peo.people_id = pro.people_id
                    where name like @person_name";
                List<AcademicCollaborationPerson_Ext> people = db.Database.SqlQuery<AcademicCollaborationPerson_Ext>(sql,
                    new SqlParameter("person_name", person_name == null ? "%%" : "%" + person_name + "%")).ToList();
                return new AlertModal<List<AcademicCollaborationPerson_Ext>>(people, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<AcademicCollaborationPerson_Ext> person(int people_id, string people_name)
        {
            try
            {
                var sql = @"-----1.7.1. Check person
                    select peo.*, pro.office_id, offi.office_name, pro.mssv_msnv
                    from General.People peo
                    inner join General.Profile pro on peo.people_id = pro.people_id
                    inner join General.Office offi on offi.office_id = pro.office_id
                    where peo.name = @people_name or peo.people_id = @people_id";
                AcademicCollaborationPerson_Ext person = db.Database.SqlQuery<AcademicCollaborationPerson_Ext>(sql,
                    new SqlParameter("people_name", people_name),
                    new SqlParameter("people_id", people_id)).FirstOrDefault();
                if (person != null)
                {
                    return new AlertModal<AcademicCollaborationPerson_Ext>(person, true);
                }
                else
                {
                    return new AlertModal<AcademicCollaborationPerson_Ext>(false, "Lấy dữ liệu về cán bộ đã có lỗi xảy ra.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicCollaborationPerson_Ext>(false, "Lấy dữ liệu về cán bộ đã có lỗi xảy ra.");
            }
        }

        public AlertModal<List<CollaborationScope>> collaborationScopes(string collab_abbreviation_name)
        {
            try
            {
                var sql = @"-----1.8. Phạm vi hợp tác
                    select * from IA_Collaboration.CollaborationScope
                    where scope_abbreviation = @collab_abbreviation_name";
                List<CollaborationScope> collaborationScopes = db.Database.SqlQuery<CollaborationScope>(sql,
                    new SqlParameter("collab_abbreviation_name", collab_abbreviation_name)).ToList();
                return new AlertModal<List<CollaborationScope>>(collaborationScopes, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<List<AcademicCollaborationStatu>> academicCollaborationStatus(int status_type_specific)
        {
            try
            {
                var sql = @"----3.Chuyển đổi trạng thái - Danh sách trạng thái
                    select collab_status_id, collab_status_name, status_type
                    from IA_AcademicCollaboration.AcademicCollaborationStatus
                    where status_type = 3 /*both long & short-term*/ or status_type = @status_type_specific /*1:short;2:long;3:both*/";
                List<AcademicCollaborationStatu> academicCollaborationStatus = db.Database.SqlQuery<AcademicCollaborationStatu>(sql,
                    new SqlParameter("status_type_specific", status_type_specific)).ToList();
                return new AlertModal<List<AcademicCollaborationStatu>>(academicCollaborationStatus, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Google.Apis.Drive.v3.Data.File uploadEvidenceFile(HttpPostedFileBase InputFile, string FolderName, int TypeFolder, bool isFolder)
        {
            string file_id = "";
            try
            {
                Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadIAFile(InputFile, FolderName, TypeFolder, isFolder);
                file_id = InputFile.FileName;
                return f;
            }
            catch (Exception e)
            {
                if (file_id != "")
                {
                    GoogleDriveService.DeleteFile(file_id);
                }
                throw e;
            }
        }

        public AlertModal<AcademicCollaboration_Ext> saveAcademicCollaboration(int direction_id, int collab_type_id,
            SaveAcadCollab_Person obj_person,
            SaveAcadCollab_Partner obj_partner,
            SaveAcadCollab_AcademicCollaboration obj_academic_collab,
            Google.Apis.Drive.v3.Data.File f, HttpPostedFileBase evidence, int account_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //check duplicate academic collaboration: person, partner, collab_scope base on time
                    if (checkDuplicateAcademicCollaboration(obj_person, obj_partner, obj_academic_collab))
                    {
                        Person person;
                        var person_id = obj_person.person_id;
                        Partner partner;
                        var partner_id = obj_partner.partner_id;
                        var partner_scope_id = 0;
                        PartnerScope partner_scope;

                        //check available person
                        if (!obj_person.available_person)
                        {
                            //add person
                            person = savePerson(obj_person);
                            person_id = person.people_id;
                            //add profile
                            //saveProfile(person, obj_person);
                        }

                        //check available partner
                        if (!obj_partner.available_partner)
                        {
                            var country = db.Countries.Find(obj_partner.partner_country_id);
                            if (country == null)
                            {
                                trans.Rollback();
                                return new AlertModal<AcademicCollaboration_Ext>(null, false, "Lỗi", "Không tìm thấy quốc gia tương ứng.");
                            }
                            //add Article 
                            var article = saveArticle(account_id);
                            //add ArticleVersion
                            saveArticleVersion(obj_partner, article);
                            //add Partner
                            partner = savePartner(obj_partner, article);
                            //get corresponsing partner_id value
                            partner_id = partner.partner_id;
                            //add partner_id & scope_id to PartnerScope
                            partner_scope = savePartnerScope(partner_id, obj_partner);
                            //get corresponding partner_scope_id
                            partner_scope_id = partner_scope.partner_scope_id;
                        }
                        else
                        {
                            //check exist partner_scope
                            partner_scope = db.PartnerScopes.Where<PartnerScope>(x => x.partner_id == partner_id && x.scope_id == obj_partner.collab_scope_id).FirstOrDefault();
                            if (partner_scope != null)
                            {
                                //incease 1 to referecen count
                                increaseReferenceCountOfPartnerScope(partner_scope);
                            }
                            else
                            {
                                //add partner_id & scope_id to PartnerScope
                                partner_scope = savePartnerScope(partner_id, obj_partner);
                            }
                            //get corresponding partner_scope_id
                            partner_scope_id = partner_scope.partner_scope_id;
                        }

                        //add Academic Collab
                        var academic_collaboration = saveAcademicCollaboration(direction_id, collab_type_id, person_id, partner_scope_id, obj_academic_collab);

                        //add infor to File
                        var evidence_file = saveFile(f, evidence);

                        //add infor to CollaborationStatusHistory
                        var collab_status_hist = saveCollabStatusHistory(evidence, academic_collaboration.collab_id, obj_academic_collab.status_id, null, evidence_file, account_id);
                        trans.Commit();
                        return new AlertModal<AcademicCollaboration_Ext>(null, true, "Thêm cán bộ giảng viên thành công.");
                    }
                    else
                    {
                        AlertModal<AcademicCollaboration_Ext> alertModal = new AlertModal<AcademicCollaboration_Ext>(null, false, "Cảnh báo", "Với thời gian kế hoạch, CBGV đang đi học tại đối tác.");
                        return alertModal;
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
        }

        public bool checkDuplicateAcademicCollaboration(SaveAcadCollab_Person obj_person, SaveAcadCollab_Partner obj_partner, SaveAcadCollab_AcademicCollaboration obj_academic_collab)
        {
            if (obj_person.available_person && obj_partner.available_partner)
            {
                PartnerScope partnerScope = db.PartnerScopes.Where(x => x.partner_id == obj_partner.partner_id
                                                                && x.scope_id == obj_partner.collab_scope_id).FirstOrDefault();
                if (partnerScope != null)
                {
                    AcademicCollaboration academicCollaboration = db.AcademicCollaborations.Where(x => x.people_id == obj_person.person_id
                                                                &&
                                                                x.collab_id != obj_academic_collab.collab_id
                                                                &&
                                                                ((x.plan_study_start_date <= obj_academic_collab.plan_start_date && x.plan_study_end_date >= obj_academic_collab.plan_start_date)
                                                                ||
                                                                (x.plan_study_start_date <= obj_academic_collab.plan_end_date && x.plan_study_end_date >= obj_academic_collab.plan_end_date)
                                                                ||
                                                                (x.plan_study_start_date <= obj_academic_collab.plan_start_date && x.plan_study_end_date >= obj_academic_collab.plan_end_date))).FirstOrDefault();
                    if (academicCollaboration != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Person savePerson(SaveAcadCollab_Person obj_person)
        {
            Person person = new Person();
            try
            {
                //add new person
                //add information to People
                person.name = obj_person.person_name;
                person.email = obj_person.person_email;
                //if (obj_person.person_profile_office_id != null) person.office_id = obj_person.person_profile_office_id;
                db.People.Add(person);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return person;
        }

        //public void saveProfile(Person person, SaveAcadCollab_Person obj_person)
        //{
        //    try
        //    {
        //        var profile = new Profile();
        //        //check office_id with Office
        //        var office = db.Offices.Find(obj_person.person_profile_office_id);
        //        if (office != null)
        //        {
        //            profile.office_id = office.office_id;
        //            profile.people_id = person.people_id;
        //            profile.mssv_msnv = ""; //can make issues for Sicence Management
        //            profile.profile_page_active = false;
        //            db.Profiles.Add(profile);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        public Article saveArticle(int account_id)
        {
            Article article;
            try
            {
                //add Article
                article = new Article()
                {
                    need_approved = false,
                    article_status_id = 2, //Chấp thuận
                    account_id = account_id
                };
                db.Articles.Add(article);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return article;
        }

        public void saveArticleVersion(SaveAcadCollab_Partner obj_partner, Article article)
        {
            try
            {
                //add ArticleVersion
                var articleVersion = new ArticleVersion()
                {
                    publish_time = DateTime.Now,
                    version_title = obj_partner.partner_name,
                    article_id = article.article_id,
                    language_id = 1 //Vietnamese
                };
                db.ArticleVersions.Add(articleVersion);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Partner savePartner(SaveAcadCollab_Partner obj_partner, Article article)
        {
            Partner partner = new Partner();
            try
            {
                partner.country_id = obj_partner.partner_country_id;
                partner.partner_name = obj_partner.partner_name;
                partner.article_id = article.article_id;
                db.Partners.Add(partner);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return partner;
        }

        public PartnerScope savePartnerScope(int partner_id, SaveAcadCollab_Partner obj_partner)
        {
            PartnerScope partner_scope;
            try
            {
                partner_scope = new PartnerScope()
                {
                    partner_id = partner_id,
                    scope_id = obj_partner.collab_scope_id,
                    reference_count = 1 //init first count for new PartnerScope
                };
                db.PartnerScopes.Add(partner_scope);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return partner_scope;
        }

        public void increaseReferenceCountOfPartnerScope(PartnerScope partner_scope)
        {
            try
            {
                partner_scope.reference_count += 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AcademicCollaboration saveAcademicCollaboration(int direction_id, int collab_type_id, int person_id, int partner_scope_id,
            SaveAcadCollab_AcademicCollaboration obj_academic_collab)
        {
            AcademicCollaboration academic_collaboration = new AcademicCollaboration();
            try
            {
                //add infor to AcademicCollaboration
                academic_collaboration.direction_id = direction_id;
                academic_collaboration.collab_type_id = collab_type_id;
                academic_collaboration.people_id = person_id;
                academic_collaboration.partner_scope_id = partner_scope_id;
                academic_collaboration.plan_study_start_date = obj_academic_collab.plan_start_date;
                academic_collaboration.plan_study_end_date = obj_academic_collab.plan_end_date;
                if (obj_academic_collab.actual_start_date != null) academic_collaboration.actual_study_start_date = obj_academic_collab.actual_start_date;
                if (obj_academic_collab.actual_end_date != null) academic_collaboration.actual_study_end_date = obj_academic_collab.actual_end_date;
                academic_collaboration.is_supported = obj_academic_collab.support;
                if (obj_academic_collab.note != null) academic_collaboration.note = obj_academic_collab.note;
                db.AcademicCollaborations.Add(academic_collaboration);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return academic_collaboration;
        }

        public File saveFile(Google.Apis.Drive.v3.Data.File f, HttpPostedFileBase evidence)
        {
            File evidence_file = new File();
            try
            {
                if (evidence != null)
                {
                    //add infor to File
                    if (evidence.FileName != null) evidence_file.name = evidence.FileName;
                    if (f.WebViewLink != null) evidence_file.link = f.WebViewLink;
                    if (f.Id != null) evidence_file.file_drive_id = f.Id;
                    db.Files.Add(evidence_file);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return evidence_file;
        }

        public CollaborationStatusHistory saveCollabStatusHistory(HttpPostedFileBase evidence, int collab_id, int collab_status_id, string note, File evidence_file, int account_id)
        {
            CollaborationStatusHistory collab_status_hist = new CollaborationStatusHistory();
            try
            {
                //add infor to CollaborationStatusHistory
                collab_status_hist.collab_id = collab_id;
                collab_status_hist.collab_status_id = collab_status_id;
                collab_status_hist.change_date = DateTime.Now;
                if (note != null) collab_status_hist.note = note;
                if (evidence != null) collab_status_hist.file_id = evidence_file.file_id;
                collab_status_hist.account_id = account_id;
                db.CollaborationStatusHistories.Add(collab_status_hist);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return collab_status_hist;
        }

        //EDIT
        public AlertModal<AcademicCollaboration_Ext> getAcademicCollaboration(int direction, int collab_type_id, int acad_collab_id)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                var sql = @"select
                            collab.collab_id, collab.partner_scope_id, collab.collab_type_id, pp.people_id, pp.[name] 'people_name', pp.email, offi.office_id, offi.office_name,
                            pn.partner_id, pn.partner_name, c.country_id, c.country_name, cs.scope_id, cs.scope_name,
                            collab.plan_study_start_date, collab.plan_study_end_date, csh.file_id, csh.file_name, csh.file_link, csh.file_drive_id, collab.actual_study_start_date, collab.actual_study_end_date,
                            acs.collab_status_id, acs.collab_status_name,
                            collab.is_supported, collab.note
                            from IA_AcademicCollaboration.AcademicCollaboration collab
                            join IA_Collaboration.PartnerScope mpc on collab.partner_scope_id = mpc.partner_scope_id
                            join IA_Collaboration.[Partner] pn on pn.partner_id = mpc.partner_id
							join IA_Collaboration.CollaborationScope cs on cs.scope_id = mpc.scope_id
                            join General.Country c on c.country_id = pn.country_id
                            join General.People pp on collab.people_id = pp.people_id 
                            join General.[Profile] pf on pf.people_id = pp.people_id
                            join General.Office offi on pf.office_id = offi.office_id
                            join (select csh1.collab_id, csh2.collab_status_id, csh1.change_date, csh2.file_id, csh2.file_name, csh2.file_link, csh2.file_drive_id
		                            from 
		                            (select csh1.collab_id, MAX(csh1.change_date) 'change_date' 
			                            from IA_AcademicCollaboration.CollaborationStatusHistory csh1
			                            group by csh1.collab_id) as csh1
		                            join 
		                            (select csh2.collab_status_id, csh2.collab_id, csh2.change_date, fi.file_id, fi.name 'file_name', fi.link 'file_link', fi.file_drive_id
			                            from IA_AcademicCollaboration.CollaborationStatusHistory csh2
										left join General.[File] fi on fi.file_id = csh2.file_id) as csh2 
		                            on csh1.collab_id = csh2.collab_id and csh1.change_date = csh2.change_date) as csh 
                            on csh.collab_id = collab.collab_id
                            join IA_AcademicCollaboration.AcademicCollaborationStatus acs on acs.collab_status_id = csh.collab_status_id
                            where collab.direction_id = @direction /*Dài hạn = 2, Ngắn hạn = 1*/ and collab.collab_type_id = @collab_type_id /*Chiều đi = 1, Chiều đến = 2*/
                            and collab.collab_id = @collab_id";
                AcademicCollaboration_Ext academicCollaboration = db.Database.SqlQuery<AcademicCollaboration_Ext>(sql,
                    new SqlParameter("direction", direction),
                    new SqlParameter("collab_type_id", collab_type_id),
                    new SqlParameter("collab_id", acad_collab_id)).FirstOrDefault();
                if (academicCollaboration != null)
                {
                    return new AlertModal<AcademicCollaboration_Ext>(academicCollaboration, true, null, null);
                }
                else
                {
                    return new AlertModal<AcademicCollaboration_Ext>(false, "Có lỗi xảy ra khi lấy dữ liệu hợp tác học thuật tương ứng");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicCollaboration_Ext>(false, "Lấy dữ liệu về hợp tác học thuật đã có lỗi xảy ra.");
            }
        }

        public Google.Apis.Drive.v3.Data.File updateEvidenceFile(File old_evidence, HttpPostedFileBase new_evidence, string folder_name, int type_folder, bool is_folder)
        {
            Google.Apis.Drive.v3.Data.File f = new Google.Apis.Drive.v3.Data.File();
            //update file
            if (new_evidence != null)
            {
                //update file
                f = uploadEvidenceFile(new_evidence, folder_name, type_folder, is_folder);
            }
            else if (new_evidence == null && old_evidence.file_id != 0)
            {
                GoogleDriveService.DeleteFile(old_evidence.file_drive_id);
            }
            return f;
        }

        public AlertModal<AcademicCollaboration_Ext> updateAcademicCollaboration(int direction_id, int collab_type_id,
            SaveAcadCollab_Person obj_person,
            SaveAcadCollab_Partner obj_partner,
            SaveAcadCollab_AcademicCollaboration obj_academic_collab,
            Google.Apis.Drive.v3.Data.File f, File old_evidence, HttpPostedFileBase new_evidence, int account_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    //check duplicate academic collaboration: person, partner, collab_scope base on time
                    if (checkDuplicateAcademicCollaboration(obj_person, obj_partner, obj_academic_collab))
                    {
                        Person person;
                        var person_id = obj_person.person_id;
                        Partner partner;
                        var partner_id = obj_partner.partner_id;
                        var partner_scope_id = 0;
                        PartnerScope partner_scope;

                        //check available person
                        if (!obj_person.available_person)
                        {
                            //add person
                            person = savePerson(obj_person);
                            person_id = person.people_id;

                            //add office
                            //check office_id with Office
                            //var office = db.Offices.Find(obj_person.person_profile_office_id);
                            //if (office == null)
                            //{
                            //    return new AlertModal<AcademicCollaboration_Ext>(false, "Không thêm được cơ sở tương ứng.");
                            //}
                            //else
                            //{
                            //    saveProfile(person, obj_person);
                            //}
                        }

                        //check available partner
                        if (!obj_partner.available_partner)
                        {
                            //add new partner
                            //check country_id with Country
                            var country = db.Countries.Find(obj_partner.partner_country_id);
                            if (country == null)
                            {
                                return new AlertModal<AcademicCollaboration_Ext>(false, "Không thêm được quốc gia tương ứng.");
                            }
                            else
                            {
                                //add Article 
                                var article = saveArticle(account_id);
                                //add ArticleVersion
                                saveArticleVersion(obj_partner, article);
                                //add Partner
                                partner = savePartner(obj_partner, article);
                                //get corresponsing partner_id value
                                partner_id = partner.partner_id;
                                //add partner_id & scope_id to PartnerScope
                                partner_scope = savePartnerScope(partner_id, obj_partner);
                                //get corresponding partner_scope_id
                                partner_scope_id = partner_scope.partner_scope_id;
                            }
                        }
                        else
                        {
                            //check exist partner_scope
                            partner_scope = db.PartnerScopes.Where<PartnerScope>(x => x.partner_id == partner_id && x.scope_id == obj_partner.collab_scope_id).FirstOrDefault();
                            if (partner_scope != null)
                            {
                                //incease 1 to referecen count
                                increaseReferenceCountOfPartnerScope(partner_scope);
                            }
                            else
                            {
                                //add partner_id & scope_id to PartnerScope
                                partner_scope = savePartnerScope(partner_id, obj_partner);
                            }
                            //get corresponding partner_scope_id
                            partner_scope_id = partner_scope.partner_scope_id;
                        }

                        //update infor to AcademicCollaboration
                        AcademicCollaboration academicCollaboration = db.AcademicCollaborations.Find(obj_academic_collab.collab_id);
                        academicCollaboration.direction_id = direction_id;
                        academicCollaboration.collab_type_id = collab_type_id;
                        academicCollaboration.people_id = person_id;
                        academicCollaboration.partner_scope_id = partner_scope_id;
                        academicCollaboration.plan_study_start_date = obj_academic_collab.plan_start_date;
                        academicCollaboration.plan_study_end_date = obj_academic_collab.plan_end_date;
                        academicCollaboration.actual_study_start_date = obj_academic_collab.actual_start_date;
                        academicCollaboration.actual_study_end_date = obj_academic_collab.actual_end_date;
                        academicCollaboration.is_supported = obj_academic_collab.support;
                        academicCollaboration.note = obj_academic_collab.note;
                        db.SaveChanges();

                        //check exist file
                        //add file
                        var evidence_file = saveFile(f, new_evidence);

                        //add infor to CollaborationStatusHistory
                        var collab_status_hist = saveCollabStatusHistory(new_evidence, academicCollaboration.collab_id, obj_academic_collab.status_id, null, evidence_file, account_id);
                        trans.Commit();
                        return new AlertModal<AcademicCollaboration_Ext>(null, true, "Cập nhật cán bộ giảng viên thành công.");
                    }
                    else
                    {
                        AlertModal<AcademicCollaboration_Ext> alertModal = new AlertModal<AcademicCollaboration_Ext>(null, false, "Cảnh báo", "Với thời gian kế hoạch, CBGV đang đi học tại đối tác.");
                        return alertModal;
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
        }

        //DELETE
        public AlertModal<string> deleteAcademicCollaboration(int acad_collab_id)
        {
            using (DbContextTransaction dbContext = db.Database.BeginTransaction())
            {
                try
                {
                    AcademicCollaboration academicCollaboration = db.AcademicCollaborations.Find(acad_collab_id);
                    //decrease reference_count in PartnerScope
                    PartnerScope partnerScope = db.PartnerScopes.Find(academicCollaboration.partner_scope_id);
                    partnerScope.reference_count -= 1;
                    //delete AcademicCollab
                    db.AcademicCollaborations.Remove(academicCollaboration);
                    db.SaveChanges();
                    //delete partner_scope records with reference_count = 0
                    if (partnerScope.reference_count <= 0)
                    {
                        db.PartnerScopes.Remove(partnerScope);
                        db.SaveChanges();
                    }
                    dbContext.Commit();
                    return new AlertModal<string>(null, true, "Thành công", "Xóa hợp tác học thuật thành công.");
                }
                catch (Exception e)
                {
                    dbContext.Rollback();
                    return new AlertModal<string>(null, false, "Lỗi", "Có lỗi xảy ra.");
                }
            }
        }

        //VIEW STATUS HISTORY
        public BaseServerSideData<StatusHistory> getStatusHistories(BaseDatatable baseDatatable, int collab_id)
        {
            try
            {
                var sql = @"select csh.change_date, acs.collab_status_id, a.full_name, 
                            ISNULL(f.name, '') 'file_name', ISNULL(f.link, '') 'file_link', csh.note
                            from IA_AcademicCollaboration.CollaborationStatusHistory csh
                            join IA_AcademicCollaboration.AcademicCollaborationStatus acs on acs.collab_status_id = csh.collab_status_id
                            join General.Account a on a.account_id = csh.account_id
                            left join General.[File] f on f.[file_id] = csh.[file_id]
                            where csh.collab_id = @collab_id
                            ORDER BY csh.change_date DESC 
                            OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY";
                List<StatusHistory> statusHistory = db.Database.SqlQuery<StatusHistory>(sql, new SqlParameter("collab_id", collab_id)).ToList();
                int totalRecords = db.CollaborationStatusHistories.Where(x => x.collab_id == collab_id).Count();
                return new BaseServerSideData<StatusHistory>(statusHistory, totalRecords);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //CHANGE STATUS HISTORY
        public AlertModal<string> changeStatus(int collab_id, HttpPostedFileBase evidence_file, string folder_name, string status_id, string note, int account_id)
        {
            using (DbContextTransaction dbContext = db.Database.BeginTransaction())
            {
                try
                {
                    if (!(String.IsNullOrEmpty(status_id)))
                    {
                        int num_status_id = Int32.Parse(status_id);
                        Google.Apis.Drive.v3.Data.File f = new Google.Apis.Drive.v3.Data.File();
                        File file = new File();
                        if (evidence_file != null)
                        {
                            //upload to Drive
                            f = uploadEvidenceFile(evidence_file, folder_name, 4, false);
                            //add file to db
                            file = saveFile(f, evidence_file);
                        }
                        //add academic collab status history
                        var collab_staus_hist = saveCollabStatusHistory(evidence_file, collab_id, num_status_id, note, file, account_id);
                        dbContext.Commit();
                        return new AlertModal<string>(null, true, "Thành công", "Chuyển trạng thái hợp tác học thuật thành công.");
                    }
                    else
                    {
                        return new AlertModal<string>(null, false, "Lỗi", "Thông tin về trạng thái chưa được chọn lựa.");
                    }
                }
                catch (Exception e)
                {
                    dbContext.Rollback();
                    throw e;
                }
            }
        }

        //LONG-TERM GET CONTENT
        public AlertModal<AcademicCollaborationTypeLanguage> getLTContent(int collab_type_id, int language_id)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                AcademicCollaborationTypeLanguage ltContent = db.AcademicCollaborationTypeLanguages.Where(x => x.collab_type_id == collab_type_id && x.language_id == language_id).FirstOrDefault();
                return new AlertModal<AcademicCollaborationTypeLanguage>(ltContent, true, null, null);
            }
            catch (Exception e)
            {
                return new AlertModal<AcademicCollaborationTypeLanguage>(null, false, "Lỗi", "Có lỗi xảy ra");
            }
        }

        //LONG-TERM UPDATE CONTENT
        public AlertModal<string> updateLTContent(int collab_type_id, int language_id, string description)
        {
            try
            {
                AcademicCollaborationTypeLanguage academicCollaborationTypeLanguage = db.AcademicCollaborationTypeLanguages.Find(language_id, collab_type_id);
                academicCollaborationTypeLanguage.description = description;
                db.SaveChanges();
                return new AlertModal<string>(null, true, "Thành công", "Cập nhật nội dung thành công.");
            }
            catch (Exception e)
            {
                return new AlertModal<string>(null, false, "Lỗi", "Có lỗi xảy ra");
            }
        }

        //LONG-TERM GET GOING || COMING CONTENT
        public AlertModal<CollaborationTypeDirectionLanguage> getLTGCContent(int direction_id, int collab_type_id, int language_id)
        {
            try
            {
                var sql = @"select ctdl.*
                            from IA_AcademicCollaboration.CollaborationTypeDirection ctd
                            join IA_AcademicCollaboration.CollaborationTypeDirectionLanguage ctdl
                            on ctd.collab_type_direction_id = ctdl.collab_type_direction_id
                            where ctd.direction_id = @direction_id and ctd.collab_type_id = @collab_type_id
                            and ctdl.language_id = @language_id";
                CollaborationTypeDirectionLanguage ltgcContent = db.Database.SqlQuery<CollaborationTypeDirectionLanguage>(sql,
                    new SqlParameter("direction_id", direction_id),
                    new SqlParameter("collab_type_id", collab_type_id),
                    new SqlParameter("language_id", language_id)).FirstOrDefault();
                return new AlertModal<CollaborationTypeDirectionLanguage>(ltgcContent, true, null, null);
            }
            catch (Exception e)
            {
                return new AlertModal<CollaborationTypeDirectionLanguage>(null, false, "Lỗi", "Có lỗi xảy ra");
            }
        }

        //LONG-TERM UPDATE GOING || COMING CONTENT
        public AlertModal<string> updateLTGCContent(int collab_type_direction_id, int language_id, string description)
        {
            try
            {
                CollaborationTypeDirectionLanguage collaborationTypeDirectionLanguage = db.CollaborationTypeDirectionLanguages.Find(collab_type_direction_id, language_id);
                collaborationTypeDirectionLanguage.description = description;
                db.SaveChanges();
                return new AlertModal<string>(null, true, "Thành công", "Cập nhật nội dung thành công.");
            }
            catch (Exception e)
            {
                return new AlertModal<string>(null, false, "Lỗi", "Có lỗi xảy ra");
            }
        }
    }
}
