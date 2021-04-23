using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.InternationalCollaboration.MasterData
{
    public class DirectionRepo
    {
        public BaseServerSideData<Direction> getListDirection(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<Direction> directions = db.Database.SqlQuery<Direction>("select * from IA_AcademicCollaboration.Direction " +
                                                                        "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                        " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from IA_AcademicCollaboration.Direction").FirstOrDefault();
                    return new BaseServerSideData<Direction>(directions, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<Direction> addDirection(string direction_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (direction_name == "")
                    {
                        return new AlertModal<Direction>(null, false, "Lỗi", "Tên chiều không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        Direction direction = db.Directions.Where(x => x.direction_name.Equals(direction_name)).FirstOrDefault();
                        if (direction == null)
                        {
                            //add
                            direction = new Direction
                            {
                                direction_name = direction_name
                            };
                            db.Directions.Add(direction);
                            db.SaveChanges();
                            return new AlertModal<Direction>(null, true, "Thành công", "Thêm chiều thành công.");
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<Direction>(null, false, "Lỗi", "Tên chiều không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<Direction>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<Direction> getDirection(int direction_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    Direction direction = db.Directions.Find(direction_id);
                    if (direction != null)
                    {
                        return new AlertModal<Direction>(direction, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<Direction>(null, false, "Lỗi", "Không xác định được chiều tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<Direction>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<Direction> editDirection(int direction_id, string direction_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (direction_name == "")
                    {
                        return new AlertModal<Direction>(null, false, "Lỗi", "Tên chiều không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        Direction direction = db.Directions.Where(x => x.direction_name.Equals(direction_name)).FirstOrDefault();
                        if (direction == null)
                        {
                            //edit
                            Direction direction_edit = db.Directions.Find(direction_id);
                            if (direction_edit != null)
                            {
                                direction_edit.direction_name = direction_name;
                                db.SaveChanges();
                                return new AlertModal<Direction>(null, true, "Thành công", "Chỉnh sửa chiều thành công");
                            }
                            else
                            {
                                return new AlertModal<Direction>(null, false, "Lỗi", "Không xác định được chiều tương ứng. Vui lòng kiểm tra lại.");
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<Direction>(null, false, "Lỗi", "Tên chiều không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<Direction>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }

        public AlertModal<Direction> deleteDirection(int direction_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    Direction direction = db.Directions.Find(direction_id);
                    try
                    {
                        db.Directions.Remove(db.Directions.Find(direction_id));
                        db.SaveChanges();
                        return new AlertModal<Direction>(null, true, "Thành công", "Xóa chiều thành công");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        return new AlertModal<Direction>(null, false, "Lỗi", "Loại kinh phí đang có dữ liệu tại các màn hình khác.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<Direction>(null, false, "Lỗi", "Có lỗi xảy ra.");
            }
        }
    }
}
