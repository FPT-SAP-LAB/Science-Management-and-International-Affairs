using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    public class AcademicActivityTypeRepo
    {
        public BaseServerSideData<AcademicActivityType> getlistAcademicActivityType(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //List<AcademicActivityType> academicActivityTypes = db.AcademicActivityTypes.ToList();
                    List<AcademicActivityType> academicActivityTypes = db.Database.SqlQuery<AcademicActivityType>("select * from SMIA_AcademicActivity.AcademicActivityType " +
                                                                        "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                        " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from SMIA_AcademicActivity.AcademicActivityType").FirstOrDefault();

                    return new BaseServerSideData<AcademicActivityType>(academicActivityTypes, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<AcademicActivityType> addAcademicActivityType(string academic_activity_type_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (academic_activity_type_name == "")
                    {
                        return new AlertModal<AcademicActivityType>(null, false, "Tên loại hoạt động học thuật không được để trống.");
                    }
                    else
                    {
                        ////check duplicate data
                        //AcademicActivityType academicActivityType = db.AcademicActivityTypes.Where(x => x.activity_type_name.Equals(academic_activity_type_name)).FirstOrDefault();
                        //if (academicActivityType == null)
                        //{
                        //    //add
                        //    academicActivityType = new AcademicActivityType
                        //    {
                        //        activity_type_name = academic_activity_type_name
                        //    };
                        //    db.AcademicActivityTypes.Add(academicActivityType);
                        //    db.SaveChanges();
                        //    return new AlertModal<AcademicActivityType>(null, true, "Thêm loại hoạt động học thuật thành công.");
                        //}
                        //else
                        //{
                        //return duplicate error
                        return new AlertModal<AcademicActivityType>(null, false, "Tên loại hoạt động không được trùng với dữ liệu đã có.");
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicActivityType>(null, false, "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicActivityType> getAcademicActivityType(int academic_activity_type_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    AcademicActivityType academicActivityType = db.AcademicActivityTypes.Find(academic_activity_type_id);
                    if (academicActivityType != null)
                    {
                        return new AlertModal<AcademicActivityType>(academicActivityType, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<AcademicActivityType>(null, false, "Không xác định được loại hoạt động tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicActivityType>(null, false, "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicActivityType> editAcademicActivityType(int academic_activity_type_id, string academic_activity_type_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (academic_activity_type_name == "")
                    {
                        return new AlertModal<AcademicActivityType>(null, false, "Tên loại hoạt động học thuật không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        //AcademicActivityType academicActivityType = db.AcademicActivityTypes.Where(x => x.activity_type_name.Equals(academic_activity_type_name)).FirstOrDefault();
                        //if (academicActivityType == null)
                        //{
                        //    //edit
                        //    AcademicActivityType academicActivityType_edit = db.AcademicActivityTypes.Find(academic_activity_type_id);
                        //    if (academicActivityType_edit != null)
                        //    {
                        //        academicActivityType_edit.activity_type_name = academic_activity_type_name;
                        //        db.SaveChanges();
                        //        return new AlertModal<AcademicActivityType>(null, true, "Chỉnh sửa loại hoạt động học thuật thành công");
                        //    }
                        //    else
                        //    {
                        //        return new AlertModal<AcademicActivityType>(null, false, "Không xác định được loại hoạt động tương ứng. Vui lòng kiểm tra lại.");
                        //    }
                        //}
                        //else
                        //{
                        //return duplicate error
                        return new AlertModal<AcademicActivityType>(null, false, "Tên loại hoạt động không được trùng với dữ liệu đã có.");
                        //}
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicActivityType>(null, false, "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicActivityType> deleteAcademicActivityType(int academic_activity_type_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    AcademicActivityType academicActivityType = db.AcademicActivityTypes.Find(academic_activity_type_id);
                    try
                    {
                        db.AcademicActivityTypes.Remove(academicActivityType);
                        db.SaveChanges();
                        return new AlertModal<AcademicActivityType>(null, true, "Xóa loại hoạt động học thuật thành công");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        db.Dispose();
                        return new AlertModal<AcademicActivityType>(null, false, "Loại hoạt động học thuật đang có dữ liệu tại các màn hình khác.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicActivityType>(null, false, "Có lỗi xảy ra.");
            }
        }
    }
}
