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
    public class GetDetailPhaseUT
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void GetDetailPhaseUT_1()
        {
            AcademicActivityPhaseRepo phaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            ENTITIES.AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            int language_id = 1;
            AcademicActivityPhaseRepo.basePhase data = phaseRepo.GetDetailPhase(language_id, activityPhase.phase_id);
            if (data != null)
                Assert.Pass();
        }
        [TestCase]
        public void GetDetailPhaseUT_2()
        {
            AcademicActivityPhaseRepo phaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            ENTITIES.AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            int language_id = 2;
            AcademicActivityPhaseRepo.basePhase data = phaseRepo.GetDetailPhase(language_id, activityPhase.phase_id);
            if (data != null)
                Assert.Pass();
        }
        [TestCase]
        public void GetDetailPhaseUT_3()
        {
            AcademicActivityPhaseRepo phaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            ENTITIES.AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            int language_id = 3;
            AcademicActivityPhaseRepo.basePhase data = phaseRepo.GetDetailPhase(language_id, activityPhase.phase_id);
            if (data == null)
                Assert.Pass();
        }
    }
}
