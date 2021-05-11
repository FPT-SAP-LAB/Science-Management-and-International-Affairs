using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace UnitTest.InternationalCollaboration.Collaboration.MOU
{
    [TestFixture]
    public class GetMOUCodeCheckUT
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        //Pre-condition: TestAddMOU(), TestDeleteMOU() - Integration Test
        [TestCase]
        public void TestAddMOU4_TestMOUCodeExisted1()
        {
            //Arrange
            new AddMOUUT().TestAddMOU4();
            MOURepo mou = new MOURepo();
            string mou_code = "2020/104";

            //Act
            bool isExisted = mou.GetMOUCodeCheck(mou_code);

            //Assert
            Assert.IsTrue(isExisted);
        }
        [TestCase]
        public void TestAddMOU1_TestMOUCodeExisted2()
        {
            //Arrange
            new DeleteMOUUnitTest().TestAddMOU1_TestDeleteMOU();
            MOURepo mou = new MOURepo();
            string mou_code = "2020/101";

            //Act
            bool isExisted = mou.GetMOUCodeCheck(mou_code);

            //Assert
            Assert.IsFalse(isExisted);
        }
    }
}
