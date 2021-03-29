using BLL.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Google.Apis.Auth;

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
                return Json(String.Empty);
            }
            if (u == null)
            {
                return Json("Tài khoản của bạn chưa được phép truy cập vào hệ thống. Vui lòng liên hệ người quản lý để biết thêm chi tiết.");
            }
            Session["User"] = u;
            return Json(String.Empty);
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