using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.Reports
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult PapersReportsByWorkplace()
        {
            return View();
        }
        public ActionResult InternationalPapersReport()
        {
            return View();
        }
        public ActionResult InCountryPapersReport()
        {
            return View();
        }
        public ActionResult IntellectualPropertyReport()
        {
            return View();
        }
        public ActionResult CitationReport()
        {
            return View();
        }
        public ActionResult ConferencesParticipationReport()
        {
            return View();
        }
        public ActionResult ConferencesParticipationReport_tgtemp()
        {
            return View();
        }
        public ActionResult RewardByAuthorReport()
        {
            return View();
        }
    }
}