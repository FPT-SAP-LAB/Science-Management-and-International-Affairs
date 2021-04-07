using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using Microsoft.AspNet.SignalR;
using System.Collections.Generic;

namespace GUEST
{
    public class ChatHub : Hub
    {
        //private HttpSessionState session;
        private static readonly Dictionary<int, List<BruhbrubLNguyen>> AccountConnections = new Dictionary<int, List<BruhbrubLNguyen>>();
        private static readonly NotificationRepo notficationRepo = new NotificationRepo();
        private readonly List<NotificationTypeLanguage> TypeLanguage = notficationRepo.Languages();
        public ChatHub()
        {
            //AccountConnections = new Dictionary<int, List<string>>();
            //this.session = session;
        }
        public void Send(int notification_id)
        {
            // Call the addNewMessageToPage method to update clients.
            Notification Noti = notficationRepo.Get(notification_id);
            AccountConnections.TryGetValue(Noti.AccountID, out List<BruhbrubLNguyen> Connections);
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
            BruhbrubLNguyen conn = new BruhbrubLNguyen(connID, language_id);
            if (AccountConnections.ContainsKey(account_id))
            {
                List<BruhbrubLNguyen> Connections = AccountConnections[account_id];
                if (Connections.Count >= 6)
                    Connections.RemoveAt(0);
                Connections.Add(conn);
            }
            else
            {
                AccountConnections.Add(account_id, new List<BruhbrubLNguyen> { conn });
            }
        }

        private class BruhbrubLNguyen
        {
            public string ConnectionID { get; set; }
            public int LanguageID { get; set; }
            public BruhbrubLNguyen() { }
            public BruhbrubLNguyen(string connectionID, int languageID)
            {
                ConnectionID = connectionID;
                LanguageID = languageID;
            }
        }
    }
}