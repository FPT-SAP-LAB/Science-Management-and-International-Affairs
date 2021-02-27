using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.AuthenticationAuthorization
{
    public class AuthenController : Controller
    {
        // GET: Authen
        public ActionResult Login()
        {
            return View();
        }
    }
}