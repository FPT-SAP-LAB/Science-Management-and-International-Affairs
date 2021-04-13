using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTest.InternationalCollaboration.Collaboration
{
    [TestFixture]
    public class AddMOUUnitTest
    {
        [TestCase]
        public void TestAddMOU1()
        {
            //Prepare data:
            MOURepo mou = new MOURepo();
            BLL.Authen.LoginRepo.User user = new BLL.Authen.LoginRepo.User
            {
                account = new ENTITIES.Account
                {
                    account_id = 16
                }
            };
            //Add MOU with an old partner.
            MOUAdd input = new MOUAdd
            {
                BasicInfo = new BasicInfo
                {
                    mou_code = "2020/99",
                    mou_end_date = "TestAddMOU1",
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
                    specialization_add = new List<int>() { 3 }
                }
            );
            mou.addMOU(input, user);
            Assert.Pass();
        }
    }
}
