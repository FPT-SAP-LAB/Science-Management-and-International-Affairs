using BLL.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth;
using BLL.Admin;

namespace GUEST.Controllers.AuthenticationAuthorization
{
    public class AuthenController : Controller
    {
        LoginRepo repo;
        [HttpPost]
        public ActionResult Logout()
        {
            Session.Abandon();
            return Json("success");
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> SigninGoogleAsync(string idtoken)
        {
            repo = new LoginRepo();
            ENTITIES.CustomModels.Authen.Gmail user = await GetUserDetailsAsync(idtoken);
            List<int> roleAccept = new List<int>() { 0 };
            LoginRepo.User u = repo.getAccount(user, roleAccept);
            if (u == null)
            {
                string EmailDomain = user.email.Split('@').Last();
                int role_id;
                if (EmailDomain.Equals("fpt.edu.vn"))
                    role_id = 5;
                else if (EmailDomain.Equals("fe.edu.vn"))
                    role_id = 6;
                else
                    return Json(new { success = false, content = "Tài khoản của bạn không được phép truy cập vào hệ thống" });
                AccountRepo accountRepo = new AccountRepo();
                accountRepo.add(new AccountRepo.baseAccount
                {
                    email = user.email,
                    role_id = role_id
                });
                u = repo.getAccount(user, roleAccept);
            }
            Session["User"] = u;
            if (u.IsValid)
                return Json(new { success = true, redirect = false });
            else
                return Json(new { success = true, redirect = true });
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