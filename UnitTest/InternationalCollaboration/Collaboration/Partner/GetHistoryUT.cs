using BLL.InternationalCollaboration.Collaboration.PartnerRepo;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.InternationalCollaboration.Collaboration.Partner
{
    [TestFixture]
    public class GetHistoryUT
    {
        private PartnerRepo partnerRepo;
        [TestCase]
        public void GetHistoryUT_1()
        {
            partnerRepo = new PartnerRepo();
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
            int listTest = partnerRepo.GetListAll(baseDatatable, searchPartner).RecordsTotal;
            Random ran = new Random();
            _ = partnerRepo.GetHistory(ran.Next(listTest) + 1);
            Assert.Pass();
        }  

        [TestCase]
        public void GetHistoryUT_2()
        {
            partnerRepo = new PartnerRepo();
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
                is_deleted = 1,
                language = 1,
            };
            int listTest = partnerRepo.GetListAll(baseDatatable, searchPartner).RecordsTotal;
            Random ran = new Random();
            _ = partnerRepo.GetHistory(ran.Next(listTest) + 1);
            Assert.Pass();
        }
    }
}
