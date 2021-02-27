using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class CitationController : Controller
    {
        // GET: Citation
        public ActionResult Pending()
        {
            ViewBag.title = "Danh sách trích dẫn đang chờ xét duyệt";
            return View();
        }

        public ActionResult Detail()
        {
            ViewBag.title = "Chi tiết trích dẫn";
            return View();
        }

        public ActionResult WaitDecision()
        {
            ViewBag.title = "Các trích dẫn đang chờ quyết định";
            return View();
        }
    }
}