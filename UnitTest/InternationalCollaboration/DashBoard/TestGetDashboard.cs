using System;
using ENTITIES;
using ENTITIES.CustomModels;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BLL.InternationalCollaboration.Dashboard;
using ENTITIES.CustomModels.InternationalCollaboration.Dashboard;


namespace UnitTest.InternationalCollaboration.DashBoard
{
    [TestFixture]
    public class TestGetDashboard
    {
        [TestCase]
        public void TestMethod1()
        {
            //Arrange
            int? year = 2019;

            //Act
            DashboardRepo dashBoardRepo = new DashboardRepo();
            List<ChartDashboard> chartDashboards = dashBoardRepo.GetDashboard(year);

            //Assert
            Assert.Pass();
        }

        [TestCase]
        public void TestMethod2()
        {
            //Arrange
            int? year = null;

            //Act
            DashboardRepo dashBoardRepo = new DashboardRepo();
            List<ChartDashboard> chartDashboards = dashBoardRepo.GetDashboard(year);

            //Assert
            Assert.Pass();
        }
    }
}
