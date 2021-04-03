using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class AdditionalProfileController : Controller
    {
        private readonly List<PageTree> pagesTree = new List<PageTree>
            {
                new PageTree("Bổ sung thông tin","/AdditionalProfile"),
            };
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        [HttpPost]
        public ActionResult Add(string mssv_msnv, int title_id, string name, string email)
        {
            return Redirect("/");
        }
    }
}