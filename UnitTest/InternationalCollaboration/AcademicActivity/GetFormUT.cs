using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BLL.InternationalCollaboration.AcademicActivity;
using static BLL.InternationalCollaboration.AcademicActivity.AcademicActivityGuestRepo;

namespace UnitTest.InternationalCollaboration.AcademicActivity
{
    [TestFixture]
    class GetFormUT
    {
        AcademicActivityGuestRepo guestRepo;
        [TestCase]
        public void GetFormUT_1()
        {
            guestRepo = new AcademicActivityGuestRepo();
            bool checkForm = guestRepo.checkForm(1);
            if(checkForm)
            {
                fullForm form = guestRepo.getForm(1);
            }
            Assert.Pass();
        }
        [TestCase]
        public void GetFormUT_2()
        {
            guestRepo = new AcademicActivityGuestRepo();
            bool checkForm = guestRepo.checkForm(1);
            if(checkForm)
            {
                fullForm form = guestRepo.getForm(1);
                Assert.IsNotNull(form);
            }
        }

    }
}
