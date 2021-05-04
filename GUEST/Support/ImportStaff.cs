using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace GUEST.Support
{
    public class ImportStaff
    {
        public static HashSet<string> LoadMail()
        {
            HashSet<string> emails = new HashSet<string>();

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string path = HostingEnvironment.MapPath("/Excel/");
                string filename = "StaffInformation.xlsx";
                FileInfo file = new FileInfo(path + filename);

                using (ExcelPackage excelPackage = new ExcelPackage(file))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                    ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();

                    int i = 2;
                    while (true)
                    {
                        if (excelWorksheet.Cells[i, 10].Value == null)
                            break;
                        emails.Add(excelWorksheet.Cells[i, 10].Value.ToString().ToLower().Trim());
                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return emails;
        }
    }
}