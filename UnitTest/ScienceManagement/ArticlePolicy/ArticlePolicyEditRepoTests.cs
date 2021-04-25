using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.ArticlePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BLL.ScienceManagement.ArticlePolicy.Tests
{
    [TestClass()]
    public class ArticlePolicyEditRepoTests
    {
        private readonly ArticlePolicyEditRepo policyEditRepo = new ArticlePolicyEditRepo();

        [TestMethod()]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        [DataRow(int.MaxValue)]
        public void DetailTest(int article_id)
        {
            ArticlePolicyEdit actual = policyEditRepo.Detail(article_id);
            Assert.AreEqual(null, actual);
        }

        [TestMethod()]
        public void EditTest1()
        {
            List<ArticleVersion> articleVersions = null;
            List<int> types = null;
            int account_id = 0;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Không có nội dung nào thay đổi");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest2()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>();
            List<int> types = null;
            int account_id = 0;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Không có nội dung nào thay đổi");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest3()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = null,
                    version_title = null
                }
            };
            List<int> types = null;
            int account_id = 0;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Không có nội dung nào thay đổi");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest4()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "",
                    version_title = null
                }
            };
            List<int> types = null;
            int account_id = 0;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Không có nội dung nào thay đổi");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest5()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = null
                }
            };
            List<int> types = null;
            int account_id = 0;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Không có nội dung nào thay đổi");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest6()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = ""
                }
            };
            List<int> types = null;
            int account_id = 0;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Không có nội dung nào thay đổi");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest7()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = null;
            int account_id = 0;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Loại chính sách không được bỏ trống");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest8()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = new List<int>();
            int account_id = 0;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Loại chính sách không được bỏ trống");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest9()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = new List<int>() { 0 };
            int account_id = 0;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Loại chính sách không được bỏ trống");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest10()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = new List<int>() { 1 };
            int account_id = 0;
            int article_id = 1;

            AlertModal<string> expected = new AlertModal<string>(false, "Tài khoản không tồn tại");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest11()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = new List<int>() { 1 };
            int account_id = -1;
            int article_id = 1;

            AlertModal<string> expected = new AlertModal<string>(false, "Tài khoản không tồn tại");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest12()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = new List<int>() { 1 };
            int account_id = int.MinValue;
            int article_id = 1;

            AlertModal<string> expected = new AlertModal<string>(false, "Tài khoản không tồn tại");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest13()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = new List<int>() { 1 };
            int account_id = int.MaxValue;
            int article_id = 1;

            AlertModal<string> expected = new AlertModal<string>(false, "Tài khoản không tồn tại");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest14()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = new List<int>() { 1 };
            int account_id = 1;
            int article_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không tồn tại");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest15()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = new List<int>() { 1 };
            int account_id = 1;
            int article_id = -1;

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không tồn tại");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void EditTest16()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>()
            {
                new ArticleVersion
                {
                    article_content = "Something",
                    version_title = "Something"
                }
            };
            List<int> types = new List<int>() { 1 };
            int account_id = 1;
            int article_id = int.MinValue;

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không tồn tại");
            AlertModal<string> actual = policyEditRepo.Edit(articleVersions, types, article_id, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }
    }
}