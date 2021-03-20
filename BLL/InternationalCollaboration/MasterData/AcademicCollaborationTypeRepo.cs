using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    public class AcademicCollaborationTypeRepo
    {
        public BaseServerSideData<AcademicCollaborationType> getListAcademicCollaborationType(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<AcademicCollaborationType> academicCollaborationTypes = db.Database.SqlQuery<AcademicCollaborationType>("select * from IA_AcademicCollaboration.AcademicCollaborationType " +
                                                                        "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                        " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from IA_AcademicCollaboration.AcademicCollaborationType").FirstOrDefault();
                    return new BaseServerSideData<AcademicCollaborationType>(academicCollaborationTypes, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<AcademicCollaborationType> addAcademicCollaborationType(string collab_type_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (collab_type_name == "")
                    {
                        return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Tên loại hợp tác học thuật tác không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        AcademicCollaborationType academicCollaborationType = db.AcademicCollaborationTypes.Where(x => x.collab_type_name.Equals(collab_type_name)).FirstOrDefault();
                        if (academicCollaborationType == null)
                        {
                            //add
                            academicCollaborationType = new AcademicCollaborationType
                            {
                                collab_type_name = collab_type_name
                            };
                            db.AcademicCollaborationTypes.Add(academicCollaborationType);
                            db.SaveChanges();
                            return new AlertModal<AcademicCollaborationType>(null, true, "Thành công", "Thêm loại hợp tác học thuật tác thành công.");
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Tên loại hợp tác học thuật tác không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicCollaborationType> getAcademicCollaborationType(int collab_type_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    AcademicCollaborationType academicCollaborationType = db.AcademicCollaborationTypes.Find(collab_type_id);
                    if (academicCollaborationType != null)
                    {
                        return new AlertModal<AcademicCollaborationType>(academicCollaborationType, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Không xác định được loại hợp tác học thuật tác tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicCollaborationType> editAcademicCollaborationType(int collab_type_id, string collab_type_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (collab_type_name == "")
                    {
                        return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Tên loại hợp tác học thuật tác không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        AcademicCollaborationType academicCollaborationType = db.AcademicCollaborationTypes.Where(x => x.collab_type_name.Equals(collab_type_name)).FirstOrDefault();
                        if (academicCollaborationType == null)
                        {
                            //edit
                            AcademicCollaborationType academicCollaborationType_edit = db.AcademicCollaborationTypes.Find(collab_type_id);
                            if (academicCollaborationType_edit != null)
                            {
                                academicCollaborationType_edit.collab_type_name = collab_type_name;
                                db.SaveChanges();
                                return new AlertModal<AcademicCollaborationType>(null, true, "Thành công", "Chỉnh sửa loại hợp tác học thuật tác thành công");
                            }
                            else
                            {
                                return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Không xác định được loại hợp tác học thuật tác tương ứng. Vui lòng kiểm tra lại.");
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Tên loại hợp tác học thuật tác không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicCollaborationType> deleteAcademicCollaborationType(int collab_type_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    AcademicCollaborationType academicCollaborationType = db.AcademicCollaborationTypes.Find(collab_type_id);
                    try
                    {
                        db.AcademicCollaborationTypes.Remove(db.AcademicCollaborationTypes.Find(collab_type_id));
                        db.SaveChanges();
                        return new AlertModal<AcademicCollaborationType>(null, true, "Thành công", "Xóa loại hợp tác học thuật tác thành công");
                    }
                    catch (Exception e)
                    {
                        return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Loại kinh phí đang có dữ liệu tại các màn hình khác.");
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<AcademicCollaborationType>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }
    }
}
