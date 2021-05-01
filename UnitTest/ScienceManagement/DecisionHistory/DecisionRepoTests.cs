using BLL.ScienceManagement.DecisionHistory;
using ENTITIES;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.DecisionHistory.Tests
{
    [TestClass()]
    public class DecisionRepoTests
    {
        private readonly DecisionRepo decisionRepo = new DecisionRepo();
        [TestMethod()]
        [DataRow(null)]
        public void GetListDecisionTest(string search)
        {
            List<Decision> list = decisionRepo.GetListDecision(search);
            bool expected = list.Count() != 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void DeleteDecisionTest(int id)
        {
            string mess = decisionRepo.DeleteDecision(id);
            Assert.AreEqual("ff", mess);
        }
    }
}