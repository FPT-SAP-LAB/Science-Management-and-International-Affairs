using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
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
    public class CheckIntersectDateUT
    {
        //Pre-condition: TestAddMOU() - Integration Test
        [TestCase]
        public void TestIntersectDate1()
        {
            ////Arrange
            //List<PartnerInfo> listPartner = new List<PartnerInfo> 
            //{
            //    new PartnerInfo
            //    {
            //        partner_id = 1
            //    },
            //    new PartnerInfo
            //    {
            //        partner_id = 2
            //    }
            //};
            //string start_date = "01/01/1900";
            //string end_date = "01/01/1900";
            //int office_id = 1;
            //MOURepo mou = new MOURepo();

            ////Act
            //IntersectPeriodMOUDate item = mou.checkIntersectPeriodMOUDate(listPartner,start_date,end_date,office_id);

            ////Assert
            //Assert.IsNull(item);
            Assert.Fail();
        }
        //Pre-condition: TestAddMOU() - Integration Test
        [TestCase]
        public void TestIntersectDate2()
        {
            ////Arrange
            //List<PartnerInfo> listPartner = new List<PartnerInfo>
            //{
            //    new PartnerInfo
            //    {
            //        partner_id = 1
            //    }
            //};
            //string start_date = "01/01/1900";
            //string end_date = "01/01/1900";
            //int office_id = 1;
            //MOURepo mou = new MOURepo();

            ////Act
            //IntersectPeriodMOUDate item = mou.checkIntersectPeriodMOUDate(listPartner, start_date, end_date, office_id);

            ////Assert
            //Assert.IsNull(item);
            Assert.Fail();
        }

        [TestCase("20/05/2020", "20/05/2025", "20/05/2019", "20/05/2026")]
        [TestCase("20/05/2020", "20/05/2025", "20/05/2019", "20/05/2024")]
        [TestCase("20/05/2020", "20/05/2025", "20/05/2021", "20/05/2026")]
        [TestCase("20/05/2020", "20/05/2025", "20/05/2021", "20/05/2024")]
        public void TestDateRangeisInvalid1(string start, string end,string test_start,string test_end)
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
    }
}
