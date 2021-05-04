using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.Authen
{
    public class LoginRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public User GetAccount(int people_id, List<int> roleAccept)
        {
            try
            {
                db = new ScienceAndInternationalAffairsEntities();
                Account a = db.Profiles.Find(people_id).Account;
                Role r = db.Roles.Find(a.role_id);
                if (!roleAccept.Contains(r.role_id) && !roleAccept.Contains(0))
                {
                    return null;
                }
                Profile profile = db.Profiles.Where(x => x.account_id == a.account_id).FirstOrDefault();
                bool IsValid = profile != null;
                User u = new User
                {
                    url = r.url,
                    rights = GetPermission(a),
                    account = a,
                    role_name = r.role_name,
                    IsValid = IsValid,
                    PeopleID = profile?.people_id
                };
                return u;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        public User GetAccount(ENTITIES.CustomModels.Authen.Gmail user, List<int> roleAccept)
        {
            db = new ScienceAndInternationalAffairsEntities();
            try
            {
                Account a = db.Accounts.Where(x => x.email.Equals(user.email)).FirstOrDefault();
                if (a == null)
                {
                    return null;
                }
                Role r = db.Roles.Find(a.role_id);
                if (!roleAccept.Contains(r.role_id) && !roleAccept.Contains(0))
                {
                    return null;
                }
                if (!a.is_login)
                {
                    a.full_name = user.name;
                    a.google_id = user.id;
                    a.is_login = true;
                    a.picture = user.imageurl;
                    db.Entry(a).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //  Tài khoản sẽ sử dụng được mọi chức năng của bên GUEST/MANAGER nếu như có profile
                Profile profile = db.Profiles.Where(x => x.account_id == a.account_id).FirstOrDefault();
                bool IsValid = profile != null;
                User u = new User
                {
                    url = r.url,
                    rights = GetPermission(a),
                    account = a,
                    role_name = r.role_name,
                    IsValid = IsValid,
                    PeopleID = profile?.people_id
                };
                return u;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        public List<int> GetPermission(Account a)
        {
            db = new ScienceAndInternationalAffairsEntities();
            try
            {
                List<int> data = new List<int>();
                //List<RightByRole> rightRoles = db.RightByRoles.Where(x => x.role_id == a.role_id).ToList();
                List<baseRight> rightAccount = GetRightByAccount(a.account_id);
                //foreach (RightByRole r in rightRoles)
                //{
                //    data.Add(r.right_id);
                //}
                foreach (baseRight r in rightAccount)
                {
                    data.Add(r.right_id);
                }
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<int>();
            }
        }
        public List<baseRight> GetRightByAccount(int account_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            try
            {
                string sql = @"select ar.right_id from General.AccountRight ar where ar.account_id = @account_id";
                List<baseRight> data = db.Database.SqlQuery<baseRight>(sql, new SqlParameter("account_id", account_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<baseRight>();
            }
        }
        public static Profile GetProfileByAccount(Account account)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            Profile profile = db.Profiles.Where(x => x.account_id == account.account_id).FirstOrDefault();
            return profile;
        }
        public class User
        {
            public List<int> rights { get; set; }
            public string url { get; set; }
            public Account account { get; set; }
            public string role_name { get; set; }
            public bool IsValid { get; set; }
            public int? PeopleID { get; set; }
        }
        public class baseRight
        {
            public int right_id { get; set; }
        }
    }
}