using BLL.ScienceManagement.Invention;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Invention;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Invention.Tests
{
    [TestClass()]
    public class InventionRepoTests
    {
        private readonly InventionRepo invention = new InventionRepo();

        [TestMethod()]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        [DataRow("0")]
        [DataRow("2,147,483,647")]
        [DataRow("-2,147,483,647")]
        [DataRow("2147483647")]
        [DataRow("-2147483647")]
        public void getDetailTest(string id)
        {
            DetailInvention item = invention.getDetail(id);
            Assert.AreEqual(null, item);
        }

        [TestMethod()]
        [DataRow("", "")]
        [DataRow(" ", " ")]
        [DataRow(null, null)]
        [DataRow("0", null)]
        [DataRow("2,147,483,647", null)]
        [DataRow("-2,147,483,647", null)]
        [DataRow("2147483647", null)]
        [DataRow("-2147483647", null)]
        public void getAuthorTest(string id, string lang)
        {
            List<AuthorInfoWithNull> list = invention.getAuthor(id, lang);
            bool expected = list == null || list.Count() == 0;
            Assert.AreEqual(true, expected);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void deleteRequestTest(int id)
        {
            string mess = invention.deleteRequest(id);
            Assert.AreEqual("ff", mess);
        }

        [TestMethod()]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void addInvenTypeTest(string name)
        {
            InventionType ck = invention.addInvenType(name);
            Assert.AreEqual(null, ck);
        }

        [TestMethod()]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        [DataRow(0)]
        public void getListCountryEditTest(int id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            List<CustomCountry> list = invention.getListCountryEdit(id);
            List<Country> listC = db.Countries.ToList();
            bool expected = list.Count() == listC.Count();
            Assert.AreEqual(true, expected);
        }
    }
}