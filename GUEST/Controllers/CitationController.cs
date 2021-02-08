using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace User.Controllers
{
    public class CitationController : Controller
    {
        // GET: Citation
        public ActionResult List()
        {
            ViewBag.title = "Số trích dẫn";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Số trích dẫn","/Citation/List"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        public ActionResult AddRequest()
        {
            ViewBag.title = "Đề xuất khen thưởng số trích dẫn";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề xuất khen thưởng số trích dẫn","/Citation/AddRequest"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}