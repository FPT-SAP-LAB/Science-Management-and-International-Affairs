using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace BLL.Authen
{
    class AccountRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public class Account_Rights
        {
            public int ID { get; set; }
            public string Right { get; set; }
            public string Module { get; set; }
            public int GroupID { get; set; }
        }
        public class Rights
        {
            public List<GetUserFunction_Result> Accept { get; set; }
            public List<GetUserFunction_Result> Deny { get; set; }
        }
        public class GetUserFunction_Result
        {
            public int ID { get; set; }
            public string Right { get; set; }
            public int GroupID { get; set; }
        }
    }
}