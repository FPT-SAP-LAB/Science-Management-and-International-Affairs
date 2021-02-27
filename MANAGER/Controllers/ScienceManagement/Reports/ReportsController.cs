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
            ViewBag.title = "THỐNG KÊ BÀI BÁO KHOA HỌC THEO KHU VỰC";
            return View();
        }
        public ActionResult InternationalPapersReports()
        {
            ViewBag.title = "THỐNG KÊ KHEN THƯỞNG BÀI BÁO QUỐC TẾ ";
            return View();
        }
        //public ActionResult InCountryPapersReports()
        //{
        //    ViewBag.title = "THỐNG KÊ KHEN THƯỞNG BÀI BÁO QUỐC TẾ ";
        //    return View();
        //}
        //public ActionResult IntellectualPropertyReports()
        //{
        //    ViewBag.title = "THỐNG KÊ KHEN THƯỞNG BÀI BÁO QUỐC TẾ ";
        //    return View();
        //}

    }
}