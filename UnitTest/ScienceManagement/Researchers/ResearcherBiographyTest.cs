using BLL.ScienceManagement.Researcher;
using BLL.ScienceManagement.ResearcherListRepo;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ScienceManagement.Researchers
{

    [TestClass]
    public class ResearcherBiographyTest
    {
        private ResearchersBiographyRepo _researcherListRepo;
        private int vieLang = 1;
        private int enLang = 2;
        [TestInitialize]
        public void Setup()
        {
            _researcherListRepo = new ResearchersBiographyRepo();
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetResearcerAwardsById(int id)
        {
            var resList = _researcherListRepo.GetAwards(id);
            Assert.AreEqual(0, resList.Count);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetResearcherPublicationsById(int id)
        {
            var res = _researcherListRepo.GetPublications(id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void getResearcherConferencePublications(int id)
        {
            var res = _researcherListRepo.GetConferencePublic(id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void getResearcherAcadHistoryVietnamese(int id)
        {
            var res = _researcherListRepo.GetAcadHistory(id, vieLang);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void getResearcherAcadHistoryEng(int id)
        {
            var res = _researcherListRepo.GetAcadHistory(id, enLang);
            Assert.AreEqual(0, res.Count);
        }
    }
}
