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
    class GetAcademicActivityDetailUT
    {
        AcademicActivityGuestRepo guestRepo;
        [TestCase]
        public void GetAcademicActivityDetailUT_1()
        {
            guestRepo = new AcademicActivityGuestRepo();
            baseAA detail = guestRepo.getBaseAADetail(1, 1);
            if (detail != null)
            {
                List<subContent> subContents = guestRepo.GetSubContent(1, 1);
            }
            Assert.Pass();
        }
        [TestCase]
        public void GetAcademicActivityDetailUT_2()
        {
            guestRepo = new AcademicActivityGuestRepo();
            baseAA detail = guestRepo.getBaseAADetail(1, 1);
            Assert.Pass();
        }
        public void GetAcademicActivityDetailUT_3()
        {
            guestRepo = new AcademicActivityGuestRepo();
            baseAA detail = guestRepo.getBaseAADetail(1, 2);
            Assert.Pass();
        }
    }
}
