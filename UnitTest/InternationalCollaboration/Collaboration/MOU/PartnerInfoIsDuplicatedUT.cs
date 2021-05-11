using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace UnitTest.InternationalCollaboration.Collaboration.MOU
{
    [TestFixture]
    public class PartnerInfoIsDuplicatedUT
    {
        //Pre-condition: TestAddMOU(), TestDeleteMOU() - Integration Test
        [TestCase]
        public void TestDuplicatedPartner()
        {
            //Arrange
            new AddMOUUT().TestAddMOU4();
            MOURepo mou = new MOURepo();
            string partner_name = "Queensland University of Technology";
            string mou_start_date = "20/05/2020";

            //Act
            DuplicatePartnerInfo item = mou.PartnerInfoIsDuplicated(partner_name, mou_start_date);

            //Assert
            Assert.IsNotNull(item);
        }
    }
}
