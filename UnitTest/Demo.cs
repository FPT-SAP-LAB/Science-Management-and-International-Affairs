//using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
//using ENTITIES.CustomModels.Datatable;
//using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
////using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;

//namespace UnitTest
//{
//    [TestFixture]
//    public class Demo
//    {
//        //[TestCase]
//        public void Demo1()
//        {
//            //Check list count
//            MOURepo mou = new MOURepo();
//            BaseDatatable baseDatatable = new BaseDatatable
//            {
//                SortColumnName = "mou_partner_id",
//                SortDirection = "desc",
//                Start = 0,
//                Length = 10
//            };
//            List<ListMOU> listTest = mou.listAllMOU(baseDatatable, "", "", "").Data;
//            Assert.AreEqual(10, listTest.Count);
//        }
//        [TestCase]
//        public void Demo2()
//        {
//            //Assert.AreEqual(1, 1);
//            //Assert.Inconclusive();
//        }
//        [TestCase]
//        public void Demo3()
//        {
//            Assert.Multiple(() =>
//            {
//                Demo2();
//                Assert.AreEqual(4, 4);
//            });
//        }
//    }
//}
