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
                                    left join SMIA_AcademicActivity.ActivityExpenseDetail b ON aec.expense_category_id = b.expense_category_id AND b.expense_type_id = 1 
                                    LEFT JOIN SMIA_AcademicActivity.ActivityExpenseDetail aed
                                    ON b.expense_category_id = aed.expense_category_id 
                                    AND aed.expense_type_id = 2 
                                    LEFT JOIN SMIA_AcademicActivity.ActivityExpenseDetail aee ON aee.expense_category_id = aec.expense_category_id
                                    AND aee.expense_type_id = 3 
                                    where ao.activity_id = @activity_id) a
                                    GROUP BY a.activity_office_id, a.office_name";
                List<infoExpense> data = db.Database.SqlQuery<infoExpense>(sql, new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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
                Console.WriteLine(e.ToString());
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
                Console.WriteLine(e.ToString());
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
                Console.WriteLine(e.ToString());
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
                        expense_price = (double)expense.expense_price,
                        expense_quantity = (int)expense.expense_quantity,
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
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool deleteExpenseType(int expense_category_id, int type)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (type == 1)
                    {
                        List<ActivityExpenseDetail> aed = db.ActivityExpenseDetails.Where(x => x.expense_category_id == expense_category_id).ToList();
                        ActivityExpenseCategory aec = db.ActivityExpenseCategories.Find(expense_category_id);
                        db.ActivityExpenseCategories.Remove(aec);
                        db.SaveChanges();
                        foreach (ActivityExpenseDetail item in aed)
                        {
                            if (item.file_id != null)
                            {
                                File f = db.Files.Find(item.file_id);
                                GoogleDriveService.DeleteFile(f.file_drive_id);
                                db.Files.Remove(f);
                            }
                        }
                    }
                    else
                    {
                        ActivityExpenseDetail aed = db.ActivityExpenseDetails.Where(x => x.expense_category_id == expense_category_id && x.expense_type_id == type).FirstOrDefault();
                        if (aed != null)
                        {
                            db.ActivityExpenseDetails.Remove(aed);
                            db.SaveChanges();
                            File f = db.Files.Find(aed.file_id);
                            if (f != null)
                            {
                                GoogleDriveService.DeleteFile(f.file_drive_id);
                                db.Files.Remove(f);
                            }
                        }
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public infoExpenseEstimate getExpenseType(int expense_category_id, int type)
        {
            try
            {
                string sql = @"select aec.expense_category_id,aec.expense_category_name,aed.expense_price,aed.expense_quantity,(aed.expense_price * aed.expense_quantity) as 'total',aed.note,f.link,f.[name]
                            from SMIA_AcademicActivity.ActivityExpenseCategory aec
                            left join SMIA_AcademicActivity.ActivityExpenseDetail aed 
							on aed.expense_category_id = aec.expense_category_id and aed.expense_type_id = @type
                            left join General.[File] f on f.[file_id] = aed.[file_id]
                            where aec.expense_category_id = @expense_category_id and (aed.expense_type_id = @type or aed.expense_type_id is null)";
                infoExpenseEstimate data = db.Database.SqlQuery<infoExpenseEstimate>(sql,
                    new SqlParameter("expense_category_id", expense_category_id),
                    new SqlParameter("type", type)).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new infoExpenseEstimate();
            }
        }
        public bool editExpenseDuTru(int activity_office_id, string activity_name, string data, HttpPostedFileBase img)
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
                    aed.expense_price = (double)expense.expense_price;
                    aed.expense_quantity = (int)expense.expense_quantity;
                    aed.note = expense.note;
                    if (img != null)
                    {
                        string sql = @"select YEAR(aa.activity_date_start) as 'year' from SMIA_AcademicActivity.AcademicActivity aa
                                    inner join SMIA_AcademicActivity.ActivityOffice ao on aa.activity_id = ao.activity_id
                                    where ao.activity_office_id = @activity_office_id";
                        int year = db.Database.SqlQuery<int>(sql, new SqlParameter("activity_office_id", activity_office_id)).FirstOrDefault();
                        if (aed.file_id != null)
                        {
                            Google.Apis.Drive.v3.Data.File fr = GoogleDriveService.UpdateFile(img.FileName, img.InputStream, img.ContentType, aed.File.file_drive_id);
                            File f = db.Files.Find(aed.file_id);
                            f.name = img.FileName;
                            db.Entry(f).State = EntityState.Modified;
                        }
                        else
                        {
                            Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadIAFile(img, "Chi phí dự trù - " + expense.expense_category_name + " (" + activity_name + " - " + year + ")", 5, false);
                            File file = new File();
                            file.name = img.FileName;
                            file.link = f.WebViewLink;
                            file.file_drive_id = f.Id;
                            File ff = db.Files.Add(file);
                            db.SaveChanges();
                            aed.file_id = ff.file_id;
                        }
                    }
                    db.Entry(aed).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public List<infoExpenseModified> getDatatableKPDieuChinh(int activity_office_id)
        {
            try
            {
                string sql = @"select expense_category_id, expense_category_name, sum(price_bandau) 'price_bandau', sum(sl_bandau) 'sl_bandau',sum(price_bandau) *sum(sl_bandau) as 'total_bandau',
                                    sum(price_dieuchinh) 'price_dieuchinh', sum(sl_dieuchinh) 'sl_dieuchinh',sum(price_dieuchinh) * sum(sl_dieuchinh) as 'total_dieuchinh', STRING_AGG(note,'') as 'note'
                                    from (SELECT aec.expense_category_id, aec.expense_category_name, case when expense_type_id = 1 then expense_price else 0 end as 'price_bandau',
                                    CASE WHEN expense_type_id = 1 then expense_quantity else 0 end as 'sl_bandau', 
                                    CASE WHEN expense_type_id = 2 then expense_price else 0 end as 'price_dieuchinh', 
                                    CASE WHEN expense_type_id = 2 then expense_quantity else 0 end as 'sl_dieuchinh', 
                                    CASE WHEN expense_type_id = 2 then note else '' end as 'note'
                                    FROM SMIA_AcademicActivity.ActivityExpenseCategory aec INNER JOIN SMIA_AcademicActivity.ActivityExpenseDetail aed
                                    ON aec.expense_category_id = aed.expense_category_id 
                                    WHERE aec.activity_office_id = @activity_office_id and (aed.expense_type_id = 1 or aed.expense_type_id = 2)) as a
                                    group by expense_category_id, expense_category_name";
                List<infoExpenseModified> data = db.Database.SqlQuery<infoExpenseModified>(sql, new SqlParameter("activity_office_id", activity_office_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<infoExpenseModified>();
            }
        }
        public bool editExpenseDieuChinh(int activity_office_id, string activity_name, string data, HttpPostedFileBase img)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    infoExpenseEstimate expense = JsonConvert.DeserializeObject<infoExpenseEstimate>(data);
                    ActivityExpenseCategory aec = db.ActivityExpenseCategories.Find(expense.expense_category_id);
                    aec.expense_category_name = expense.expense_category_name;
                    db.Entry(aec).State = EntityState.Modified;
                    ActivityExpenseDetail aed = db.ActivityExpenseDetails.Where(x => x.expense_category_id == expense.expense_category_id && x.expense_type_id == 2).FirstOrDefault();
                    if (aed != null)
                    {
                        aed.expense_price = (double)expense.expense_price;
                        aed.expense_quantity = (int)expense.expense_quantity;
                        aed.note = expense.note;
                        db.Entry(aed).State = EntityState.Modified;
                    }
                    else
                    {
                        aed = db.ActivityExpenseDetails.Add(new ActivityExpenseDetail
                        {
                            expense_price = (double)expense.expense_price,
                            expense_quantity = (int)expense.expense_quantity,
                            note = expense.note,
                            expense_type_id = 2,
                            expense_category_id = expense.expense_category_id
                        });
                    }
                    db.SaveChanges();
                    if (img != null)
                    {
                        string sql = @"select YEAR(aa.activity_date_start) as 'year' from SMIA_AcademicActivity.AcademicActivity aa
                                    inner join SMIA_AcademicActivity.ActivityOffice ao on aa.activity_id = ao.activity_id
                                    where ao.activity_office_id = @activity_office_id";
                        int year = db.Database.SqlQuery<int>(sql, new SqlParameter("activity_office_id", activity_office_id)).FirstOrDefault();
                        if (aed.file_id != null)
                        {
                            Google.Apis.Drive.v3.Data.File fr = GoogleDriveService.UpdateFile(img.FileName, img.InputStream, img.ContentType, aed.File.file_drive_id);
                            File f = db.Files.Find(aed.file_id);
                            f.name = img.FileName;
                            db.Entry(f).State = EntityState.Modified;
                        }
                        else
                        {
                            Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadIAFile(img, "Chi phí điều chỉnh - " + expense.expense_category_name + " (" + activity_name + " - " + year + ")", 5, false);
                            File file = new File
                            {
                                name = img.FileName,
                                link = f.WebViewLink,
                                file_drive_id = f.Id
                            };
                            File ff = db.Files.Add(file);
                            db.SaveChanges();
                            aed.file_id = ff.file_id;
                        }
                    }
                    db.Entry(aed).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public List<infoExpenseModified> getDatatableKPThucTe(int activity_office_id)
        {
            try
            {
                string sql = @"select aec.expense_category_id, aec.expense_category_name, 
                                    case when aed.expense_price is null then 0 else aed.expense_price end 'price_bandau', 
                                    case when aed.expense_quantity is null then 0 else aed.expense_quantity end 'sl_bandau', 
                                    case when aed.expense_price * aed.expense_quantity is null then 0 else aed.expense_price * aed.expense_quantity end 'total_bandau',
                                    case when aee.expense_price is null then 0 else aee.expense_price end 'price_dieuchinh',
                                    case when aee.expense_quantity is null then 0 else aee.expense_quantity end 'sl_dieuchinh',
                                    case when aee.expense_price * aee.expense_quantity is null then 0 else aee.expense_price * aee.expense_quantity end 'total_dieuchinh',
                                    aee.note from 
                                    SMIA_AcademicActivity.ActivityExpenseCategory aec left join
                                    (select expense_category_id, max(expense_type_id) 'expense_type_id'
                                    from SMIA_AcademicActivity.ActivityExpenseDetail
                                    where expense_type_id < 3
                                    group by expense_category_id) a on aec.expense_category_id = a.expense_category_id 
                                    left join SMIA_AcademicActivity.ActivityExpenseDetail aed
                                    on a.expense_category_id = aed.expense_category_id and a.expense_type_id = aed.expense_type_id 
                                    left join SMIA_AcademicActivity.ActivityExpenseDetail aee on aee.expense_category_id = aed.expense_category_id
                                    and aee.expense_type_id = 3
                                    where aec.activity_office_id = @activity_office_id";
                List<infoExpenseModified> data = db.Database.SqlQuery<infoExpenseModified>(sql, new SqlParameter("activity_office_id", activity_office_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<infoExpenseModified>();
            }
        }
        public bool editExpenseThucTe(int activity_office_id, string activity_name, string data, HttpPostedFileBase img)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    infoExpenseEstimate expense = JsonConvert.DeserializeObject<infoExpenseEstimate>(data);
                    ActivityExpenseCategory aec = db.ActivityExpenseCategories.Find(expense.expense_category_id);
                    aec.expense_category_name = expense.expense_category_name;
                    db.Entry(aec).State = EntityState.Modified;
                    ActivityExpenseDetail aed = db.ActivityExpenseDetails.Where(x => x.expense_category_id == expense.expense_category_id && x.expense_type_id == 3).FirstOrDefault();
                    if (aed != null)
                    {
                        aed.expense_price = (double)expense.expense_price;
                        aed.expense_quantity = (int)expense.expense_quantity;
                        aed.note = expense.note;
                        db.Entry(aed).State = EntityState.Modified;
                    }
                    else
                    {
                        aed = db.ActivityExpenseDetails.Add(new ActivityExpenseDetail
                        {
                            expense_price = (double)expense.expense_price,
                            expense_quantity = (int)expense.expense_quantity,
                            note = expense.note,
                            expense_type_id = 3,
                            expense_category_id = expense.expense_category_id
                        });
                    }
                    db.SaveChanges();
                    if (img != null)
                    {
                        string sql = @"select YEAR(aa.activity_date_start) as 'year' from SMIA_AcademicActivity.AcademicActivity aa
                                    inner join SMIA_AcademicActivity.ActivityOffice ao on aa.activity_id = ao.activity_id
                                    where ao.activity_office_id = @activity_office_id";
                        int year = db.Database.SqlQuery<int>(sql, new SqlParameter("activity_office_id", activity_office_id)).FirstOrDefault();
                        if (aed.file_id != null)
                        {
                            Google.Apis.Drive.v3.Data.File fr = GoogleDriveService.UpdateFile(img.FileName, img.InputStream, img.ContentType, aed.File.file_drive_id);
                            File f = db.Files.Find(aed.file_id);
                            f.name = img.FileName;
                            db.Entry(f).State = EntityState.Modified;
                        }
                        else
                        {
                            Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadIAFile(img, "Chi phí thực tế - " + expense.expense_category_name + " (" + activity_name + " - " + year + ")", 5, false);
                            File file = new File();
                            file.name = img.FileName;
                            file.link = f.WebViewLink;
                            file.file_drive_id = f.Id;
                            File ff = db.Files.Add(file);
                            db.SaveChanges();
                            aed.file_id = ff.file_id;
                        }
                    }
                    db.Entry(aed).State = EntityState.Modified;
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public Statistic getTotal(int activity_id)
        {
            try
            {
                string sql = @"select '' as 'title', a.dang_ky, b.tham_du 
                                    from (select ap.activity_id, count(p.participant_id) 'dang_ky'
                                    from SMIA_AcademicActivity.Participant p inner join SMIA_AcademicActivity.ParticipantRole pr on 
                                    p.participant_role_id = pr.participant_role_id inner join SMIA_AcademicActivity.AcademicActivityPhase ap
                                    on ap.phase_id = pr.phase_id
                                    where ap.activity_id = @activity_id
                                    group by ap.activity_id) a inner join 
                                    (select ap.activity_id, count(p.participant_id) 'tham_du'
                                    from SMIA_AcademicActivity.Participant p inner join SMIA_AcademicActivity.ParticipantRole pr on 
                                    p.participant_role_id = pr.participant_role_id inner join SMIA_AcademicActivity.AcademicActivityPhase ap
                                    on ap.phase_id = pr.phase_id
                                    where ap.activity_id = @activity_id and p.is_checked = 1
                                    group by ap.activity_id) b on a.activity_id = b.activity_id";
                Statistic data = db.Database.SqlQuery<Statistic>(sql, new SqlParameter("activity_id", activity_id)).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new Statistic();
            }
        }
        public List<Statistic> getStatisticOffice(int activity_id)
        {
            try
            {
                string sql = @"select a.office_name as 'title', a.dang_ky, b.tham_du 
                                    from (select ap.activity_id, o.office_id, o.office_name, count(p.participant_id) 'dang_ky'
                                    from SMIA_AcademicActivity.Participant p inner join SMIA_AcademicActivity.ParticipantRole pr on 
                                    p.participant_role_id = pr.participant_role_id inner join SMIA_AcademicActivity.AcademicActivityPhase ap
                                    on ap.phase_id = pr.phase_id inner join General.Office o ON o.office_id = p.office_id
                                    where ap.activity_id = @activity_id
                                    group by ap.activity_id, o.office_id, o.office_name) a inner join 
                                    (select ap.activity_id, o.office_id, o.office_name, count(p.participant_id) 'tham_du'
                                    from SMIA_AcademicActivity.Participant p inner join SMIA_AcademicActivity.ParticipantRole pr on 
                                    p.participant_role_id = pr.participant_role_id inner join SMIA_AcademicActivity.AcademicActivityPhase ap
                                    on ap.phase_id = pr.phase_id inner join General.Office o ON o.office_id = p.office_id
                                    where ap.activity_id = @activity_id and p.is_checked = 1
                                    group by ap.activity_id, o.office_id, o.office_name) b on a.activity_id = b.activity_id and a.office_id = b.office_id";
                List<Statistic> data = db.Database.SqlQuery<Statistic>(sql, new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<Statistic>();
            }
        }
        public List<Statistic> getStatisticUnit(int activity_id)
        {
            try
            {
                string sql = @"select a.unit_name as 'title', a.dang_ky, b.tham_du 
                                    from (select ap.activity_id, u.unit_id, u.unit_name, count(p.participant_id) 'dang_ky'
                                    from SMIA_AcademicActivity.Participant p inner join SMIA_AcademicActivity.ParticipantRole pr on 
                                    p.participant_role_id = pr.participant_role_id inner join SMIA_AcademicActivity.AcademicActivityPhase ap
                                    on ap.phase_id = pr.phase_id inner join General.Office o ON o.office_id = p.office_id
                                    inner join General.InternalUnit u ON o.unit_id = u.unit_id
                                    where ap.activity_id = @activity_id
                                    group by ap.activity_id, u.unit_id, u.unit_name) a inner join 
                                    (select ap.activity_id, u.unit_id, u.unit_name, count(p.participant_id) 'tham_du'
                                    from SMIA_AcademicActivity.Participant p inner join SMIA_AcademicActivity.ParticipantRole pr on 
                                    p.participant_role_id = pr.participant_role_id inner join SMIA_AcademicActivity.AcademicActivityPhase ap
                                    on ap.phase_id = pr.phase_id inner join General.Office o ON o.office_id = p.office_id
                                    inner join General.InternalUnit u ON o.unit_id = u.unit_id
                                    where ap.activity_id = @activity_id and p.is_checked = 1
                                    group by ap.activity_id, u.unit_id, u.unit_name) b on a.activity_id = b.activity_id and a.unit_id =b.unit_id";
                List<Statistic> data = db.Database.SqlQuery<Statistic>(sql, new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<Statistic>();
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
            public int? expense_category_id { get; set; }
            public string expense_category_name { get; set; }
            public double? expense_price { get; set; }
            public int? expense_quantity { get; set; }
            public double? total { get; set; }
            public string note { get; set; }
            public string link { get; set; }
            public string name { get; set; }
        }
        public class infoExpenseModified
        {
            public int expense_category_id { get; set; }
            public string expense_category_name { get; set; }
            public double? price_bandau { get; set; }
            public int? sl_bandau { get; set; }
            public double? total_bandau { get; set; }
            public double? price_dieuchinh { get; set; }
            public int? sl_dieuchinh { get; set; }
            public double? total_dieuchinh { get; set; }
            public string note { get; set; }
        }
        public class Statistic
        {
            public string title { get; set; }
            public int dang_ky { get; set; }
            public int tham_du { get; set; }
        }
    }
}