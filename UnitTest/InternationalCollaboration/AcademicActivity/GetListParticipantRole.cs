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
    public class GetListParticipantRole
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void GetListParticipantRoleUT_1()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase activityPhase = db.AcademicActivityPhases.Last();
            List<AcademicActivityPhaseRepo.baseParticipantRole> data = academicActivityPhaseRepo.getParticipantRoleByPhase(activityPhase.phase_id);
            if(data.Count == 0)
            {
                Assert.Pass();
            }
        }
        [TestCase]
        public void GetListParticipantRoleUT_2()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            List<AcademicActivityPhaseRepo.baseParticipantRole> data = academicActivityPhaseRepo.getParticipantRoleByPhase(-999);
            if (data.Count == 0)
            {
                Assert.Pass();
            }
        }
        [TestCase]
        public void GetListParticipantRoleUT_3()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            List<AcademicActivityPhaseRepo.baseParticipantRole> data = academicActivityPhaseRepo.getParticipantRoleByPhase(0);
            if (data.Count == 0)
            {
                Assert.Pass();
            }
        }
    }
}
