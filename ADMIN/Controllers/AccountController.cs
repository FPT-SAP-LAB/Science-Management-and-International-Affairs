using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENTITIES;

namespace ADMIN.Controllers
{
    public class AccountController : Controller
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public ActionResult List()
        {
            ViewBag.pageTitle = "Quản lí tài khoản";
            return View();
        }
    }
}