using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace BLL.ModelDAL
{
    public class NotificationRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public int Add(int account_id, int notification_type_id, string URL)
        {
            db = new ScienceAndInternationalAffairsEntities();
            NotificationBase notification = new NotificationBase
            {
                account_id = account_id,
                created_date = DateTime.Now,
                is_read = false,
                notification_type_id = notification_type_id,
                URL = URL
            };
            db.NotificationBases.Add(notification);
            db.SaveChanges();
            return notification.notification_id;
        }
        public List<NotificationTypeLanguage> Languages()
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.NotificationTypeLanguages.ToList();
        }
        public Notification Get(int notification_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var list = (from a in db.NotificationBases
                        join b in db.NotificationTypes on a.notification_type_id equals b.notification_type_id
                        where a.notification_id == notification_id
                        select new Notification
                        {
                            Icon = b.icon,
                            IsRead = a.is_read,
                            URL = a.URL,
                            CreatedDate = a.created_date,
                            AccountID = a.account_id
                        }).FirstOrDefault();
            return list;
        }
        public List<Notification> List(int account_id, int start, int language_id = 1)
        {
            db = new ScienceAndInternationalAffairsEntities();
            var list = (from a in db.NotificationBases
                        join b in db.NotificationTypes on a.notification_type_id equals b.notification_type_id
                        join c in db.NotificationTypeLanguages on b.notification_type_id equals c.notification_type_id
                        where a.account_id == account_id && c.language_id == language_id
                        orderby a.created_date descending
                        select new Notification
                        {
                            Icon = b.icon,
                            Template = c.notification_template,
                            IsRead = a.is_read,
                            URL = a.URL,
                            CreatedDate = a.created_date
                        }).Skip(start).Take(10).ToList();
            return list;
        }
    }
}