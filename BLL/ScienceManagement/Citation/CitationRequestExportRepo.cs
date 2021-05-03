using ENTITIES;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace BLL.ScienceManagement.Citation
{
    public class CitationRequestExportRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public byte[] ExportExcel()
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                string fileName = HostingEnvironment.MapPath("/Excel_template/Citation.xlsx");
                byte[] byteArray = System.IO.File.ReadAllBytes(fileName);
                using (var stream = new MemoryStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    using (ExcelPackage excelPackage = new ExcelPackage(stream))
                    {
                        ExcelWorkbook excelWorkbook = excelPackage.Workbook;

                        var list = (from a in db.RequestCitations
                                    join b in db.BaseRequests on a.request_id equals b.request_id
                                    join c in db.Profiles on b.account_id equals c.account_id
                                    join d in db.People on c.people_id equals d.people_id
                                    join e in db.Offices on d.office_id equals e.office_id
                                    where a.citation_status_id == 4 && a.total_reward != null
                                    select new
                                    {
                                        Ms = c.mssv_msnv,
                                        Name = d.name,
                                        Office = e.office_name,
                                        Scholars = a.Citations.Where(x => x.citation_type_id == 1).Select(x => x.count).ToList(),
                                        Scopus = a.Citations.Where(x => x.citation_type_id == 2).Select(x => x.count).ToList(),
                                        Reward = a.total_reward.Value
                                    }).ToList();

                        ExcelWorksheet excelWorksheet1 = excelWorkbook.Worksheets[0];
                        int i = 2;
                        int count = 1;
                        foreach (var item in list)
                        {
                            excelWorksheet1.Cells[i, 1].Value = count;
                            excelWorksheet1.Cells[i, 2].Value = item.Name;
                            excelWorksheet1.Cells[i, 3].Value = item.Ms;
                            excelWorksheet1.Cells[i, 4].Value = item.Office;
                            excelWorksheet1.Cells[i, 5].Value = item.Scopus;
                            excelWorksheet1.Cells[i, 6].Value = item.Scholars;
                            count++;
                            i++;
                        }

                        ExcelWorksheet excelWorksheet2 = excelWorkbook.Worksheets[1];
                        i = 2;
                        count = 1;
                        foreach (var item in list)
                        {
                            excelWorksheet2.Cells[i, 1].Value = count;
                            excelWorksheet2.Cells[i, 2].Value = item.Name;
                            excelWorksheet2.Cells[i, 3].Value = item.Ms;
                            excelWorksheet2.Cells[i, 4].Value = item.Office;
                            excelWorksheet2.Cells[i, 5].Value = item.Reward;
                            count++;
                            i++;
                        }

                        excelPackage.Save();
                        return excelPackage.GetAsByteArray();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
