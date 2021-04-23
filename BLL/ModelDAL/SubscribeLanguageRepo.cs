using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelDAL
{
    public class SubscribeLanguageRepo
    {
        private ScienceAndInternationalAffairsEntities db;
        public List<SubscribeLanguage> GetSubscribes(int account_id, int language_id)
        {
            db = db ?? new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            List<SubscribeLanguage> subscribes = (from a in db.NotificationTypeLanguages
                                                  join b in db.NotificationSubscribes on a.notification_type_id equals b.notification_type_id into subs
                                                  from c in subs.DefaultIfEmpty()
                                                  where (c == null || c.account_id == account_id) && a.language_id == language_id
                                                  select new SubscribeLanguage
                                                  {
                                                      account_id = account_id,
                                                      is_subscribe = c == null || c.is_subscribe,
                                                      notification_type_id = a.notification_type_id,
                                                      TypeName = a.notification_type_name
                                                  }).ToList();
            return subscribes;
        }

        public AlertModal<string> UpdateSubscribe(List<NotificationSubscribe> subscribes, int account_id)
        {
            db = db ?? new ScienceAndInternationalAffairsEntities();
            db.Configuration.LazyLoadingEnabled = false;
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    List<NotificationSubscribe> before = db.NotificationSubscribes.Where(x => x.account_id == account_id).ToList();
                    foreach (var item in subscribes)
                    {
                        NotificationSubscribe subscribe = before.Where(x => x.notification_type_id == item.notification_type_id).FirstOrDefault();
                        if (subscribe == null)
                        {
                            item.account_id = account_id;
                            db.NotificationSubscribes.Add(item);
                        }
                        else
                        {
                            subscribe.is_subscribe = item.is_subscribe;
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return new AlertModal<string>(false);
                }
            }
        }
    }
}
