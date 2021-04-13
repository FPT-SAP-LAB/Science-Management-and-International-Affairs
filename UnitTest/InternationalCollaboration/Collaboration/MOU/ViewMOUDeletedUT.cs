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
    public class ViewMOUDeletedUT
    {
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
            List<ListMOU> listTest = mou.listAllMOUDeleted(baseDatatable, "", "", "").Data;
            Assert.AreEqual(10, listTest.Count);
        }
        [TestCase]
        public void TestSearchResultsMOUDeleted1()
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
            List<ListMOU> listTest = mou.listAllMOUDeleted(baseDatatable, partner_name, contact_point_name, mou_code).Data;
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
        public void TestSearchResultsMOUDeleted2()
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
            List<ListMOU> listTest = mou.listAllMOUDeleted(baseDatatable, partner_name, contact_point_name, mou_code).Data;
            foreach (ListMOU item in listTest)
            {
                if (!(item.partner_name.Contains(partner_name) && item.contact_point_name.Contains(contact_point_name)))
                {
                    Assert.Fail();
                }
            }
            Assert.Pass();
        }
        [TestCase]
        public void TestSearchResultsMOUDeleted3()
        {
            string partner_name = "James Cook University";
            string contact_point_name = "KEVIN ANDERSON";
            string mou_code = "2018/1";
            //Check result of searching.
            MOURepo mou = new MOURepo();
            BaseDatatable baseDatatable = new BaseDatatable
            {
                SortColumnName = "mou_partner_id",
                SortDirection = "desc",
                Start = 0,
                Length = 20
            };
            List<ListMOU> listTest = mou.listAllMOUDeleted(baseDatatable, partner_name, contact_point_name, mou_code).Data;
            foreach (ListMOU item in listTest)
            {
                if (!(item.partner_name.Contains(partner_name) && item.contact_point_name.Contains(contact_point_name)
                    && item.mou_code.Contains(mou_code)))
                {
                    Assert.Fail();
                }
            }
            Assert.Pass();
        }
        [TestCase]
        public void TestSearchResultsMOUDeleted4()
        {
            string partner_name = "James Cook University";
            string contact_point_name = "KEVIN ANDERSON";
            string mou_code = "2020/69";
            //Check result of searching.
            MOURepo mou = new MOURepo();
            BaseDatatable baseDatatable = new BaseDatatable
            {
                SortColumnName = "mou_partner_id",
                SortDirection = "desc",
                Start = 0,
                Length = 20
            };
            List<ListMOU> listTest = mou.listAllMOUDeleted(baseDatatable, partner_name, contact_point_name, mou_code).Data;
            foreach (ListMOU item in listTest)
            {
                if (!(item.partner_name.Contains(partner_name) && item.contact_point_name.Contains(contact_point_name)
                    && item.mou_code.Contains(mou_code)))
                {
                    Assert.Fail();
                }
            }
            Assert.Pass();
        }
    }
}
