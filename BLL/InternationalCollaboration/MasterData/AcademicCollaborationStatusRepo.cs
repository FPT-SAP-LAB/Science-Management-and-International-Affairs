using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    public class AcademicCollaborationStatusRepo
    {
        public BaseServerSideData<AcademicCollaborationStatu_Ext> getListAcademicCollaborationStatu(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<AcademicCollaborationStatu_Ext> academicCollaborationStatus = db.Database.SqlQuery<AcademicCollaborationStatu_Ext>(@"select collab_status_id, collab_status_name,
                                                                        CASE
                                                                        WHEN status_type = 1 THEN N'Dài hạn'
                                                                        WHEN status_type = 2 THEN N'Ngắn hạn'
                                                                        WHEN status_type = 3 THEN N'Dài hạn và ngắn hạn'
                                                                        END as 'status_type_name'
                                                                        from IA_AcademicCollaboration.AcademicCollaborationStatus " +
                                                                        "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                        " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from IA_AcademicCollaboration.AcademicCollaborationStatus").FirstOrDefault();
                    return new BaseServerSideData<AcademicCollaborationStatu_Ext>(academicCollaborationStatus, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<AcademicCollaborationStatu> addAcademicCollaborationStatu(string collab_status_name, int status_type)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (collab_status_name == "" || status_type.ToString() == "")
                    {
                        return new AlertModal<AcademicCollaborationStatu>(null, false, "Trạng thái hoặc loại trạng thái không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        AcademicCollaborationStatu academicCollaborationStatu = db.AcademicCollaborationStatus.Where(x => x.collab_status_name.Equals(collab_status_name) && x.status_type == status_type).FirstOrDefault();
                        if (academicCollaborationStatu == null)
                        {
                            //add
                            academicCollaborationStatu = new AcademicCollaborationStatu
                            {
                                collab_status_name = collab_status_name,
                                status_type = status_type
                            };
                            db.AcademicCollaborationStatus.Add(academicCollaborationStatu);
                            db.SaveChanges();
                            return new AlertModal<AcademicCollaborationStatu>(null, true, "Thêm Trạng thái hợp tác học thuật thành công.");
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<AcademicCollaborationStatu>(null, false, "Tên Trạng thái hợp tác học thuật không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicCollaborationStatu>(null, false, "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicCollaborationStatu> getAcademicCollaborationStatu(int collab_status_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    AcademicCollaborationStatu academicCollaborationStatu = db.AcademicCollaborationStatus.Find(collab_status_id);
                    if (academicCollaborationStatu != null)
                    {
                        return new AlertModal<AcademicCollaborationStatu>(academicCollaborationStatu, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<AcademicCollaborationStatu>(null, false, "Không xác định được Trạng thái hợp tác học thuật tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicCollaborationStatu>(null, false, "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicCollaborationStatu> editAcademicCollaborationStatu(int collab_status_id, string collab_status_name, int status_type)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (collab_status_name == "" || status_type.ToString() == "")
                    {
                        return new AlertModal<AcademicCollaborationStatu>(null, false, "Tên Trạng thái hợp tác học thuật không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        AcademicCollaborationStatu academicCollaborationStatu = db.AcademicCollaborationStatus.Where(x => x.collab_status_name.Equals(collab_status_name) && x.status_type == status_type).FirstOrDefault();
                        if (academicCollaborationStatu == null)
                        {
                            //edit
                            AcademicCollaborationStatu academicCollaborationStatu_edit = db.AcademicCollaborationStatus.Find(collab_status_id);
                            if (academicCollaborationStatu_edit != null)
                            {
                                academicCollaborationStatu_edit.collab_status_name = collab_status_name;
                                academicCollaborationStatu_edit.status_type = status_type;
                                db.SaveChanges();
                                return new AlertModal<AcademicCollaborationStatu>(null, true, "Chỉnh sửa Trạng thái hợp tác học thuật thành công");
                            }
                            else
                            {
                                return new AlertModal<AcademicCollaborationStatu>(null, false, "Không xác định được Trạng thái hợp tác học thuật tương ứng. Vui lòng kiểm tra lại.");
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<AcademicCollaborationStatu>(null, false, "Tên Trạng thái hợp tác học thuật không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicCollaborationStatu>(null, false, "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicCollaborationStatu> deleteAcademicCollaborationStatu(int collab_status_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    AcademicCollaborationStatu academicCollaborationStatu = db.AcademicCollaborationStatus.Find(collab_status_id);
                    try
                    {
                        db.AcademicCollaborationStatus.Remove(db.AcademicCollaborationStatus.Find(collab_status_id));
                        db.SaveChanges();
                        return new AlertModal<AcademicCollaborationStatu>(null, true, "Xóa Trạng thái hợp tác học thuật thành công");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        return new AlertModal<AcademicCollaborationStatu>(null, false, "Loại kinh phí đang có dữ liệu tại các màn hình khác.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicCollaborationStatu>(null, false, "Có lỗi xảy ra.");
            }
        }
    }
}
