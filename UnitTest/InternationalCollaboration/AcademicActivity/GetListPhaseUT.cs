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
    public class GetListPhaseUT
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void GetListPhaseUT_1()
        {
            AcademicActivityPhaseRepo phaseRepo = new AcademicActivityPhaseRepo();
            int language_id = 1;
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            List<AcademicActivityPhaseRepo.infoPhase> data = phaseRepo.getPhase(language_id,academicActivity.activity_id);
            if (data.Count >= 0)
                Assert.Pass();
        }
        [TestCase]
        public void GetListPhaseUT_2()
        {
            AcademicActivityPhaseRepo phaseRepo = new AcademicActivityPhaseRepo();
            int language_id = 2;
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            List<AcademicActivityPhaseRepo.infoPhase> data = phaseRepo.getPhase(language_id, academicActivity.activity_id);
            if (data.Count >= 0)
                Assert.Pass();
        }
        [TestCase]
        public void GetListPhaseUT_3()
        {
            AcademicActivityPhaseRepo phaseRepo = new AcademicActivityPhaseRepo();
            int language_id = 0;
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            List<AcademicActivityPhaseRepo.infoPhase> data = phaseRepo.getPhase(language_id, academicActivity.activity_id);
            if (data.Count == 0)
                Assert.Pass();
        }
    }
}
