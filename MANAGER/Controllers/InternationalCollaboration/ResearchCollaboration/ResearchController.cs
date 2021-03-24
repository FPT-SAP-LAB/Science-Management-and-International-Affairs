using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.ResearchCollaboration
{
    public class ResearchController : Controller
    {
        // GET: Research
        public ActionResult Index()
        {
            ViewBag.pageTitle = "Danh sách hợp tác nghiên cứu";
            return View();
        }
    }
}