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
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public List<AcademicActivityType> getlistAcademicActivityType(BaseDatatable baseDatatable)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                //List<AcademicActivityType> academicActivityTypes = db.AcademicActivityTypes.ToList();

                List<AcademicActivityType> academicActivityTypes = db.Database.SqlQuery<AcademicActivityType>("select * from SMIA_AcademicActivity.AcademicActivityType " +
                                                                    "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                    " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY").ToList();

                return academicActivityTypes;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public AlertModal addAcademicActivityType(string academic_activity_name)
        {
            try
            {
                if (academic_activity_name == "" || academic_activity_name == null)
                {
                    AlertModal alertModal = new AlertModal
                    {
                        success = false,
                        title = "Lỗi",
                        content = "Tên loại hoạt động học thuật không được để trống."
                    };
                    return alertModal;
                }
                else
                {

                }

                return null;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
