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
        public ActionResult InternationalPapersReports()
        {
            return View();
        }
        public ActionResult InCountryPapersReports()
        {
            return View();
        }
        public ActionResult IntellectualPropertyReports()
        {

            return View();
        }

    }
}