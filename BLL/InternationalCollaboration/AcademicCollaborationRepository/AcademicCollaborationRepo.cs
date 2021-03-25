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
                        join General.Country c on c.country_id = pn.country_id
                        join General.People pp on collab.people_id = pp.people_id
                        join General.[Profile] pf on pf.people_id = pp.people_id
                        join General.Office offi on pf.office_id = offi.office_id
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
                        ORDER BY @sortColumnName @sortDirection OFFSET @start ROWS FETCH NEXT @length ROWS ONLY";
                //filter checking
                SqlParameter country_name_param, year_param, partner_name_param, office_name_param;

                ///country_name
                if (obj_searching.country_name != null)
                {
                    country_name_param = new SqlParameter("country_name", "%" + obj_searching.country_name + "%");
                }
                else
                {
                    country_name_param = new SqlParameter("country_name", "%%");
                }

                ///partner_name
                if (obj_searching.partner_name != null)
                {
                    partner_name_param = new SqlParameter("partner_name", "%" + obj_searching.partner_name + "%");
                }
                else
                {
                    partner_name_param = new SqlParameter("partner_name", "%%");
                }

                ///office_name
                if (obj_searching.office_name != null)
                {
                    office_name_param = new SqlParameter("office_name", "%" + obj_searching.office_name + "%");
                }
                else
                {
                    office_name_param = new SqlParameter("office_name", "%%");
                }

                ///year
                year_param = new SqlParameter("year", obj_searching.year);

                List<AcademicCollaboration_Ext> academicCollaborations = db.Database.SqlQuery<AcademicCollaboration_Ext>(sql,
                                                    new SqlParameter("direction", direction),
                                                    new SqlParameter("collab_type_id", collab_type_id),
                                                    country_name_param,
                                                    partner_name_param,
                                                    office_name_param,
                                                    year_param,
                                                    new SqlParameter("sortColumnName", baseDatatable.SortColumnName),
                                                    new SqlParameter("sortDirection", baseDatatable.SortDirection),
                                                    new SqlParameter("start", baseDatatable.Start),
                                                    new SqlParameter("length", baseDatatable.Length)).ToList();

                int recordsTotal = db.Database.SqlQuery<int>("select count(*) from IA_AcademicCollaboration.AcademicCollaboration").FirstOrDefault();
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
                return new AlertModal<List<Country>>(countries, true, null, null);
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
                return new AlertModal<YearSearching>(yearSearching, true, null, null);
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
                return new AlertModal<List<AcademicCollaborationPartner_Ext>>(partners, true, null, null);
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
                return new AlertModal<AcademicCollaborationPartner_Ext>(partner, true, null, null);
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
                return new AlertModal<List<Office>>(offices, true, null, null);
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
                return new AlertModal<List<AcademicCollaborationPerson_Ext>>(people, true, null, null);
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
                    return new AlertModal<AcademicCollaborationPerson_Ext>(person, true, null, null);
                }
                else
                {
                    return new AlertModal<AcademicCollaborationPerson_Ext>(null, false, "Lỗi", "Lấy dữ liệu về cán bộ đã có lỗi xảy ra.");
                }
            }
            catch (Exception e)
            {
                return new AlertModal<AcademicCollaborationPerson_Ext>(null, false, "Lỗi", "Lấy dữ liệu về cán bộ đã có lỗi xảy ra.");
            }
        }

        public AlertModal<List<CollaborationScope>> collaborationScopes(string collab_abbreviation_name)
        {
            try
            {
                var sql = @"-----1.8. Phạm vi hợp tác
                    select * from IA_Collaboration.CollaborationScope
                    where scope_abbreviation like @collab_abbreviation_name
                    or scope_name like @collab_abbreviation_name";
                List<CollaborationScope> collaborationScopes = db.Database.SqlQuery<CollaborationScope>(sql,
                    new SqlParameter("collab_abbreviation_name", collab_abbreviation_name == null ? "%%" : "%" + collab_abbreviation_name + "%")).ToList();
                return new AlertModal<List<CollaborationScope>>(collaborationScopes, true, null, null);
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
                return new AlertModal<List<AcademicCollaborationStatu>>(academicCollaborationStatus, true, null, null);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<AcademicCollaboration_Ext> saveAcademicCollaboration(int direction_id, int collab_type_id, SaveAcadCollab_Person obj_person, SaveAcadCollab_Partner obj_partner, SaveAcadCollab_AcademicCollaboration obj_academic_collab, int account_id)
        {
            try
            {
                using (DbContextTransaction trans = db.Database.BeginTransaction())
                {
                    Person person;
                    var person_id = 0;
                    Partner partner;
                    var partner_id = 0;
                    var partner_scope_id = 0;

                    //check available person
                    if (!obj_person.available_person)
                    {
                        //add new person
                        //add information to People
                        person = new Person()
                        {
                            name = obj_person.person_name,
                            email = obj_person.person_email
                        };
                        db.People.Add(person);
                        db.SaveChanges();

                        //check office_id with Office
                        var office = db.Offices.Find(obj_person.person_profile_office_id);
                        if (office == null)
                        {
                            return new AlertModal<AcademicCollaboration_Ext>(null, false, "Lỗi", "Không thêm được cơ sở tương ứng.");
                        }
                        else
                        {
                            var profile = new Profile()
                            {
                                people_id = person.people_id,
                                office_id = obj_person.person_profile_office_id
                            };
                            db.Profiles.Add(profile);
                        }

                        //pass person_id value
                        person_id = person.people_id;
                    }
                    else
                    {
                        person_id = obj_person.person_id;
                    }

                    //check available partner
                    if (!obj_partner.available_partner)
                    {
                        //add new partner
                        partner = new Partner()
                        {
                            partner_name = obj_partner.partner_name
                        };

                        //check country_id with Country
                        var country = db.Countries.Find(obj_partner.partner_country_id);
                        if (country == null)
                        {
                            return new AlertModal<AcademicCollaboration_Ext>(null, false, "Lỗi", "Không thêm được quốc gia tương ứng.");
                        }
                        else
                        {
                            partner.country_id = obj_partner.partner_country_id;
                            db.Partners.Add(partner);
                            db.SaveChanges();

                            //pass partner_id value
                            partner_id = partner.partner_id;
                        }
                    }
                    else
                    {
                        partner_id = obj_partner.partner_id;
                        //check exist partner_scope
                        var partner_scope = db.PartnerScopes.Where<PartnerScope>(x => x.partner_id == partner_id && x.scope_id == obj_partner.collab_scope_id).FirstOrDefault();
                        if (partner_scope != null)
                        {
                            //get corresponding partner_scope_id
                            partner_scope_id = partner_scope.partner_scope_id;
                        }
                        else
                        {
                            //add partner_id & scope_id to PartnerScope
                            partner_scope = new PartnerScope()
                            {
                                partner_id = partner_id,
                                scope_id = obj_partner.collab_scope_id
                            };
                            db.PartnerScopes.Add(partner_scope);
                            db.SaveChanges();

                            //get corresponding partner_scope_id
                            partner_scope_id = partner_scope.partner_scope_id;
                        }
                    }

                    //add infor to AcademicCollaboration
                    var academic_collaboration = new AcademicCollaboration()
                    {
                        direction_id = direction_id,
                        collab_type_id = collab_type_id,
                        people_id = person_id,
                        partner_scope_id = partner_scope_id,
                        plan_study_start_date = obj_academic_collab.plan_start_date,
                        plan_study_end_date = obj_academic_collab.plan_end_date,
                        actual_study_start_date = obj_academic_collab.actual_start_date,
                        actual_study_end_date = obj_academic_collab.actual_end_date,
                        is_supported = obj_academic_collab.support,
                        note = obj_academic_collab.note
                    };
                    db.AcademicCollaborations.Add(academic_collaboration);
                    db.SaveChanges();

                    //add infor to CollaborationStatusHistory

                    var collab_status_hist = new CollaborationStatusHistory()
                    {
                        collab_id = academic_collaboration.collab_id,
                        collab_status_id = obj_academic_collab.status_id,
                        change_date = DateTime.Now,
                        evidence = obj_academic_collab.evidence_link,
                        account_id = account_id
                    };
                    db.CollaborationStatusHistories.Add(collab_status_hist);
                    db.SaveChanges();

                    trans.Commit();
                    return new AlertModal<AcademicCollaboration_Ext>(null, true, "Thành công", "Thêm cán bộ giảng viên thành công.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
