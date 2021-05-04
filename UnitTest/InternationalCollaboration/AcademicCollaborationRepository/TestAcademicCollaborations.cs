using BLL.InternationalCollaboration.AcademicActivity;
using BLL.InternationalCollaboration.AcademicCollaborationRepository;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.InternationalCollaboration.AcademicActivity.AcademicActivityGuestRepo;

namespace UnitTest.InternationalCollaboration.AcademicCollaborationRepository
{
    [TestFixture]

    class TestAcademicCollaborations
    {
        [TestCase]
        public void TestMethod1()
        {
            //Arrange
            int direction = 1;
            int collab_type_id = 1;
            string country_name = "";
            int year = 2020;
            string partner_name = "Daekin University";
            string office_name = "";

            ObjectSearching_AcademicCollaboration obj_searching = new ObjectSearching_AcademicCollaboration
            {
                country_name = country_name,
                office_name = office_name,
                partner_name = partner_name,
                year = year
            };
            BaseDatatable baseDatatable = new BaseDatatable
            {
                Length = 5,
                SortColumnName = "collab_id",
                SortDirection = "asc",
                Start = 0
            };
            //Act
            new AcademicCollaborationRepo().AcademicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod2()
        {
            //Arrange
            int direction = 1;
            int collab_type_id = 2;
            string country_name = "";
            int year = 2020;
            string partner_name = "Daekin University";
            string office_name = "";

            ObjectSearching_AcademicCollaboration obj_searching = new ObjectSearching_AcademicCollaboration
            {
                country_name = country_name,
                office_name = office_name,
                partner_name = partner_name,
                year = year
            };
            BaseDatatable baseDatatable = new BaseDatatable
            {
                Length = 5,
                SortColumnName = "collab_id",
                SortDirection = "asc",
                Start = 0
            };
            //Act
            new AcademicCollaborationRepo().AcademicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod3()
        {
            //Arrange
            int direction = 1;
            int collab_type_id = 1;
            string country_name = "";
            int year = 2020;
            string partner_name = "Daekin University";
            string office_name = "";

            ObjectSearching_AcademicCollaboration obj_searching = new ObjectSearching_AcademicCollaboration
            {
                country_name = country_name,
                office_name = office_name,
                partner_name = partner_name,
                year = year
            };
            BaseDatatable baseDatatable = new BaseDatatable
            {
                Length = 5,
                SortColumnName = "collab_id",
                SortDirection = "asc",
                Start = 0
            };
            //Act
            new AcademicCollaborationRepo().AcademicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod4()
        {
            //Arrange
            int direction = 1;
            int collab_type_id = 1;
            string country_name = "";
            int year = 2020;
            string partner_name = "Daekin University";
            string office_name = "";

            ObjectSearching_AcademicCollaboration obj_searching = new ObjectSearching_AcademicCollaboration
            {
                country_name = country_name,
                office_name = office_name,
                partner_name = partner_name,
                year = year
            };
            BaseDatatable baseDatatable = new BaseDatatable
            {
                Length = 5,
                SortColumnName = "collab_id",
                SortDirection = "asc",
                Start = 0
            };
            //Act
            new AcademicCollaborationRepo().AcademicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod5()
        {
            //Arrange
            int direction = 1;
            int collab_type_id = 1;
            string country_name = "";
            int year = 2020;
            string partner_name = "Daekin University";
            string office_name = "";

            ObjectSearching_AcademicCollaboration obj_searching = new ObjectSearching_AcademicCollaboration
            {
                country_name = country_name,
                office_name = office_name,
                partner_name = partner_name,
                year = year
            };
            BaseDatatable baseDatatable = new BaseDatatable
            {
                Length = 5,
                SortColumnName = "collab_id",
                SortDirection = "asc",
                Start = 0
            };
            //Act
            new AcademicCollaborationRepo().AcademicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod6()
        {
            //Arrange
            int direction = 1;
            int collab_type_id = 1;
            string country_name = "";
            int year = 2020;
            string partner_name = "Daekin University";
            string office_name = "";

            ObjectSearching_AcademicCollaboration obj_searching = new ObjectSearching_AcademicCollaboration
            {
                country_name = country_name,
                office_name = office_name,
                partner_name = partner_name,
                year = year
            };
            BaseDatatable baseDatatable = new BaseDatatable
            {
                Length = 5,
                SortColumnName = "collab_id",
                SortDirection = "asc",
                Start = 0
            };
            //Act
            new AcademicCollaborationRepo().AcademicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod7()
        {
            //Arrange
            int direction = 1;
            int collab_type_id = 1;
            string country_name = "";
            int year = 2020;
            string partner_name = "Daekin University";
            string office_name = "";

            ObjectSearching_AcademicCollaboration obj_searching = new ObjectSearching_AcademicCollaboration
            {
                country_name = country_name,
                office_name = office_name,
                partner_name = partner_name,
                year = year
            };
            BaseDatatable baseDatatable = new BaseDatatable
            {
                Length = 5,
                SortColumnName = "collab_id",
                SortDirection = "asc",
                Start = 0
            };
            //Act
            new AcademicCollaborationRepo().AcademicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod8()
        {
            //Arrange
            int direction = 1;
            int collab_type_id = 1;
            string country_name = "";
            int year = 2020;
            string partner_name = "Daekin University";
            string office_name = "";

            ObjectSearching_AcademicCollaboration obj_searching = new ObjectSearching_AcademicCollaboration
            {
                country_name = country_name,
                office_name = office_name,
                partner_name = partner_name,
                year = year
            };
            BaseDatatable baseDatatable = new BaseDatatable
            {
                Length = 5,
                SortColumnName = "collab_id",
                SortDirection = "asc",
                Start = 0
            };
            //Act
            new AcademicCollaborationRepo().AcademicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);

            //Assert
            Assert.Pass();
        }
    }
}
