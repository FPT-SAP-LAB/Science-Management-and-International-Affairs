using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;

namespace BLL.ModelDAL
{
    public class NotificationRepo
    {
        public static string GuestURI;
        public static string ManagerURI;
        private ScienceAndInternationalAffairsEntities db;
        public NotificationRepo() { }
        public NotificationRepo(ScienceAndInternationalAffairsEntities db)
        {
            this.db = db;
        }
        public int AddByAccountID(int account_id, int notification_type_id, string URL, bool ToManager)
        {
            int notification_id = 0;
            db = db ?? new ScienceAndInternationalAffairsEntities();
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
                notification_id = notification.notification_id;
            }
            if (subscribe == null || subscribe.mail_subscribe)
            {
                Account account = db.Accounts.Find(account_id);
                NotificationTypeLanguage typeLanguage = db.NotificationTypeLanguages
                    .Where(x => x.notification_type_id == notification_type_id && x.language_id == 1).FirstOrDefault();

                Regex regexText = new Regex("@URL");
                string URI = ToManager ? ManagerURI : GuestURI;
                string BodyText = regexText.Replace(typeLanguage.mail_template, URI + URL);

                SmtpMailService.Send(account.email, typeLanguage.notification_type_name, BodyText, true);
            }
            return notification_id;
        }
        public List<int> AddByRightID(int right_id, int notification_type_id, string URL, bool ToManager)
        {
            return AddByRightID(new List<int> { right_id }, notification_type_id, URL, ToManager);
        }
        public List<int> AddByRightID(List<int> rights, int notification_type_id, string URL, bool ToManager)
        {
            db = db ?? new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;

            List<Account> accounts = (from a in db.Accounts
                                      join b in db.AccountRights.Where(x => rights.Contains(x.right_id)).Select(x => x.account_id).Distinct()
                                      on a.account_id equals b
                                      select a).ToList();

            List<int> accountIds = accounts.Select(x => x.account_id).ToList();
            List<NotificationSubscribe> subscribes = (from a in db.NotificationSubscribes
                                                      where a.notification_type_id == notification_type_id && accountIds.Contains(a.account_id)
                                                      select a).ToList();

            List<int> notis = new List<int>();
            List<string> emails = new List<string>();
            foreach (var account in accounts)
            {
                var subscribe = subscribes.Where(x => x.account_id == account.account_id).FirstOrDefault();

                if (subscribe == null || subscribe.is_subscribe)
                {
                    NotificationBase notification = new NotificationBase
                    {
                        account_id = account.account_id,
                        created_date = DateTime.Now,
                        is_read = false,
                        notification_type_id = notification_type_id,
                        URL = URL
                    };
                    db.NotificationBases.Add(notification);
                    db.SaveChanges();
                    notis.Add(notification.notification_id);
                }
                if (subscribe == null || subscribe.mail_subscribe)
                {
                    emails.Add(account.email);
                }
            }
            NotificationTypeLanguage typeLanguage = db.NotificationTypeLanguages
                .Where(x => x.notification_type_id == notification_type_id && x.language_id == 1).FirstOrDefault();

            Regex regexText = new Regex("@URL");
            string URI = ToManager ? ManagerURI : GuestURI;
            string BodyText = regexText.Replace(typeLanguage.mail_template, URI + URL);

            SmtpMailService.Send(emails, typeLanguage.notification_type_name, BodyText, true);
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
                            TypeID = a.notification_type_id,
                            NotificationID = a.notification_id
                        }).FirstOrDefault();
            return list;
        }
        public Notification Get(int notification_id, int language_id)
        {
            var list = (from a in db.NotificationBases
                        join b in db.NotificationTypes on a.notification_type_id equals b.notification_type_id
                        join c in db.NotificationTypeLanguages on a.notification_type_id equals c.notification_type_id
                        where a.notification_id == notification_id && c.language_id == language_id
                        select new Notification
                        {
                            Icon = b.icon,
                            IsRead = a.is_read,
                            URL = a.URL,
                            CreatedDate = a.created_date,
                            AccountID = a.account_id,
                            TypeID = a.notification_type_id,
                            NotificationID = a.notification_id,
                            Template = c.notification_template
                        }).FirstOrDefault();
            return list;
        }
        public BaseServerSideData<Notification> List(int account_id, int start, int language_id = 1)
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
                            CreatedDate = a.created_date,
                            NotificationID = a.notification_id
                        }).Skip(start).ToList();
            list.ForEach(x => x.StringDate = x.CreatedDate.ToString("HH:mm dd/MM/yyyy"));
            int unread = (from a in db.NotificationBases
                          join b in db.NotificationTypes on a.notification_type_id equals b.notification_type_id
                          join c in db.NotificationTypeLanguages on b.notification_type_id equals c.notification_type_id
                          where a.account_id == account_id && c.language_id == language_id && !a.is_read
                          orderby a.created_date descending
                          select a).Count();
            return new BaseServerSideData<Notification>(list, unread);
        }
        public string Read(int id)
        {
            string URL = "/";
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                NotificationBase notification = db.NotificationBases.Find(id);
                notification.is_read = true;
                db.SaveChanges();
                URL = notification.URL;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return URL;
        }
    }
}