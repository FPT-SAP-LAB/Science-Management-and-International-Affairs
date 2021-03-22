using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.Admin
{
    public class RightRepo
    {
        public BaseServerSideData<infoRight> getRights(BaseDatatable baseDatatable)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    string sql = @"select r.right_id,r.right_name,m.module_name,r.group_id
                                from General.[Right] r inner join General.Module m on r.module_id = m.module_id 
                                ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY";
                    int recordTotal = db.Database.SqlQuery<int>("select count(*) from General.[Right] r inner join General.Module m on r.module_id = m.module_id").FirstOrDefault();
                    List<infoRight> data = db.Database.SqlQuery<infoRight>(sql).ToList();
                    return new BaseServerSideData<infoRight>(data, recordTotal);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new BaseServerSideData<infoRight>(new List<infoRight>(), 0);
            }
        }
        public List<Module> getModules()
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    List<Module> data = db.Modules.ToList();
                    return data;
                }
            }
            catch (Exception e)
            {
                return new List<Module>();
            }
        }
        public Right GetBaseRight(int right_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    Right data = db.Rights.Find(right_id);
                    return data;
                }
            }
            catch (Exception e)
            {
                return new Right();
            }
        }
        public bool add(baseRight obj)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Rights.Add(new Right
                    {
                        right_name = obj.right_name,
                        group_id = obj.group_id,
                        module_id = int.Parse(obj.module_name)
                    });
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool edit(infoRight obj)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    Right r = db.Rights.Find(obj.right_id);
                    r.right_name = obj.right_name;
                    r.module_id = int.Parse(obj.module_name);
                    r.group_id = obj.group_id;
                    db.Entry(r).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool delete(int right_id)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    Right r = db.Rights.Find(right_id);
                    db.Rights.Remove(r);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public class infoRight : baseRight
        {
            public int right_id { get; set; }
        }
        public class baseRight
        {
            public string right_name { get; set; }
            public string module_name { get; set; }
            public int group_id { get; set; }
        }
    }
}