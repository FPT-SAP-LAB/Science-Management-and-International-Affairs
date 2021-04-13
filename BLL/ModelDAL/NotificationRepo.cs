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
        public NotificationRepo() { }
        public NotificationRepo(ScienceAndInternationalAffairsEntities db)
        {
            this.db = db;
        }
        public int AddByAccountID(int account_id, int notification_type_id, string URL)
        {
            NotificationSubscribe subscribe = db.NotificationSubscribes.Where(x => x.account_id == account_id && x.notification_type_id == notification_type_id).FirstOrDefault();
            if (subscribe == null || subscribe.is_subscribe)
            {
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
            else
            {
                return 0;
            }
        }
        public List<int> AddByRightID(int right_id, int notification_type_id, string URL)
        {
            return AddByRightID(new List<int> { right_id }, notification_type_id, URL);
        }
        public List<int> AddByRightID(List<int> rights, int notification_type_id, string URL)
        {
            db.Configuration.LazyLoadingEnabled = false;

            List<int> accounts = db.AccountRights.Where(x => rights.Contains(x.right_id)).Select(x => x.account_id).Distinct().ToList();

            List<NotificationSubscribe> subscribes = db.NotificationSubscribes.Where(x => accounts.Contains(x.account_id) && x.notification_type_id == notification_type_id).ToList();

            List<int> notis = new List<int>();
            foreach (var account in accounts)
            {
                var subscribe = subscribes.Where(x => x.account_id == account).FirstOrDefault();

                if (subscribe == null || subscribe.is_subscribe)
                {
                    NotificationBase notification = new NotificationBase
                    {
                        account_id = account,
                        created_date = DateTime.Now,
                        is_read = false,
                        notification_type_id = notification_type_id,
                        URL = URL
                    };
                    db.NotificationBases.Add(notification);
                    db.SaveChanges();
                    notis.Add(notification.notification_id);
                }
            }
            return notis;
        }
        public List<NotificationTypeLanguage> Languages()
        {
            db = new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            return db.NotificationTypeLanguages.ToList();
        }
        public Notification Get(int notification_id)
        {
            var list = (from a in db.NotificationBases
                        join b in db.NotificationTypes on a.notification_type_id equals b.notification_type_id
                        where a.notification_id == notification_id
                        select new Notification
                        {
                            Icon = b.icon,
                            IsRead = a.is_read,
                            URL = a.URL,
                            CreatedDate = a.created_date,
                            AccountID = a.account_id,
                            TypeID = a.notification_type_id
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