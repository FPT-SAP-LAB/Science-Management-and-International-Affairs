using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                string sql = @"SELECT aec.expense_category_id, aec.expense_category_name, aed.expense_price, aed.expense_quantity,(aed.expense_price * aed.expense_quantity) as 'total', aed.note
                                    FROM SMIA_AcademicActivity.ActivityExpenseCategory aec INNER JOIN SMIA_AcademicActivity.ActivityExpenseDetail aed
                                    ON aec.expense_category_id = aed.expense_category_id 
                                    WHERE aec.activity_office_id = @activity_office_id and aed.expense_type_id = 1
                                    group by aec.expense_category_id, aec.expense_category_name, aed.expense_price, aed.expense_quantity, aed.note";
                List<infoExpenseEstimate> data = db.Database.SqlQuery<infoExpenseEstimate>(sql, new SqlParameter("activity_office_id", activity_office_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<infoExpenseEstimate>();
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
        }
    }
}