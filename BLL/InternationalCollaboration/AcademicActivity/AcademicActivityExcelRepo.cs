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
        public MemoryStream ExportDuTruExcel()
        {
            string path = HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/");
            string filename = "MOU.xlsx";
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
        public MemoryStream ExportDieuChinhExcel()
        {
            string path = HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/");
            string filename = "MOU.xlsx";
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
        public MemoryStream ExportThucTeExcel()
        {
            string path = HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/");
            string filename = "MOU.xlsx";
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
        public MemoryStream ExportTongHopExcel()
        {
            string path = HostingEnvironment.MapPath("/Content/assets/excel/Collaboration/");
            string filename = "MOU.xlsx";
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
    }
}
