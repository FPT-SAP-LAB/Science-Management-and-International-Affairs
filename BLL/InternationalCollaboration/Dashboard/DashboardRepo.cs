using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.Dashboard
{
    public class DashboardRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public List<ChartDashboard> GetDashboard(int? year)
        {
            db = new ScienceAndInternationalAffairsEntities();
            try
            {
                string query = @"SELECT p1.[year], p1.signed, p2.total, (p2.total - p1.signed) as 'not_sign_yet' FROM
                            (SELECT b.[year], COUNT(b.partner_id) 'signed' FROM
                            (SELECT DISTINCT a.[year], p.partner_id
                            FROM (SELECT DISTINCT YEAR(mou_start_date) 'year' FROM IA_Collaboration.MOUPartner 
                            where YEAR(mou_start_date) <= {0}) a 
                            JOIN IA_Collaboration.MOUPartner mp
                            ON a.[year] >= YEAR(mou_start_date) JOIN IA_Collaboration.[Partner] p
                            ON mp.partner_id = p.partner_id JOIN IA_Collaboration.MOU mou
                            ON mou.mou_id = mp.mou_id AND a.[year] BETWEEN YEAR(mp.mou_start_date) AND YEAR(mou.mou_end_date) 
                            JOIN IA_Collaboration.PartnerScope ps ON ps.partner_id = p.partner_id LEFT JOIN SMIA_AcademicActivity.ActivityPartner ap
                            ON ps.partner_scope_id = ap.partner_scope_id AND YEAR(ap.cooperation_date_start) = a.[year]
                            AND ap.cooperation_date_start BETWEEN mp.mou_start_date AND mou.mou_end_date
                            LEFT JOIN IA_AcademicCollaboration.AcademicCollaboration ac ON ac.partner_scope_id = ps.partner_scope_id
                            AND YEAR(ac.plan_study_start_date) = a.[year] AND ac.plan_study_start_date BETWEEN mp.mou_start_date AND mou.mou_end_date
                            WHERE (ap.activity_partner_id IS NOT NULL) OR (ac.collab_id IS NOT NULL))b
                            GROUP BY b.[year]) p1 JOIN 
                            (SELECT YEAR(mp.mou_start_date) 'year', COUNT(mp.mou_partner_id) 'total' 
                            FROM IA_Collaboration.MOUPartner mp
                            GROUP BY YEAR(mp.mou_start_date)
                            ) p2 ON p1.[year] = p2.[year]
                            order by p1.year desc ";

                if (year is null)
                {
                    year = DateTime.Now.Year;
                }
                List<ChartDashboard> chartDashboard = db.Database.SqlQuery<ChartDashboard>(query, year).ToList();

                return chartDashboard;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public BaseServerSideData<DashboardDatatable> GetTable(int collab_type_id, int year, BaseDatatable baseDatatable)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                string query = @"SELECT CASE WHEN ac.direction_id = 1 THEN p.partner_name ELSE 'Đại học FPT' END 'training',
                            CASE WHEN ac.direction_id = 1 THEN o.office_name ELSE p.partner_name END 'working', acs.collab_status_name, count(ac.collab_id) 'count'
                            FROM IA_Collaboration.[Partner] p JOIN IA_Collaboration.PartnerScope ps 
                            ON p.partner_id = ps.partner_id JOIN IA_AcademicCollaboration.AcademicCollaboration ac 
                            ON ac.partner_scope_id = ps.partner_scope_id JOIN General.People pe 
                            ON pe.people_id = ac.people_id JOIN 
                            (SELECT collab_id, MAX(change_date) 'change_date' FROM IA_AcademicCollaboration.CollaborationStatusHistory GROUP BY collab_id) [max]
                            ON ac.collab_id = [max].collab_id JOIN IA_AcademicCollaboration.CollaborationStatusHistory cs  
                            ON [max].collab_id = cs.collab_id AND [max].change_date = cs.change_date JOIN IA_AcademicCollaboration.AcademicCollaborationStatus acs
                            ON acs.collab_status_id = cs.collab_status_id LEFT JOIN General.Office o 
                            ON o.office_id = pe.office_id
                            WHERE ac.collab_type_id = {0} AND YEAR(ac.plan_study_start_date) = {1}
                            GROUP BY p.partner_name, o.office_name, acs.collab_status_name, ac.direction_id ";

                string paging = @" ORDER BY " + baseDatatable.SortColumnName + " "
                            + baseDatatable.SortDirection +
                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT "
                            + baseDatatable.Length + " ROWS ONLY";

                List<DashboardDatatable> list = db.Database.SqlQuery<DashboardDatatable>(query + paging, collab_type_id, year).ToList();
                int totalRecord = db.Database.SqlQuery<DashboardDatatable>(query, collab_type_id, year).Count();
                return new BaseServerSideData<DashboardDatatable>(list, totalRecord);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
