using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL.ScienceManagement.Paper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;

namespace BLL.ScienceManagement.Paper.Tests
{
    [TestClass()]
    public class PaperRepoTests
    {
        private readonly PaperRepo paperRepo = new PaperRepo();
        [TestMethod()]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        [DataRow("0")]
        [DataRow("2,147,483,647")]
        [DataRow("-2,147,483,647")]
        [DataRow("2147483647")]
        [DataRow("-2147483647")]
        public void GetDetailTest(string id)
        {
            DetailPaper item = paperRepo.GetDetail(id);
            Assert.AreEqual(null, item);
        }

        [TestMethod()]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        [DataRow("0")]
        [DataRow("2,147,483,647")]
        [DataRow("-2,147,483,647")]
        [DataRow("2147483647")]
        [DataRow("-2147483647")]
        public void GetCriteriaTest(string id)
        {
            List<ListCriteriaOfOnePaper> list = paperRepo.GetCriteria(id);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        [DataRow("0")]
        [DataRow("2,147,483,647")]
        [DataRow("-2,147,483,647")]
        [DataRow("2147483647")]
        [DataRow("-2147483647")]
        public void GetAuthorReceived_allTest(string id)
        {
            Author p = paperRepo.GetAuthorReceived_all(id);
            Assert.AreEqual(null, p);
        }

        [TestMethod()]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow(null, null)]
        [DataRow("0", null)]
        [DataRow("2,147,483,647", null)]
        [DataRow("-2,147,483,647", null)]
        [DataRow("2147483647", null)]
        [DataRow("-2147483647", null)]
        public void GetAuthorPaperTest(string id, string lang)
        {
            List<AuthorInfoWithNull> list = paperRepo.GetAuthorPaper(id, lang);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow(null, null)]
        [DataRow("0", null)]
        [DataRow("2,147,483,647", null)]
        [DataRow("-2,147,483,647", null)]
        [DataRow("2147483647", null)]
        [DataRow("-2147483647", null)]
        public void GetAuthorPaper_FETest(string id, string lang)
        {
            List<AuthorInfoWithNull> list = paperRepo.GetAuthorPaper_FE(id, lang);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }
    }
}