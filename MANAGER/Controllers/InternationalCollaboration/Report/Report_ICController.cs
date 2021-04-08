using BLL.InternationalCollaboration.Dashboard;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Dashboard;
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
        DashboardRepo dashboardRepo;

        [Auther(RightID = "1")]
        public ActionResult Dashboard()
        {
            dashboardRepo = new DashboardRepo();
            List<ChartDashboard> dashboards = dashboardRepo.GetDashboard(DateTime.Now.Year);
            List<int?> year = new List<int?>();
            List<int?> signed = new List<int?>();
            List<int?> not_sign_yet = new List<int?>();
            foreach (var i in dashboards)
            {
                year.Add(i.year);
                signed.Add(i.signed);
                not_sign_yet.Add(i.not_sign_yet);
            }
            ViewBag.year = Json(new { year });
            ViewBag.signed = Json(new { signed });
            ViewBag.not_sign_yet = Json(new { not_sign_yet });
            int this_year = DateTime.Today.Year;
            ViewBag.widget_mou = dashboardRepo.WidgetMou(this_year);
            ViewBag.widget_collab = dashboardRepo.WidgetCollab(this_year);
            ViewBag.widget_support = dashboardRepo.WidgetSupport(this_year);
            return View();
        }

        public ActionResult LoadTable(int collab_type_id, int year)
        {
            try
            {
                dashboardRepo = new DashboardRepo();
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<DashboardDatatable> baseServerSideData =
                    dashboardRepo.GetTable(collab_type_id, year, baseDatatable);
                return Json(new
                {
                    success = true,
                    data = baseServerSideData.Data,
                    draw = Request["draw"],
                    recordsTotal = baseServerSideData.RecordsTotal,
                    recordsFiltered = baseServerSideData.RecordsTotal
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = e.Message });
            }
        }
    }
}