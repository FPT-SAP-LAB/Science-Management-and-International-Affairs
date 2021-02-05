using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace User.Controllers
{
    public class ListAllPaperController : Controller
    {
        // GET: ListAllPaper
        [Route("paper/list")]
        public ActionResult Index()
        {
            ViewBag.title = "Sản phẩm khoa học";
            var pagesTree = new List<String> { "Trang chủ", "Sản phẩm khoa học" };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}