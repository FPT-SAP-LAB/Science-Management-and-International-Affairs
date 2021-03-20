using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    public class InternalUnitRepo
    {
        public BaseServerSideData<InternalUnit> getListInternalUnit(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<InternalUnit> internalUnits = db.Database.SqlQuery<InternalUnit>("select * from General.InternalUnit " +
                                                                        "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                        " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from General.InternalUnit").FirstOrDefault();
                    return new BaseServerSideData<InternalUnit>(internalUnits, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<InternalUnit> addInternalUnit(string unit_name, string unit_abbreviation)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (unit_name == "" || unit_abbreviation == "")
                    {
                        return new AlertModal<InternalUnit>(null, false, "Lỗi", "Tên đơn vị nội bộ không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        InternalUnit internalUnit = db.InternalUnits.Where(x => x.unit_name.Equals(unit_name) && x.unit_abbreviation.Equals(unit_abbreviation)).FirstOrDefault();
                        if (internalUnit == null)
                        {
                            //add
                            internalUnit = new InternalUnit
                            {
                                unit_name = unit_name,
                                unit_abbreviation = unit_abbreviation
                            };
                            db.InternalUnits.Add(internalUnit);
                            db.SaveChanges();
                            return new AlertModal<InternalUnit>(null, true, "Thành công", "Thêm đơn vị nội bộ thành công.");
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<InternalUnit>(null, false, "Lỗi", "Tên đơn vị nội bộ không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<InternalUnit>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<InternalUnit> getInternalUnit(int unit_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    InternalUnit internalUnit = db.InternalUnits.Find(unit_id);
                    if (internalUnit != null)
                    {
                        return new AlertModal<InternalUnit>(internalUnit, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<InternalUnit>(null, false, "Lỗi", "Không xác định được đơn vị nội bộ tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<InternalUnit>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<InternalUnit> editInternalUnit(int unit_id, string unit_name, string unit_abbreviation)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (unit_name == "")
                    {
                        return new AlertModal<InternalUnit>(null, false, "Lỗi", "Tên đơn vị nội bộ không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        InternalUnit internalUnit = db.InternalUnits.Where(x => x.unit_name.Equals(unit_name) && x.unit_abbreviation.Equals(unit_abbreviation)).FirstOrDefault();
                        if (internalUnit == null)
                        {
                            //edit
                            InternalUnit internalUnit_edit = db.InternalUnits.Find(unit_id);
                            if (internalUnit_edit != null)
                            {
                                internalUnit_edit.unit_name = unit_name;
                                internalUnit_edit.unit_abbreviation = unit_abbreviation;
                                db.SaveChanges();
                                return new AlertModal<InternalUnit>(null, true, "Thành công", "Chỉnh sửa đơn vị nội bộ thành công");
                            }
                            else
                            {
                                return new AlertModal<InternalUnit>(null, false, "Lỗi", "Không xác định được đơn vị nội bộ tương ứng. Vui lòng kiểm tra lại.");
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<InternalUnit>(null, false, "Lỗi", "Tên đơn vị nội bộ không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<InternalUnit>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<InternalUnit> deleteInternalUnit(int unit_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    InternalUnit internalUnit = db.InternalUnits.Find(unit_id);
                    try
                    {
                        db.InternalUnits.Remove(db.InternalUnits.Find(unit_id));
                        db.SaveChanges();
                        return new AlertModal<InternalUnit>(null, true, "Thành công", "Xóa đơn vị nội bộ thành công");
                    }
                    catch (Exception e)
                    {
                        return new AlertModal<InternalUnit>(null, false, "Lỗi", "Loại kinh phí đang có dữ liệu tại các màn hình khác.");
                    }
                }
            }
            catch (Exception e)
            {
                return new AlertModal<InternalUnit>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }
    }
}
