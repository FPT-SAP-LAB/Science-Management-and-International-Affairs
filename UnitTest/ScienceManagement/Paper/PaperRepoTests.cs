using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetLstEmailAuthorTest(int reseacher)
        {
            List<string> list = paperRepo.GetLstEmailAuthor(reseacher);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue, "")]
        [DataRow(int.MaxValue, " ")]
        [DataRow(int.MaxValue, null)]
        [DataRow(int.MinValue, " ")]
        [DataRow(int.MinValue, null)]
        [DataRow(int.MinValue, "")]
        public void GetListAppendix3_4Test(int reseacher, string type)
        {
            List<Paper_Apendix_3> list = paperRepo.GetListAppendix3_4(type, reseacher);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue, "")]
        [DataRow(int.MaxValue, " ")]
        [DataRow(int.MaxValue, null)]
        [DataRow(int.MinValue, " ")]
        [DataRow(int.MinValue, null)]
        [DataRow(int.MinValue, "")]
        public void GetListAppendix1_2Test(int reseacher, string type)
        {
            List<Paper_Appendix_1> list = paperRepo.GetListAppendix1_2(type, reseacher);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue, "")]
        [DataRow(int.MaxValue, " ")]
        [DataRow(int.MaxValue, null)]
        [DataRow(int.MinValue, " ")]
        [DataRow(int.MinValue, null)]
        [DataRow(int.MinValue, "")]
        public void GetListWwaitDecision2Test(int reseacher, string type)
        {
            List<WaitDecisionPaper> list = paperRepo.GetListWwaitDecision2(type, reseacher);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue, "")]
        [DataRow(int.MaxValue, " ")]
        [DataRow(int.MaxValue, null)]
        [DataRow(int.MinValue, " ")]
        [DataRow(int.MinValue, null)]
        [DataRow(int.MinValue, "")]
        public void GetListWwaitDecisionTest(int reseacher, string type)
        {
            List<WaitDecisionPaper> list = paperRepo.GetListWwaitDecision(type, reseacher);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void UpdateCriteria_ManagerCheckTest(int id)
        {
            string mess = paperRepo.UpdateCriteria_ManagerCheck(id);
            Assert.AreEqual("ss", mess);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetListWait_UploadNCVTest(int reseacher)
        {
            List<WaitDecisionPaper> list = paperRepo.GetListWait_UploadNCV(reseacher);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetListWait_UploadQDGVTest(int reseacher)
        {
            List<WaitDecisionPaper> list = paperRepo.GetListWait_UploadQDGV(reseacher);
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
        public void GetAuthorReceivedTest(string id)
        {
            string p = paperRepo.GetAuthorReceived(id);
            Assert.AreEqual(null, p);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void AddBaseRequestTest(int account_id)
        {
            BaseRequest b = paperRepo.AddBaseRequest(account_id);
            Assert.AreEqual(null, b);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void GetDecisionLinkTest(int id)
        {
            List<string> link = paperRepo.GetDecisionLink(id);
            bool expected = link == null || link.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void DeleteRequestTest(int id)
        {
            string mess = paperRepo.DeleteRequest(id);
            Assert.AreEqual("ff", mess);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void EditAuthorRewardTest(int request_id)
        {
            bool expected = paperRepo.EditAuthorReward(request_id);
            Assert.AreEqual(false, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void ConfirmRewardTest(int request_id)
        {
            bool expected = paperRepo.ConfirmReward(request_id);
            Assert.AreEqual(false, expected);
        }
    }
}