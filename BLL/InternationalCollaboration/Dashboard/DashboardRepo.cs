using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration.Dashboard;
using ENTITIES.CustomModels.ScienceManagement.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;

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
                string query = @"WITH c AS (
                                SELECT [year] FROM
                                (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + 2014 AS 'year'
                                FROM (VALUES(0),(0),(0),(0),(0),(0),(0),(0),(0),(0)) a(n)
                                CROSS JOIN (VALUES(0),(0),(0),(0),(0),(0),(0),(0),(0),(0)) b(n)
                                ) AS a
                                WHERE [year] <= {0})
                                SELECT top(5) p2.[year], CASE WHEN p1.signed IS NULL THEN 0 ELSE p1.signed END 'signed', p2.total, CASE WHEN p1.signed IS NULL THEN p2.total ELSE (p2.total - p1.signed) END as 'not_sign_yet' FROM
                                (SELECT b.[year], COUNT(b.partner_id) 'signed' FROM
                                (SELECT DISTINCT c.[year], p.partner_id
                                FROM IA_Collaboration.MOUPartner mp JOIN IA_Collaboration.[Partner] p
                                ON mp.partner_id = p.partner_id JOIN IA_Collaboration.MOU mou
                                ON mou.mou_id = mp.mou_id
                                JOIN IA_Collaboration.PartnerScope ps ON ps.partner_id = p.partner_id 
                                LEFT JOIN IA_Collaboration.MOUBonus mb ON mou.mou_id = mb.mou_id JOIN c
                                ON ((c.[year] BETWEEN YEAR(mp.mou_start_date) AND YEAR(mb.mou_bonus_end_date)) OR (c.[year] BETWEEN YEAR(mp.mou_start_date) 
                                AND YEAR(mou.mou_end_date) AND mb.mou_bonus_end_date IS NULL)) JOIN IA_Collaboration.MOUPartnerScope mps ON mps.mou_id = mou.mou_id
                                LEFT JOIN SMIA_AcademicActivity.ActivityPartner ap
                                ON ps.partner_scope_id = ap.partner_scope_id AND YEAR(ap.cooperation_date_start) = c.[year]
                                AND ap.cooperation_date_start BETWEEN mp.mou_start_date AND mou.mou_end_date AND ap.partner_scope_id = mps.partner_scope_id
                                LEFT JOIN IA_AcademicCollaboration.AcademicCollaboration ac ON ac.partner_scope_id = ps.partner_scope_id
                                AND YEAR(ac.plan_study_start_date) = c.[year] AND ac.plan_study_start_date BETWEEN mp.mou_start_date AND mou.mou_end_date
                                AND ac.partner_scope_id = mps.partner_scope_id
                                WHERE ((ap.activity_partner_id IS NOT NULL) OR (ac.collab_id IS NOT NULL)))b
                                GROUP BY b.[year]) p1 RIGHT JOIN 
                                (SELECT d.[year], COUNT(d.partner_id) 'total' FROM(SELECT DISTINCT c.[year], mp.partner_id
                                FROM IA_Collaboration.MOUPartner mp JOIN IA_Collaboration.MOU mou
                                ON mp.mou_id = mou.mou_id LEFT JOIN IA_Collaboration.MOUBonus mb ON mb.mou_id = mou.mou_id JOIN c
                                ON ((c.[year] BETWEEN YEAR(mp.mou_start_date) AND YEAR(mb.mou_bonus_end_date)) OR (c.[year] BETWEEN YEAR(mp.mou_start_date) 
                                AND YEAR(mou.mou_end_date) AND mb.mou_bonus_end_date IS NULL))
                                ) d GROUP BY d.[year]) p2 ON p1.[year] = p2.[year]
                                ORDER BY p2.[year] DESC
 ";

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
                string query = @"SELECT CASE WHEN ac.direction_id = 1 THEN p.partner_name ELSE N'Đại học FPT' END 'training',
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

        public DashboardNumber GetHomeData()
        {
            throw new NotImplementedException();
        }

        public string WidgetMou(int year)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                string query_total = @"SELECT COUNT(mou_id) 'count' FROM(
                                        SELECT DISTINCT mou.mou_id 
                                        FROM IA_Collaboration.MOU mou JOIN IA_Collaboration.MOUPartner mp
                                        ON mou.mou_id = mp.mou_id
                                        WHERE (YEAR(mp.mou_start_date) <= {0})
                                        UNION ALL
                                        SELECT DISTINCT moa.moa_id
                                        FROM IA_Collaboration.MOA moa JOIN IA_Collaboration.MOAPartner mp
                                        ON moa.moa_id = mp.moa_id
                                        WHERE (YEAR(mp.moa_start_date) <= {0})) a
";

                int total = db.Database.SqlQuery<int>(query_total, year).FirstOrDefault();

                string query_expired = @"SELECT COUNT(mou_id) 'expired' FROM
                        (SELECT DISTINCT mou.mou_id
                        FROM IA_Collaboration.MOU mou LEFT JOIN IA_Collaboration.MOUBonus mb
                        ON mou.mou_id = mb.mou_id 
                        WHERE (DATEDIFF(day, mb.mou_bonus_end_date, GETDATE()) <= 90) OR 
                        (mb.mou_bonus_end_date IS NULL AND DATEDIFF(day, mb.mou_bonus_end_date, GETDATE()) <= 90)) b";

                int expired = db.Database.SqlQuery<int>(query_expired).FirstOrDefault();

                string display_mou_widget = total + "/" + expired;
                return display_mou_widget;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "0/0";
            }
        }
        public int WidgetCollab(int year)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                string query = @"SELECT COUNT(partner_id) 'count' FROM (SELECT DISTINCT (mp.partner_id)
                            FROM IA_Collaboration.MOU mou JOIN IA_Collaboration.MOUPartner mp
                            ON mou.mou_id = mp.mou_id
                            WHERE (YEAR(mp.mou_start_date) <= {0} AND mou.is_deleted = 0)) a";

                int collab = db.Database.SqlQuery<int>(query, year).FirstOrDefault();

                return collab;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
        public int WidgetSupport(int year)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                string query = @"SELECT COUNT(collab_id) 'count'
                        FROM IA_AcademicCollaboration.AcademicCollaboration
                        WHERE YEAR(plan_study_start_date) = {0} and is_supported = 1 and direction_id = 1 and collab_type_id = 2
                        GROUP BY is_supported, direction_id, collab_type_id";

                int support = db.Database.SqlQuery<int>(query, year).FirstOrDefault();

                return support;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
    }
}
