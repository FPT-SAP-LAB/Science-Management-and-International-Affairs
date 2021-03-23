using BLL.Authen;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADMIN.Controllers
{
    public class HomeController : Controller
    {
        LoginRepo repo;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> SigninGoogleAsync(string idtoken)
        {
            repo = new LoginRepo();
            ENTITIES.CustomModels.Authen.Gmail user = await GetUserDetailsAsync(idtoken);
            List<int> roleAccept = new List<int>() { 1 };
            LoginRepo.User u = repo.getAccount(user, roleAccept);
            if (u == null)
            {
                return Json(String.Empty);
            }
            u.rights.Add(0);
            Session["User"] = u;
            return Redirect(u.url);
        }
        public async System.Threading.Tasks.Task<ENTITIES.CustomModels.Authen.Gmail> GetUserDetailsAsync(string providerToken)
        {
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(providerToken);
            if (!payload.Audience.Equals("24917390994-co652l6gu3eeoqaf96oc9h4av23eprot.apps.googleusercontent.com"))
                return null;
            if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
                return null;
            if (payload.ExpirationTimeSeconds == null)
                return null;
            else
            {
                DateTime now = DateTime.Now.ToUniversalTime();
                DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
                if (now > expiration)
                {
                    return null;
                }
            }
            return new ENTITIES.CustomModels.Authen.Gmail
            {
                email = payload.Email,
                id = payload.Subject,
                name = payload.Name,
                imageurl = payload.Picture
            };
        }
    }
}