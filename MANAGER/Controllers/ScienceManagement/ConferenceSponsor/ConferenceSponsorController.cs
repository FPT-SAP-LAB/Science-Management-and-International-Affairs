using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class ConferenceSponsorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [ChildActionOnly]
        public ActionResult CostMenu(int id)
        {
            ViewBag.id = id;
            ViewBag.CheckboxColumn = id == 2;
            ViewBag.ReimbursementColumn = id >= 3;
            return PartialView();
        }
    }
}