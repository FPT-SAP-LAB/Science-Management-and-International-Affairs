using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;

namespace BLL.Authen
{
    public class Login
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public string getAccount(ENTITIES.CustomModels.Authen.Gmail user)
        {
            try
            {
                Account obj = db.Accounts.Where(x => x.email.Equals(user.email)).FirstOrDefault();
                if (obj == null)
                {
                    return String.Empty;
                }
                Account a = new Account()
                {
                    email = user.email,
                    full_name = user.name,
                    google_id = user.id,
                    picture = user.imageurl
                };
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
                string url = a.Role.role_name;
                return url;
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }
    }
}
