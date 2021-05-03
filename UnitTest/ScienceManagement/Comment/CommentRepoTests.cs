using BLL.ScienceManagement.Comment;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BLL.ScienceManagement.Comment.Tests
{
    [TestClass()]
    public class CommentRepoTests
    {
        private readonly CommentRepo commentRepo = new CommentRepo();

        [TestMethod()]
        [DataRow(int.MinValue, 1, "Testing")]
        [DataRow(0, 1, "Testing")]
        [DataRow(int.MaxValue, 1, "Testing")]
        public void AddCommentTest1(int request_id, int account_id, string content)
        {
            int role_id = 1;
            bool is_manager = true;
            string path = null;
            AlertModal<string> actual = commentRepo.AddComment(request_id, account_id, content, role_id, is_manager, path);
            Assert.AreEqual("Đề nghị không tồn tại", actual.content);
        }

        [TestMethod()]
        [DataRow(1, int.MinValue, "Testing")]
        [DataRow(1, 0, "Testing")]
        [DataRow(1, int.MaxValue, "Testing")]
        public void AddCommentTest2(int request_id, int account_id, string content)
        {
            int role_id = 1;
            bool is_manager = true;
            string path = null;
            AlertModal<string> actual = commentRepo.AddComment(request_id, account_id, content, role_id, is_manager, path);
            Assert.AreEqual("Tài khoản không tồn tại", actual.content);
        }

        [TestMethod()]
        [DataRow(1, 1, null)]
        [DataRow(1, 1, "")]
        [DataRow(1, 1, " ")]
        public void AddCommentTest3(int request_id, int account_id, string content)
        {
            int role_id = 1;
            bool is_manager = true;
            string path = null;
            AlertModal<string> actual = commentRepo.AddComment(request_id, account_id, content, role_id, is_manager, path);
            Assert.AreEqual("Nội dung không được bỏ trống", actual.content);
        }

        [TestMethod()]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        [DataRow(int.MaxValue)]
        public void GetCommentTest(int request_id)
        {
            List<DetailComment> actual = commentRepo.GetComment(request_id);
            Assert.AreEqual(0, actual.Count);
        }
    }
}