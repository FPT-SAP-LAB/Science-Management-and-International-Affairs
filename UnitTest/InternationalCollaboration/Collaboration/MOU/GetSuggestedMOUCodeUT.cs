using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace UnitTest.InternationalCollaboration.Collaboration.MOU
{
    [TestFixture]
    public class GetSuggestedMOUCodeUT
    {
        [TestCase]
        public void TestSuggestedMOUCode()
        {
            //Arrange
            MOURepo mou = new MOURepo();
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

            //Act
            string mou_code = mou.getSuggestedMOUCode();
            bool isExisted = db.MOUs.Any(x => x.mou_code.Equals(mou_code));

            //Assert
            Assert.IsFalse(isExisted);
        }
    }
}
