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
    public class DeleteParticipantRole
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void DeleteParticipantRoleUT_1()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddParticipantRole().AddParticipantRoleUT_1();
            ParticipantRole participantRole = db.ParticipantRoles.Last();
            bool res = academicActivityPhaseRepo.DeleteParticipantRole(participantRole.participant_role_id);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void DeleteParticipantRoleUT_2()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            bool res = academicActivityPhaseRepo.DeleteParticipantRole(0);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void DeleteParticipantRoleUT_3()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            bool res = academicActivityPhaseRepo.DeleteParticipantRole(-999);
            if (res)
                Assert.Pass();
        }
    }
}
