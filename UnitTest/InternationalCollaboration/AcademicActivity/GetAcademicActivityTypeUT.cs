using BLL.InternationalCollaboration.AcademicActivity;
using ENTITIES;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.InternationalCollaboration.AcademicActivity
{
    [TestFixture]
    public class GetAcademicActivityTypeUT
    {
        [TestCase]
        public void GetAcademicActivityType_1()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            int language_id = 1;
            List<AcademicActivityTypeLanguage> data = activityRepo.GetType(language_id);
            if (data.Count > 0)
                Assert.Pass();
        }
        [TestCase]
        public void GetAcademicActivityType_2()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            int language_id = 2;
            List<AcademicActivityTypeLanguage> data = activityRepo.GetType(language_id);
            if (data.Count > 0)
                Assert.Pass();
        }
        [TestCase]
        public void GetAcademicActivityType_3()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            int language_id = 0;
            List<AcademicActivityTypeLanguage> data = activityRepo.GetType(language_id);
            if (data.Count == 0)
                Assert.Pass();
        }
        [TestCase]
        public void GetAcademicActivityType_4()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            int language_id = -999;
            List<AcademicActivityTypeLanguage> data = activityRepo.GetType(language_id);
            if (data.Count == 0)
                Assert.Pass();
        }
    }
}
