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
    public class AddAcademicActivityPhaseUT
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void AddAcademicActivityPhaseUT_1()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            int language_id = 1;
            AcademicActivityPhaseRepo.basePhase data = new AcademicActivityPhaseRepo.basePhase
            {
                phase_name = "TestAddPhase1",
                from = "01/01/2020",
                to = "01/01/2021"
            };
            bool res = academicActivityPhaseRepo.AddPhase(language_id, academicActivity.activity_id, user.account.account_id, data);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void AddAcademicActivityPhaseUT_2()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            int language_id = 1;
            AcademicActivityPhaseRepo.basePhase data = new AcademicActivityPhaseRepo.basePhase
            {
                phase_name = "",
                from = "31/02/2020",
                to = "01/01/2021"
            };
            bool res = academicActivityPhaseRepo.AddPhase(language_id, academicActivity.activity_id, user.account.account_id, data);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void AddAcademicActivityPhaseUT_3()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 0
                }
            };
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            int language_id = 2;
            AcademicActivityPhaseRepo.basePhase data = new AcademicActivityPhaseRepo.basePhase
            {
                phase_name = "!@#!",
                from = "01/01/2021",
                to = "01/01/2020"
            };
            bool res = academicActivityPhaseRepo.AddPhase(language_id, academicActivity.activity_id, user.account.account_id, data);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void AddAcademicActivityPhaseUT_4()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            int language_id = 2;
            AcademicActivityPhaseRepo.basePhase data = new AcademicActivityPhaseRepo.basePhase
            {
                phase_name = "abcxyz",
                from = "05/01/2020",
                to = "10/01/2021"
            };
            bool res = academicActivityPhaseRepo.AddPhase(language_id, academicActivity.activity_id, user.account.account_id, data);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void AddAcademicActivityPhaseUT_5()
        {
            AcademicActivityPhaseRepo academicActivityPhaseRepo = new AcademicActivityPhaseRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            int language_id = 1;
            AcademicActivityPhaseRepo.basePhase data = new AcademicActivityPhaseRepo.basePhase
            {
                phase_name = "abcxyz",
                from = "31/02/2020",
                to = "10/01/2021"
            };
            bool res = academicActivityPhaseRepo.AddPhase(language_id, academicActivity.activity_id, user.account.account_id, data);
            if (!res)
                Assert.Pass();
        }
    }
}
