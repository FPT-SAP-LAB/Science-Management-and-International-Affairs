using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTest.InternationalCollaboration.Collaboration
{
    [TestFixture]
    public class ViewMOUUnitTest
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
        [TestCase]
        public void TestSearchResultsMOU3()
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
            List<ListMOU> listTest = mou.listAllMOU(baseDatatable, partner_name, contact_point_name, mou_code).Data;
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
        public void TestSearchResultsMOU4()
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
            List<ListMOU> listTest = mou.listAllMOU(baseDatatable, partner_name, contact_point_name, mou_code).Data;
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
        public void TestListMOUToExportExcel()
        {
            //Check list count
            MOURepo mou = new MOURepo();
            BaseDatatable baseDatatable = new BaseDatatable
            {
                SortColumnName = "mou_partner_id",
                SortDirection = "desc",
                Start = 0,
                Length = 100
            };
            List<ListMOU> listTest1 = mou.listAllMOU(baseDatatable, "", "", "").Data;
            List<ListMOU> listTest2 = mou.listAllMOUToExportExcel();
            Assert.AreEqual(listTest1.Count, listTest2.Count);
        }
    }
}
