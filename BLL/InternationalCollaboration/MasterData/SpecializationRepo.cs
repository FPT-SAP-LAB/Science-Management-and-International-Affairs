using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    public class SpecializationRepo
    {
        public BaseServerSideData<Specialization> getListSpecialization(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<Specialization> specializations = db.Database.SqlQuery<Specialization>("select * from General.Specialization " +
                                                                            "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from General.Specialization").FirstOrDefault();
                    return new BaseServerSideData<Specialization>(specializations, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<Specialization> getListSpecializationUpdated()
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<Specialization> specializations = db.Specializations.ToList();
                    return specializations;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public AlertModal<Specialization> deleteSpecialization(int spe_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    Specialization specialization = db.Specializations.Find(spe_id);
                    try
                    {
                        db.Specializations.Remove(specialization);
                        db.SaveChanges();
                        return new AlertModal<Specialization>(null, true, "Thành công", "Xóa lĩnh vực hợp tác thành công");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        return new AlertModal<Specialization>(null, false, "Lỗi", "Thông tin lĩnh vực hợp tác có dữ liệu tại các màn hình khác.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<Specialization>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }
        public AlertModal<Specialization> getSpecialization(int spe_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    Specialization specialization = db.Specializations.Find(spe_id);
                    if (specialization != null)
                    {
                        return new AlertModal<Specialization>(specialization, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<Specialization>(null, false, "Lỗi", "Không xác định được lĩnh vực hợp tác tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<Specialization>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }
        public AlertModal<Specialization> addSpecialization(string spe_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (spe_name == "")
                    {
                        return new AlertModal<Specialization>(null, false, "Lỗi", "Tên lĩnh vực hợp tác không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        Specialization specialization = db.Specializations.Where(x => x.specialization_name.Equals(spe_name)).FirstOrDefault();
                        if (specialization == null)
                        {
                            //add
                            specialization = new Specialization
                            {
                                specialization_name = spe_name
                            };
                            db.Specializations.Add(specialization);
                            db.SaveChanges();
                            return new AlertModal<Specialization>(null, true, "Thành công", "Thêm lĩnh vực hợp tác thành công.");
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<Specialization>(null, false, "Lỗi", "Tên lĩnh vực hợp tác không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<Specialization>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }
        public AlertModal<Specialization> editSpecialization(int spe_id, string spe_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (spe_name == "")
                    {
                        return new AlertModal<Specialization>(null, false, "Lỗi", "Tên lĩnh vực hợp tác không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        Specialization specialization_check = db.Specializations.Where(x => x.specialization_name.Equals(spe_name) && x.specialization_id != spe_id).FirstOrDefault();
                        if (specialization_check == null)
                        {
                            //edit
                            Specialization specialization_edit = db.Specializations.Find(spe_id);
                            if (specialization_edit != null)
                            {
                                specialization_edit.specialization_name = spe_name;
                                db.SaveChanges();
                                return new AlertModal<Specialization>(null, true, "Thành công", "Chỉnh sửa lĩnh vực hợp tác thành công");
                            }
                            else
                            {
                                return new AlertModal<Specialization>(null, false, "Lỗi", "Không xác định được lĩnh vực hợp tác tương ứng. Vui lòng kiểm tra lại.");
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<Specialization>(null, false, "Lỗi", "Tên lĩnh vực hợp tác không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<Specialization>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }
    }
}
