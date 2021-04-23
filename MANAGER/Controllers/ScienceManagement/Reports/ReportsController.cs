using BLL.ModelDAL;
using BLL.ScienceManagement.Report;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.Report;
using ENTITIES.CustomModels.ScienceManagement.SearchFilter;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.Reports
{
    public class ReportsController : Controller
    {
        RewardsReportRepo rewardsReportRepo;
        //[Auther(RightID = "24")]
        public ActionResult PapersReportsByWorkplace()
        {
            return View();
        }
        public ActionResult InternationalPapersReport()
        {
            OfficeRepo o = new OfficeRepo();
            rewardsReportRepo = new RewardsReportRepo();
            List<Office> listOffices = o.GetList();
            ViewBag.listOffices = listOffices;
            List<String> years = rewardsReportRepo.getListYearPaper();
            List<PaperCriteria> criterias = rewardsReportRepo.getListCriteria();
            ViewBag.years = years;
            ViewBag.criterias = criterias;
            return View();
        }
        public ActionResult InCountryPapersReport()
        {
            OfficeRepo o = new OfficeRepo();
            rewardsReportRepo = new RewardsReportRepo();
            List<Office> listOffices = o.GetList();
            ViewBag.listOffices = listOffices;
            List<String> years = rewardsReportRepo.getListYearPaper();
            ViewBag.years = years;
            return View();
        }
        public ActionResult IntellectualPropertyReport()
        {
            OfficeRepo o = new OfficeRepo();
            rewardsReportRepo = new RewardsReportRepo();
            List<Office> listOffices = o.GetList();
            ViewBag.listOffices = listOffices;
            List<String> years = rewardsReportRepo.getListYear(50);
            ViewBag.years = years;
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
            OfficeRepo o = new OfficeRepo();
            rewardsReportRepo = new RewardsReportRepo();
            List<Office> listOffices = o.GetList();
            ViewBag.listOffices = listOffices;
            List<String> years = rewardsReportRepo.getListYearPaper();
            ViewBag.years = years;
            return View();
        }
        public ActionResult ListOfIncomePaid()
        {
            return View();
        }
        public ActionResult TotalBonusByYear()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public JsonResult getAwardMoneyInoutCountry()
        {
            try
            {
                int? office_id;
                int? hang;
                int? type;
                if (Request["coso"] == null || Request["coso"] == "") { office_id = null; }
                else { office_id = Int32.Parse(Request["coso"]); }
                /////////////////////////////////////////////////////////////////////////
                if (Request["hang"] == null || Request["hang"] == "") { hang = null; }
                else { hang = Int32.Parse(Request["hang"]); }
                /////////////////////////////////////////////////////////////////////////
                if (Request["type"] == null || Request["type"] == "") { type = null; }
                else { type = Int32.Parse(Request["type"]); }
                //////////////////////////////////////////////////////////////////////////
                string name = Request["name"];
                string year = Request["year"];
                SearchFilter searchs = new SearchFilter()
                {
                    office_id = office_id,
                    name = name,
                    year = year,
                    hang = hang
                };
                rewardsReportRepo = new RewardsReportRepo();
                BaseDatatable datatable = new BaseDatatable(Request);
                Tuple<BaseServerSideData<ArticlesInoutCountryReports>, String> output = rewardsReportRepo.getAriticlesByAreaReports(datatable, searchs, type);
                for (int i = 0; i < output.Item1.Data.Count; i++)
                {
                    output.Item1.Data[i].valid_date_string = output.Item1.Data[i].valid_date.ToString("dd/MM/yyyy");
                    output.Item1.Data[i].rownum = datatable.Start + 1 + i;
                }
                return Json(new { success = true, total = output.Item2, data = output.Item1.Data, draw = Request["draw"], recordsTotal = output.Item1.RecordsTotal, recordsFiltered = output.Item1.RecordsTotal });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = e.Message
                });
            }
        }
        public JsonResult getAwardByAuthors()
        {
            try
            {
                int? office_id;
                if (Request["coso"] == null || Request["coso"] == "")
                {
                    office_id = null;
                }
                else
                {
                    office_id = Int32.Parse(Request["coso"]);
                }
                SearchFilter searchs = new SearchFilter()
                {
                    office_id = office_id,
                    name = Request["name"].ToString(),
                    year = Request["year"]
                };
                rewardsReportRepo = new RewardsReportRepo();
                BaseDatatable datatable = new BaseDatatable(Request);
                BaseServerSideData<ReportByAuthorAward> data = rewardsReportRepo.getAwardReportByAuthor(datatable, searchs);
                for (int i = 0; i < data.Data.Count; i++)
                {
                    data.Data[i].rowNum = datatable.Start + 1 + i;
                    data.Data[i].paperAward = data.Data[i].paperAward == "" ? "0" : data.Data[i].paperAward;
                    data.Data[i].inventionAmount = data.Data[i].inventionAmount == "" ? "0" : data.Data[i].inventionAmount;
                    data.Data[i].CitationAward = data.Data[i].CitationAward == "" ? "0" : data.Data[i].CitationAward;
                }
                return Json(new { success = true, data = data.Data, draw = Request["draw"], recordsTotal = data.RecordsTotal, recordsFiltered = data.RecordsTotal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = e.Message
                });
            }
        }
        public JsonResult getIntellectualPropertyReport()
        {
            try
            {
                int? office_id;
                int? hang;
                if (Request["coso"] == null || Request["coso"] == "") { office_id = null; }
                else { office_id = Int32.Parse(Request["coso"]); }
                //////////////////////////////////////////////////////////////////////////
                string name = Request["name"];
                string year = Request["year"];
                SearchFilter searchs = new SearchFilter()
                {
                    office_id = office_id,
                    name = name,
                    year = year,
                };
                rewardsReportRepo = new RewardsReportRepo();
                BaseDatatable datatable = new BaseDatatable(Request);
                Tuple<BaseServerSideData<IntellectualPropertyReport>, String> output = rewardsReportRepo.getIntellectualPropertyReport(datatable, searchs);
                for (int i = 0; i < output.Item1.Data.Count; i++)
                {
                    output.Item1.Data[i].date_string = output.Item1.Data[i].date.Value.ToString("dd/MM/yyyy");
                    output.Item1.Data[i].rownum = datatable.Start + 1 + i;
                }
                return Json(new { success = true, total = output.Item2, data = output.Item1.Data, draw = Request["draw"], recordsTotal = output.Item1.RecordsTotal, recordsFiltered = output.Item1.RecordsTotal });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = e.Message
                });
            }
        }
    }
}