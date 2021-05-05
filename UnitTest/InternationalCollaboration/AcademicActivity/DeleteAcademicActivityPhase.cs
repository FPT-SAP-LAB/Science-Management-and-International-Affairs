using BLL.InternationalCollaboration.AcademicActivity;
using ENTITIES;
using ENTITIES.CustomModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.InternationalCollaboration.AcademicActivity
{
    [TestFixture]
    public class DeleteAcademicActivityPhase
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void DeleteAcademicActivityPhaseUT_1()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            bool res = academicActivityPhaseRepo.deletePhase(activityPhase.phase_id);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void DeleteAcademicActivityPhaseUT_2()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            bool res = academicActivityPhaseRepo.deletePhase(0);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void DeleteAcademicActivityPhaseUT_3()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            bool res = academicActivityPhaseRepo.deletePhase(-999);
            if (res)
                Assert.Pass();
        }
    }
}
