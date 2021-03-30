//using MANAGER.Support;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using ENTITIES;
//using BLL.ScienceManagement.Report;
//using ENTITIES.CustomModels.ScienceManagement.Report;
//using ENTITIES.CustomModels;
//using System.Globalization;

//namespace MANAGER.Controllers.ScienceManagement.Reports
//{
//    public class ReportsController : Controller
//    {
//        RewardsReportRepo rewardsReportRepo;
//        //[Auther(RightID = "24")]
//        public ActionResult PapersReportsByWorkplace()
//        {
//            return View();
//        }
//        public ActionResult InternationalPapersReport()
//        {
//            return View();
//        }
//        public ActionResult InCountryPapersReport()
//        {
//            return View();
//        }
//        public ActionResult IntellectualPropertyReport()
//        {
//            return View();
//        }
//        public ActionResult CitationReport()
//        {
//            return View();
//        }
//        public ActionResult ConferencesParticipationReport()
//        {
//            return View();
//        }
//        public ActionResult ConferencesParticipationReport_tgtemp()
//        {
//            return View();
//        }
//        public ActionResult RewardByAuthorReport()
//        {
//            return View();
//        }
//        public ActionResult ListOfIncomePaid()
//        {
//            return View();
//        }
//        public ActionResult TotalBonusByYear()
//        {
//            return View();
//        }
//        public ActionResult Dashboard()
//        {
//            return View();
//        }
//        public ActionResult getIncountryReward()
//        {
//            rewardsReportRepo = new RewardsReportRepo();
//            BaseDatatable datatable = new BaseDatatable(Request);
//            BaseServerSideData<ArticlesInCountryReport> output = rewardsReportRepo.getAriticlesInCountryReport(datatable);
//            for (int i = 0; i < output.Data.Count; i++)
//            {
//                //output.Data[i].valid_date = DateTime.ParseExact(output.Data[i].valid_date, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();
//                output.Data[i].rownum = datatable.Start + 1 + i;
//            }
//            return Json(new { success = true, data = output.Data, draw = Request["draw"], recordsTotal = output.RecordsTotal, recordsFiltered = output.RecordsTotal }, JsonRequestBehavior.AllowGet);
//        }
//    }
//}