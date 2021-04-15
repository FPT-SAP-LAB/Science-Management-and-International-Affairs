using System;

namespace ENTITIES.CustomModels
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Icon { get; set; }
        public string Template { get; set; }
        public bool IsRead { get; set; }
        public string URL { get; set; }
        public DateTime CreatedDate { get; set; }
        public int AccountID { get; set; }
        public string StringDate { get; set; }
        public int TypeID { get; set; }
    }
}
