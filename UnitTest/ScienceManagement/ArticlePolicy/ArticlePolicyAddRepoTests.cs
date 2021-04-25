using ENTITIES;
using ENTITIES.CustomModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BLL.ScienceManagement.ArticlePolicy.Tests
{
    [TestClass()]
    public class ArticlePolicyAddRepoTests
    {
        private readonly ArticlePolicyAddRepo policyAddRepo = new ArticlePolicyAddRepo();

        [TestMethod()]
        public void AddTest1()
        {
            List<ArticleVersion> articleVersions = null;
            List<int> types = null;
            int account_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không có nội dung ngôn ngữ");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest2()
        {
            List<ArticleVersion> articleVersions = new List<ArticleVersion>();
            List<int> types = null;
            int account_id = 0;

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không có nội dung ngôn ngữ");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest3()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không có nội dung ngôn ngữ");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest4()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không có nội dung ngôn ngữ");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest5()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không có nội dung ngôn ngữ");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest6()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không có nội dung ngôn ngữ");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest7()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không có loại chính sách");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest8()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không có loại chính sách");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest9()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Bài đăng không có loại chính sách");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest10()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Tài khoản không tồn tại");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest11()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Tài khoản không tồn tại");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest12()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Tài khoản không tồn tại");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }

        [TestMethod()]
        public void AddTest13()
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

            AlertModal<string> expected = new AlertModal<string>(false, "Tài khoản không tồn tại");
            AlertModal<string> actual = policyAddRepo.Add(articleVersions, types, account_id);

            Assert.AreEqual(expected.content, actual.content);
        }
    }
}