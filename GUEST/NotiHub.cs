using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GUEST
{
    public class NotiHub : Hub
    {
        //private HttpSessionState session;
        private static readonly Dictionary<int, BruhbrubLNguyen[]> AccountConnections = new Dictionary<int, BruhbrubLNguyen[]>();
        private static readonly NotificationRepo notficationRepo = new NotificationRepo();
        private static readonly List<NotificationTypeLanguage> TypeLanguage = notficationRepo.Languages();
        public void Send(int notification_id)
        {
            // Call the addNewMessageToPage method to update clients.
            Notification Noti = notficationRepo.Get(notification_id);
            AccountConnections.TryGetValue(Noti.AccountID, out BruhbrubLNguyen[] Connections);
            if (Connections != null)
                foreach (var conn in Connections)
                {
                    foreach (var type in TypeLanguage)
                    {
                        if (conn.LanguageID == type.language_id)
                            Clients.Client(conn.ConnectionID).addNewMessageToPage(Noti.URL, Noti.Icon, type.notification_template, Noti.CreatedDate.ToString("HH:mm dd/MM/yyyy"));
                    }
                }
        }
        public void Register(string connID, int account_id, int language_id)
        {
            if (AccountConnections.ContainsKey(account_id))
            {
                bool FullInit = true;
                //  Đừng tạo biến array, nó tốn dung lượng
                for (int i = 0; i < AccountConnections[account_id].Length; i++)
                {
                    if (AccountConnections[account_id][i] == null)
                    {
                        AccountConnections[account_id][i] = new BruhbrubLNguyen(connID, language_id, DateTime.Now);
                        FullInit = false;
                        break;
                    }
                }

                if (FullInit)
                {
                    BruhbrubLNguyen nguyen = AccountConnections[account_id].OrderBy(x => x.TimeAdded).First();
                    nguyen.ConnectionID = connID;
                    nguyen.LanguageID = language_id;
                    nguyen.TimeAdded = DateTime.Now;
                }
            }
            else
            {
                AccountConnections.Add(account_id, new BruhbrubLNguyen[6]);
            }
        }

        private class BruhbrubLNguyen
        {
            public string ConnectionID { get; set; }
            public int LanguageID { get; set; }
            public DateTime TimeAdded { get; set; }
            public BruhbrubLNguyen(string connectionID, int languageID, DateTime timeAdded)
            {
                ConnectionID = connectionID;
                LanguageID = languageID;
                TimeAdded = timeAdded;
            }
        }
    }
}