using BLL.InternationalCollaboration.Collaboration.PartnerRepo;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.InternationalCollaboration.Collaboration.Partner
{
    [TestFixture]
    public class GetListAllUT
    {
        [TestCase]
        public void GetListAllUT_1()
        {
            //Check list count
            PartnerRepo partnerRepo = new PartnerRepo();
            BaseDatatable baseDatatable = new BaseDatatable
            {
                SortColumnName = "no",
                SortDirection = "asc",
                Start = 0,
                Length = 10
            };
            SearchPartner searchPartner = new SearchPartner
            {
                partner_name = "",
                nation = "",
                specialization = "",
                is_collab = 0,
                is_deleted = 0,
                language = 1,
            };
            List<PartnerList> listTest = partnerRepo.GetListAll(baseDatatable, searchPartner).Data;
            Assert.AreEqual(10, listTest.Count);
        }

        [TestCase]
        public void GetListAllUT_2()
        {
            //Check list count
            PartnerRepo partnerRepo = new PartnerRepo();
            BaseDatatable baseDatatable = new BaseDatatable
            {
                SortColumnName = "no",
                SortDirection = "asc",
                Start = 0,
                Length = 10
            };
            SearchPartner searchPartner = new SearchPartner
            {
                partner_name = "",
                nation = "",
                specialization = "",
                is_collab = 0,
                is_deleted = 0,
                language = 1,
            };
            SearchPartner searchPartner_is_collab = new SearchPartner
            {
                partner_name = "",
                nation = "",
                specialization = "",
                is_collab = 1,
                is_deleted = 0,
                language = 1,
            };
            SearchPartner searchPartner_not_collab = new SearchPartner
            {
                partner_name = "",
                nation = "",
                specialization = "",
                is_collab = 2,
                is_deleted = 0,
                language = 1,
            };
            int listTest = partnerRepo.GetListAll(baseDatatable, searchPartner).RecordsTotal;
            int listTest_is_collab = partnerRepo.GetListAll(baseDatatable, searchPartner_is_collab).RecordsTotal;
            int listTest_not_collab = partnerRepo.GetListAll(baseDatatable, searchPartner_not_collab).RecordsTotal;
            Assert.AreEqual(listTest, listTest_is_collab + listTest_not_collab);
        }
    }
}
