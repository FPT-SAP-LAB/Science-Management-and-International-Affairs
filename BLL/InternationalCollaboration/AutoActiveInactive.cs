using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.AutoActiveInactiveMOUMOA;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration
{
    public class AutoActiveInactive
    {
        public void activeMOU(int partner_scope_id, ScienceAndInternationalAffairsEntities db)
        {
            try
            {
                var sql_check = @"SELECT DISTINCT mou.mou_id
                                FROM IA_Collaboration.MOUPartnerScope mps
                                JOIN IA_Collaboration.MOU mou ON mou.mou_id = mps.mou_id AND mps.partner_scope_id = @partner_scope_id
                                JOIN IA_Collaboration.MOUPartner mp ON mp.mou_id = mou.mou_id
                                JOIN (SELECT msh.mou_id, MAX([datetime]) 'datetime'
                                FROM IA_Collaboration.MOUStatusHistory msh
                                GROUP BY msh.mou_id) a ON a.mou_id = mps.mou_id 
                                JOIN IA_Collaboration.MOUStatusHistory c ON a.mou_id = c.mou_id AND a.[datetime] = c.[datetime] AND c.mou_status_id = 2
                                LEFT JOIN SMIA_AcademicActivity.ActivityPartner ap ON mps.partner_scope_id = ap.partner_scope_id
                                LEFT JOIN IA_AcademicCollaboration.AcademicCollaboration ac ON ac.partner_scope_id = mps.partner_scope_id
                                LEFT JOIN SMIA_AcademicActivity.AcademicActivity aa ON ap.activity_id = aa.activity_id 
                                WHERE ((ap.partner_scope_id IS NOT NULL AND aa.activity_date_start BETWEEN mp.mou_start_date 
                                AND mou.mou_end_date AND aa.activity_date_end BETWEEN mp.mou_start_date AND mou.mou_end_date) 
                                OR (ac.partner_scope_id IS NOT NULL AND ac.plan_study_start_date BETWEEN mp.mou_start_date 
                                AND mou.mou_end_date AND ac.plan_study_end_date BETWEEN mp.mou_start_date AND mou.mou_end_date))";
                List<MOUPartnerScope_Ext> listMps = db.Database.SqlQuery<MOUPartnerScope_Ext>(sql_check,
                    new SqlParameter("partner_scope_id", partner_scope_id)).ToList();
                if (listMps.Count != 0)
                {
                    foreach (MOUPartnerScope_Ext item in listMps)
                    {
                        MOUStatusHistory mOUStatusHistory = new MOUStatusHistory();
                        mOUStatusHistory.datetime = DateTime.Now;
                        mOUStatusHistory.reason = "Có hoạt động đi kèm";
                        mOUStatusHistory.mou_status_id = 1;
                        mOUStatusHistory.mou_id = item.mou_id;
                        //insert into MOUStatusHistory
                        db.MOUStatusHistories.Add(mOUStatusHistory);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void inactiveMOU(int partner_scope_id, ScienceAndInternationalAffairsEntities db)
        {
            try
            {
                var sql_check = @"SELECT InactiveMOU.mou_id, checker
                                FROM
                                (SELECT DISTINCT mou.mou_id,
                                CASE WHEN ((ap.partner_scope_id IS NULL OR aa.activity_date_start NOT BETWEEN mp.mou_start_date 
                                AND mou.mou_end_date OR aa.activity_date_end NOT BETWEEN mp.mou_start_date AND mou.mou_end_date) 
                                AND (ac.partner_scope_id IS NULL OR ac.plan_study_start_date NOT  BETWEEN mp.mou_start_date 
                                AND mou.mou_end_date OR ac.plan_study_end_date NOT BETWEEN mp.mou_start_date AND mou.mou_end_date)) THEN 1 ELSE 0 END 'checker'
                                FROM IA_Collaboration.MOUPartnerScope mps
                                JOIN IA_Collaboration.MOU mou ON mou.mou_id = mps.mou_id AND mps.partner_scope_id = @partner_scope_id
                                JOIN IA_Collaboration.MOUPartnerScope mps2 ON mps2.mou_id = mou.mou_id
                                JOIN IA_Collaboration.MOUPartner mp ON mp.mou_id = mou.mou_id
                                JOIN (SELECT msh.mou_id, MAX([datetime]) 'datetime'
                                FROM IA_Collaboration.MOUStatusHistory msh
                                GROUP BY msh.mou_id) a ON a.mou_id = mps.mou_id 
                                JOIN IA_Collaboration.MOUStatusHistory c ON a.mou_id = c.mou_id AND a.[datetime] = c.[datetime] AND c.mou_status_id = 1
                                LEFT JOIN SMIA_AcademicActivity.ActivityPartner ap ON mps2.partner_scope_id = ap.partner_scope_id
                                LEFT JOIN IA_AcademicCollaboration.AcademicCollaboration ac ON ac.partner_scope_id = mps2.partner_scope_id
                                LEFT JOIN SMIA_AcademicActivity.AcademicActivity aa ON ap.activity_id = aa.activity_id) AS InactiveMOU";
                List<MOUPartnerScope_Ext_Inactive> listMps = db.Database.SqlQuery<MOUPartnerScope_Ext_Inactive>(sql_check,
                    new SqlParameter("partner_scope_id", partner_scope_id)).ToList();

                List<List<MOUPartnerScope_Ext_Inactive>> list_list_mps = new List<List<MOUPartnerScope_Ext_Inactive>>();
                List<MOUPartnerScope_Ext_Inactive> list_mps = new List<MOUPartnerScope_Ext_Inactive>();
                if (listMps.Count != 0)
                {
                    MOUPartnerScope_Ext_Inactive temp = new MOUPartnerScope_Ext_Inactive();
                    for (int i = 0; i < listMps.Count; i++)
                    {
                        MOUPartnerScope_Ext_Inactive item = listMps[i];
                        if (listMps.Count == 1)
                        {
                            list_mps.Add(item);
                            list_list_mps.Add(list_mps);
                            break;
                        }
                        if (temp.mou_id != item.mou_id)
                        {
                            list_mps = new List<MOUPartnerScope_Ext_Inactive>();
                        }
                        list_mps.Add(item);
                        if (list_mps.Count == 2)
                        {
                            list_list_mps.Add(list_mps);
                            list_mps = new List<MOUPartnerScope_Ext_Inactive>();
                        }
                        temp = item;
                    }
                    //process
                    foreach (var list_item in list_list_mps)
                    {
                        if (list_item.Count == 1 && list_item[0].checker == 1)
                        {
                            MOUStatusHistory mOUStatusHistory = new MOUStatusHistory();
                            mOUStatusHistory.datetime = DateTime.Now;
                            mOUStatusHistory.reason = "Chưa có hoạt động nào đi kèm";
                            mOUStatusHistory.mou_status_id = 2;
                            mOUStatusHistory.mou_id = list_item[0].mou_id;
                            //insert into MOUStatusHistory
                            db.MOUStatusHistories.Add(mOUStatusHistory);
                        }
                    }
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void activeMOA(int partner_scope_id, ScienceAndInternationalAffairsEntities db)
        {
            try
            {
                var sql_check = @"SELECT DISTINCT moa.moa_id
                                FROM IA_Collaboration.MOAPartnerScope mps
                                JOIN IA_Collaboration.MOA moa ON moa.moa_id = mps.moa_id AND mps.partner_scope_id = @partner_scope_id
                                JOIN IA_Collaboration.MOAPartner mp ON mp.moa_id = moa.moa_id
                                JOIN (SELECT msh.moa_id, MAX([datetime]) 'datetime'
                                FROM IA_Collaboration.MOAStatusHistory msh
                                GROUP BY msh.moa_id) a ON a.moa_id = mps.moa_id 
                                JOIN IA_Collaboration.MOAStatusHistory c ON a.moa_id = c.moa_id AND a.[datetime] = c.[datetime] 
                                AND c.mou_status_id = 2
                                LEFT JOIN SMIA_AcademicActivity.ActivityPartner ap ON mps.partner_scope_id = ap.partner_scope_id
                                LEFT JOIN IA_AcademicCollaboration.AcademicCollaboration ac ON ac.partner_scope_id = mps.partner_scope_id
                                LEFT JOIN SMIA_AcademicActivity.AcademicActivity aa ON ap.activity_id = aa.activity_id 
                                WHERE ((ap.partner_scope_id IS NOT NULL AND aa.activity_date_start BETWEEN mp.moa_start_date 
                                AND moa.moa_end_date AND aa.activity_date_end BETWEEN mp.moa_start_date AND moa.moa_end_date) 
                                OR (ac.partner_scope_id IS NOT NULL AND ac.plan_study_start_date BETWEEN mp.moa_start_date 
                                AND moa.moa_end_date AND ac.plan_study_end_date BETWEEN mp.moa_start_date AND moa.moa_end_date))";
                List<MOAPartnerScope_Ext> listMps = db.Database.SqlQuery<MOAPartnerScope_Ext>(sql_check,
                    new SqlParameter("partner_scope_id", partner_scope_id)).ToList();
                if (listMps.Count != 0)
                {
                    foreach (MOAPartnerScope_Ext item in listMps)
                    {
                        MOAStatusHistory mOAStatusHistory = new MOAStatusHistory();
                        mOAStatusHistory.datetime = DateTime.Now;
                        mOAStatusHistory.reason = "Có hoạt động đi kèm";
                        mOAStatusHistory.mou_status_id = 1;
                        mOAStatusHistory.moa_id = item.moa_id;
                        //insert into MOUStatusHistory
                        db.MOAStatusHistories.Add(mOAStatusHistory);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void inactiveMOA(int partner_scope_id, ScienceAndInternationalAffairsEntities db)
        {
            try
            {
                var sql_check = @"SELECT InactiveMOA.moa_id, checker
                                FROM
                                (SELECT DISTINCT moa.moa_id,
                                CASE WHEN ((ap.partner_scope_id IS NULL OR aa.activity_date_start NOT BETWEEN mp.moa_start_date 
                                AND moa.moa_end_date OR aa.activity_date_end NOT BETWEEN mp.moa_start_date AND moa.moa_end_date) 
                                AND (ac.partner_scope_id IS NULL OR ac.plan_study_start_date NOT  BETWEEN mp.moa_start_date 
                                AND moa.moa_end_date OR ac.plan_study_end_date NOT BETWEEN mp.moa_start_date AND moa.moa_end_date)) THEN 1 ELSE 0 END 'checker'
                                FROM IA_Collaboration.MOAPartnerScope mps
                                JOIN IA_Collaboration.MOA moa ON moa.moa_id = mps.moa_id AND mps.partner_scope_id = @partner_scope_id
                                JOIN IA_Collaboration.MOAPartner mp ON mp.moa_id = moa.moa_id 
                                JOIN (SELECT msh.moa_id, MAX([datetime]) 'datetime'
                                FROM IA_Collaboration.MOAStatusHistory msh
                                GROUP BY msh.moa_id) a ON a.moa_id = mps.moa_id 
                                JOIN IA_Collaboration.MOAStatusHistory c ON a.moa_id = c.moa_id AND a.[datetime] = c.[datetime] AND c.mou_status_id = 1
                                LEFT JOIN SMIA_AcademicActivity.ActivityPartner ap ON mps.partner_scope_id = ap.partner_scope_id
                                LEFT JOIN IA_AcademicCollaboration.AcademicCollaboration ac ON ac.partner_scope_id = mps.partner_scope_id
                                LEFT JOIN SMIA_AcademicActivity.AcademicActivity aa ON ap.activity_id = aa.activity_id) InactiveMOA";
                List<MOAPartnerScope_Ext_Inactive> listMps = db.Database.SqlQuery<MOAPartnerScope_Ext_Inactive>(sql_check,
                    new SqlParameter("partner_scope_id", partner_scope_id)).ToList();
                if (listMps.Count != 0)
                {
                    List<List<MOAPartnerScope_Ext_Inactive>> list_list_mps = new List<List<MOAPartnerScope_Ext_Inactive>>();
                    List<MOAPartnerScope_Ext_Inactive> list_mps = new List<MOAPartnerScope_Ext_Inactive>();
                    if (listMps.Count != 0)
                    {
                        MOAPartnerScope_Ext_Inactive temp = new MOAPartnerScope_Ext_Inactive();
                        for (int i = 0; i < listMps.Count; i++)
                        {
                            MOAPartnerScope_Ext_Inactive item = listMps[i];
                            if (listMps.Count == 1)
                            {
                                list_mps.Add(item);
                                list_list_mps.Add(list_mps);
                                break;
                            }
                            if (temp.moa_id != item.moa_id)
                            {
                                list_mps = new List<MOAPartnerScope_Ext_Inactive>();
                            }
                            list_mps.Add(item);
                            if (list_mps.Count == 2)
                            {
                                list_list_mps.Add(list_mps);
                                list_mps = new List<MOAPartnerScope_Ext_Inactive>();
                            }
                            temp = item;
                        }
                        //process
                        foreach (var list_item in list_list_mps)
                        {
                            if (list_item.Count == 1 && list_item[0].checker == 1)
                            {
                                MOAStatusHistory mOAStatusHistory = new MOAStatusHistory();
                                mOAStatusHistory.datetime = DateTime.Now;
                                mOAStatusHistory.reason = "Chưa có hoạt động nào đi kèm";
                                mOAStatusHistory.mou_status_id = 2;
                                mOAStatusHistory.moa_id = list_item[0].moa_id;
                                //insert into MOUStatusHistory
                                db.MOAStatusHistories.Add(mOAStatusHistory);
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void changeStatusMOUMOA(int partner_scope_id, ScienceAndInternationalAffairsEntities db)
        {
            try
            {
                activeMOU(partner_scope_id, db);
                activeMOA(partner_scope_id, db);
                inactiveMOU(partner_scope_id, db);
                inactiveMOA(partner_scope_id, db);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
