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
    public class AddParticipantToPhase
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void AddParticipantToPhaseUT_1()
        {
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase academicActivityPhase = db.AcademicActivityPhases.Last();
            CheckInRepo repo = new CheckInRepo();
            CheckInRepo.infoParticipant obj = new CheckInRepo.infoParticipant
            {
                name = "duong",
                participant_role_id = 101,
                email = "aa@gmail.com",
                participant_number = "12328181",
                office_id = 1
            };
            bool res = repo.addParticipant(obj);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void AddParticipantToPhaseUT_2()
        {
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase academicActivityPhase = db.AcademicActivityPhases.Last();
            CheckInRepo repo = new CheckInRepo();
            CheckInRepo.infoParticipant obj = new CheckInRepo.infoParticipant
            {
                name = "!@#!@",
                participant_role_id = 102,
                email = "duongpl@gmail.com",
                participant_number = "",
                office_id = 0
            };
            bool res = repo.addParticipant(obj);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void AddParticipantToPhaseUT_3()
        {
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase academicActivityPhase = db.AcademicActivityPhases.Last();
            CheckInRepo repo = new CheckInRepo();
            CheckInRepo.infoParticipant obj = new CheckInRepo.infoParticipant
            {
                name = "",
                participant_role_id = 102,
                email = "",
                participant_number = "0128dasda1",
                office_id = 0
            };
            bool res = repo.addParticipant(obj);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void AddParticipantToPhaseUT_4()
        {
            new AddAcademicActivityPhaseUT().AddAcademicActivityPhaseUT_1();
            AcademicActivityPhase academicActivityPhase = db.AcademicActivityPhases.Last();
            CheckInRepo repo = new CheckInRepo();
            CheckInRepo.infoParticipant obj = new CheckInRepo.infoParticipant
            {
                name = "duong",
                participant_role_id = 101,
                email = "duongpl@gmail.com",
                participant_number = "12328181",
                office_id = 2
            };
            bool res = repo.addParticipant(obj);
            if (res)
                Assert.Pass();
        }
    }
}
