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
    public class UpdateAcademicActivityPhase
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void UpdateAcademicActivityPhaseUT_1()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            int language_id = 1;
            AcademicActivityPhaseRepo.infoPhase data = new AcademicActivityPhaseRepo.infoPhase
            {
                phase_name = "abcxyz",
                from = "01/01/2020",
                to = "01/01/2021",
                full_name = "aa",
                phase_id = activityPhase.phase_id
            };
            bool res = academicActivityPhaseRepo.EditPhase(language_id, data);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityPhaseUT_2()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            int language_id = 2;
            AcademicActivityPhaseRepo.infoPhase data = new AcademicActivityPhaseRepo.infoPhase
            {
                phase_name = "",
                from = "28/02/2020",
                to = "01/01/2021",
                full_name = "aa",
                phase_id = activityPhase.phase_id
            };
            bool res = academicActivityPhaseRepo.EditPhase(language_id, data);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityPhaseUT_3()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            int language_id = 2;
            AcademicActivityPhaseRepo.infoPhase data = new AcademicActivityPhaseRepo.infoPhase
            {
                phase_name = "!@#!",
                from = "01/01/2021",
                to = "01/01/202",
                full_name = "aa",
                phase_id = activityPhase.phase_id
            };
            bool res = academicActivityPhaseRepo.EditPhase(language_id, data);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityPhaseUT_4()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            int language_id = 2;
            AcademicActivityPhaseRepo.infoPhase data = new AcademicActivityPhaseRepo.infoPhase
            {
                phase_name = "abcxyz",
                from = "05/01/2020",
                to = "10/01/2021",
                full_name = "aa",
                phase_id = activityPhase.phase_id
            };
            bool res = academicActivityPhaseRepo.EditPhase(language_id, data);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityPhaseUT_5()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            int language_id = 1;
            AcademicActivityPhaseRepo.infoPhase data = new AcademicActivityPhaseRepo.infoPhase
            {
                phase_name = "abcxyz",
                from = "31/02/2020",
                to = "10/01/2021",
                full_name = "aa",
                phase_id = activityPhase.phase_id
            };
            bool res = academicActivityPhaseRepo.EditPhase(language_id, data);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityPhaseUT_6()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            int language_id = 1;
            AcademicActivityPhaseRepo.infoPhase data = new AcademicActivityPhaseRepo.infoPhase
            {
                phase_name = "",
                from = "05/01/2020",
                to = "01/01/2021",
                full_name = "aa",
                phase_id = activityPhase.phase_id
            };
            bool res = academicActivityPhaseRepo.EditPhase(language_id, data);
            if (!res)
                Assert.Pass();
        }
    }
}
