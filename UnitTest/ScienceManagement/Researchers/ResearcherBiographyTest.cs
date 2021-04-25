using BLL.ScienceManagement.Researcher;
using BLL.ScienceManagement.ResearcherListRepo;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ScienceManagement.Researchers
{

    [TestFixture]
    class ResearcherBiographyTest
    {
        private ResearchersBiographyRepo _researcherListRepo;
        [SetUp]
        public void Setup()
        {
            _researcherListRepo = new ResearchersBiographyRepo();
        }
        [Test]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void GetListResearchersTest(int resId)
        {
            BaseDatatable bd = new BaseDatatable();
            var resList = _researcherListRepo.GetAwards(resId);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(0, resList.Count);
        }

    }
}
