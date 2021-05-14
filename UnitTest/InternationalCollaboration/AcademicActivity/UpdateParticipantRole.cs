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
    public class UpdateParticipantRole
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void UpdateParticipantRoleUT_1()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddParticipantRole().AddParticipantRoleUT_1();
            ParticipantRole pr = db.ParticipantRoles.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = pr.participant_role_id,
                participant_role_name = "abcxyz",
                price = "1000"
            };
            string quantity = "1";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.EditParticipantRole(data, arrOffice, "False", quantity, 1);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateParticipantRoleUT_2()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddParticipantRole().AddParticipantRoleUT_1();
            ParticipantRole pr = db.ParticipantRoles.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = pr.participant_role_id,
                participant_role_name = "",
                price = "-999"
            };
            string quantity = "-999";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.EditParticipantRole(data, arrOffice, "False", quantity, 1);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateParticipantRoleUT_3()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddParticipantRole().AddParticipantRoleUT_1();
            ParticipantRole pr = db.ParticipantRoles.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = pr.participant_role_id,
                participant_role_name = "!@#!",
                price = "-999"
            };
            string quantity = "1";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.EditParticipantRole(data, arrOffice, "False", quantity, 1);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateParticipantRoleUT_4()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddParticipantRole().AddParticipantRoleUT_1();
            ParticipantRole pr = db.ParticipantRoles.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = 0,
                participant_role_name = "abcxyz",
                price = "-999"
            };
            string quantity = "1000";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.EditParticipantRole(data, arrOffice, "False", quantity, 1);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateParticipantRoleUT_5()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddParticipantRole().AddParticipantRoleUT_1();
            ParticipantRole pr = db.ParticipantRoles.Last();
            AcademicActivityPhaseRepo.baseParticipantRole data = new AcademicActivityPhaseRepo.baseParticipantRole
            {
                participant_role_id = 0,
                participant_role_name = "abcxyz",
                price = "1000"
            };
            string quantity = "0";
            List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice = new List<AcademicActivityPhaseRepo.basePlanParticipant>();
            bool res = academicActivityPhaseRepo.EditParticipantRole(data, arrOffice, "False", quantity, 1);
            if (res)
                Assert.Pass();
        }
    }
}
