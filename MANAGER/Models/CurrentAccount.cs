using ENTITIES;
using System.Web;
using static BLL.Authen.LoginRepo;

namespace MANAGER.Models
{
    public static class CurrentAccount
    {
        public static int AccountID(HttpSessionStateBase Session)
        {
            return Account(Session).account_id;
        }
        public static int RoleID(HttpSessionStateBase Session)
        {
            return Account(Session).role_id;
        }
        public static Account Account(HttpSessionStateBase Session)
        {
            Account acc = new Account();
            if (Session["User"] != null)
            {
                User u = (User)Session["User"];
                acc = u.account;
            }
            return acc;
        }
    }
}