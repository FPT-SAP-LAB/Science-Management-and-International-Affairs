using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using Microsoft.AspNet.SignalR;

namespace GUEST
{
    public class ChatHub : Hub
    {
        //private HttpSessionState session;
        private static readonly Dictionary<int, List<string>> AccountConnections = new Dictionary<int, List<string>>();
        public ChatHub()
        {
            //AccountConnections = new Dictionary<int, List<string>>();
            //this.session = session;
        }
        public void Send(int account_id, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            AccountConnections.TryGetValue(account_id, out List<string> Connections);
            if (Connections != null)
                foreach (var conn in Connections)
                {
                    Clients.Client(conn).addNewMessageToPage(account_id, message);
                }
        }
        public void Register(string connID, int account_id)
        {
            if (AccountConnections.ContainsKey(account_id))
            {
                List<string> Connections = AccountConnections[account_id];
                if (Connections.Count >= 6)
                    Connections.RemoveAt(0);
                Connections.Add(connID);
            }
            else
            {
                AccountConnections.Add(account_id, new List<string> { connID });
            }
        }
    }
}