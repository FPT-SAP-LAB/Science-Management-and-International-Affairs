using BLL.ScienceManagement.ArticlePolicy;
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
    public class ArticlePolicyDetailRepoTests
    {
        private readonly ArticlePolicyDetailRepo policyDetailRepo = new ArticlePolicyDetailRepo();

        [TestMethod()]
        [DataRow(int.MinValue, int.MinValue)]
        [DataRow(0, int.MinValue)]
        [DataRow(int.MaxValue, int.MinValue)]
        [DataRow(1, int.MinValue)]
        [DataRow(1, 0)]
        [DataRow(1, int.MaxValue)]
        public void DetailTest(int article_id, int language_id)
        {
            ArticlePolicyIndex articlePolicy = policyDetailRepo.Detail(article_id, language_id);
            Assert.AreEqual(null, articlePolicy);
        }
    }
}