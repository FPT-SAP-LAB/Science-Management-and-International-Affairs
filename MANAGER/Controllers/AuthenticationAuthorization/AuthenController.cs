using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENTITIES;
using BLL;

namespace MANAGER.Controllers.AuthenticationAuthorization
{
    public class AuthenController : Controller
    {
        // GET: Authen
        private static BLL.Authen.Login authen = new BLL.Authen.Login();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SigninGoogle(ENTITIES.CustomModels.Authen.Gmail user)
        {
            string url = authen.getAccount(user);
            if (String.IsNullOrEmpty(url))
            {
                return Json("not allow", JsonRequestBehavior.AllowGet);
            }
            return Json(url, JsonRequestBehavior.AllowGet);
        }
    }
}