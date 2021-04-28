using BLL.ScienceManagement.Citation;
using BLL.Support;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Citation.Tests
{
    [TestClass()]
    public class CitationRepoTests
    {
        private readonly CitationRepo citationRepo = new CitationRepo();

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void GetListTest(int account_id)
        {
            List<ListOnePerson_Citation> actual = citationRepo.GetList(account_id);
            Assert.AreEqual(null, actual);
        }

        [TestMethod()]
        [DataRow("-9223372036854775808")]   //Int64.MinValue
        [DataRow("9223372036854775807")]    //Int64.MaxValue
        [DataRow("-2147483648")]            //Int32.MinValue
        [DataRow("2147483647")]             //Int32.MaxValue
        [DataRow("0")]
        [DataRow(null)]
        [DataRow("")]
        public void GetStatusTest(string request_id)
        {
            int actual = citationRepo.GetStatus(request_id);

            Assert.AreEqual(0, actual);
        }

        [TestMethod()]
        [DataRow("-9223372036854775808")]   //Int64.MinValue
        [DataRow("9223372036854775807")]    //Int64.MaxValue
        [DataRow("-2147483648")]            //Int32.MinValue
        [DataRow("2147483647")]             //Int32.MaxValue
        [DataRow("0")]
        [DataRow(null)]
        [DataRow("")]
        public void GetAuthorTest(string request_id)
        {
            AuthorInfo actual = citationRepo.GetAuthor(request_id);

            Assert.AreEqual(null, actual);
        }

        [TestMethod()]
        [DataRow("-9223372036854775808")]   //Int64.MinValue
        [DataRow("9223372036854775807")]    //Int64.MaxValue
        [DataRow("-2147483648")]            //Int32.MinValue
        [DataRow("2147483647")]             //Int32.MaxValue
        [DataRow("0")]
        [DataRow(null)]
        [DataRow("")]
        public void GetRequestCitationTest(string request_id)
        {
            RequestCitation actual = citationRepo.GetRequestCitation(request_id);

            Assert.AreEqual(null, actual);
        }

        [TestMethod()]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        [DataRow(int.MaxValue)]
        public void DeleteRequestTest(int request_id)
        {
            string actual = citationRepo.DeleteRequest(request_id);

            Assert.AreEqual("ff", actual);
        }

        [TestMethod()]
        [DataRow("-9223372036854775808")]   //Int64.MinValue
        [DataRow("9223372036854775807")]    //Int64.MaxValue
        [DataRow("-2147483648")]            //Int32.MinValue
        [DataRow("2147483647")]             //Int32.MaxValue
        [DataRow("0")]
        [DataRow(null)]
        [DataRow("")]
        public void ChangeStatusTest(string request_id)
        {
            string actual = citationRepo.ChangeStatus(request_id);

            Assert.AreEqual("ff", actual);
        }

        [TestMethod()]
        [DataRow("-9223372036854775808")]   //Int64.MinValue
        [DataRow("9223372036854775807")]    //Int64.MaxValue
        [DataRow("-2147483648")]            //Int32.MinValue
        [DataRow("2147483647")]             //Int32.MaxValue
        [DataRow("0")]
        [DataRow(null)]
        [DataRow("")]
        public void GetCitationTest(string request_id)
        {
            List<CustomCitation> actual = citationRepo.GetCitation(request_id);

            Assert.AreEqual(null, actual);
        }

        [TestMethod()]
        public void EditAuthorTest1()
        {
            List<AddAuthor> people = null;
            Author actual = citationRepo.EditAuthor(people);

            Assert.AreEqual(null, actual);
        }

        [TestMethod()]
        public void EditAuthorTest2()
        {
            List<AddAuthor> people = new List<AddAuthor>();
            Author actual = citationRepo.EditAuthor(people);

            Assert.AreEqual(null, actual);
        }

        [TestMethod()]
        [DataRow(null, "abc@gmail.com")]
        [DataRow("", "abc@gmail.com")]
        [DataRow("abc", null)]
        [DataRow("abc", "")]
        public void EditAuthorTest3(string name, string email)
        {
            List<AddAuthor> people = new List<AddAuthor>()
            {
                new AddAuthor
                {
                    name = name,
                    email = email
                }
            };
            Author actual = citationRepo.EditAuthor(people);

            Assert.AreEqual(null, actual);
        }
    }
}