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
    public class CloneAcademicActivityUT
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void CloneAcademicActivityUT_1()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.cloneBase data = new AcademicActivityRepo.cloneBase
            {
                id = academicActivity.activity_id,
                content = new List<string> { "KP", "ND" },
                activity_name = "TestAddAA1",
                activity_type_id = 1,
                location = "TestAddAA1",
                from = "01/01/2021",
                to = "01/01/2021",
                language_id = 1,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.Clone(data, user.account.account_id);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void CloneAcademicActivityUT_2()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.cloneBase data = new AcademicActivityRepo.cloneBase
            {
                id = academicActivity.activity_id,
                content = new List<string>(),
                activity_name = "TestCloneAA2",
                activity_type_id = 2,
                location = "TestAddAA2",
                from = "01/01/2020",
                to = "01/01/2021",
                language_id = 2,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.Clone(data, user.account.account_id);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void CloneAcademicActivityUT_3()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.cloneBase data = new AcademicActivityRepo.cloneBase
            {
                id = academicActivity.activity_id,
                content = new List<string>(),
                activity_name = "TestCloneAA3",
                activity_type_id = 3,
                location = "TestAddAA3",
                from = "28/02/2020",
                to = "30/04/2021",
                language_id = 2,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.Clone(data, user.account.account_id);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void CloneAcademicActivityUT_4()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.cloneBase data = new AcademicActivityRepo.cloneBase
            {
                id = academicActivity.activity_id,
                content = new List<string>(),
                activity_name = "TestCloneAA4",
                activity_type_id = 5,
                location = "TestAddAA4",
                from = "01/01/2020",
                to = "01/01/2021",
                language_id = 1,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.Clone(data, user.account.account_id);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void CloneAcademicActivityUT_5()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 0
                }
            };
            AcademicActivityRepo.cloneBase data = new AcademicActivityRepo.cloneBase
            {
                id = academicActivity.activity_id,
                content = new List<string>(),
                activity_name = "TestCloneAA5",
                activity_type_id = 3,
                location = "TestAddAA5",
                from = "01/01/2020",
                to = "01/01/2021",
                language_id = 2,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.Clone(data, user.account.account_id);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void CloneAcademicActivityUT_6()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.cloneBase data = new AcademicActivityRepo.cloneBase
            {
                id = academicActivity.activity_id,
                content = new List<string>(),
                activity_name = "TestCloneAA6",
                activity_type_id = 2,
                location = "TestAddAA6",
                from = "01/01/2020",
                to = "31/02/2021",
                language_id = 2,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.Clone(data, user.account.account_id);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void CloneAcademicActivityUT_7()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.cloneBase data = new AcademicActivityRepo.cloneBase
            {
                id = academicActivity.activity_id,
                content = new List<string>(),
                activity_name = "TestCloneAA7",
                activity_type_id = 1,
                location = "TestAddAA7",
                from = "01/01/2020",
                to = "01/01/2021",
                language_id = 3,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.Clone(data, user.account.account_id);
            if (!res)
                Assert.Pass();
        }
    }
}
