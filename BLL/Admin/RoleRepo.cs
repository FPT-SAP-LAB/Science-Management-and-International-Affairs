using ENTITIES;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.Admin
{
    public class RoleRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<infoRole> GetRoles()
        {
            try
            {
                List<infoRole> data = db.Database.SqlQuery<infoRole>("select * from General.[Role]").ToList();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new List<infoRole>();
            }
        }
        public List<baseRight> GetRightByRole(int role_id)
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
        public List<Right> GetRightsByModule(int module_id)
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
        public infoRole GetBaseRole(int role_id)
        {
            try
            {
                infoRole data = db.Database.SqlQuery<infoRole>("select r.role_name,r.url from General.[Role] r where r.role_id = @role_id",
                                                new SqlParameter("role_id", role_id)).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new infoRole();
            }
        }
        public bool Add(baseRole obj)
        {
            try
            {
                db.Roles.Add(new Role
                {
                    role_name = obj.role_name,
                    url = obj.url
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
        public bool Edit(infoRole obj)
        {
            try
            {
                Role r = db.Roles.Find(obj.role_id);
                r.role_name = obj.role_name;
                r.url = obj.url;
                db.Entry(r).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        public bool Delete(int role_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<RightByRole> rv = db.RightByRoles.Where(x => x.role_id == role_id).ToList();
                    db.RightByRoles.RemoveRange(rv);
                    db.SaveChanges();
                    Role r = db.Roles.Find(role_id);
                    db.Roles.Remove(r);
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
        public bool UpdateRight(int[] arrAccept, int role_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    string sql = @"delete from General.[RightByRole] where role_id = @role_id";
                    db.Database.ExecuteSqlCommand(sql, new SqlParameter("role_id", role_id));
                    db.SaveChanges();
                    if (arrAccept != null)
                    {
                        foreach (int x in arrAccept)
                        {
                            db.RightByRoles.Add(new RightByRole
                            {
                                right_id = x,
                                role_id = role_id
                            });
                        }
                        db.SaveChanges();
                    }
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
        public class baseRight
        {
            public int right_id { get; set; }
            public string right_name { get; set; }
        }
        public class baseRole
        {
            public string role_name { get; set; }
            public string url { get; set; }
        }
        public class infoRole
        {
            public int role_id { get; set; }
            public string role_name { get; set; }
            public string url { get; set; }
        }
    }
}
