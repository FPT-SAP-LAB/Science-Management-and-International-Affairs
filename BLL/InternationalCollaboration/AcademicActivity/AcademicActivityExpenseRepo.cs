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
                string sql = @"SELECT a.expense_category_id,a.office_id, a.office_name, SUM(a.cp_du_tru) 'cp_du_tru', SUM(a.cp_dieu_chinh) 'cp_dieu_chinh', SUM(a.cp_thuc_te) 'cp_thuc_te',
                                CASE WHEN  SUM(a.cp_dieu_chinh) IS NULL THEN SUM(a.cp_du_tru) - SUM(a.cp_thuc_te) ELSE SUM(a.cp_dieu_chinh) - SUM(a.cp_thuc_te) END AS 'chenh_lech' 
                                FROM 
                                (SELECT aec.office_id,o.office_name, aec.expense_category_id, aec.expense_category_name, b.expense_price* b.expense_quantity 'cp_du_tru',
                                aed.expense_price*aed.expense_quantity 'cp_dieu_chinh', aee.expense_price*aee.expense_quantity 'cp_thuc_te',  
                                aee.note from SMIA_AcademicActivity.ActivityExpenseDetail  b LEFT JOIN SMIA_AcademicActivity.ActivityExpenseDetail aed
                                ON b.expense_category_id = aed.expense_category_id 
                                AND b.expense_type_id = 1 AND aed.expense_type_id = 2 LEFT JOIN 
                                SMIA_AcademicActivity.ActivityExpenseCategory aec ON aec.expense_category_id = aed.expense_category_id
                                LEFT JOIN SMIA_AcademicActivity.ActivityExpenseDetail aee ON aee.expense_category_id = aed.expense_category_id
                                AND aee.expense_type_id = 3
                                LEFT JOIN General.Office o ON o.office_id = aec.office_id
                                where aec.activity_id = @activity_id) a
                                GROUP BY a.expense_category_id,a.office_id, a.office_name";
                List<infoExpense> data = db.Database.SqlQuery<infoExpense>(sql, new SqlParameter("activity_id", activity_id)).ToList();
                return data;
            }catch(Exception e)
            {
                return new List<infoExpense>();
            }
        }
        public bool addExpense(baseExpense data)
        {
            try
            {
                db.ActivityExpenseCategories.Add(new ActivityExpenseCategory
                {
                    expense_category_name = data.expense_category_name,
                    activity_id = data.activity_id,
                    office_id = data.office_id
                });
                db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
        public bool deleteExpense(int expense_category_id)
        {
            try
            {
                ActivityExpenseCategory aec = db.ActivityExpenseCategories.Find(expense_category_id);
                db.ActivityExpenseCategories.Remove(aec);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        //public List<infoExpenseEstimate> getDatatableExpenseEstimate(int expense_category_id)
        //{
        //    try
        //    {
        //        string sql = @"";
        //    }catch(Exception e)
        //    {

        //    }
        //}
        public class infoExpense
        {
            public int expense_category_id { get; set; }
            public int office_id { get; set; }
            public string office_name { get; set; }
            public double cp_du_tru { get; set; }
            public double cp_dieu_chinh { get; set; }
            public double cp_thuc_te { get; set; }
            public double chenh_lech { get; set; }
        }
        public class baseExpense
        {
            public string expense_category_name { get; set; }
            public int activity_id { get; set; }
            public int office_id { get; set; }
        }
        public class infoExpenseEstimate
        {
            public int expense_category_id { get; set; }
            public string expense_category_name { get; set; }
            public double expense_price { get; set; }
            public double expense_quantity { get; set; }
            public string note { get; set; }
            
        }
    }
}
