using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;

namespace BLL.ModelDAL
{
    public class QsUniversityRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public BaseServerSideData<QsUniversity> List(BaseDatatable baseDatatable, string university)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var data = db.QsUniversities.Where(x => x.university.Contains(university)).OrderBy("row_id asc")
                .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();

            int recordsTotal = db.QsUniversities.Count();

            return new BaseServerSideData<QsUniversity>(data, recordsTotal);
        }
        public AlertModal<string> Add(HttpPostedFileBase ListUniversity)
        {
            db = new ScienceAndInternationalAffairsEntities();
            using (ExcelPackage excelPackage = new ExcelPackage(ListUniversity.InputStream))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                ExcelWorkbook excelWorkbook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkbook.Worksheets.First();

                using (DbContextTransaction trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<QsUniversity> qsUniversities = new List<QsUniversity>();
                        int i = 2;
                        while (true)
                        {
                            if (excelWorksheet.Cells[i, 1].Value == null)
                                break;
                            qsUniversities.Add(new QsUniversity
                            {
                                ranking = excelWorksheet.Cells[i, 1].Value.ToString(),
                                university = excelWorksheet.Cells[i, 2].Value.ToString()
                            });
                            i++;
                        }
                        db.Database.ExecuteSqlCommand("DELETE FROM SM_Conference.QsUniversity");
                        db.QsUniversities.AddRange(qsUniversities);
                        db.SaveChanges();
                        trans.Commit();
                        return new AlertModal<string>(true);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        trans.Rollback();
                    }
                }
            }
            return new AlertModal<string>(false);
        }
        public string GetRanking(string university)
        {
            db = new ScienceAndInternationalAffairsEntities();
            return db.QsUniversities.Where(x => x.university == university).Select(x => x.ranking).FirstOrDefault();
        }
    }
}
