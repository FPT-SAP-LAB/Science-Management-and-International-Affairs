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
    public class getParticipantByPhaseUT
    {
        [TestCase]
        public void getParticipantByPhaseUT_1()
        {
            CheckInRepo checkInRepo = new CheckInRepo();
            List<CheckInRepo.dataParticipant> data = checkInRepo.getParticipantByPhase(12);
            Assert.Pass();
        }
    }
}
