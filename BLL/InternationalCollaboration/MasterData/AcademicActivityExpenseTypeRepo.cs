using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    public class AcademicActivityExpenseTypeRepo
    {
        

        public BaseServerSideData<ActivityExpenseType> getListActivityExpenseType(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<ActivityExpenseType> activityExpenseTypes = db.Database.SqlQuery<ActivityExpenseType>("select * from SMIA_AcademicActivity.ActivityExpenseType " +
                                                                            "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from SMIA_AcademicActivity.ActivityExpenseType").FirstOrDefault();
                    return new BaseServerSideData<ActivityExpenseType>(activityExpenseTypes, recordsTotal);
                }
            } catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<ActivityExpenseType> addAcademicActivityExpenseType(string expense_type_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (expense_type_name == "")
                    {
                        return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Tên loại kinh phí không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        ActivityExpenseType academicActivityExpenseType = db.ActivityExpenseTypes.Where(x => x.expense_type_name.Equals(expense_type_name)).FirstOrDefault();
                        if (academicActivityExpenseType == null)
                        {
                            //add
                            academicActivityExpenseType = new ActivityExpenseType
                            {
                                expense_type_name = expense_type_name
                            };
                            db.ActivityExpenseTypes.Add(academicActivityExpenseType);
                            db.SaveChanges();
                            return new AlertModal<ActivityExpenseType>(null, true, "Thành công", "Thêm loại kinh phí thành công.");
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Tên loại hoạt động không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<ActivityExpenseType> getActivityExpenseType(int expense_type_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    ActivityExpenseType activityExpenseType = db.ActivityExpenseTypes.Where(x => x.expense_type_id == expense_type_id).FirstOrDefault();
                    if (activityExpenseType != null)
                    {
                        return new AlertModal<ActivityExpenseType>(activityExpenseType, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Không xác định được loại kinh phí tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<ActivityExpenseType> editActivityExpenseType(int expense_type_id, string expense_type_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (expense_type_name == "")
                    {
                        return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Tên loại kinh phí không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        ActivityExpenseType activityExpenseType = db.ActivityExpenseTypes.Where(x => x.expense_type_name.Equals(expense_type_name)).FirstOrDefault();
                        if (activityExpenseType == null)
                        {
                            //edit
                            ActivityExpenseType activityExpenseType_edit = db.ActivityExpenseTypes.Where(x => x.expense_type_id == expense_type_id).FirstOrDefault();
                            if (activityExpenseType_edit != null)
                            {
                                activityExpenseType_edit.expense_type_name = expense_type_name;
                                db.SaveChanges();
                                return new AlertModal<ActivityExpenseType>(null, true, "Thành công", "Chỉnh sửa loại kinh phí thành công");
                            }
                            else
                            {
                                return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Không xác định được loại hoạt động tương ứng. Vui lòng kiểm tra lại.");
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Tên loại hoạt động không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<ActivityExpenseType> deleteActivityExpenseType(int expense_type_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    try
                    {
                        db.ActivityExpenseTypes.Remove(db.ActivityExpenseTypes.Find(expense_type_id));
                        db.SaveChanges();
                        return new AlertModal<ActivityExpenseType>(null, true, "Thành công", "Xóa loại kinh phí thành công");
                    }
                    catch (Exception e)
                    {
                        return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Loại kinh phí đang có dữ liệu tại các màn hình khác.");
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<ActivityExpenseType>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }
    }
}
