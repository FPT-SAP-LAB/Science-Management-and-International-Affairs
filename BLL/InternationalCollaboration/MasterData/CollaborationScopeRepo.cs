using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    public class CollaborationScopeRepo
    {
        public BaseServerSideData<CollaborationScope> getListCollaborationScope(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<CollaborationScope> collaborationScopes = db.Database.SqlQuery<CollaborationScope>("select * from IA_Collaboration.CollaborationScope " +
                                                                            "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from IA_Collaboration.CollaborationScope").FirstOrDefault();
                    return new BaseServerSideData<CollaborationScope>(collaborationScopes, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<CollaborationScope> addCollaborationScope(string scope_name, string scope_abbreviation)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (scope_name == "" || scope_abbreviation == "")
                    {
                        return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Tên chi tiết hoặc tên viết tắt phạm vi hợp tác không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        CollaborationScope collaborationScope = db.CollaborationScopes.Where(x => x.scope_name.Equals(scope_name) && x.scope_abbreviation.Equals(scope_abbreviation)).FirstOrDefault();
                        if (collaborationScope == null)
                        {
                            //add
                            collaborationScope = new CollaborationScope
                            {
                                scope_name = scope_name,
                                scope_abbreviation = scope_abbreviation
                            };
                            db.CollaborationScopes.Add(collaborationScope);
                            db.SaveChanges();
                            return new AlertModal<CollaborationScope>(null, true, "Thành công", "Thêm phạm vi hợp tác thành công.");
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Tên phạm vi hợp tác không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<CollaborationScope> getCollaborationScope(int scope_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    CollaborationScope collaborationScope = db.CollaborationScopes.Find(scope_id);
                    if (collaborationScope != null)
                    {
                        return new AlertModal<CollaborationScope>(collaborationScope, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Không xác định được phạm vi hợp tác tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<CollaborationScope> editCollaborationScope(int scope_id, string scope_name, string scope_abbreviation)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (scope_name == "" || scope_abbreviation == "")
                    {
                        return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Tên chi tiết hoặc tên viết tắt phạm vi hợp tác không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        CollaborationScope collaborationScope = db.CollaborationScopes.Where(x => x.scope_name.Equals(scope_name) && x.scope_abbreviation.Equals(scope_abbreviation)).FirstOrDefault();
                        if (collaborationScope == null)
                        {
                            //edit
                            CollaborationScope collaborationScope_edit = db.CollaborationScopes.Find(scope_id);
                            if (collaborationScope_edit != null)
                            {
                                collaborationScope_edit.scope_name = scope_name;
                                collaborationScope_edit.scope_abbreviation = scope_abbreviation;
                                db.SaveChanges();
                                return new AlertModal<CollaborationScope>(null, true, "Thành công", "Chỉnh sửa phạm vi hợp tác thành công");
                            }
                            else
                            {
                                return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Không xác định được phạm vi hợp tác tương ứng. Vui lòng kiểm tra lại.");
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Tên phạm vi hợp tác không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<CollaborationScope> deleteCollaborationScope(int scope_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    CollaborationScope collaborationScope = db.CollaborationScopes.Find(scope_id);
                    try
                    {
                        db.CollaborationScopes.Remove(collaborationScope);
                        db.SaveChanges();
                        return new AlertModal<CollaborationScope>(null, true, "Thành công", "Xóa phạm vi hợp tác thành công");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Loại kinh phí đang có dữ liệu tại các màn hình khác.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<CollaborationScope>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }
    }
}
