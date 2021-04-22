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
    public class UpdateAcademicActivityUT
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void UpdateAcademicActivityUT_1()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestUpdateAA1",
                activity_type_id = 1,
                location = "TestUpdateAA1",
                from = "01/01/2021",
                to = "01/01/2021",
                language_id = 1,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.updateBaseAAA(academicActivity.activity_id,
                baseAA.activity_type_id, baseAA.activity_name, baseAA.location,
                baseAA.from, baseAA.to, baseAA.language_id, null, user);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityUT_2()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestUpdateAA2",
                activity_type_id = 4,
                location = "TestUpdateAA2",
                from = "31/01/2020",
                to = "01/02/2021",
                language_id = 2,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.updateBaseAAA(academicActivity.activity_id,
                baseAA.activity_type_id, baseAA.activity_name, baseAA.location,
                baseAA.from, baseAA.to, baseAA.language_id, null, user);
            if (res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityUT_3()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestUpdateAA3",
                activity_type_id = 5,
                location = "TestUpdateAA3",
                from = "01/01/2020",
                to = "01/01/2021",
                language_id = 2,
                file_id = 1073,
                file_drive_id = "1BrSpGL63zR_qtp3yGbkzJnvRb01LnH4G",
                file_link = "https://drive.google.com/file/d/1BrSpGL63zR_qtp3yGbkzJnvRb01LnH4G/view?usp=drivesdk",
                file_name = "IS1307_SSC101_Assignment 1_Nguyenttthe130020.pdf"
            };
            bool res = activityRepo.updateBaseAAA(academicActivity.activity_id,
                baseAA.activity_type_id, baseAA.activity_name, baseAA.location,
                baseAA.from, baseAA.to, baseAA.language_id, null, user);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityUT_4()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 0
                }
            };
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestUpdateAA4",
                activity_type_id = 2,
                location = "TestUpdateAA4",
                from = "01/01/2020",
                to = "01/01/2021",
                language_id = 1,
                file_id = 1073,
                file_drive_id = "1BrSpGL63zR_qtp3yGbkzJnvRb01LnH4G",
                file_link = "https://drive.google.com/file/d/1BrSpGL63zR_qtp3yGbkzJnvRb01LnH4G/view?usp=drivesdk",
                file_name = "IS1307_SSC101_Assignment 1_Nguyenttthe130020.pdf"
            };
            bool res = activityRepo.updateBaseAAA(academicActivity.activity_id,
                baseAA.activity_type_id, baseAA.activity_name, baseAA.location,
                baseAA.from, baseAA.to, baseAA.language_id, null, user);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityUT_5()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 0
                }
            };
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestUpdateAA5",
                activity_type_id = 3,
                location = "TestUpdateAA5",
                from = "31/02/2020",
                to = "01/01/2021",
                language_id = 2,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.updateBaseAAA(academicActivity.activity_id,
                baseAA.activity_type_id, baseAA.activity_name, baseAA.location,
                baseAA.from, baseAA.to, baseAA.language_id, null, user);
            if (!res)
                Assert.Pass();
        }
        [TestCase]
        public void UpdateAcademicActivityUT_6()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            new AddAcademicActivityUT().TestAddAcademicActivity_1();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 0
                }
            };
            ENTITIES.AcademicActivity academicActivity = db.AcademicActivities.Last();
            AcademicActivityRepo.baseAA baseAA = new AcademicActivityRepo.baseAA
            {
                activity_name = "TestUpdateAA6",
                activity_type_id = 3,
                location = "TestUpdateAA6",
                from = "01/01/2020",
                to = "01/01/2021",
                language_id = 4,
                file_id = null,
                file_drive_id = "",
                file_link = "",
                file_name = ""
            };
            bool res = activityRepo.updateBaseAAA(academicActivity.activity_id,
                baseAA.activity_type_id, baseAA.activity_name, baseAA.location,
                baseAA.from, baseAA.to, baseAA.language_id, null, user);
            if (!res)
                Assert.Pass();
        }
    }
}
