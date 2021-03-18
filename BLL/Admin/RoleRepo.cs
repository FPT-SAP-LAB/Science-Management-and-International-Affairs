using ENTITIES;
using ENTITIES.CustomModels;
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
        public List<infoRole> getRoles()
        {
            try
            {
                List<infoRole> data = db.Database.SqlQuery<infoRole>("select * from General.[Role]").ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<infoRole>();
            }
        }
        public List<baseRight> getRightByRole(int role_id)
        {
            try
            {
                string sql = @"select rt.right_id,rt.right_name,rt.module_id from General.RightByRole rr inner join General.[Role] r on rr.role_id = r.role_id
                                inner join General.[Right] rt on rr.right_id = rt.right_id where r.role_id = @role_id";
                List<baseRight> data = db.Database.SqlQuery<baseRight>(sql, new SqlParameter("role_id", role_id)).ToList();
                return data;
            }
            catch (Exception e)
            {
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
                return new List<Right>();
            }
        }
        public Role GetBaseRole(int role_id)
        {
            try
            {
                Role data = db.Roles.Find(role_id);
                return data;
            }
            catch (Exception e)
            {
                return new Role();
            }
        }
        public bool add(baseRole obj)
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
                return false;
            }
        }
        public bool edit(Role obj)
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
                return false;
            }
        }
        public bool delete(int role_id)
        {
            try
            {
                Role r = db.Roles.Find(role_id);
                db.Roles.Remove(r);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        //public bool UpdateRight(int[] arrAccept,int role_id)
        //{
        //    try
        //    {
        //        foreach(int x in arrAccept)
        //        {

        //        }
        //        return true;
        //    }catch(Exception e)
        //    {
        //        return false;
        //    }
        //}
        public class baseRight
        {
            public int right_id { get; set; }
            public string right_name { get; set; }
            public int module_id { get; set; }
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
