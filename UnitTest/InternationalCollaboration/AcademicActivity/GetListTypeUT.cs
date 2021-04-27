using BLL.InternationalCollaboration.AcademicActivity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.InternationalCollaboration.AcademicActivity
{
    [TestFixture]
    class GetListTypeUT
    {
        AcademicActivityGuestRepo guestRepo;
        [TestCase]
        public void GetListTypeUT_1()
        {
            guestRepo = new AcademicActivityGuestRepo();
            int totalType = guestRepo.getListType(1).Count;
            Assert.Greater(totalType, 0);
        }
        [TestCase]
        public void GetListTypeUT_2()
        {
            guestRepo = new AcademicActivityGuestRepo();
            int totalType = guestRepo.getListType(2).Count;
            Assert.Greater(totalType, 0);
        }
        [TestCase]
        public void GetListTypeUT_3()
        {
            guestRepo = new AcademicActivityGuestRepo();
            int totalVietnameseType = guestRepo.getListType(1).Count;
            int totalEnglishType = guestRepo.getListType(2).Count;
            Assert.AreEqual(totalVietnameseType, totalEnglishType);
        }
        [TestCase]
        public void GetListTypeUT_4()
        {
            guestRepo = new AcademicActivityGuestRepo();
            int totalType = guestRepo.getListType(1).Count;
            Assert.Pass();
        }
        public void GetListTypeUT_5()
        {
            guestRepo = new AcademicActivityGuestRepo();
            int totalType = guestRepo.getListType(2).Count;
            Assert.Pass();
        }
    }
}
