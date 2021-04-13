using ENTITIES;
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
            List<ListMOU> listMOU = new List<ListMOU>();

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
                    excelWorksheet.Cells[i + startRow, 10].Value = listMOU.ElementAt(i).mou_start_date.ToString("dd'/'MM'/'yyyy");
                    excelWorksheet.Cells[i + startRow, 11].Value = listMOU.ElementAt(i).mou_end_date.ToString("dd'/'MM'/'yyyy");
                    excelWorksheet.Cells[i + startRow, 14].Value = listMOU.ElementAt(i).mou_status_id == 1 ? "Active" : "Inactive";
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
            List<ListMOU> listMOU = new List<ListMOU>();

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
                    excelWorksheet.Cells[i + startRow, 10].Value = listMOU.ElementAt(i).mou_start_date.ToString("dd'/'MM'/'yyyy");
                    excelWorksheet.Cells[i + startRow, 11].Value = listMOU.ElementAt(i).mou_end_date.ToString("dd'/'MM'/'yyyy");
                    excelWorksheet.Cells[i + startRow, 14].Value = listMOU.ElementAt(i).mou_status_id == 1 ? "Active" : "Inactive";
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
        public MemoryStream ExportThucTeExcel(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "TemplateKinhPhiThucTe.xlsx";
            FileInfo file = new FileInfo(path + filename);
            List<ListMOU> listMOU = new List<ListMOU>();

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
                    excelWorksheet.Cells[i + startRow, 10].Value = listMOU.ElementAt(i).mou_start_date.ToString("dd'/'MM'/'yyyy");
                    excelWorksheet.Cells[i + startRow, 11].Value = listMOU.ElementAt(i).mou_end_date.ToString("dd'/'MM'/'yyyy");
                    excelWorksheet.Cells[i + startRow, 14].Value = listMOU.ElementAt(i).mou_status_id == 1 ? "Active" : "Inactive";
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
        public MemoryStream ExportTongHopExcel(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "TemplateKinhPhiTongHop.xlsx";
            FileInfo file = new FileInfo(path + filename);
            List<ListMOU> listMOU = new List<ListMOU>();

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
                    excelWorksheet.Cells[i + startRow, 10].Value = listMOU.ElementAt(i).mou_start_date.ToString("dd'/'MM'/'yyyy");
                    excelWorksheet.Cells[i + startRow, 11].Value = listMOU.ElementAt(i).mou_end_date.ToString("dd'/'MM'/'yyyy");
                    excelWorksheet.Cells[i + startRow, 14].Value = listMOU.ElementAt(i).mou_status_id == 1 ? "Active" : "Inactive";
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
    }
}
