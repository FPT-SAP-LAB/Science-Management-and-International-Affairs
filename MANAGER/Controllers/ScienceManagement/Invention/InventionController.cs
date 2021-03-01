using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class InventionController : Controller
    {
        // GET: Invention
        public ActionResult Pending()
        {
            ViewBag.title = "Danh sách bằng sáng chế đang chờ xét duyệt";
            return View();
        }

        public ActionResult Detail()
        {
            ViewBag.title = "Chi tiết bằng sáng chế";
            return View();
        }

        public ActionResult WaitDecision()
        {
            ViewBag.title = "Chờ quyết định khen thưởng";
            return View();
        }
    }
}