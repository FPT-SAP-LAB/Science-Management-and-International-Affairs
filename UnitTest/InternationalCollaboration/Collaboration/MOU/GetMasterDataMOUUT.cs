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
    public class GetMasterDataMOUUT
    {
        [TestCase]
        public void TestGetOffice()
        {
            //Act
            MOURepo mou = new MOURepo();
            List<CustomOffice> list = mou.GetOffice();

            //Assert
            Assert.Greater(list.Count, 0);
        }
        [TestCase]
        public void TestGetPartners()
        {
            //Act
            MOURepo mou = new MOURepo();
            List<ENTITIES.Partner> list = mou.GetPartners();

            //Assert
            Assert.Greater(list.Count, 0);
        }
        [TestCase]
        public void TestGetSpecializations()
        {
            //Act
            MOURepo mou = new MOURepo();
            List<Specialization> list = mou.GetSpecializations();

            //Assert
            Assert.Greater(list.Count, 0);
        }
        [TestCase]
        public void TestGetCountries()
        {
            //Act
            MOURepo mou = new MOURepo();
            List<Country> list = mou.GetCountries();

            //Assert
            Assert.Greater(list.Count, 0);
        }
        [TestCase]
        public void TestGetCollaborationScopes()
        {
            //Act
            MOURepo mou = new MOURepo();
            List<CollaborationScope> list = mou.GetCollaborationScopes();

            //Assert
            Assert.Greater(list.Count, 0);
        }
    }
}
