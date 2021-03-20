using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.AcademicCollaboration
{
    public class AcademicCollaborationRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public BaseServerSideData<AcademicCollaboration_Ext> academicCollaborations(int direction, int collab_type_id)
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
                        where collab.direction_id = @direction /*Dài hạn = 2, Ngắn hạn = 1*/ and collab.collab_type_id = @collab_type_id /*Chiều đi = 1, Chiều đến = 2*/";
                List<AcademicCollaboration_Ext> academicCollaborations = db.Database.SqlQuery<AcademicCollaboration_Ext>(sql,
                                                    new SqlParameter("direction", direction), new SqlParameter("collab_type_id", collab_type_id)).ToList();
                int recordsTotal = db.Database.SqlQuery<int>("select count(*) from IA_AcademicCollaboration.AcademicCollaboration").FirstOrDefault();
                return new BaseServerSideData<AcademicCollaboration_Ext>(academicCollaborations, recordsTotal);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Country> countries()
        {
            try
            {

                var sql = @"-----1.1. Danh sách Country
                        select * from General.Country";
                List<Country> countries = db.Database.SqlQuery<Country>(sql).ToList();
                return countries;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public YearSearching yearSearching()
        {
            try
            {
                var sql = @"-----1.2. Danh sách Năm 
                    select YEAR(MIN(plan_study_start_date)) as 'year_from', YEAR(GETDATE()) as 'year_to' 
                    from IA_AcademicCollaboration.AcademicCollaboration";
                YearSearching yearSearching = db.Database.SqlQuery<YearSearching>(sql).FirstOrDefault();
                return yearSearching;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Partner> partners()
        {
            try
            {
                var sql = @"-----1.3. Đơn vị đào tạo - chiều đi/chiều đến -> partner/office
                    select * from IA_Collaboration.[Partner]
                    where is_deleted = 0";
                List<Partner> partners = db.Database.SqlQuery<Partner>(sql).ToList();
                return partners;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Office> offices()
        {
            try
            {
                var sql = @"-----1.4. Đơn vị công tác - chiều đi/chiều đến -> office/partner
                    select * from General.Office";
                List<Office> offices = db.Database.SqlQuery<Office>(sql).ToList();
                return offices;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
