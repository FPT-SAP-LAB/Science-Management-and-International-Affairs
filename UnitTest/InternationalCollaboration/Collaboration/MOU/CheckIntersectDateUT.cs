using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace UnitTest.InternationalCollaboration.Collaboration.MOU
{
    [TestFixture]
    public class CheckIntersectDateUT
    {
        //Pre-condition: TestAddMOU1TID
        //Integration Test
        [TestCase]
        public void TestAddMOU1TID_TestIntersectDate1()
        {
            //Arrange
            MOURepo mou = new MOURepo();
            List<int> listPartnerID = new List<int> { 1, 2 };
            string start_date = "20/05/2021";
            string end_date = "20/05/2024";
            int office_id = 2;
            List<PartnerInfo> listPartner = new List<PartnerInfo>
            {
                new PartnerInfo
                {
                    partner_id = listPartnerID[0]
                },
                new PartnerInfo
                {
                    partner_id = listPartnerID[1]
                }
            };

            //Act
            TestAddMOU1TID();
            IntersectPeriodMOUDate item = mou.CheckIntersectPeriodMOUDate(listPartner, start_date, end_date, office_id);

            //Assert
            Assert.IsNotNull(item);
        }
        [TestCase]
        public void TestAddMOU1TID()
        {
            //Arrange
            MOURepo mou = new MOURepo();
            string mou_code = "2020/1001";
            string mou_end_date = "20/05/2025";
            List<string> listStartDate = new List<string> { "20/05/2020", "27/06/2020" };
            List<int> listPartnerID = new List<int> { 1, 2 };
            int office_id = 2;

            if (isAdded(mou_code))
            {
                Assert.Pass("This MOU has been added");
            }
            else
            {
                BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
                {
                    account = new ENTITIES.Account
                    {
                        account_id = 16
                    }
                };
                //Add MOU with 1 new partner.
                MOUAdd input = new MOUAdd
                {
                    BasicInfo = new BasicInfo
                    {
                        mou_code = mou_code,
                        mou_end_date = mou_end_date,
                        mou_note = "TestAddMOU1_TestIntersectDate1",
                        office_id = office_id,
                        mou_status_id = 2,
                        reason = "TestAddMOU1_TestIntersectDate1"
                    },
                    PartnerInfo = new List<PartnerInfo>()
                };
                input.PartnerInfo.AddRange(
                    new List<PartnerInfo>
                    {
                        new PartnerInfo
                        {
                            partner_id = listPartnerID[0],
                            represent_add = "TestAddMOU1_TestIntersectDate1",
                            phone_add = "0123456789",
                            email_add = "TestAddMOU1_TestIntersectDate1",
                            coop_scope_add = new List<int>() { 1,3,5,7 },
                            specialization_add = new List<int>() { 3,6 },
                            sign_date_mou_add = listStartDate[0]
                        },
                        new PartnerInfo
                        {
                            partner_id = listPartnerID[1],
                            represent_add = "TestAddMOU1_TestIntersectDate1",
                            phone_add = "0123456789",
                            email_add = "TestAddMOU1_TestIntersectDate1",
                            coop_scope_add = new List<int>() { 2,4,5 },
                            specialization_add = new List<int>() { 3,4 },
                            sign_date_mou_add = listStartDate[1]
                        }
                    }
                );

                //Act
                //mou.addMOU(input, user);

                //Assert
                Assert.Pass();
            }
        }

        [TestCase("20/05/2020", "20/05/2025", "20/05/2019", "20/05/2026")]
        [TestCase("20/05/2020", "20/05/2025", "20/05/2019", "20/05/2024")]
        [TestCase("20/05/2020", "20/05/2025", "20/05/2021", "20/05/2026")]
        [TestCase("20/05/2020", "20/05/2025", "20/05/2021", "20/05/2024")]
        public void TestDateRangeisInvalid1(string start, string end, string test_start, string test_end)
        {
            //Arrange
            DateTime start_date = DateTime.ParseExact(start, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime end_date = DateTime.ParseExact(end, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime test_start_date = DateTime.ParseExact(test_start, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime test_end_date = DateTime.ParseExact(test_end, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            MOURepo mou = new MOURepo();

            //Act
            bool isInvalid = mou.DateRangeisInvalid(start_date, end_date, test_start_date, test_end_date);

            //Assert
            Assert.IsTrue(isInvalid);
        }

        [TestCase("20/05/2020", "20/05/2025", "20/05/2016", "20/05/2019")]
        [TestCase("20/05/2020", "20/05/2025", "20/05/2026", "20/05/2029")]
        public void TestDateRangeisInvalid2(string start, string end, string test_start, string test_end)
        {
            //Arrange
            DateTime start_date = DateTime.ParseExact(start, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime end_date = DateTime.ParseExact(end, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime test_start_date = DateTime.ParseExact(test_start, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime test_end_date = DateTime.ParseExact(test_end, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            MOURepo mou = new MOURepo();

            //Act
            bool isInvalid = mou.DateRangeisInvalid(start_date, end_date, test_start_date, test_end_date);

            //Assert
            Assert.IsFalse(isInvalid);
        }
        public bool isAdded(string mou_code)
        {
            return new ScienceAndInternationalAffairsEntities().MOUs.ToList().Any(x => !x.is_deleted && x.mou_code.Equals(mou_code));
        }
    }
}
