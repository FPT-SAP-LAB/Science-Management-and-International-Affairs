using BLL.InternationalCollaboration.AcademicActivity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.InternationalCollaboration.AcademicActivity.AcademicActivityGuestRepo;

namespace UnitTest.InternationalCollaboration.AcademicActivity
{
    [TestFixture]
    class GetAcademicActivityUT
    {
        [TestCase]
        public void GetAcademicActivityUT_1()
        {
            AcademicActivityGuestRepo guestRepo = new AcademicActivityGuestRepo();
            List<int> type = new List<int> { 1, 2, 3, 4 };
            List<baseAA> alist = guestRepo.getBaseAA(0, type, 1, null);
            Assert.Pass();
        }
        [TestCase]
        public void GetAcademicActivityUT_2()
        {

        }
    }
}
