﻿using System;
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
        LoginRepo repo;
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> SigninGoogleAsync(string idtoken)
        {
            repo = new LoginRepo();
            ENTITIES.CustomModels.Authen.Gmail user = await GetUserDetailsAsync(idtoken);
            List<int> roleAccept = new List<int>() { 1, 2, 3, 7 };
            LoginRepo.User u = repo.getAccount(user, roleAccept);
            if (u == null)
            {
                return Json(String.Empty);
            }
            if (u.account.role_id == 1)
                u.url = "/Report_IC/Dashboard";
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