using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.MOA
{
    public class MOAController : Controller
    {
        // GET: Partner
        public ActionResult Detail_MOA(string id)
        {
            ViewBag.pageTitle = "CHI TIẾT BIÊN BẢN THỎA THUẬN";
            return View();
        }
    }
}