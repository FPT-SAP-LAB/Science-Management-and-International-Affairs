using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityExcelRepo
    {
        AcademicActivityExpenseRepo expenseRepo;
        public MemoryStream ExportDuTruExcel(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "TemplateKinhPhiDuTru.xlsx";
            FileInfo file = new FileInfo(path + filename);
            List<AcademicActivityExpenseRepo.excelEstimate> list = expenseRepo.getExcelDuTru(activity_id);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();
                int startRow = 4;
                string save_office = list.ElementAt(0).office_name;
                using (ExcelRange Rng = excelWorksheet.Cells[3, 1, 3, 6])
                {
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    Rng.Value = save_office;
                }
                int stt = 1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list.ElementAt(i).office_name.Equals(save_office))
                    {
                        excelWorksheet.Cells[i + startRow, 1].Value = stt;
                        excelWorksheet.Cells[i + startRow, 2].Value = list.ElementAt(i).expense_category_name;
                        excelWorksheet.Cells[i + startRow, 3].Style.Numberformat.Format = "###,###,##0";
                        excelWorksheet.Cells[i + startRow, 3].Value = list.ElementAt(i).expense_price;
                        excelWorksheet.Cells[i + startRow, 4].Value = list.ElementAt(i).expense_quantity;
                        excelWorksheet.Cells[i + startRow, 5].Style.Numberformat.Format = "###,###,##0";
                        excelWorksheet.Cells[i + startRow, 5].Value = list.ElementAt(i).total;
                        excelWorksheet.Cells[i + startRow, 6].Value = list.ElementAt(i).note;
                        stt += 1;
                    }
                    else
                    {
                        save_office = list.ElementAt(i).office_name;
                        using (ExcelRange Rng = excelWorksheet.Cells[i+startRow, 1, i + startRow, 6])
                        {
                            Rng.Merge = true;
                            Rng.Style.Font.Bold = true;
                            Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            Rng.Value = save_office;
                            stt = 1;
                            excelWorksheet.Cells[i + startRow+1, 1].Value = stt;
                            excelWorksheet.Cells[i + startRow+1, 2].Value = list.ElementAt(i).expense_category_name;
                            excelWorksheet.Cells[i + startRow + 1, 3].Style.Numberformat.Format = "###,###,##0";
                            excelWorksheet.Cells[i + startRow+1, 3].Value = list.ElementAt(i).expense_price;
                            excelWorksheet.Cells[i + startRow+1, 4].Value = list.ElementAt(i).expense_quantity;
                            excelWorksheet.Cells[i + startRow + 1, 5].Style.Numberformat.Format = "###,###,##0";
                            excelWorksheet.Cells[i + startRow+1, 5].Value = list.ElementAt(i).total;
                            excelWorksheet.Cells[i + startRow+1, 6].Value = list.ElementAt(i).note;
                        }
                    }
                }
                string Flocation = "/Excel_template/Download/kinh-phi-du-tru.xlsx";
                string savePath = HostingEnvironment.MapPath(Flocation);
                string handle = Guid.NewGuid().ToString();
                excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/Download/kinh-phi-du-tru.xlsx")));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    excelPackage.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
        }
        public MemoryStream ExportDieuChinhExcel(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "TemplateKinhPhiDieuChinh.xlsx";
            FileInfo file = new FileInfo(path + filename);
            List<AcademicActivityExpenseRepo.excelModified> list = expenseRepo.getExcelDieuChinh(activity_id);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();
                int startRow = 4;
                string save_office = list.ElementAt(0).office_name;
                using (ExcelRange Rng = excelWorksheet.Cells[3, 1, 3, 6])
                {
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    Rng.Value = save_office;
                }
                int stt = 1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list.ElementAt(i).office_name.Equals(save_office))
                    {
                        excelWorksheet.Cells[i + startRow, 1].Value = stt;
                        excelWorksheet.Cells[i + startRow, 2].Value = list.ElementAt(i).expense_category_name;
                        excelWorksheet.Cells[i + startRow, 3].Style.Numberformat.Format = "###,###,##0";
                        excelWorksheet.Cells[i + startRow, 3].Value = list.ElementAt(i).price_dieuchinh;
                        excelWorksheet.Cells[i + startRow, 4].Value = list.ElementAt(i).sl_dieuchinh;
                        excelWorksheet.Cells[i + startRow, 5].Style.Numberformat.Format = "###,###,##0";
                        excelWorksheet.Cells[i + startRow, 5].Value = list.ElementAt(i).total_dieuchinh;
                        excelWorksheet.Cells[i + startRow, 6].Value = list.ElementAt(i).note;
                        stt += 1;
                    }
                    else
                    {
                        save_office = list.ElementAt(i).office_name;
                        using (ExcelRange Rng = excelWorksheet.Cells[i + startRow, 1, i + startRow, 6])
                        {
                            Rng.Merge = true;
                            Rng.Style.Font.Bold = true;
                            Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            Rng.Value = save_office;
                            stt = 1;
                            excelWorksheet.Cells[i + startRow + 1, 1].Value = stt;
                            excelWorksheet.Cells[i + startRow + 1, 2].Value = list.ElementAt(i).expense_category_name;
                            excelWorksheet.Cells[i + startRow + 1, 3].Style.Numberformat.Format = "###,###,##0";
                            excelWorksheet.Cells[i + startRow + 1, 3].Value = list.ElementAt(i).price_dieuchinh;
                            excelWorksheet.Cells[i + startRow + 1, 4].Value = list.ElementAt(i).sl_dieuchinh;
                            excelWorksheet.Cells[i + startRow + 1, 5].Style.Numberformat.Format = "###,###,##0";
                            excelWorksheet.Cells[i + startRow + 1, 5].Value = list.ElementAt(i).total_dieuchinh;
                            excelWorksheet.Cells[i + startRow + 1, 6].Value = list.ElementAt(i).note;
                        }
                    }
                }
                string Flocation = "/Excel_template/Download/kinh-phi-dieu-chinh.xlsx";
                string savePath = HostingEnvironment.MapPath(Flocation);
                string handle = Guid.NewGuid().ToString();
                excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/Download/kinh-phi-dieu-chinh.xlsx")));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    excelPackage.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
        }
        public MemoryStream ExportTongHopExcel(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "TemplateKinhPhiTongHop.xlsx";
            FileInfo file = new FileInfo(path + filename);
            List<AcademicActivityExpenseRepo.excelModified> list = expenseRepo.getExcelTongHop(activity_id);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();
                int startRow = 4;
                string save_office = list.ElementAt(0).office_name;
                using (ExcelRange Rng = excelWorksheet.Cells[3, 1, 3, 6])
                {
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    Rng.Value = save_office;
                }
                int stt = 1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list.ElementAt(i).office_name.Equals(save_office))
                    {
                        excelWorksheet.Cells[i + startRow, 1].Value = stt;
                        excelWorksheet.Cells[i + startRow, 2].Value = list.ElementAt(i).expense_category_name;
                        excelWorksheet.Cells[i + startRow, 3].Style.Numberformat.Format = "###,###,##0";
                        excelWorksheet.Cells[i + startRow, 3].Value = list.ElementAt(i).total_dieuchinh;
                        excelWorksheet.Cells[i + startRow, 4].Style.Numberformat.Format = "###,###,##0";
                        excelWorksheet.Cells[i + startRow, 4].Value = list.ElementAt(i).total_bandau;
                        excelWorksheet.Cells[i + startRow, 5].Style.Numberformat.Format = "###,###,##0";
                        excelWorksheet.Cells[i + startRow, 5].Value = list.ElementAt(i).total_bandau - list.ElementAt(i).total_dieuchinh;
                        excelWorksheet.Cells[i + startRow, 6].Value = list.ElementAt(i).note;
                        stt += 1;
                    }
                    else
                    {
                        save_office = list.ElementAt(i).office_name;
                        using (ExcelRange Rng = excelWorksheet.Cells[i + startRow, 1, i + startRow, 6])
                        {
                            Rng.Merge = true;
                            Rng.Style.Font.Bold = true;
                            Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            Rng.Value = save_office;
                            stt = 1;
                            excelWorksheet.Cells[i + startRow + 1, 1].Value = stt;
                            excelWorksheet.Cells[i + startRow + 1, 2].Value = list.ElementAt(i).expense_category_name;
                            excelWorksheet.Cells[i + startRow + 1, 3].Style.Numberformat.Format = "###,###,##0";
                            excelWorksheet.Cells[i + startRow + 1, 3].Value = list.ElementAt(i).total_dieuchinh;
                            excelWorksheet.Cells[i + startRow + 1, 4].Style.Numberformat.Format = "###,###,##0";
                            excelWorksheet.Cells[i + startRow + 1, 4].Value = list.ElementAt(i).total_bandau;
                            excelWorksheet.Cells[i + startRow + 1, 5].Style.Numberformat.Format = "###,###,##0";
                            excelWorksheet.Cells[i + startRow + 1, 5].Value = list.ElementAt(i).total_bandau - list.ElementAt(i).total_dieuchinh;
                            excelWorksheet.Cells[i + startRow + 1, 6].Value = list.ElementAt(i).note;
                        }
                    }
                }
                string Flocation = "/Excel_template/Download/kinh-phi-tong-hop.xlsx";
                string savePath = HostingEnvironment.MapPath(Flocation);
                string handle = Guid.NewGuid().ToString();
                excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/Download/kinh-phi-tong-hop.xlsx")));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    excelPackage.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
        }
        public MemoryStream ExportThucTeExcel(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "TemplateKinhPhiThucTe.xlsx";
            FileInfo file = new FileInfo(path + filename);
            List<AcademicActivityExpenseRepo.excelModified> list = expenseRepo.getExcelTongHop(activity_id);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(file))
            {
                ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();
                int startRow = 4;
                string save_office = list.ElementAt(0).office_name;
                using (ExcelRange Rng = excelWorksheet.Cells[3, 1, 3, 6])
                {
                    Rng.Merge = true;
                    Rng.Style.Font.Bold = true;
                    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    Rng.Value = save_office;
                }
                int stt = 1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list.ElementAt(i).office_name.Equals(save_office))
                    {
                        excelWorksheet.Cells[i + startRow, 1].Value = stt;
                        excelWorksheet.Cells[i + startRow, 2].Value = list.ElementAt(i).expense_category_name;
                        excelWorksheet.Cells[i + startRow, 3].Style.Numberformat.Format = "###,###,##0";
                        excelWorksheet.Cells[i + startRow, 3].Value = list.ElementAt(i).price_dieuchinh;
                        excelWorksheet.Cells[i + startRow, 4].Value = list.ElementAt(i).sl_dieuchinh;
                        excelWorksheet.Cells[i + startRow, 5].Style.Numberformat.Format = "###,###,##0";
                        excelWorksheet.Cells[i + startRow, 5].Value = list.ElementAt(i).total_dieuchinh;
                        excelWorksheet.Cells[i + startRow, 6].Value = list.ElementAt(i).note;
                        stt += 1;
                    }
                    else
                    {
                        save_office = list.ElementAt(i).office_name;
                        using (ExcelRange Rng = excelWorksheet.Cells[i + startRow, 1, i + startRow, 6])
                        {
                            Rng.Merge = true;
                            Rng.Style.Font.Bold = true;
                            Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            Rng.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                            Rng.Value = save_office;
                            stt = 1;
                            excelWorksheet.Cells[i + startRow + 1, 1].Value = stt;
                            excelWorksheet.Cells[i + startRow + 1, 2].Value = list.ElementAt(i).expense_category_name;
                            excelWorksheet.Cells[i + startRow + 1, 3].Style.Numberformat.Format = "###,###,##0";
                            excelWorksheet.Cells[i + startRow + 1, 3].Value = list.ElementAt(i).price_dieuchinh;
                            excelWorksheet.Cells[i + startRow + 1, 4].Value = list.ElementAt(i).sl_dieuchinh;
                            excelWorksheet.Cells[i + startRow + 1, 5].Style.Numberformat.Format = "###,###,##0";
                            excelWorksheet.Cells[i + startRow + 1, 5].Value = list.ElementAt(i).total_dieuchinh;
                            excelWorksheet.Cells[i + startRow + 1, 6].Value = list.ElementAt(i).note;
                        }
                    }
                }
                string Flocation = "/Excel_template/Download/kinh-phi-thuc-te.xlsx";
                string savePath = HostingEnvironment.MapPath(Flocation);
                string handle = Guid.NewGuid().ToString();
                excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/Download/kinh-phi-thuc-te.xlsx")));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    excelPackage.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream;
                }
            }
        }
    }
}
