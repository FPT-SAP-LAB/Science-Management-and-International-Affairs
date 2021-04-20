using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;
using ENTITIES.CustomModels;
using BLL.InternationalCollaboration.AcademicActivity;

namespace UnitTest.InternationalCollaboration.AcademicActivity
{
    [TestFixture]
    public class AddAcademicActivityUT
    {
        [TestCase]
        public void TestAddAcademicActivity_1()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
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
            activityRepo.AddAA(baseAA, 1, user);
            Assert.Pass();
        }
        [TestCase]
        public void TestAddAcademicActivity_2()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestAddAA2",
                activity_type_id = 5,
                location = "TestAddAA2",
                from = "01/01/2021",
                to = "01/01/2021",
                language_id = 2,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.AddAA(baseAA, 2, user);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void TestAddAcademicActivity_3()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestAddAA3",
                activity_type_id = 2,
                location = "TestAddAA3",
                from = "01/01/2021",
                to = "01/01/2020",
                language_id = 1,
                file_id = 1073,
                file_drive_id = "1BrSpGL63zR_qtp3yGbkzJnvRb01LnH4G",
                file_link = "https://drive.google.com/file/d/1BrSpGL63zR_qtp3yGbkzJnvRb01LnH4G/view?usp=drivesdk",
                file_name = "IS1307_SSC101_Assignment 1_Nguyenttthe130020.pdf"
            };
            activityRepo.AddAA(baseAA, 1, user);
            Assert.Pass();
        }
        [TestCase]
        public void TestAddAcademicActivity_4()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestAddAA4",
                activity_type_id = 3,
                location = "TestAddAA4",
                from = "05/01/2020",
                to = "10/01/2021",
                language_id = 1,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            activityRepo.AddAA(baseAA, 2, user);
            Assert.Pass();
        }
        [TestCase]
        public void TestAddAcademicActivity_5()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestAddAA5",
                activity_type_id = 4,
                location = "TestAddAA5",
                from = "05/01/2020",
                to = "10/01/2021",
                language_id = 2,
                file_id = 1074,
                file_drive_id = "1TPOMZBxGVuB5v_mq17BZzMcMRKvBvJ_t",
                file_link = "https://drive.google.com/file/d/1TPOMZBxGVuB5v_mq17BZzMcMRKvBvJ_t/view?usp=drivesdk",
                file_name = "IS1307_SSC101_Assignment 1_Nguyenttthe130020.pdf"
            };
            activityRepo.AddAA(baseAA, 2, user);
            Assert.Pass();
        }
        [TestCase]
        public void TestAddAcademicActivity_6()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 0
                }
            };
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestAddAA6",
                activity_type_id = 1,
                location = "TestAddAA6",
                from = "05/01/2020",
                to = "10/01/2021",
                language_id = 1,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.AddAA(baseAA, 1, user);
            if(!res) Assert.Pass();
        }
        [TestCase]
        public void TestAddAcademicActivity_7()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestAddAA7",
                activity_type_id = 1,
                location = "TestAddAA7",
                from = "31/02/2020",
                to = "10/01/2021",
                language_id = 1,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.AddAA(baseAA, 1, user);
            if (!res) Assert.Pass();
        }
        [TestCase]
        public void TestAddAcademicActivity_8()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestAddAA8",
                activity_type_id = 1,
                location = "TestAddAA8",
                from = "28/02/2020",
                to = "10/01/2021",
                language_id = 1,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            activityRepo.AddAA(baseAA, 1, user);
            Assert.Pass();
        }
    }
}
