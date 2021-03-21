using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;

namespace BLL.Authen
{
    public class LoginRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public User getAccount(ENTITIES.CustomModels.Authen.Gmail user, List<int> roleAccept)
        {
            try
            {
                Account a = db.Accounts.Where(x => x.email.Equals(user.email)).FirstOrDefault();
                if (a == null)
                {
                    return null;
                }
                Role r = db.Roles.Find(a.role_id);
                if (!roleAccept.Contains(r.role_id))
                {
                    return null;
                }
                if ((bool)!a.login)
                {
                    a.full_name = user.name;
                    a.google_id = user.id;
                    a.login = true;
                    a.picture = user.imageurl;
                    db.Entry(a).State = EntityState.Modified;
                    db.SaveChanges();
                }
                User u = new User
                {
                    url = r.url,
                    rights = getPermission(a),
                    account = a,
                    role_name = r.role_name
                };
                return u;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<int> getPermission(Account a)
        {
            try
            {
                List<int> data = new List<int>();
                List<RightByRole> rightRoles = db.RightByRoles.Where(x => x.role_id == a.role_id).ToList();
                List<baseRight> rightAccount = getRightByAccount(a.account_id);
                foreach (RightByRole r in rightRoles)
                {
                    data.Add(r.right_id);
                }
                foreach (baseRight r in rightAccount)
                {
                    data.Add(r.right_id);
                }
                return data;
            }
            catch (Exception e)
            {
                return new List<int>();
            }
        }
        public List<baseRight> getRightByAccount(int account_id)
        {
            try
            {
                string sql = @"select ar.right_id from General.AccountRight ar where ar.account_id = @account_id";
                List<baseRight> data = db.Database.SqlQuery<baseRight>(sql, new SqlParameter("account_id", account_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<baseRight>();
            }
        }
        public class User
        {
            public List<int> rights { get; set; }
            public string url { get; set; }
            public Account account { get; set; }
            public string role_name { get; set; }
        }
        public class baseRight
        {
            public int right_id { get; set; }
        }
    }
}