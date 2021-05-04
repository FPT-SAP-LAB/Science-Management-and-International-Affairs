using BLL.ScienceManagement.Researcher;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTest.ScienceManagement.Researchers
{
    [TestClass]
    public class ResearcherBiographyTest
    {
        private ResearchersBiographyRepo _researcherListRepo;
        private readonly int vieLang = 1;
        private readonly int enLang = 2;
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
            List<BaseRecord<ENTITIES.Award>> resList = _researcherListRepo.GetAwards(id);
            Assert.AreEqual(0, resList.Count);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetResearcherPublicationsById(int id)
        {
            List<ResearcherPublications> res = _researcherListRepo.GetPublications(id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetResearcherConferencePublications(int id)
        {
            List<ResearcherPublications> res = _researcherListRepo.GetConferencePublic(id);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetResearcherAcadHistoryVietnamese(int id)
        {
            List<AcadBiography> res = _researcherListRepo.GetAcadHistory(id, vieLang);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetResearcherAcadHistoryEng(int id)
        {
            List<AcadBiography> res = _researcherListRepo.GetAcadHistory(id, enLang);
            Assert.AreEqual(0, res.Count);
        }
    }
}
