using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.Researchers
{
    public class ResearchersController : Controller
    {
        // GET: Researchers
        public ActionResult List()
        {
            return View();
        }
        public ActionResult ViewInfo()
        {
            return View("~/Views/Researchers/ResearcherInfo.cshtml");
        }
        public ActionResult Biography()
        {
            return View("~/Views/Researchers/ResearcherBio.cshtml");
        }
        public ActionResult Publications()
        {
            return View("~/Views/Researchers/ResearcherPublications.cshtml");
        }
        public ActionResult Rewards()
        {
            return View("~/Views/Researchers/ResearcherRewards.cshtml");
        }
    }
}