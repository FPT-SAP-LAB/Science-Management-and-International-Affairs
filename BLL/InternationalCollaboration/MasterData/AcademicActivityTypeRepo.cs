using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration.MasterData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.InternationalCollaboration.MasterData
{
    public class AcademicActivityTypeRepo
    {
        public BaseServerSideData<AcademicActivityType_Ext> getlistAcademicActivityType(BaseDatatable baseDatatable, int language_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //List<AcademicActivityType> academicActivityTypes = db.AcademicActivityTypes.ToList();
                    List<AcademicActivityType_Ext> academicActivityTypes = db.Database.SqlQuery<AcademicActivityType_Ext>(@"select aat.activity_type_id, aatl.activity_type_name from SMIA_AcademicActivity.AcademicActivityType aat
                                                                                                                            join SMIA_AcademicActivity.AcademicActivityTypeLanguage aatl on aat.activity_type_id = aatl.activity_type_id 
                                                                                                                            join Localization.[Language] l on l.language_id = aatl.language_id 
                                                                                                                            where l.language_id = @language_id 
                                                                                                                            ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                                                                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY",
                                                                                                                            new SqlParameter("language_id", language_id)).ToList();
                    int recordsTotal = db.Database.SqlQuery<int>(@"select count(*) from SMIA_AcademicActivity.AcademicActivityType aat
                                                                join SMIA_AcademicActivity.AcademicActivityTypeLanguage aatl on aat.activity_type_id = aatl.activity_type_id 
                                                                join Localization.[Language] l on l.language_id = aatl.language_id 
                                                                where l.language_id = @language_id",
                                                                new SqlParameter("language_id", language_id)).FirstOrDefault();

                    return new BaseServerSideData<AcademicActivityType_Ext>(academicActivityTypes, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static AlertModal<List<Language>> getLanguages()
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<Language> languages = db.Languages.ToList();
                    return new AlertModal<List<Language>>(languages, true, null, null);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal<AcademicActivityType_Ext> addAcademicActivityType(int language_id, string activity_type_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (activity_type_name == "")
                    {
                        return new AlertModal<AcademicActivityType_Ext>(null, false, "Tên loại hoạt động học thuật không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        AcademicActivityType_Ext academicActivityType_Ext = db.Database.SqlQuery<AcademicActivityType_Ext>(@"select aat.activity_type_id, aatl.activity_type_name from SMIA_AcademicActivity.AcademicActivityType aat
                                                                                                                        join SMIA_AcademicActivity.AcademicActivityTypeLanguage aatl on aat.activity_type_id = aatl.activity_type_id
                                                                                                                        join Localization.[Language] l on l.language_id = aatl.language_id
                                                                                                                        where l.language_id = @language_id and aatl.activity_type_name = @activity_type_name",
                                                                                                                        new SqlParameter("language_id", language_id),
                                                                                                                        new SqlParameter("activity_type_name", activity_type_name)).FirstOrDefault();
                        if (academicActivityType_Ext == null)
                        {
                            using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                            {
                                try
                                {
                                    //add to AcademicActivity
                                    AcademicActivityType academicActivityType = new AcademicActivityType();
                                    db.AcademicActivityTypes.Add(academicActivityType);
                                    db.SaveChanges();

                                    //add to AcademicActivityLanguage
                                    AcademicActivityTypeLanguage academicActivityTypeLanguage = new AcademicActivityTypeLanguage
                                    {
                                        activity_type_id = academicActivityType.activity_type_id,
                                        activity_type_name = activity_type_name,
                                        language_id = language_id
                                    };
                                    db.AcademicActivityTypeLanguages.Add(academicActivityTypeLanguage);
                                    db.SaveChanges();
                                    dbContext.Commit();
                                    return new AlertModal<AcademicActivityType_Ext>(null, true, "Thêm loại hoạt động học thuật thành công.");
                                }
                                catch (Exception e)
                                {
                                    dbContext.Rollback();
                                    throw e;
                                }
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<AcademicActivityType_Ext>(null, false, "Tên loại hoạt động không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicActivityType_Ext>(null, false, "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicActivityType_Ext> getAcademicActivityType(int language_id, int activity_type_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    var sql = @"select aat.activity_type_id, aatl.activity_type_name from SMIA_AcademicActivity.AcademicActivityType aat
                                join SMIA_AcademicActivity.AcademicActivityTypeLanguage aatl on aat.activity_type_id = aatl.activity_type_id
                                join Localization.[Language] l on l.language_id = aatl.language_id
                                where l.language_id = @language_id and aatl.activity_type_id = @activity_type_id";
                    AcademicActivityType_Ext academicActivityType = db.Database.SqlQuery<AcademicActivityType_Ext>(sql,
                                                                    new SqlParameter("language_id", language_id),
                                                                    new SqlParameter("activity_type_id", activity_type_id)).FirstOrDefault();
                    if (academicActivityType != null)
                    {
                        return new AlertModal<AcademicActivityType_Ext>(academicActivityType, true, null, null);
                    }
                    else
                    {
                        return new AlertModal<AcademicActivityType_Ext>(null, false, "Không xác định được loại hoạt động tương ứng. Vui lòng kiểm tra lại.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicActivityType_Ext>(null, false, "Có lỗi xảy ra.");
            }
        }

        public AlertModal<AcademicActivityType_Ext> editAcademicActivityType(int language_id, int activity_type_id, string activity_type_name)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    //empty error
                    if (activity_type_name == "")
                    {
                        return new AlertModal<AcademicActivityType_Ext>(null, false, "Tên loại hoạt động học thuật không được để trống.");
                    }
                    else
                    {
                        //check duplicate data
                        if (checkDuplicateAcademicActivityType(language_id, activity_type_id, activity_type_name, db))
                        {
                            //edit
                            AcademicActivityTypeLanguage academicActivityTypeLanguage = db.AcademicActivityTypeLanguages.Find(language_id, activity_type_id);
                            if (academicActivityTypeLanguage != null)
                            {
                                academicActivityTypeLanguage.activity_type_name = activity_type_name;
                                db.SaveChanges();
                                return new AlertModal<AcademicActivityType_Ext>(null, true, "Chỉnh sửa loại hoạt động học thuật thành công");
                            }
                            else
                            {
                                return new AlertModal<AcademicActivityType_Ext>(null, false, "Không xác định được loại hoạt động tương ứng. Vui lòng kiểm tra lại.");
                            }
                        }
                        else
                        {
                            //return duplicate error
                            return new AlertModal<AcademicActivityType_Ext>(null, false, "Tên loại hoạt động không được trùng với dữ liệu đã có.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicActivityType_Ext>(null, false, "Có lỗi xảy ra.");
            }
        }

        public bool checkDuplicateAcademicActivityType(int language_id, int activity_type_id, string activity_type_name, ScienceAndInternationalAffairsEntities db)
        {
            var sql = @"select aat.activity_type_id, aatl.activity_type_name from SMIA_AcademicActivity.AcademicActivityType aat
                                join SMIA_AcademicActivity.AcademicActivityTypeLanguage aatl on aat.activity_type_id = aatl.activity_type_id
                                join Localization.[Language] l on l.language_id = aatl.language_id
                                where l.language_id = @language_id and aatl.activity_type_id != @activity_type_id and aatl.activity_type_name = @activity_type_name";
            AcademicActivityType_Ext academicActivityType_Ext = db.Database.SqlQuery<AcademicActivityType_Ext>(sql,
                                                                    new SqlParameter("language_id", language_id),
                                                                    new SqlParameter("activity_type_id", activity_type_id),
                                                                    new SqlParameter("activity_type_name", activity_type_name)).FirstOrDefault();
            if (academicActivityType_Ext == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AlertModal<AcademicActivityType_Ext> deleteAcademicActivityType(int language_id, int activity_type_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    using (DbContextTransaction dbContext = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Configuration.LazyLoadingEnabled = false;
                            //delete AcademicLanguageTypeLanguage
                            AcademicActivityTypeLanguage academicActivityTypeLanguage = db.AcademicActivityTypeLanguages.Find(language_id, activity_type_id);
                            db.AcademicActivityTypeLanguages.Remove(academicActivityTypeLanguage);
                            db.SaveChanges();
                            try
                            {
                                AcademicActivityType academicActivityType = db.AcademicActivityTypes.Find(activity_type_id);
                                db.AcademicActivityTypes.Remove(academicActivityType);
                                db.SaveChanges();
                                dbContext.Commit();
                                return new AlertModal<AcademicActivityType_Ext>(null, true, "Xóa loại hoạt động học thuật thành công");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                                dbContext.Rollback();
                                return new AlertModal<AcademicActivityType_Ext>(null, false, "Loại hoạt động học thuật đang có dữ liệu tại các màn hình khác.");
                            }
                        }
                        catch (Exception e)
                        {
                            dbContext.Rollback();
                            throw e;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicActivityType_Ext>(null, false, "Có lỗi xảy ra.");
            }
        }
    }
}
