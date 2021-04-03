using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ENTITIES.CustomModels;
using Newtonsoft.Json;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityExpenseRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<infoExpense> getDatatableKP(int activity_id)
        {
            try
            {
                string sql = @"SELECT a.activity_office_id, a.office_name, SUM(a.cp_du_tru) 'cp_du_tru', SUM(a.cp_dieu_chinh) 'cp_dieu_chinh', SUM(a.cp_thuc_te) 'cp_thuc_te',
                                    CASE WHEN  SUM(a.cp_dieu_chinh) IS NULL THEN SUM(a.cp_du_tru) - SUM(a.cp_thuc_te) ELSE SUM(a.cp_dieu_chinh) - SUM(a.cp_thuc_te) END AS 'chenh_lech'
                                    FROM 
                                    (SELECT ao.activity_office_id, o.office_name, aec.expense_category_id, aec.expense_category_name, b.expense_price* b.expense_quantity 'cp_du_tru',
                                    aed.expense_price*aed.expense_quantity 'cp_dieu_chinh', aee.expense_price*aee.expense_quantity 'cp_thuc_te',  
                                    aee.note 
                                    from SMIA_AcademicActivity.ActivityOffice ao LEFT JOIN General.Office o ON o.office_id = ao.office_id
                                    LEFT JOIN SMIA_AcademicActivity.ActivityExpenseCategory aec ON ao.activity_office_id 
                                    = aec.activity_office_id
                                    left join SMIA_AcademicActivity.ActivityExpenseDetail b ON aec.expense_category_id = b.expense_category_id
                                    LEFT JOIN SMIA_AcademicActivity.ActivityExpenseDetail aed
                                    ON b.expense_category_id = aed.expense_category_id 
                                    AND b.expense_type_id = 1 AND aed.expense_type_id = 2 
                                    LEFT JOIN SMIA_AcademicActivity.ActivityExpenseDetail aee ON aee.expense_category_id = aed.expense_category_id
                                    AND aee.expense_type_id = 3 
                                    where ao.activity_id = @activity_id) a
                                    GROUP BY a.activity_office_id, a.office_name";
                List<infoExpense> data = db.Database.SqlQuery<infoExpense>(sql, new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<infoExpense>();
            }
        }
        public string addExpense(baseExpense data)
        {
            try
            {
                List<ActivityOffice> ao = db.ActivityOffices.Where(x => x.activity_id == data.activity_id).ToList();
                List<int?> list_id = ao.Select(x => x.office_id).ToList();
                if (list_id.Contains(data.office_id))
                    return "Không thể thêm đơn vị đã tồn tại.";
                db.ActivityOffices.Add(new ActivityOffice
                {
                    activity_id = data.activity_id,
                    office_id = data.office_id
                });
                db.SaveChanges();
                return "Thêm mục kinh phí thành công.";
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }
        public bool deleteExpense(int activity_office_id)
        {
            try
            {
                ActivityOffice ao = db.ActivityOffices.Find(activity_office_id);
                db.ActivityOffices.Remove(ao);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public List<infoExpenseEstimate> getDatatableKPDuTru(int activity_office_id)
        {
            try
            {
                string sql = @"SELECT aec.expense_category_id, aec.expense_category_name, aed.expense_price, aed.expense_quantity,(aed.expense_price * aed.expense_quantity) as 'total', aed.note,f.link,f.[name]
                                    FROM SMIA_AcademicActivity.ActivityExpenseCategory aec INNER JOIN SMIA_AcademicActivity.ActivityExpenseDetail aed
                                    ON aec.expense_category_id = aed.expense_category_id 
                                    left join General.[File] f on f.[file_id] = aed.[file_id]
                                    WHERE aec.activity_office_id = @activity_office_id and aed.expense_type_id = 1
                                    group by aec.expense_category_id, aec.expense_category_name, aed.expense_price, aed.expense_quantity, aed.note,f.link,f.[name]";
                List<infoExpenseEstimate> data = db.Database.SqlQuery<infoExpenseEstimate>(sql, new SqlParameter("activity_office_id", activity_office_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<infoExpenseEstimate>();
            }
        }
        public bool addExpenseDuTru(int activity_office_id, string activity_name, string data, HttpPostedFileBase img)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    infoExpenseEstimate expense = JsonConvert.DeserializeObject<infoExpenseEstimate>(data);
                    ActivityExpenseCategory aec = db.ActivityExpenseCategories.Add(new ActivityExpenseCategory
                    {
                        activity_office_id = activity_office_id,
                        expense_category_name = expense.expense_category_name
                    });
                    db.SaveChanges();
                    ActivityExpenseDetail aed = db.ActivityExpenseDetails.Add(new ActivityExpenseDetail
                    {
                        expense_type_id = 1,
                        note = expense.note,
                        expense_price = expense.expense_price,
                        expense_quantity = expense.expense_quantity,
                        expense_category_id = aec.expense_category_id
                    });
                    db.SaveChanges();
                    string sql = @"select YEAR(aa.activity_date_start) as 'year' from SMIA_AcademicActivity.AcademicActivity aa
                                    inner join SMIA_AcademicActivity.ActivityOffice ao on aa.activity_id = ao.activity_id
                                    where ao.activity_office_id = @activity_office_id";
                    int year = db.Database.SqlQuery<int>(sql, new SqlParameter("activity_office_id", activity_office_id)).FirstOrDefault();
                    if (img != null)
                    {
                        Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadIAFile(img, "Chi phí dự trù - " + expense.expense_category_name + " (" + activity_name + " - " + year + ")", 5, false);
                        File file = new File();
                        file.name = img.FileName;
                        file.link = f.WebViewLink;
                        file.file_drive_id = f.Id;
                        File ff = db.Files.Add(file);
                        db.SaveChanges();
                        aed.file_id = ff.file_id;
                        db.Entry(aed).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool deleteExpenseDuTru(int expense_category_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    ActivityExpenseDetail aed = db.ActivityExpenseDetails.Where(x => x.expense_category_id == expense_category_id).FirstOrDefault();
                    File f = db.Files.Find(aed.file_id);
                    if (f != null)
                    {
                        GoogleDriveService.DeleteFile(f.file_drive_id);
                    }
                    ActivityExpenseCategory aec = db.ActivityExpenseCategories.Find(expense_category_id);
                    db.ActivityExpenseCategories.Remove(aec);
                    db.SaveChanges();
                    db.Files.Remove(f);
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public infoExpenseEstimate getExpenseDuTru(int expense_category_id)
        {
            try
            {
                string sql = @"select aed.expense_category_id,aec.expense_category_name,aed.expense_price,aed.expense_quantity,(aed.expense_price * aed.expense_quantity) as 'total',aed.note,f.link,f.[name]
                            from SMIA_AcademicActivity.ActivityExpenseCategory aec
                            inner join SMIA_AcademicActivity.ActivityExpenseDetail aed on aed.expense_category_id = aec.expense_category_id
                            left join General.[File] f on f.[file_id] = aed.[file_id]
                            where aed.expense_category_id = @expense_category_id";
                infoExpenseEstimate data = db.Database.SqlQuery<infoExpenseEstimate>(sql, new SqlParameter("expense_category_id", expense_category_id)).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                return new infoExpenseEstimate();
            }
        }
        public bool editExpenseDuTru(string data, HttpPostedFileBase img)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    infoExpenseEstimate expense = JsonConvert.DeserializeObject<infoExpenseEstimate>(data);
                    ActivityExpenseCategory aec = db.ActivityExpenseCategories.Find(expense.expense_category_id);
                    aec.expense_category_name = expense.expense_category_name;
                    db.Entry(aec).State = EntityState.Modified;
                    ActivityExpenseDetail aed = db.ActivityExpenseDetails.Where(x => x.expense_category_id == aec.expense_category_id).FirstOrDefault();
                    aed.expense_price = expense.expense_price;
                    aed.expense_quantity = expense.expense_quantity;
                    aed.note = expense.note;
                    if (img != null)
                    {
                        Google.Apis.Drive.v3.Data.File fr = GoogleDriveService.UpdateFile(img.FileName, img.InputStream, img.ContentType, aed.File.file_drive_id);
                        File f = db.Files.Find(aed.file_id);
                        f.name = img.FileName;
                        db.Entry(f).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public class infoExpense
        {
            public int activity_office_id { get; set; }
            public string office_name { get; set; }
            public double? cp_du_tru { get; set; }
            public double? cp_dieu_chinh { get; set; }
            public double? cp_thuc_te { get; set; }
            public double? chenh_lech { get; set; }
        }
        public class baseExpense
        {
            public int activity_id { get; set; }
            public int office_id { get; set; }
        }
        public class infoExpenseEstimate
        {
            public int expense_category_id { get; set; }
            public string expense_category_name { get; set; }
            public double expense_price { get; set; }
            public int expense_quantity { get; set; }
            public double total { get; set; }
            public string note { get; set; }
            public string link { get; set; }
            public string name { get; set; }
        }
    }
}