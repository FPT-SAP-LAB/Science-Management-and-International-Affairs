using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLL.ScienceManagement.ArticlePolicy.Tests
{
    [TestClass()]
    public class ArticlePolicyEditRepoTests
    {
        private readonly ArticlePolicyEditRepo EditRepo = new ArticlePolicyEditRepo();

        [TestMethod()]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        [DataRow(int.MaxValue)]
        public void DetailTest(int article_id)
        {
            ArticlePolicyEdit actual = EditRepo.Detail(article_id);
            Assert.AreEqual(null, actual);
        }
    }
}