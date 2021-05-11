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
    public class DeleteMOUUnitTest
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        //Pre-condition: TestAddMOU1() - Integration Test
        [TestCase]
        public void TestAddMOU1_TestDeleteMOU()
        {
            //Arrange
            new AddMOUUT().TestAddMOU1();
            MOURepo mou = new MOURepo();
            int mou_id = db.MOUs.Where(x => !x.is_deleted && x.mou_code == "2020/101").First().mou_id;

            if (db.MOUs.Any(x => x.mou_id == mou_id && x.is_deleted))
            {
                //Assert
                Assert.Pass("Is deleted before");
            }
            else
            {
                //Act
                mou.DeleteMOU(mou_id);

                //Assert
                Assert.Pass();
            }
        }
    }
}
