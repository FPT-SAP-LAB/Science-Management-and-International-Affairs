using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.Collaboration.PartnerRepo
{
    public class PartnerRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public BaseServerSideData<PartnerList> getListAll(BaseDatatable baseDatatable, SearchPartner searchPartner)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                string sql = @" select ROW_NUMBER() OVER(ORDER BY a.partner_id ASC) 'no' , partner_name,
                                    a.partner_id, a.is_deleted, a.website, a.address, a.is_collab,
                                STRING_AGG(a.specialization_name, ',') 'specialization_name', a.country_name from 
                                (select distinct t1.partner_name, t1.partner_id, t1.is_deleted, t1.website, t1.address,
		                        t4.specialization_name,
		                        t5.country_name,
                                t2.partner_id = t1.partner_id left join IA_Collaboration.MOU t6
		                        on t6.mou_id = t2.mou_id 
                                left join IA_Collaboration.MOUPartnerSpecialization t3 on
                                t3.mou_partner_id = t2.mou_partner_id
                                left join General.Specialization t4 on 
                                t4.specialization_id = t3.specialization_id 
		                        left join General.Country t5 on 
		                        t1.country_id = t5.country_id
                                where t1.is_deleted = 0 ) as a
								where isnull(a.partner_name, '') like {0} and
								isnull(a.specialization_name, '') like {1} and
								isnull(a.country_name, '') like {2} and
								isnull(a.is_collab , '') like {3}
		                        group by a.partner_name, a.partner_id, 
		                        a.is_deleted, a.website, a.address, a.partner_id,
		                        a.country_name, a.is_collab ";

                string paging = @" ORDER BY " + baseDatatable.SortColumnName + " "
                            + baseDatatable.SortDirection +
                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT "
                            + baseDatatable.Length + " ROWS ONLY";

                List<PartnerList> listPartner = db.Database.SqlQuery<PartnerList>(sql + paging, 
                      searchPartner.partner_name == null? "%%": "%" + searchPartner.partner_name + "%", 
                    "%" + searchPartner.specialization == null? "%%" : "%" + searchPartner.specialization + "%", 
                    "%" + searchPartner.nation == null? "%%" : "%" + searchPartner.nation + "%", 
                     searchPartner.is_collab == 0? "%%" : "%" + searchPartner.is_collab + "%").ToList();

                int totalRecord = db.Database.SqlQuery<PartnerList>(sql,
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

        public void addPartner()
        {
            return;
        }
        public void deletePartner()
        {
            return;
        }
        public void editPartner()
        {
            return;
        }
        public PartnerHistoryList<PartnerHistory> getHistory(int id)
        {
            try
            {
                string query = @"WITH b AS(
                                SELECT ps.partner_scope_id, ps.partner_id
                                FROM IA_Collaboration.PartnerScope ps 
                                WHERE ps.partner_id = {0}
                                )
                                SELECT '' as 'code', N'Tổ chức hoạt động học thuật' 'activity', '' AS 'full_name', aa.activity_date_start, aa.activity_date_end
                                FROM SMIA_AcademicActivity.AcademicActivity aa INNER JOIN SMIA_AcademicActivity.ActivityPartner ap
                                ON aa.activity_id = ap.activity_id INNER JOIN b 
                                ON b.partner_scope_id = ap.partner_scope_id
                                UNION ALL
                                SELECT '' as 'code', N'Hợp tác nghiên cứu' 'activity', '', rc.project_start_date, rc.project_end_date
                                FROM IA_ResearchCollaboration.ResearchCollaboration rc INNER JOIN IA_ResearchCollaboration.ResearchPartner rp
                                ON rc.project_id = rp.project_id INNER JOIN b ON rp.partner_scope_id = b.partner_scope_id
                                UNION ALL
                                SELECT '' as 'code', N'Hợp tác học thuật' 'activity', '', ac.plan_study_start_date, ac.plan_study_end_date
                                FROM IA_AcademicCollaboration.AcademicCollaboration ac INNER JOIN b 
                                ON ac.partner_scope_id = b.partner_scope_id
                                UNION ALL
                                SELECT DISTINCT mou.mou_code, N'Ký kết biên bản ghi nhớ' 'activity', a.full_name,
                                mp.mou_start_date, mou.mou_end_date
                                FROM b INNER JOIN IA_Collaboration.MOUPartner mp ON b.partner_id = mp.partner_id
                                INNER JOIN IA_Collaboration.MOU mou ON mou.mou_id = mp.mou_id INNER JOIN General.[Account] a
                                ON mou.account_id = a.account_id
                                UNION ALL
                                SELECT DISTINCT moa.moa_code, N'Ký kết biên bản thỏa thuận' 'activity', a.full_name, mp.moa_start_date, moa.moa_end_date
                                FROM b INNER JOIN IA_Collaboration.MOAPartner mp ON b.partner_id = mp.partner_id
                                INNER JOIN IA_Collaboration.MOA moa ON moa.moa_id = mp.moa_id 
                                INNER JOIN General.Account a ON a.account_id = moa.account_id
                                UNION ALL
                                SELECT DISTINCT mb.mou_bonus_code, N'Ký kết biên bản ghi nhớ bổ sung' 'activity', '', mb.mou_bonus_decision_date, mb.mou_bonus_end_date
                                FROM b INNER JOIN IA_Collaboration.MOUPartner mp ON b.partner_id = mp.partner_id
                                INNER JOIN IA_Collaboration.MOUBonus mb ON mp.mou_id = mb.mou_id 
                                LEFT JOIN IA_Collaboration.MOuPartnerScope mps ON mps.mou_bonus_id = mb.mou_bonus_id
                                WHERE (mb.mou_bonus_end_date IS NOT NULL) OR (mps.partner_scope_id IS NOT NULL)
                                UNION ALL
                                SELECT DISTINCT mb.moa_bonus_code, N'Ký kết biên bản thỏa thuận bổ sung' 'activity', '', MB.moa_bonus_decision_date, mb.moa_bonus_end_date
                                FROM b INNER JOIN IA_Collaboration.MOAPartner mp ON b.partner_id = mp.partner_id
                                INNER JOIN IA_Collaboration.MOABonus mb ON mp.moa_id = mb.moa_id 
                                LEFT JOIN IA_Collaboration.MOAPartnerScope mps ON mps.moa_bonus_id = mb.moa_bonus_id
                                WHERE (mb.moa_bonus_end_date IS NOT NULL) OR (mps.partner_scope_id IS NOT NULL)
                                ORDER BY activity_date_start ASC ";
                List<PartnerHistory> list = db.Database.SqlQuery<PartnerHistory>(query, id).ToList();
                string partner_name = db.Partners.Where(x => x.partner_id == id).Select(x => x.partner_name).FirstOrDefault();
                return new PartnerHistoryList<PartnerHistory>(list, partner_name);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        public void getPreView()
        {
            return;
        }
        public void exportExcel()
        {
            return;
        }
    }
}
