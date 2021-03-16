using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.MasterData
{
    public class AcademicActivityTypeRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public List<ENTITIES.AcademicActivityType> getlistAcademicActivityType()
        {
            List<ENTITIES.AcademicActivityType> academicActivityTypes = db.AcademicActivityTypes.ToList();
            return academicActivityTypes;
        }
    }
}
