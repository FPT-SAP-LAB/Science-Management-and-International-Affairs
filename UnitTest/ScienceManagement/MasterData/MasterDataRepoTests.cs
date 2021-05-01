using ENTITIES;
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
    }
}