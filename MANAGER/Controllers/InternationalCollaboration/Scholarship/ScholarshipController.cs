using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Scholarship
{
    public class ScholarshipController : Controller
    {
        // GET: Scholarship
        public ActionResult List()
        {
            ViewBag.pageTitle = "Danh sách học bổng";
            return View();
        }
    }
}