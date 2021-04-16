using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.InternationalCollaboration.Collaboration.MOU
{
    [TestFixture]
    public class CheckPartnerUT
    {
        [TestCase]
        public void TestCheckPartner1()
        {
            //Arrange
            string name = "Deakin University";
            int country_id_expected = 13;
            MOURepo mou = new MOURepo();

            //Act
            CustomPartner item = mou.CheckPartner(name);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(name, item.partner_name);
                Assert.AreEqual(country_id_expected, item.country_id);
            });
        }
        [TestCase]
        public void TestCheckPartner2()
        {
            //Arrange
            string name = "Deakin University A";
            MOURepo mou = new MOURepo();

            //Act
            CustomPartner item = mou.CheckPartner(name);

            //Assert
            Assert.IsNull(item);
        }
    }
}
