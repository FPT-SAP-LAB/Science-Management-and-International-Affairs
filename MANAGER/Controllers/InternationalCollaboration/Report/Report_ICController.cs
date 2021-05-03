using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using BLL.InternationalCollaboration.Dashboard;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration.Dashboard;
using MANAGER.Support;
using System;
using System.Collections.Generic;
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
            int this_year = DateTime.Today.Year;
            //23 22 21 20 19 18 17 16 15
            List<int> year_select = new List<int>
            {
                this_year + 2,
                this_year + 1,
                this_year
            };
            for (int i = 1; i <= (this_year - 2015); i++)
            {
                year_select.Add(this_year - i);
            }
            ViewBag.year_select = year_select;

            //update noti for mou.
            new MOURepo().getNoti();
            return View();
        }

        [HttpPost]
        public ActionResult LoadData(string year_select)
        {
            try
            {
                int this_year = DateTime.Today.Year;
                if (year_select != null)
                {
                    this_year = Int32.Parse(year_select);
                }
                dashboardRepo = new DashboardRepo();
                List<ChartDashboard> dashboards = dashboardRepo.GetDashboard(this_year);
                List<int?> year = new List<int?>();
                List<int?> signed = new List<int?>();
                List<int?> not_sign_yet = new List<int?>();
                foreach (var i in dashboards)
                {
                    year.Add(i.year);
                    signed.Add(i.signed);
                    not_sign_yet.Add(i.not_sign_yet);
                }
                string widget_mou = dashboardRepo.WidgetMou(this_year);
                string widget_collab = dashboardRepo.WidgetCollab(this_year) + "";
                string widget_support = dashboardRepo.WidgetSupport(this_year) + "";
                return Json(new
                {
                    success = true,
                    year,
                    signed,
                    not_sign_yet,
                    widget_mou,
                    widget_collab,
                    widget_support
                });
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = e.Message });
            }
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