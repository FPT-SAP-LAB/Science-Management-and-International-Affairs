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
    public class GetNotiUT
    {
        [TestCase]
        public void TestGetNoti()
        {
            //Act
            MOURepo mou = new MOURepo();
            NotificationInfo item = mou.getNoti();

            //Assert
            Assert.GreaterOrEqual(item.InactiveNumber, 0);
        }
    }
}
