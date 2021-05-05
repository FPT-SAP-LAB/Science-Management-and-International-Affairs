using BLL.InternationalCollaboration.Dashboard;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration.Dashboard;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.InternationalCollaboration.DashBoard
{
    [TestFixture]
    public class TestGetTable
    {
        [TestCase]
        public void TestMethod1()
        {
            //Arrange
            int year = 2019;
            int collab_type_id = 1;
            BaseDatatable baseDataTable = new BaseDatatable
            {
                Length = 10,
                SortColumnName = "training",
                SortDirection = "asc",
                Start = 0
            };

            //Act
            DashboardRepo dashBoardRepo = new DashboardRepo();
            BaseServerSideData<DashboardDatatable> baseDataTables = dashBoardRepo.GetTable(collab_type_id, year, baseDataTable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod2()
        {
            //Arrange
            int year = 2021;
            int collab_type_id = 1;
            BaseDatatable baseDataTable = new BaseDatatable
            {
                Length = 10,
                SortColumnName = "training",
                SortDirection = "asc",
                Start = 0
            };

            //Act
            DashboardRepo dashBoardRepo = new DashboardRepo();
            BaseServerSideData<DashboardDatatable> baseDataTables = dashBoardRepo.GetTable(collab_type_id, year, baseDataTable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod3()
        {
            //Arrange
            int year = 2019;
            int collab_type_id = 2;
            BaseDatatable baseDataTable = new BaseDatatable
            {
                Length = 10,
                SortColumnName = "training",
                SortDirection = "asc",
                Start = 0
            };

            //Act
            DashboardRepo dashBoardRepo = new DashboardRepo();
            BaseServerSideData<DashboardDatatable> baseDataTables = dashBoardRepo.GetTable(collab_type_id, year, baseDataTable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod4()
        {
            //Arrange
            int year = 2021;
            int collab_type_id = 2;
            BaseDatatable baseDataTable = new BaseDatatable
            {
                Length = 10,
                SortColumnName = "training",
                SortDirection = "asc",
                Start = 0
            };

            //Act
            DashboardRepo dashBoardRepo = new DashboardRepo();
            BaseServerSideData<DashboardDatatable> baseDataTables = dashBoardRepo.GetTable(collab_type_id, year, baseDataTable);

            //Assert
            Assert.Pass();
        }
    }
}
