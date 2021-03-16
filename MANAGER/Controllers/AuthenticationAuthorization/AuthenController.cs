using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENTITIES;
using BLL.Authen;
using Google.Apis.Auth;

namespace MANAGER.Controllers.AuthenticationAuthorization
{
    public class AuthenController : Controller
    {
        // GET: Authen
        private static Login authen = new Login();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> SigninGoogleAsync(string idtoken)
        {
            ENTITIES.CustomModels.Authen.Gmail user = await GetUserDetailsAsync(idtoken);
            string url = authen.getAccount(user);
            if (String.IsNullOrEmpty(url))
            {
                return RedirectToAction("Login");
            }
            return Redirect(url);
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