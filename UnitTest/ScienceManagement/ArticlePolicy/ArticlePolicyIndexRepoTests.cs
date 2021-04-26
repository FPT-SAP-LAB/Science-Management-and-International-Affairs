using BLL.ScienceManagement.ArticlePolicy;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.ArticlePolicy.Tests
{
    [TestClass()]
    public class ArticlePolicyIndexRepoTests
    {
        private readonly ArticlePolicyIndexRepo articlePolicyIndex = new ArticlePolicyIndexRepo();

        [TestMethod()]
        [DataRow(0)]
        [DataRow(int.MinValue)]
        [DataRow(int.MaxValue)]
        public void ListTest(int language_id)
        {
            List<ArticlePolicyIndex> actual = articlePolicyIndex.List(language_id);
            Assert.AreEqual(null, actual);
        }
    }
}