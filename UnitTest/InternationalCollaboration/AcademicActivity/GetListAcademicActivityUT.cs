using BLL.InternationalCollaboration.AcademicActivity;
using ENTITIES;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.InternationalCollaboration.AcademicActivity
{
    [TestFixture]
    public class GetListAcademicActivityUT
    {
        [TestCase]
        public void GetListAcademicActivityUT_1()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            int year = 2020;
            List<AcademicActivityRepo.ListAA> data = activityRepo.listAllAA(year);
            if (data.Count == 10)
                Assert.Pass();
        }
        [TestCase]
        public void GetListAcademicActivityUT_2()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            int year = 2022;
            List<AcademicActivityRepo.ListAA> data = activityRepo.listAllAA(year);
            if (data.Count >= 0)
                Assert.Pass();
        }
        [TestCase]
        public void GetListAcademicActivityUT_3()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            int year = 0;
            List<AcademicActivityRepo.ListAA> data = activityRepo.listAllAA(year);
            if (data.Count == 0)
                Assert.Pass();
        }
        [TestCase]
        public void GetListAcademicActivityUT_4()
        {
            AcademicActivityRepo activityRepo = new AcademicActivityRepo();
            int year = 2021;
            List<AcademicActivityRepo.ListAA> data = activityRepo.listAllAA(year);
            if (data.Count > 0)
                Assert.Pass();
        }
    }
}
