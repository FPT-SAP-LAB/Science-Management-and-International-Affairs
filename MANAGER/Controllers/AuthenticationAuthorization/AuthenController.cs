using BLL.Authen;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
namespace MANAGER.Controllers.AuthenticationAuthorization
{
    public class AuthenController : Controller
    {
        LoginRepo repo;
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                repo = new LoginRepo();
                var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
                ENTITIES.CustomModels.Authen.Gmail user = new ENTITIES.CustomModels.Authen.Gmail
                {
                    email = userClaims?.FindFirst("preferred_username")?.Value,
                    id = String.Empty,
                    name = userClaims?.FindFirst("name")?.Value,
                };
                List<int> roleAccept = new List<int>() { 2, 3 };
                LoginRepo.User u = repo.getAccount(user, roleAccept);
                if (u != null)
                {
                    Session["User"] = u;
                    return Redirect(u.url);
                }
                else
                {
                    return View();
                }
            }
            else
                return View();
        }
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);
            Session.Abandon();
            return RedirectToAction("Login");
        }
        //[HttpPost]
        //public async System.Threading.Tasks.Task<ActionResult> SigninGoogleAsync(string idtoken)
        //{
        //    repo = new LoginRepo();
        //    ENTITIES.CustomModels.Authen.Gmail user = await GetUserDetailsAsync(idtoken);
        //    List<int> roleAccept = new List<int>() { 2, 3 };
        //    LoginRepo.User u = repo.getAccount(user, roleAccept);
        //    if (u == null)
        //    {
        //        return Json(String.Empty);
        //    }
        //    Session["User"] = u;
        //    return Redirect(u.url);
        //}
        //public async System.Threading.Tasks.Task<ENTITIES.CustomModels.Authen.Gmail> GetUserDetailsAsync(string providerToken)
        //{
        //    GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(providerToken);
        //    if (!payload.Audience.Equals("24917390994-co652l6gu3eeoqaf96oc9h4av23eprot.apps.googleusercontent.com"))
        //        return null;
        //    if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
        //        return null;
        //    if (payload.ExpirationTimeSeconds == null)
        //        return null;
        //    else
        //    {
        //        DateTime now = DateTime.Now.ToUniversalTime();
        //        DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
        //        if (now > expiration)
        //        {
        //            return null;
        //        }
        //    }
        //    return new ENTITIES.CustomModels.Authen.Gmail
        //    {
        //        email = payload.Email,
        //        id = payload.Subject,
        //        name = payload.Name,
        //        imageurl = payload.Picture
        //    };
        //}
    }
}