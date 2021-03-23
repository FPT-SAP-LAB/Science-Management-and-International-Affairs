using ENTITIES;
using System.Web;

namespace GUEST.Models
{
    public static class CurrentAccount
    {
        public static int AccountID(HttpSessionStateBase Session)
        {
            return Account(Session).account_id;
        }
        public static Account Account(HttpSessionStateBase Session)
        {
            Account acc = new Account();
            if (Session["User"] != null)
            {
                BLL.Authen.LoginRepo.User u = (BLL.Authen.LoginRepo.User)Session["User"];
                acc = u.account;
            }
            return acc;
        }
    }
}