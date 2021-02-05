using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace User.Controllers
{
    public class ListAllPaperController : Controller
    {
        // GET: ListAllPaper
        //[Route("paper/list")]
        public ActionResult Index()
        {
            ViewBag.title = "Sản phẩm khoa học";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Sản phẩm khoa học","/ListAllPaper"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}