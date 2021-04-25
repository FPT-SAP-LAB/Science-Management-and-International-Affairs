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
    public class ResearcherDetailTests
    {
        private ResearchersDetailRepo _researcherDetailRepo;
        private int vieLang = 1;
        private int enLang = 2;

        [TestInitialize]
        public void Setup()
        {
            _researcherDetailRepo = new ResearchersDetailRepo();
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetResearcerProfileByIdMANAGER(int id)
        {
            var resVie = _researcherDetailRepo.GetProfile(id,vieLang);
            var resEng = _researcherDetailRepo.GetProfile(id,enLang);
            Assert.AreEqual(null,resVie);
            Assert.AreEqual(null,resEng);
        }

        [TestMethod]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetResearcerProfileByIdGUEST(int id)
        {
            var resVie = _researcherDetailRepo.GetDetailView(id, vieLang);
            var resEng = _researcherDetailRepo.GetDetailView(id, enLang);
            Assert.AreEqual(null, resVie);
            Assert.AreEqual(null, resEng);
        }

    }
}
