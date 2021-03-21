using MANAGER.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Report
{
    public class Report_ICController : Controller
    {
        [Auther(RightID = "1")]
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}