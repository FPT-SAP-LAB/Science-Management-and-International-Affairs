using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class PaperController : Controller
    {
        // GET: Paper
        public ActionResult Pending()
        {
            ViewBag.title = "Danh sách bài báo đang chờ xét duyệt";
            return View();
        }

        public ActionResult Detail()
        {
            ViewBag.title = "Chi tiết bài báo";
            return View();
        }
    }
}