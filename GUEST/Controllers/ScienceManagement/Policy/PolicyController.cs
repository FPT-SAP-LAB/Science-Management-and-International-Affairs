using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers.ScienceManagement.Policy
{
    public class PolicyController : Controller
    {
        // GET: Policy
        public ActionResult Index()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Thông tin chính sách","/Policy"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}