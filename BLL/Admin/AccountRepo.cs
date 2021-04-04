using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.Admin
{
    public class AccountRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<extendAccount> getAccounts()
        {
            try
            {
                string sql = @"select a.account_id,a.full_name,a.email,r.role_name,case when a.is_login = 0 then N'Chưa kích hoạt' 
                                when a.is_login = 1 then N'Đã kích hoạt' end as 'is_login' 
                                from General.Account a inner join General.[Role] r on r.role_id = a.role_id";
                List<extendAccount> data = db.Database.SqlQuery<extendAccount>(sql).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<extendAccount>();
            }
        }
        public List<baseRight> getRightByAccount(int account_id)
        {
            try
            {
                Account a = db.Accounts.Find(account_id);
                List<baseRight> roleA = getRightByRole(a.role_id);
                string sql = @"select r.right_id,r.right_name from General.[Right] r inner join General.AccountRight ar on r.right_id = ar.right_id
                                inner join General.Account a on a.account_id = ar.account_id where a.account_id = @account_id";
                List<baseRight> data = db.Database.SqlQuery<baseRight>(sql, new SqlParameter("account_id", account_id)).ToList();
                data.AddRange(roleA);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<baseRight>();
            }
        }
        public List<baseRight> getRightByRole(int role_id)
        {
            try
            {
                string sql = @"select rt.right_id,rt.right_name from General.RightByRole rr inner join General.[Role] r on rr.role_id = r.role_id
                                inner join General.[Right] rt on rr.right_id = rt.right_id where r.role_id = @role_id";
                List<baseRight> data = db.Database.SqlQuery<baseRight>(sql, new SqlParameter("role_id", role_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<baseRight>();
            }
        }
        public List<Right> getRightsByModule(int module_id)
        {
            try
            {
                List<Right> data = db.Rights.Where(x => x.module_id == module_id).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<Right>();
            }
        }
        public baseAccount GetBaseAccount(int account_id)
        {
            try
            {
                baseAccount data = db.Database.SqlQuery<baseAccount>("select a.email,a.role_id from General.Account a where a.account_id = @account_id",
                                                new SqlParameter("account_id", account_id)).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new baseAccount();
            }
        }
        public bool add(baseAccount obj)
        {
            try
            {
                db.Accounts.Add(new Account
                {
                    email = obj.email,
                    is_login = false,
                    role_id = obj.role_id
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        public string edit(infoAccount obj)
        {
            try
            {
                Account a = db.Accounts.Find(obj.account_id);
                if (a.role_id == 1)
                {
                    List<Account> num = db.Accounts.Where(x => x.role_id == 1).ToList();
                    if (num.Count == 1)
                    {
                        return "cons";
                    }
                }
                a.email = obj.email;
                a.role_id = obj.role_id;
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
                return "ok";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return String.Empty;
            }
        }
        public string delete(int account_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Account a = db.Accounts.Find(account_id);
                    if (a.role_id == 1)
                    {
                        List<Account> num = db.Accounts.Where(x => x.role_id == 1).ToList();
                        if (num.Count == 1)
                        {
                            return "cons";
                        }
                    }
                    List<AccountRight> rv = db.AccountRights.Where(x => x.account_id == account_id).ToList();
                    db.AccountRights.RemoveRange(rv);
                    db.SaveChanges();
                    db.Accounts.Remove(a);
                    db.SaveChanges();
                    transaction.Commit();
                    return "ok";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return String.Empty;
                }
            }
        }
        public bool UpdateRight(int[] arrAccept, int account_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<AccountRight> rv = db.AccountRights.Where(x => x.account_id == account_id).ToList();
                    db.AccountRights.RemoveRange(rv);
                    db.SaveChanges();
                    Account a = db.Accounts.Find(account_id);
                    List<baseRight> rightRole = getRightByRole(a.role_id);
                    List<int> Accepts = new List<int>(arrAccept);
                    Accepts.RemoveAll(x => rightRole.Any(y => y.right_id == x));
                    foreach (int x in Accepts)
                    {
                        db.AccountRights.Add(new AccountRight
                        {
                            right_id = x,
                            account_id = account_id
                        });
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public List<baseRole> getRoles()
        {
            try
            {
                string sql = @"select r.role_id, r.role_name from General.[Role] r";
                List<baseRole> data = db.Database.SqlQuery<baseRole>(sql).ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<baseRole>();
            }
        }
        public class baseRight
        {
            public int right_id { get; set; }
            public string right_name { get; set; }
        }
        public class baseRole
        {
            public int role_id { get; set; }
            public string role_name { get; set; }
        }
        public class baseAccount
        {
            public string email { get; set; }
            public int role_id { get; set; }
        }
        public class infoAccount : baseAccount
        {
            public int account_id { get; set; }
        }
        public class extendAccount
        {
            public int account_id { get; set; }
            public string full_name { get; set; }
            public string email { get; set; }
            public string role_name { get; set; }
            public string is_login { get; set; }
        }
    }
}
