using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADMIN.Controllers
{
    public class RoleController : Controller
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public ActionResult Index()
        {
            ViewBag.pageTitle = "Quản lí chức danh";
            return View();
        }
    }
}