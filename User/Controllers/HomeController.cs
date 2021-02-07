using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace User.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Bảng tin","/"),
            };
            ViewBag.pagesTree = pagesTree;
            //var a = User_Application.Resources.en_US.ResourceManager;
            ViewBag.a = User_Application.Resources.en_US.ResourceManager;
            return View();
        }
    }
}