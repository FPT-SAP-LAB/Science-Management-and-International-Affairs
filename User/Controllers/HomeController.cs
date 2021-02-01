using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace User.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            /////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////
            ViewBag.title = "Tổ chức giáo dục FPT - Khoa học & Hợp tác quốc tế";
            var pagesTree = new List<String> { "Trang chủ", "Bảng tin" };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}