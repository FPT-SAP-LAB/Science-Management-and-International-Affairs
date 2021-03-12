using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityController : Controller
    {
        // GET: AcademicActivity
        public ActionResult Index()
        {
            ViewBag.title = "Hoạt động học thuật";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Hoạt động học thuật","/AcademicActivity/Index"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public ActionResult Detail(int id)
        {
            ViewBag.title = "Hoạt động học thuật";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Hoạt động học thuật","/AcademicActivity/Detail"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}