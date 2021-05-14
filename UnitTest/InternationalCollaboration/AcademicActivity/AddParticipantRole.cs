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
    public class AddParticipantRole
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void AddParticipantRoleUT_1()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = 1,
                participant_role_name = "abcxyz",
                price = "1000"
            };
            string quantity = "1";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.AddParticipantRole(data, arrOffice, "False", quantity, activityPhase.phase_id);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void AddParticipantRoleUT_2()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = 1,
                participant_role_name = "",
                price = "-999"
            };
            string quantity = "-999";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.AddParticipantRole(data, arrOffice, "False", quantity, activityPhase.phase_id);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void AddParticipantRoleUT_3()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = 1,
                participant_role_name = "!@#!",
                price = "-999"
            };
            string quantity = "1";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.AddParticipantRole(data, arrOffice, "False", quantity, activityPhase.phase_id);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void AddParticipantRoleUT_4()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = 1,
                participant_role_name = "abcxyz",
                price = "-999"
            };
            string quantity = "1000";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.AddParticipantRole(data, arrOffice, "False", quantity, 0);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void AddParticipantRoleUT_5()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = 1,
                participant_role_name = "abcxyz",
                price = "1000"
            };
            string quantity = "0";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.AddParticipantRole(data, arrOffice, "False", quantity, 0);
            if (!res)
                Assert.Pass();
        }
    }
}
