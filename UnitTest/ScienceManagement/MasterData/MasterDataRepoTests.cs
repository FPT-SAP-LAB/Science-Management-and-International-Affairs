using BLL.ScienceManagement.MasterData;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BLL.ScienceManagement.MasterData.Tests
{
    [TestClass()]
    public class MasterDataRepoTests
    {
        private readonly MasterDataRepo masterRepo = new MasterDataRepo();
        [TestMethod()]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void GetSpecTest(string language)
        {
            List<SpecializationLanguage> list = masterRepo.GetSpec(language);
            bool expected = list == null || list.Count == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void GetTitleTest(string language)
        {
            List<TitleWithName> list = masterRepo.GetTitle(language);
            bool expected = list == null || list.Count == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void GetAuthorTest(string ms)
        {
            AddAuthor item = masterRepo.GetAuthor(ms);
            Assert.AreEqual(null, item);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void GetTitleWithNameTest(int id)
        {
            Title2Name t = masterRepo.GetTitleWithName(id);
            Assert.AreEqual(null, t);
        }

        [TestMethod()]
        [DataRow(0, "", "")]
        [DataRow(0, " ", " ")]
        [DataRow(0, null, null)]
        [DataRow(int.MaxValue, "", "")]
        [DataRow(int.MaxValue, " ", " ")]
        [DataRow(int.MaxValue, null, null)]
        [DataRow(int.MinValue, "", "")]
        [DataRow(int.MinValue, " ", " ")]
        [DataRow(int.MinValue, null, null)]
        public void UpdateTitleTest(int id, string tv, string ta)
        {
            string mess = masterRepo.UpdateTitle(id, tv, ta);
            Assert.AreEqual("ff", mess);
        }

        [TestMethod()]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow(null, null)]
        public void AddTitleTest(string tv, string ta)
        {
            int result = masterRepo.AddTitle(tv, ta);
            Assert.AreEqual(0, result);
        }
    }
}