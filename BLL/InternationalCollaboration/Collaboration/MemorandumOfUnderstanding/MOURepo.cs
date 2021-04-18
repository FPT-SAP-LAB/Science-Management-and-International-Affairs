﻿using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class MOURepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public BaseServerSideData<ListMOU> listAllMOU(BaseDatatable baseDatatable, string partner_name, string contact_point_name, string mou_code)
        {
            try
            {
                if (baseDatatable.SortColumnName is null || baseDatatable.SortColumnName == "")
                {
                    baseDatatable.SortColumnName = "mou_partner_id";
                }
                if (baseDatatable.SortDirection is null)
                {
                    baseDatatable.SortDirection = "desc";
                }
                string sql_mouList =
                    @"select tb2.mou_partner_id, tb1.mou_code,tb3.partner_id,tb3.partner_name,tb11.country_name,
                        tb3.website,tb2.contact_point_name,tb2.contact_point_email,tb2.contact_point_phone,tb1.evidence,
                        tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.office_abbreviation, 
                        tb4.scope_abbreviation,
                        STRING_AGG(tb7.specialization_name, ',') 'specialization_name', tb9.mou_status_id,tb1.mou_id
                        from IA_Collaboration.MOU tb1 inner join IA_Collaboration.MOUPartner tb2
                        on tb1.mou_id = tb2.mou_id inner join IA_Collaboration.[Partner] tb3 
                        on tb2.partner_id = tb3.partner_id inner join
                        (select t1.mou_id,t2.partner_id, STRING_AGG(tb5.scope_abbreviation, ',') 'scope_abbreviation'
                        from IA_Collaboration.MOUPartnerScope t1 inner join
                        IA_Collaboration.PartnerScope t2 on t1.partner_scope_id = t2.partner_scope_id
                        inner join IA_Collaboration.CollaborationScope tb5 on
                        tb5.scope_id  = t2.scope_id
                        group by t1.mou_id,t2.partner_id) tb4
                        on tb4.mou_id = tb2.mou_id and tb3.partner_id = tb4.partner_id
                        inner join IA_Collaboration.MOUPartnerSpecialization tb6
                        on tb6.mou_partner_id = tb2.mou_partner_id 
                        inner join General.Specialization tb7
                        on tb7.specialization_id = tb6.specialization_id
                        inner join 
                        (select a.mou_id,a.mou_status_id from IA_Collaboration.MOUStatusHistory a
                        inner join 
                        (select max(datetime) as max_date,mou_id from IA_Collaboration.MOUStatusHistory
                        group by mou_id) b on
                        a.datetime = b.max_date and a.mou_id = b.mou_id) tb8 on
                        tb8.mou_id = tb1.mou_id
                        inner join IA_Collaboration.CollaborationStatus tb9 on
                        tb9.mou_status_id = tb8.mou_status_id
                        inner join General.Office tb10 on
                        tb10.office_id = tb1.office_id
                        inner join General.Country tb11 on 
                        tb11.country_id = tb3.country_id
                        where tb1.is_deleted = 0 ";
                string sql_recordsTotal = @"select count(*) from IA_Collaboration.MOUPartner t1 inner join 
                        IA_Collaboration.MOU t2 on t2.mou_id = t1.mou_id
                        inner join IA_Collaboration.Partner t3 on t3.partner_id = t1.partner_id
                        where t2.is_deleted = 0 ";
                if (partner_name != "")
                {
                    sql_mouList += "and tb3.partner_name like @partner_name ";
                    sql_recordsTotal += "and t3.partner_name like @partner_name ";
                }
                if (contact_point_name != "")
                {
                    sql_mouList += "and tb2.contact_point_name like @contact_point_name ";
                    sql_recordsTotal += "and t1.contact_point_name like @contact_point_name ";
                }
                if (mou_code != "")
                {
                    sql_mouList += "and tb1.mou_code like @mou_code ";
                    sql_recordsTotal += "and t2.mou_code like @mou_code ";
                }
                string sql_BonusQuery = @"
                        GROUP BY tb2.mou_partner_id, tb1.mou_code,tb3.partner_id,tb3.partner_name,tb11.country_name,
                        tb3.website,tb2.contact_point_name,tb2.contact_point_email,tb2.contact_point_phone,tb1.evidence,
                        tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.office_abbreviation, tb9.mou_status_id,tb1.mou_id,
                        tb4.scope_abbreviation
                        order by " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection + " " +
                        "OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY";
                sql_mouList += sql_BonusQuery;

                List<ListMOU> mouList = db.Database.SqlQuery<ListMOU>(sql_mouList,
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("contact_point_name", '%' + contact_point_name + '%'),
                    new SqlParameter("mou_code", '%' + mou_code + '%')).ToList();
                int recordsTotal = db.Database.SqlQuery<int>(sql_recordsTotal,
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("contact_point_name", '%' + contact_point_name + '%'),
                    new SqlParameter("mou_code", '%' + mou_code + '%')).First();
                handlingMOUListData(mouList, baseDatatable.Start);
                return new BaseServerSideData<ListMOU>(mouList, recordsTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BaseServerSideData<ListMOU> listAllMOUDeleted(BaseDatatable baseDatatable, string partner_name, string contact_point_name, string mou_code)
        {
            try
            {
                if (baseDatatable.SortColumnName is null || baseDatatable.SortColumnName == "")
                {
                    baseDatatable.SortColumnName = "mou_partner_id";
                }
                if (baseDatatable.SortDirection is null)
                {
                    baseDatatable.SortDirection = "desc";
                }
                string sql_mouList =
                    @"select tb2.mou_partner_id, tb1.mou_code,tb3.partner_id,tb3.partner_name,tb11.country_name,
                        tb3.website,tb2.contact_point_name,tb2.contact_point_email,tb2.contact_point_phone,tb1.evidence,
                        tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.office_abbreviation, 
                        tb4.scope_abbreviation,
                        STRING_AGG(tb7.specialization_name, ',') 'specialization_name', tb9.mou_status_id,tb1.mou_id
                        from IA_Collaboration.MOU tb1 inner join IA_Collaboration.MOUPartner tb2
                        on tb1.mou_id = tb2.mou_id inner join IA_Collaboration.[Partner] tb3 
                        on tb2.partner_id = tb3.partner_id inner join
                        (select t1.mou_id,t2.partner_id, STRING_AGG(tb5.scope_abbreviation, ',') 'scope_abbreviation'
                        from IA_Collaboration.MOUPartnerScope t1 inner join
                        IA_Collaboration.PartnerScope t2 on t1.partner_scope_id = t2.partner_scope_id
                        inner join IA_Collaboration.CollaborationScope tb5 on
                        tb5.scope_id  = t2.scope_id
                        group by t1.mou_id,t2.partner_id) tb4
                        on tb4.mou_id = tb2.mou_id and tb3.partner_id = tb4.partner_id
                        inner join IA_Collaboration.MOUPartnerSpecialization tb6
                        on tb6.mou_partner_id = tb2.mou_partner_id 
                        inner join General.Specialization tb7
                        on tb7.specialization_id = tb6.specialization_id
                        inner join 
                        (select a.mou_id,a.mou_status_id from IA_Collaboration.MOUStatusHistory a
                        inner join 
                        (select max(datetime) as max_date,mou_id from IA_Collaboration.MOUStatusHistory
                        group by mou_id) b on
                        a.datetime = b.max_date and a.mou_id = b.mou_id) tb8 on
                        tb8.mou_id = tb1.mou_id
                        inner join IA_Collaboration.CollaborationStatus tb9 on
                        tb9.mou_status_id = tb8.mou_status_id
                        inner join General.Office tb10 on
                        tb10.office_id = tb1.office_id
                        inner join General.Country tb11 on 
                        tb11.country_id = tb3.country_id
                        where tb1.is_deleted = 1";
                string sql_recordsTotal = @"select count(*) from IA_Collaboration.MOUPartner t1 inner join 
                        IA_Collaboration.MOU t2 on t2.mou_id = t1.mou_id
                        inner join IA_Collaboration.Partner t3 on t3.partner_id = t1.partner_id
                        where t2.is_deleted = 1 ";
                if (partner_name != "")
                {
                    sql_mouList += "and tb3.partner_name like @partner_name ";
                    sql_recordsTotal += "and t3.partner_name like @partner_name ";
                }
                if (contact_point_name != "")
                {
                    sql_mouList += "and tb2.contact_point_name like @contact_point_name ";
                    sql_recordsTotal += "and t1.contact_point_name like @contact_point_name ";
                }
                if (mou_code != "")
                {
                    sql_mouList += "and tb1.mou_code like @mou_code ";
                    sql_recordsTotal += "and t2.mou_code like @mou_code ";
                }
                string sql_BonusQuery = @"
                        GROUP BY tb2.mou_partner_id, tb1.mou_code,tb3.partner_id,tb3.partner_name,tb11.country_name,
                        tb3.website,tb2.contact_point_name,tb2.contact_point_email,tb2.contact_point_phone,tb1.evidence,
                        tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.office_abbreviation, tb9.mou_status_id,tb1.mou_id,
                        tb4.scope_abbreviation
                        order by " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection + " " +
                        "OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY";
                sql_mouList += sql_BonusQuery;
                List<ListMOU> mouList = db.Database.SqlQuery<ListMOU>(sql_mouList,
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("contact_point_name", '%' + contact_point_name + '%'),
                    new SqlParameter("mou_code", '%' + mou_code + '%')).ToList();
                int recordsTotal = db.Database.SqlQuery<int>(sql_recordsTotal,
                    new SqlParameter("partner_name", '%' + partner_name + '%'),
                    new SqlParameter("contact_point_name", '%' + contact_point_name + '%'),
                    new SqlParameter("mou_code", '%' + mou_code + '%')).First();
                handlingMOUListData(mouList, baseDatatable.Start);
                return new BaseServerSideData<ListMOU>(mouList, recordsTotal);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ListMOU> listAllMOUToExportExcel()
        {
            try
            {
                string sql_mouList =
                    @"select tb2.mou_partner_id, tb1.mou_code,tb3.partner_id,tb3.partner_name,tb11.country_name,
                        tb3.website,tb2.contact_point_name,tb2.contact_point_email,tb2.contact_point_phone,tb1.evidence,
                        tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.office_abbreviation, 
                        tb4.scope_abbreviation,
                        STRING_AGG(tb7.specialization_name, ',') 'specialization_name', tb9.mou_status_id,tb1.mou_id
                        from IA_Collaboration.MOU tb1 inner join IA_Collaboration.MOUPartner tb2
                        on tb1.mou_id = tb2.mou_id inner join IA_Collaboration.[Partner] tb3 
                        on tb2.partner_id = tb3.partner_id inner join
                        (select t1.mou_id,t2.partner_id, STRING_AGG(tb5.scope_abbreviation, ',') 'scope_abbreviation'
                        from IA_Collaboration.MOUPartnerScope t1 inner join
                        IA_Collaboration.PartnerScope t2 on t1.partner_scope_id = t2.partner_scope_id
                        inner join IA_Collaboration.CollaborationScope tb5 on
                        tb5.scope_id  = t2.scope_id
                        group by t1.mou_id,t2.partner_id) tb4
                        on tb4.mou_id = tb2.mou_id and tb3.partner_id = tb4.partner_id
                        inner join IA_Collaboration.MOUPartnerSpecialization tb6
                        on tb6.mou_partner_id = tb2.mou_partner_id 
                        inner join General.Specialization tb7
                        on tb7.specialization_id = tb6.specialization_id
                        inner join 
                        (select a.mou_id,a.mou_status_id from IA_Collaboration.MOUStatusHistory a
                        inner join 
                        (select max(datetime) as max_date,mou_id from IA_Collaboration.MOUStatusHistory
                        group by mou_id) b on
                        a.datetime = b.max_date and a.mou_id = b.mou_id) tb8 on
                        tb8.mou_id = tb1.mou_id
                        inner join IA_Collaboration.CollaborationStatus tb9 on
                        tb9.mou_status_id = tb8.mou_status_id
                        inner join General.Office tb10 on
                        tb10.office_id = tb1.office_id
                        inner join General.Country tb11 on 
                        tb11.country_id = tb3.country_id
                        where tb1.is_deleted = 0
                        GROUP BY tb2.mou_partner_id, tb1.mou_code,tb3.partner_id,tb3.partner_name,tb11.country_name,
                        tb3.website,tb2.contact_point_name,tb2.contact_point_email,tb2.contact_point_phone,tb1.evidence,
                        tb2.mou_start_date, tb1.mou_end_date, tb1.mou_note, tb10.office_abbreviation, tb9.mou_status_id,tb1.mou_id,
                        tb4.scope_abbreviation 
                        ";
                List<ListMOU> mouList = db.Database.SqlQuery<ListMOU>(sql_mouList).ToList();
                return mouList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void addMOU(MOUAdd input, BLL.Authen.LoginRepo.User user,
            Google.Apis.Drive.v3.Data.File file, HttpPostedFileBase evidence)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    //add MOU
                    //Check Partner
                    //add MOUPartner => 
                    //check or add PartnerScope
                    //add MOUPartnerScope
                    //add MOUPartnerSpecialization
                    //add MOUStatusHistory
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    DateTime mou_end_date = DateTime.ParseExact(input.BasicInfo.mou_end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    MOU m = new MOU
                    {
                        mou_code = input.BasicInfo.mou_code,
                        mou_end_date = mou_end_date,
                        mou_note = input.BasicInfo.mou_note,
                        //evidence = input.BasicInfo.evidence is null ? "" : input.BasicInfo.evidence,
                        //add ??
                        office_id = input.BasicInfo.office_id,
                        account_id = user is null ? 1 : user.account.account_id,
                        add_time = DateTime.Now,
                        is_deleted = false,
                        noti_count = 0
                    };
                    db.MOUs.Add(m);
                    //checkpoint 1
                    db.SaveChanges();
                    MOU objMOU = db.MOUs.Where(x => x.mou_code == input.BasicInfo.mou_code).First();

                    //Add MOUStatusHistory
                    db.MOUStatusHistories.Add(new ENTITIES.MOUStatusHistory
                    {
                        datetime = DateTime.Now,
                        reason = input.BasicInfo.reason,
                        mou_id = objMOU.mou_id,
                        mou_status_id = input.BasicInfo.mou_status_id
                    });

                    foreach (PartnerInfo item in input.PartnerInfo.ToList())
                    {
                        int partner_id_item = 0;
                        //new partner
                        if (item.partner_id == 0)
                        {
                            //add Article.
                            //add ArticleVersion.
                            //add Partner.
                            Article a = db.Articles.Add(new Article
                            {
                                need_approved = false,
                                article_status_id = 2,
                                account_id = user is null ? 1 : user.account.account_id,
                            });
                            ArticleVersion av = db.ArticleVersions.Add(new ArticleVersion
                            {
                                publish_time = DateTime.Now,
                                version_title = "",
                                article_id = a.article_id,
                                language_id = 1
                            });
                            db.Partners.Add(new ENTITIES.Partner
                            {
                                partner_name = item.partnername_add,
                                website = item.website_add,
                                address = item.address_add,
                                country_id = (int)item.nation_add,
                                article_id = a.article_id
                            });
                            //checkpoint 2
                            db.SaveChanges();
                            ENTITIES.Partner objPartner = db.Partners.Where(x => x.partner_name == item.partnername_add).First();
                            partner_id_item = objPartner.partner_id;
                        }
                        else //old partner
                        {
                            partner_id_item = (int)item.partner_id;
                        }
                        //add to MOUPartner via each partner of MOU
                        db.MOUPartners.Add(new ENTITIES.MOUPartner
                        {
                            mou_id = objMOU.mou_id,
                            partner_id = partner_id_item,
                            mou_start_date = DateTime.ParseExact(item.sign_date_mou_add, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            contact_point_name = item.represent_add,
                            contact_point_email = item.email_add,
                            contact_point_phone = item.phone_add
                        });
                        //PartnerScope and MOUPartnerScope
                        foreach (int tokenScope in item.coop_scope_add.ToList())
                        {
                            PartnerScope objPS = db.PartnerScopes.Where(x => x.partner_id == partner_id_item && x.scope_id == tokenScope).FirstOrDefault();
                            int partner_scope_id = 0;
                            if (objPS == null)
                            {
                                db.PartnerScopes.Add(new PartnerScope
                                {
                                    partner_id = partner_id_item,
                                    scope_id = tokenScope,
                                    reference_count = 1
                                });
                                //checkpoint 3
                                db.SaveChanges();
                                PartnerScope newObjPS = db.PartnerScopes.Where(x => x.partner_id == partner_id_item && x.scope_id == tokenScope).FirstOrDefault();
                                partner_scope_id = newObjPS.partner_scope_id;
                                //add to total PS List
                                totalRelatedPS.Add(newObjPS);
                            }
                            else
                            {
                                objPS.reference_count += 1;
                                db.Entry(objPS).State = EntityState.Modified;
                                partner_scope_id = objPS.partner_scope_id;
                                //add to total PS List
                                totalRelatedPS.Add(objPS);
                            }
                            db.MOUPartnerScopes.Add(new MOUPartnerScope
                            {
                                partner_scope_id = partner_scope_id,
                                mou_id = objMOU.mou_id
                            });
                        }
                        //checkpoint 4
                        db.SaveChanges();
                        //MOUPartnerSpe
                        MOUPartner objMOUPartner = db.MOUPartners.Where(x => (x.mou_id == objMOU.mou_id && x.partner_id == partner_id_item)).First();
                        foreach (int tokenSpe in item.specialization_add.ToList())
                        {
                            db.MOUPartnerSpecializations.Add(new MOUPartnerSpecialization
                            {
                                mou_partner_id = objMOUPartner.mou_partner_id,
                                specialization_id = tokenSpe,
                            });
                        }
                    }
                    db.SaveChanges();
                    transaction.Commit();

                    //change status corressponding MOU/MOA
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<int> listPS = totalRelatedPS.Select(x => x.partner_scope_id).Distinct().ToList();
                            new AutoActiveInactive().changeStatusMOUMOA(listPS, db);
                            dbContext.Commit();
                        }
                        catch (Exception e)
                        {
                            dbContext.Rollback();
                            throw e;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public MemoryStream ExportMOUExcel()
        {
            try
            {
                string path = HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/");
                string filename = "MOU.xlsx";
                FileInfo file = new FileInfo(path + filename);
                List<ListMOU> listMOU = listAllMOUToExportExcel();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();
                    int startRow = 3;
                    for (int i = 0; i < listMOU.Count; i++)
                    {
                        excelWorksheet.Cells[i + startRow, 1].Value = i + 1;
                        excelWorksheet.Cells[i + startRow, 2].Value = listMOU.ElementAt(i).mou_code;
                        excelWorksheet.Cells[i + startRow, 3].Value = listMOU.ElementAt(i).partner_name;
                        excelWorksheet.Cells[i + startRow, 4].Value = listMOU.ElementAt(i).country_name;
                        excelWorksheet.Cells[i + startRow, 5].Value = listMOU.ElementAt(i).website;
                        excelWorksheet.Cells[i + startRow, 6].Value = listMOU.ElementAt(i).specialization_name;
                        excelWorksheet.Cells[i + startRow, 7].Value = listMOU.ElementAt(i).contact_point_name;
                        excelWorksheet.Cells[i + startRow, 8].Value = listMOU.ElementAt(i).contact_point_email;
                        excelWorksheet.Cells[i + startRow, 9].Value = listMOU.ElementAt(i).contact_point_phone;
                        excelWorksheet.Cells[i + startRow, 10].Value = listMOU.ElementAt(i).mou_start_date.ToString("dd'/'MM'/'yyyy");
                        excelWorksheet.Cells[i + startRow, 11].Value = listMOU.ElementAt(i).mou_end_date.ToString("dd'/'MM'/'yyyy");
                        excelWorksheet.Cells[i + startRow, 12].Value = listMOU.ElementAt(i).office_abbreviation;
                        excelWorksheet.Cells[i + startRow, 13].Value = listMOU.ElementAt(i).scope_abbreviation;
                        excelWorksheet.Cells[i + startRow, 14].Value = listMOU.ElementAt(i).mou_status_id == 1 ? "Active" : "Inactive";
                    }
                    string Flocation = "/Content/assets/excel/Collaboration/Download/MOU.xlsx";
                    string savePath = HostingEnvironment.MapPath(Flocation);
                    //string downloadFile = "MOUDownload.xlsx";
                    string handle = Guid.NewGuid().ToString();
                    excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/Download/MOU.xlsx")));

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        excelPackage.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        return memoryStream;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void deleteMOU(int mou_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<PartnerScope> totalRelatedPS = new List<PartnerScope>();
                    //delete partner_scope_id 
                    //delete from ExMOA => MOA => ExMOU => MOU
                    string sql_ex_moa = @"select t3.* from IA_Collaboration.MOABonus t1
                        inner join IA_Collaboration.MOAPartnerScope t2
                        on t2.moa_bonus_id = t1.moa_bonus_id
                        inner join IA_Collaboration.PartnerScope t3
                        on t3.partner_scope_id = t2.partner_scope_id
                        inner join IA_Collaboration.MOA t4
                        on t4.moa_id = t1.moa_id
                        where t4.mou_id = @mou_id";
                    string sql_moa = @"select t2.* from IA_Collaboration.MOAPartnerScope t1
                        inner join IA_Collaboration.PartnerScope t2 on 
                        t1.partner_scope_id = t2.partner_scope_id
                        inner join IA_Collaboration.MOA t3 on
                        t3.moa_id = t1.moa_id
                        where t3.mou_id = @mou_id";
                    string sql_ex_mou = @"select t3.* from IA_Collaboration.MOUBonus t1
                        inner join IA_Collaboration.MOUPartnerScope t2
                        on t2.mou_bonus_id = t1.mou_bonus_id
                        inner join IA_Collaboration.PartnerScope t3
                        on t3.partner_scope_id = t2.partner_scope_id
                        where t1.mou_id = @mou_id";
                    string sql_mou = @"select t2.* from IA_Collaboration.MOUPartnerScope t1
                        inner join IA_Collaboration.PartnerScope t2 on 
                        t1.partner_scope_id = t2.partner_scope_id
                        where t1.mou_id = @mou_id";
                    List<PartnerScope> ex_moa_list = db.Database.SqlQuery<PartnerScope>(sql_ex_moa,
                        new SqlParameter("mou_id", mou_id)).ToList();
                    List<PartnerScope> moa_list = db.Database.SqlQuery<PartnerScope>(sql_moa,
                        new SqlParameter("mou_id", mou_id)).ToList();
                    List<PartnerScope> ex_mou_list = db.Database.SqlQuery<PartnerScope>(sql_ex_mou,
                        new SqlParameter("mou_id", mou_id)).ToList();
                    List<PartnerScope> mou_list = db.Database.SqlQuery<PartnerScope>(sql_mou,
                        new SqlParameter("mou_id", mou_id)).ToList();

                    //if (ex_moa_list != null)
                    //{
                    //    foreach (PartnerScope item in ex_moa_list)
                    //    {
                    //        db.PartnerScopes.Find(item.partner_scope_id).reference_count -= 1;
                    //    }
                    //}
                    //if (moa_list != null)
                    //{
                    //    foreach (PartnerScope item in moa_list)
                    //    {
                    //        db.PartnerScopes.Find(item.partner_scope_id).reference_count -= 1;
                    //    }
                    //}
                    if (ex_mou_list != null)
                    {
                        foreach (PartnerScope item in ex_mou_list)
                        {
                            db.PartnerScopes.Find(item.partner_scope_id).reference_count -= 1;
                        }
                    }
                    if (mou_list != null)
                    {
                        foreach (PartnerScope item in mou_list)
                        {
                            db.PartnerScopes.Find(item.partner_scope_id).reference_count -= 1;
                        }
                    }
                    db.SaveChanges();
                    totalRelatedPS.AddRange(ex_moa_list);
                    totalRelatedPS.AddRange(moa_list);
                    totalRelatedPS.AddRange(ex_mou_list);
                    totalRelatedPS.AddRange(mou_list);

                    MOU mou = db.MOUs.Find(mou_id);
                    mou.is_deleted = true;
                    db.Entry(mou).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();

                    //change status corressponding MOU/MOA
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            List<int> listPS = totalRelatedPS.Select(x => x.partner_scope_id).Distinct().ToList();
                            new AutoActiveInactive().changeStatusMOUMOA(listPS, db);
                            dbContext.Commit();
                        }
                        catch (Exception e)
                        {
                            dbContext.Rollback();
                            throw e;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public string getSuggestedMOUCode()
        {
            try
            {
                string sql_mouCode = @"select count(*) from IA_Collaboration.MOU where mou_code like @year";
                string sql_checkDup = @"select count(*) from IA_Collaboration.MOU where mou_code = @newCode";
                bool isDuplicated = false;
                string newCode = "";
                int countInYear = db.Database.SqlQuery<int>(sql_mouCode,
                        new SqlParameter("year", '%' + DateTime.Now.Year + '%')).First();
                //fix duplicate mou_code:
                do
                {
                    countInYear++;
                    newCode = DateTime.Now.Year + "/" + countInYear;
                    isDuplicated = db.Database.SqlQuery<int>(sql_checkDup,
                        new SqlParameter("newCode", newCode)).First() == 1 ? true : false;
                } while (isDuplicated);
                return newCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CustomOffice> GetOffice()
        {
            try
            {
                string sql_unitList = @"select office_id,office_abbreviation from General.Office";
                List<CustomOffice> unitList = db.Database.SqlQuery<CustomOffice>(sql_unitList).ToList();
                return unitList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ENTITIES.Partner> GetPartners()
        {
            try
            {
                List<ENTITIES.Partner> partnerList = db.Partners.ToList();
                return partnerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Specialization> GetSpecializations()
        {
            try
            {
                List<Specialization> speList = db.Specializations.ToList();
                return speList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Country> GetCountries()
        {
            try
            {
                List<Country> countryList = db.Countries.ToList();
                return countryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CollaborationScope> GetCollaborationScopes()
        {
            try
            {
                string sql_scopeList = @"select * from IA_Collaboration.CollaborationScope";
                List<CollaborationScope> scopeList = db.Database.SqlQuery<CollaborationScope>(sql_scopeList).ToList();
                return scopeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CustomPartner CheckPartner(string partner_name)
        {
            try
            {
                string sql = @"select t1.partner_id,t1.partner_name,t2.country_id,t2.country_name
                    ,t1.address,t1.website from IA_Collaboration.Partner t1
                    left join General.Country t2 on
                    t1.country_id = t2.country_id where t1.partner_name = @partner_name";
                CustomPartner p = db.Database.SqlQuery<CustomPartner>(sql,
                    new SqlParameter("partner_name", partner_name)).FirstOrDefault();
                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void handlingMOUListData(List<ListMOU> mouList, int start)
        {
            //handling spe and scope data.
            //handling calender display
            foreach (ListMOU item in mouList.ToList())
            {
                item.mou_start_date_string = item.mou_start_date.ToString("dd'/'MM'/'yyyy");
                item.mou_end_date_string = item.mou_end_date.ToString("dd'/'MM'/'yyyy");
                item.RowNumber = ++start;
            }
        }
        public int getDuration()
        {
            DateTime today = DateTime.Today;
            DateTime end_date = new DateTime(2021, 05, 20);
            TimeSpan value = end_date.Subtract(today);
            return value.Duration().Days;
        }
        public NotificationInfo getNoti()
        {
            try
            {
                NotificationInfo noti = new NotificationInfo();
                DateTime nextMonth = DateTime.Now.AddMonths(1);
                DateTime next3Months = DateTime.Now.AddMonths(3);
                //Warning 1: end_date < next3Months && notiCount = 0
                //warning 2: end_date < nextMonths && notiCount = 1
                string sql_inactive_number
                    = @"select count(*) from IA_Collaboration.MOU tb1 left join 
                        (
                        select max([datetime]) as 'maxdate',mou_status_id, mou_id
                        from IA_Collaboration.MOUStatusHistory 
                        group by mou_status_id, mou_id) tb2 
                        on tb1.mou_id = tb2.mou_id
                        where tb1.is_deleted = 0";
                string sql_expired
                    = @"select mou_code from IA_Collaboration.MOU
                        where (mou_end_date < @next3Months and mou_end_date > getdate() and noti_count = 0) or 
                        (mou_end_date < @nextMonth and mou_end_date > getdate() and noti_count = 1)";
                noti.InactiveNumber = db.Database.SqlQuery<int>(sql_inactive_number).First();
                noti.ExpiredMOUCode = db.Database.SqlQuery<string>(sql_expired,
                    new SqlParameter("@next3Months", next3Months),
                    new SqlParameter("@nextMonth", nextMonth)).ToList();
                updateNotiCount(noti);
                return noti;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void updateNotiCount(NotificationInfo noti)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (string mouCode in noti.ExpiredMOUCode)
                    {
                        MOU mou = db.MOUs.Where(x => x.mou_code.Equals(mouCode)).First();
                        mou.noti_count += 1;
                        db.Entry(mou).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public void UpdateStatusMOU()
        {
            //get current date
            //get all expired ActiveMOU.
            //if number > 0: update status for MOU: Active => Inactive.
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string sql_expired = @"select mou_id from IA_Collaboration.MOU tb1 where tb1.mou_end_date < @current_date
                        and tb1.mou_id in(
                        select t1.mou_id from IA_Collaboration.MOUStatusHistory t1
                        inner join 
                        (select max(datetime) as max_date,mou_id from IA_Collaboration.MOUStatusHistory
                        group by mou_id) t2 on
                        t1.datetime = t2.max_date and t1.mou_id = t2.mou_id
                        where mou_status_id = 1)";
                    List<int> mouIdList = db.Database.SqlQuery<int>(sql_expired,
                        new SqlParameter("current_date", DateTime.Now)).ToList();
                    if (mouIdList.Count > 0)
                    {
                        foreach (int id in mouIdList)
                        {
                            MOUStatusHistory mou = new MOUStatusHistory();
                            mou.mou_id = id;
                            mou.mou_status_id = 2;
                            mou.datetime = DateTime.Now;
                            mou.reason = "Quá hạn";
                            db.MOUStatusHistories.Add(mou);
                            db.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        public DuplicatePartnerInfo partnerInfoIsDuplicated(string partner_name, string mou_start_date_string)
        {
            try
            {
                DateTime mou_start_date = DateTime.ParseExact(mou_start_date_string, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                string sql_check = @"select t1.mou_code,t3.partner_name,t2.mou_start_date,t4.full_name
                    from IA_Collaboration.MOU t1
                    inner
                    join IA_Collaboration.MOUPartner t2
                    on t2.mou_id = t1.mou_id
                    inner
                    join IA_Collaboration.Partner t3
                    on t3.partner_id = t2.partner_id
                    inner
                    join General.Account t4
                    on t4.account_id = t1.account_id
                    where partner_name like @partner_name
                    and mou_start_date = @date";
                DuplicatePartnerInfo info = db.Database.SqlQuery<DuplicatePartnerInfo>(sql_check,
                    new SqlParameter("partner_name", partner_name),
                    new SqlParameter("date", mou_start_date)).FirstOrDefault();
                if (info != null)
                {
                    mou_start_date_string = mou_start_date.ToString("dd'/'MM'/'yyyy");
                }
                return info;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool getMOUCodeCheck(string mou_code)
        {
            try
            {
                MOU obj = db.MOUs.Where(x => x.mou_code == mou_code && !x.is_deleted).FirstOrDefault();
                return obj == null ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IntersectPeriodMOUDate checkIntersectPeriodMOUDate(List<PartnerInfo> PartnerInfo, string start_date, string end_date, int office_id)
        {
            string partner_id_para = "";
            foreach (PartnerInfo item in PartnerInfo)
            {
                partner_id_para += (item.partner_id + ",");
            }
            partner_id_para = partner_id_para.Remove(partner_id_para.Length - 1);
            string query = @"select count(*) as num_check,max(mou_start_date) as mou_start_date
                , mou_end_date, t2.mou_id, t2.mou_code, t2.office_id
                 from IA_Collaboration.MOUPartner t1
                inner join IA_Collaboration.MOU t2
                on t2.mou_id = t1.mou_id
                where t1.partner_id in (" + partner_id_para + @") and t2.is_deleted = 0 and t2.office_id = @office_id
                group by mou_end_date, t2.mou_id, t2.mou_code , t2.office_id
                having count(*) = @partner_count
                order by mou_id";
            List<IntersectPeriodMOUDate> obj = db.Database.SqlQuery<IntersectPeriodMOUDate>(query,
                    new SqlParameter("partner_count", PartnerInfo.Count),
                    new SqlParameter("office_id", office_id)).ToList();
            DateTime current_start_date = DateTime.ParseExact(start_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime current_end_date = DateTime.ParseExact(end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            foreach (IntersectPeriodMOUDate item in obj)
            {
                if (DateRangeisInvalid(item.mou_start_date, item.mou_end_date, current_start_date, current_end_date))
                {
                    item.mou_start_date_string = item.mou_start_date.ToString("dd'/'MM'/'yyyy");
                    item.mou_end_date_string = item.mou_end_date.ToString("dd'/'MM'/'yyyy");
                    return item;
                }
            }
            return new IntersectPeriodMOUDate();
        }
        public bool DateRangeisInvalid(DateTime start, DateTime end, DateTime test_start, DateTime test_end)
        {
            return !(test_end < start || test_start > end);
        }

        public static Google.Apis.Drive.v3.Data.File UploadMOUFile(HttpPostedFileBase InputFile, string FolderName, int TypeFolder, bool isFolder)
        {
            return UploadMOUFile(new List<HttpPostedFileBase> { InputFile }, FolderName, TypeFolder, isFolder)[0];
        }

        public static List<Google.Apis.Drive.v3.Data.File> UploadMOUFile(List<HttpPostedFileBase> InputFiles, string FolderName, int TypeFolder, bool isFolder)
        {
            string SubFolderName, SecondSubFolderName;
            switch (TypeFolder)
            {
                case 1:
                    SubFolderName = "MOU";
                    SecondSubFolderName = "BaseFile";
                    break;
                case 2:
                    SubFolderName = "MOU";
                    SecondSubFolderName = "AdditionalFile";
                    break;
                case 3:
                    SubFolderName = "MOA";
                    SecondSubFolderName = "BaseFile";
                    break;
                case 4:
                    SubFolderName = "MOA";
                    SecondSubFolderName = "AdditionalFile";
                    break;
                default:
                    throw new ArgumentException("Loại folder không tồn tại");
            }

            var MOUFolder = GoogleDriveService.FindFirstFolder(SubFolderName, GoogleDriveService.IADrive) ?? GoogleDriveService.CreateFolder(SubFolderName, GoogleDriveService.IADrive);
            var TypeMOUFolder = GoogleDriveService.FindFirstFolder(SecondSubFolderName, MOUFolder.Id) ?? GoogleDriveService.CreateFolder(SecondSubFolderName, MOUFolder.Id);

            var folder = GoogleDriveService.FindFirstFolder(FolderName, TypeMOUFolder.Id) ?? GoogleDriveService.CreateFolder(FolderName, TypeMOUFolder.Id);

            List<Google.Apis.Drive.v3.Data.File> UploadedFiles = new List<Google.Apis.Drive.v3.Data.File>();

            foreach (HttpPostedFileBase item in InputFiles)
            {
                var file = GoogleDriveService.UploadFile(item.FileName, item.InputStream, item.ContentType, folder.Id);

                UploadedFiles.Add(file);

                GoogleDriveService.ShareWithAnyone(file.Id);
            }

            if (isFolder)
            {
                return new List<Google.Apis.Drive.v3.Data.File>
                {
                    folder //return parent files
                };
            }
            else
            {
                return UploadedFiles;
            }
        }

        public Google.Apis.Drive.v3.Data.File uploadEvidenceFile(HttpPostedFileBase InputFile, string FolderName, int TypeFolder, bool isFolder)
        {
            string file_id = "";
            try
            {
                Google.Apis.Drive.v3.Data.File f = UploadMOUFile(InputFile, FolderName, TypeFolder, isFolder);
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
    }
}
