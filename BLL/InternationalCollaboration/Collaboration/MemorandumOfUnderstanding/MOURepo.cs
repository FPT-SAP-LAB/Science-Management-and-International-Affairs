using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class MOURepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public List<ListMOU> listAllMOU()
        {
            return new List<ListMOU>();
        }

        public int getDuration()
        {
            DateTime today = DateTime.Today;
            DateTime end_date = new DateTime(2021, 05, 20);
            TimeSpan value = end_date.Subtract(today);
            return value.Duration().Days;
        }

        public class ListMOU {
            
        }

        public class NotificationInfo
        {
            public NotificationInfo() { }
            public int InactiveNumber { get; set; }
            public List<string> ExpiredMOUCode { get; set; }
        }

    }
}
