using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityController : Controller
    {
        // GET: AcademicActivity
        public ActionResult List()
        {
            ViewBag.pageTitle = "Danh sách hoạt động trong năm";
            return View();
        }
    }
}