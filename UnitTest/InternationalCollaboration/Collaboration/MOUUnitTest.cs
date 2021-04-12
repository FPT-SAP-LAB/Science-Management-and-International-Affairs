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
    public class MOUUnitTest
    {
        //[TestCase]
        //public void TestMethod1()
        //{
        //    for (int i=0; i < 5; i++)
        //    {
        //        if (i == 6)
        //        {
        //            Assert.Fail("This is a fail message");
        //        }
        //    }
        //    Assert.Pass();
        //}
        [TestCase]
        public void TestListCountMOU()
        {
            //Check list count
            MOURepo mou = new MOURepo();
            BaseDatatable baseDatatable = new BaseDatatable
            {
                SortColumnName = "mou_partner_id",
                SortDirection = "desc",
                Start = 0,
                Length = 10
            };
            List<ListMOU> listTest = mou.listAllMOU(baseDatatable, "", "", "").Data;
            Assert.AreEqual(10, listTest.Count);
        }
        [TestCase]
        public void TestSearchResultsMOU1()
        {
            string partner_name = "Daekin University";
            string contact_point_name = "";
            string mou_code = "";
            //Check result of searching.
            MOURepo mou = new MOURepo();
            BaseDatatable baseDatatable = new BaseDatatable
            {
                SortColumnName = "mou_partner_id",
                SortDirection = "desc",
                Start = 0,
                Length = 20
            };
            List<ListMOU> listTest = mou.listAllMOU(baseDatatable, partner_name, contact_point_name, mou_code).Data;
            foreach (ListMOU item in listTest)
            {
                if (!item.partner_name.Contains(partner_name))
                {
                    Assert.Fail();
                }
            }
            Assert.Pass();
        }
        [TestCase]
        public void TestSearchResultsMOU2()
        {
            string partner_name = "James Cook University";
            string contact_point_name = "KEVIN ANDERSON";
            string mou_code = "";
            //Check result of searching.
            MOURepo mou = new MOURepo();
            BaseDatatable baseDatatable = new BaseDatatable
            {
                SortColumnName = "mou_partner_id",
                SortDirection = "desc",
                Start = 0,
                Length = 20
            };
            List<ListMOU> listTest = mou.listAllMOU(baseDatatable, partner_name, contact_point_name, mou_code).Data;
            foreach (ListMOU item in listTest)
            {
                if (!(item.partner_name.Contains(partner_name) && item.contact_point_name.Contains(contact_point_name)))
                {
                    Assert.Fail();
                }
            }
            Assert.Pass();
        }
    }
}
