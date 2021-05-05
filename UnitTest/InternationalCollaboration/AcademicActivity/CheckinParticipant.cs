using BLL.InternationalCollaboration.AcademicActivity;
using ENTITIES;
using ENTITIES.CustomModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.InternationalCollaboration.AcademicActivity
{
    [TestFixture]
    public class CheckinParticipant
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        [TestCase]
        public void CheckinParticipantUT_1()
        {
            new AddParticipantToPhase().AddParticipantToPhaseUT_1();
        }
        [TestCase]
        public void CheckinParticipantUT_2()
        {
            new AddParticipantToPhase().AddParticipantToPhaseUT_1();
        }
    }
}
