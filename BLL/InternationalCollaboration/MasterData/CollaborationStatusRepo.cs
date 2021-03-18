using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    public class CollaborationStatusRepo
    {
        public BaseServerSideData<CollaborationStatu> getListCollaborationStatu(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<CollaborationStatu> activityExpenseTypes = db.Database.SqlQuery<CollaborationStatu>("select * from IA_Collaboration.CollaborationStatus " +
                                                                        "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                        " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from IA_Collaboration.CollaborationStatus").FirstOrDefault();
                    return new BaseServerSideData<CollaborationStatu>(activityExpenseTypes, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<CollaborationStatu> addCollaborationStatu(string mou_status_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (mou_status_name == "")
                    {
                        return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Tên trạng thái thỏa thuận hợp tác không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        CollaborationStatu collaborationStatu = db.CollaborationStatus.Where(x => x.mou_status_name.Equals(mou_status_name)).FirstOrDefault();
                        if (collaborationStatu == null)
                        {
                            //add
                            collaborationStatu = new CollaborationStatu
                            {
                                mou_status_name = mou_status_name
                            };
                            db.CollaborationStatus.Add(collaborationStatu);
                            db.SaveChanges();
                            return new AlertModal<CollaborationStatu>(null, true, "Thành công", "Thêm trạng thái thỏa thuận hợp tác thành công.");
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Tên trạng thái thỏa thuận hợp tác không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<CollaborationStatu> getCollaborationStatu(int mou_status_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    CollaborationStatu collaborationStatu = db.CollaborationStatus.Find(mou_status_id);
                    if (collaborationStatu != null)
                    {
                        return new AlertModal<CollaborationStatu>(collaborationStatu, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Không xác định được trạng thái thỏa thuận hợp tác tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<CollaborationStatu> editCollaborationStatu(int mou_status_id, string mou_status_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (mou_status_name == "")
                    {
                        return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Tên trạng thái thỏa thuận hợp tác không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        CollaborationStatu collaborationStatu = db.CollaborationStatus.Where(x => x.mou_status_name.Equals(mou_status_name)).FirstOrDefault();
                        if (collaborationStatu == null)
                        {
                            //edit
                            CollaborationStatu collaborationStatu_edit = db.CollaborationStatus.Find(mou_status_id);
                            if (collaborationStatu_edit != null)
                            {
                                collaborationStatu_edit.mou_status_name = mou_status_name;
                                db.SaveChanges();
                                return new AlertModal<CollaborationStatu>(null, true, "Thành công", "Chỉnh sửa trạng thái thỏa thuận hợp tác thành công");
                            }
                            else
                            {
                                return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Không xác định được trạng thái thỏa thuận hợp tác tương ứng. Vui lòng kiểm tra lại.");
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Tên trạng thái thỏa thuận hợp tác không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<CollaborationStatu> deleteCollaborationStatu(int mou_status_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    CollaborationStatu collaborationStatu = db.CollaborationStatus.Find(mou_status_id);
                    try
                    {
                        db.CollaborationStatus.Remove(db.CollaborationStatus.Find(mou_status_id));
                        db.SaveChanges();
                        return new AlertModal<CollaborationStatu>(null, true, "Thành công", "Xóa trạng thái thỏa thuận hợp tác thành công");
                    }
                    catch (Exception e)
                    {
                        return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Loại kinh phí đang có dữ liệu tại các màn hình khác.");
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<CollaborationStatu>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

    }
}
