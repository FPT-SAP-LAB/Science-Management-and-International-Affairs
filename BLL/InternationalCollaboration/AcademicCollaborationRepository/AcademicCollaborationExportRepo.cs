using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace BLL.InternationalCollaboration.AcademicCollaborationRepository
{
    public class AcademicCollaborationExportRepo
    {
        public MemoryStream ExportACExcel(int direction, int collab_type_id, ObjectSearching_AcademicCollaboration obj_searching)
        {
            try
            {
                string path = HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/");
                string filename = "";
                if (direction == 1)
                {
                    filename = "AC_Going.xlsx";
                }else
                {
                    filename = "AC_Coming.xlsx";
                }
                    System.IO.FileInfo file = new System.IO.FileInfo(path + filename);
                List<AcademicCollaboration_Ext> AC_List = AC_ListToExportExcel(direction, collab_type_id, obj_searching);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();
                    int startRow = 3;
                    if(direction==1)
                    {
                        for (int i = 0; i < AC_List.Count; i++)
                        {
                            excelWorksheet.Cells[i + startRow, 1].Value = i + 1;
                            excelWorksheet.Cells[i + startRow, 2].Value = AC_List.ElementAt(i).people_name;
                            excelWorksheet.Cells[i + startRow, 3].Value = AC_List.ElementAt(i).email;
                            excelWorksheet.Cells[i + startRow, 4].Value = AC_List.ElementAt(i).office_name;
                            excelWorksheet.Cells[i + startRow, 5].Value = AC_List.ElementAt(i).partner_name;
                            excelWorksheet.Cells[i + startRow, 6].Value = AC_List.ElementAt(i).country_name;
                            excelWorksheet.Cells[i + startRow, 7].Value = AC_List.ElementAt(i).plan_study_start_date.ToString("dd-MM-yyyy");
                            excelWorksheet.Cells[i + startRow, 8].Value = AC_List.ElementAt(i).plan_study_end_date.ToString("dd-MM-yyyy");
                            if (AC_List.ElementAt(i).collab_status_id == 1)
                            {
                                excelWorksheet.Cells[i + startRow, 9].Value = "Đề xuất";
                            }
                            else if (AC_List.ElementAt(i).collab_status_id == 2)
                            {
                                excelWorksheet.Cells[i + startRow, 9].Value = "Đang thực hiện";
                            }
                            else if (AC_List.ElementAt(i).collab_status_id == 3)
                            {
                                excelWorksheet.Cells[i + startRow, 9].Value = "Không hoàn thành";
                            }
                            else if (AC_List.ElementAt(i).collab_status_id == 4)
                            {
                                excelWorksheet.Cells[i + startRow, 9].Value = "Đã hoàn thành";
                            }
                            excelWorksheet.Cells[i + startRow, 10].Value =
                                AC_List.ElementAt(i).is_supported == true ? "Có hỗ trợ" : "Không hỗ trợ";
                            excelWorksheet.Cells[i + startRow, 11].Value = AC_List.ElementAt(i).note;
                        }
                        string Flocation = "/Content/assets/excel/Collaboration/Download/AC_Going.xlsx";
                        string savePath = HostingEnvironment.MapPath(Flocation);
                        string handle = Guid.NewGuid().ToString();
                        excelPackage.SaveAs(new System.IO.FileInfo(HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/Download/AC_Going.xlsx")));
                    }
                    else
                    {
                        for (int i = 0; i < AC_List.Count; i++)
                        {
                            excelWorksheet.Cells[i + startRow, 1].Value = i + 1;
                            excelWorksheet.Cells[i + startRow, 2].Value = AC_List.ElementAt(i).people_name;
                            excelWorksheet.Cells[i + startRow, 3].Value = AC_List.ElementAt(i).email;
                            excelWorksheet.Cells[i + startRow, 4].Value = AC_List.ElementAt(i).partner_name;
                            excelWorksheet.Cells[i + startRow, 5].Value = AC_List.ElementAt(i).country_name;
                            excelWorksheet.Cells[i + startRow, 6].Value = AC_List.ElementAt(i).plan_study_start_date.ToString("dd-MM-yyyy");
                            excelWorksheet.Cells[i + startRow, 7].Value = AC_List.ElementAt(i).plan_study_end_date.ToString("dd-MM-yyyy");
                            if (AC_List.ElementAt(i).collab_status_id == 1)
                            {
                                excelWorksheet.Cells[i + startRow, 8].Value = "Đề xuất";
                            }
                            else if (AC_List.ElementAt(i).collab_status_id == 2)
                            {
                                excelWorksheet.Cells[i + startRow, 8].Value = "Đang thực hiện";
                            }
                            else if (AC_List.ElementAt(i).collab_status_id == 3)
                            {
                                excelWorksheet.Cells[i + startRow, 8].Value = "Không hoàn thành";
                            }
                            else if (AC_List.ElementAt(i).collab_status_id == 4)
                            {
                                excelWorksheet.Cells[i + startRow, 8].Value = "Đã hoàn thành";
                            }
                            excelWorksheet.Cells[i + startRow, 9].Value =
                                AC_List.ElementAt(i).is_supported == true ? "Có hỗ trợ" : "Không hỗ trợ";
                            excelWorksheet.Cells[i + startRow, 10].Value = AC_List.ElementAt(i).note;
                        }
                        string Flocation = "/Content/assets/excel/Collaboration/Download/AC_Coming.xlsx";
                        string savePath = HostingEnvironment.MapPath(Flocation);
                        string handle = Guid.NewGuid().ToString();
                        excelPackage.SaveAs(new System.IO.FileInfo(HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/Download/AC_Coming.xlsx")));
                    }
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
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

        public List<AcademicCollaboration_Ext> AC_ListToExportExcel(int direction, int collab_type_id, ObjectSearching_AcademicCollaboration obj_searching)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

            try
            {
                var sql = @"select
                        collab.collab_id, pp.people_id, pp.[name] 'people_name', pp.email, offi.office_name, pn.partner_name, c.country_name,
                        collab.plan_study_start_date, collab.plan_study_end_date,
                        acs.collab_status_id, acs.collab_status_id, acs.collab_status_name,
                        collab.is_supported, collab.note
                        from IA_AcademicCollaboration.AcademicCollaboration collab
                        join IA_Collaboration.PartnerScope mpc on collab.partner_scope_id = mpc.partner_scope_id
                        join IA_Collaboration.[Partner] pn on pn.partner_id = mpc.partner_id
                        left join General.Country c on c.country_id = pn.country_id
                        join General.People pp on collab.people_id = pp.people_id
                        left join General.[Profile] pf on pf.people_id = pp.people_id
                        left join General.Office offi on pp.office_id = offi.office_id
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
                        and ISNULL(offi.office_name, '') like @office_name ";
                if (obj_searching.year != 0)
                {
                    sql += @"and @year between YEAR(collab.plan_study_start_date) and YEAR(collab.plan_study_end_date)";
                }

                List<AcademicCollaboration_Ext> academicCollaborations;
                if (obj_searching.year != 0)
                {
                    academicCollaborations = db.Database.SqlQuery<AcademicCollaboration_Ext>(sql,
                                                        new SqlParameter("direction", direction),
                                                        new SqlParameter("collab_type_id", collab_type_id),
                                                        new SqlParameter("country_name", obj_searching.country_name == null ? "%%" : "%" + obj_searching.country_name + "%"),
                                                        new SqlParameter("partner_name", obj_searching.partner_name == null ? "%%" : "%" + obj_searching.partner_name + "%"),
                                                        new SqlParameter("office_name", obj_searching.office_name == null ? "%%" : "%" + obj_searching.office_name + "%"),
                                                        new SqlParameter("year", obj_searching.year)).ToList();
                }
                else
                {
                    academicCollaborations = db.Database.SqlQuery<AcademicCollaboration_Ext>(sql,
                                                        new SqlParameter("direction", direction),
                                                        new SqlParameter("collab_type_id", collab_type_id),
                                                        new SqlParameter("country_name", obj_searching.country_name == null ? "%%" : "%" + obj_searching.country_name + "%"),
                                                        new SqlParameter("partner_name", obj_searching.partner_name == null ? "%%" : "%" + obj_searching.partner_name + "%"),
                                                        new SqlParameter("office_name", obj_searching.office_name == null ? "%%" : "%" + obj_searching.office_name + "%")).ToList();
                }
                return academicCollaborations;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
