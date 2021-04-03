using ENTITIES.CustomModels;
using System.Collections.Generic;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
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
            return View();
        }
    }
}
