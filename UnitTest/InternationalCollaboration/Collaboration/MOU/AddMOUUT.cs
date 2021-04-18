using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.InternationalCollaboration.Collaboration
{
    [TestFixture]
    public class AddMOUUT
    {
        [TestCase]
        public void TestAddMOU1()
        {
            //Arrange
            string mou_code = "2020/101";
            MOURepo mou = new MOURepo();

            if (isAdded(mou_code))
            {
                Assert.Inconclusive("This MOU has been added");
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
                MOUAdd input = new MOUAdd
                {
                    BasicInfo = new BasicInfo
                    {
                        mou_code = mou_code,
                        mou_end_date = "20/05/2025",
                        mou_note = "TestAddMOU1",
                        office_id = 1,
                        mou_status_id = 2,
                        reason = "TestAddMOU1"
                    },
                    PartnerInfo = new List<PartnerInfo>()
                };
                input.PartnerInfo.Add(
                    new PartnerInfo
                    {
                        partner_id = 1,
                        represent_add = "TestAddMOU1",
                        phone_add = "0123456789",
                        email_add = "TestAddMOU1",
                        coop_scope_add = new List<int>() { 3, 5 },
                        specialization_add = new List<int>() { 3 },
                        sign_date_mou_add = "20/05/2020"
                    }
                );

                //Act
                //mou.addMOU(input, user);

                //Assert
                Assert.Pass();
            }
        }
        [TestCase]
        public void TestAddMOU2()
        {
            //Arrange
            string mou_code = "2020/102";
            MOURepo mou = new MOURepo();
            if (isAdded(mou_code))
            {
                Assert.Inconclusive("This MOU has been added");
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
                //Add MOU with 1 old partner + 1 new partner.
                MOUAdd input = new MOUAdd
                {
                    BasicInfo = new BasicInfo
                    {
                        mou_code = mou_code,
                        mou_end_date = "20/05/2025",
                        mou_note = "TestAddMOU2",
                        office_id = 2,
                        mou_status_id = 2,
                        reason = "TestAddMOU2"
                    },
                    PartnerInfo = new List<PartnerInfo>()
                };
                input.PartnerInfo.AddRange(
                    new List<PartnerInfo>
                    {
                        new PartnerInfo
                        {
                            partner_id = 1,
                            represent_add = "TestAddMOU2",
                            phone_add = "0123456789",
                            email_add = "TestAddMOU2",
                            coop_scope_add = new List<int>() { 3, 5 },
                            specialization_add = new List<int>() { 3 },
                            sign_date_mou_add = "20/05/2020"
                        },
                        new PartnerInfo
                        {
                            partnername_add = "New Partner",
                            partner_id = 0,
                            website_add = "new.com",
                            nation_add = 1,
                            address_add = "TestAddMOU2",
                            represent_add = "TestAddMOU2",
                            phone_add = "0123456789",
                            email_add = "TestAddMOU2",
                            coop_scope_add = new List<int>() { 1, 2 },
                            specialization_add = new List<int>() { 1 },
                            sign_date_mou_add = "20/05/2020"
                        }
                    }
                );

                //Act
                //mou.addMOU(input, user);

                //Assert
                Assert.Pass();
            }
        }
        [TestCase]
        public void TestAddMOU3()
        {
            //Arrange
            MOURepo mou = new MOURepo();
            string mou_code = "2020/103";
            if (isAdded(mou_code))
            {
                Assert.Inconclusive("This MOU has been added");
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
                        mou_end_date = "20/05/2025",
                        mou_note = "TestAddMOU3",
                        office_id = 2,
                        mou_status_id = 2,
                        reason = "TestAddMOU3"
                    },
                    PartnerInfo = new List<PartnerInfo>()
                };
                input.PartnerInfo.AddRange(
                    new List<PartnerInfo>
                    {
                        new PartnerInfo
                        {
                            partnername_add = "New Partner 2",
                            partner_id = 0,
                            website_add = "new.com",
                            nation_add = 1,
                            address_add = "TestAddMOU3",
                            represent_add = "TestAddMOU3",
                            phone_add = "0123456789",
                            email_add = "TestAddMOU3",
                            coop_scope_add = new List<int>() { 1, 2 },
                            specialization_add = new List<int>() { 1 },
                            sign_date_mou_add = "20/04/2020"
                        }
                    }
                );
                //Act
                //mou.addMOU(input, user);

                //Assert
                Assert.Pass();
            }
        }
        //for testing MOUCodeCheck
        [TestCase]
        public void TestAddMOU4()
        {
            //Arrange
            string mou_code = "2020/104";
            MOURepo mou = new MOURepo();

            if (isAdded(mou_code))
            {
                Assert.Inconclusive("This MOU has been added");
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
                MOUAdd input = new MOUAdd
                {
                    BasicInfo = new BasicInfo
                    {
                        mou_code = mou_code,
                        mou_end_date = "20/05/2025",
                        mou_note = "TestAddMOU4",
                        office_id = 1,
                        mou_status_id = 2,
                        reason = "TestAddMOU4"
                    },
                    PartnerInfo = new List<PartnerInfo>()
                };
                input.PartnerInfo.Add(
                    new PartnerInfo
                    {
                        partner_id = 4,
                        represent_add = "TestAddMOU4",
                        phone_add = "0123456789",
                        email_add = "TestAddMOU4",
                        coop_scope_add = new List<int>() { 3, 5 },
                        specialization_add = new List<int>() { 3 },
                        sign_date_mou_add = "20/05/2020"
                    }
                );

                //Act
                //mou.addMOU(input, user);

                //Assert
                Assert.Pass();
            }
        }
        public bool isAdded(string mou_code)
        {
            return new ScienceAndInternationalAffairsEntities().MOUs.ToList().Any(x => !x.is_deleted && x.mou_code.Equals(mou_code));
        }
    }
}
