using BLL.Authen;
using BLL.ScienceManagement.ConferenceSponsor;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Conference;
using MANAGER.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class ConferenceSponsorController : Controller
    {
        ConferenceSponsorIndexRepo IndexRepos;
        ConferenceSponsorDetailRepo DetailRepos;
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
            IndexRepos = new ConferenceSponsorIndexRepo();
            BaseDatatable datatable = new BaseDatatable(Request);
            BaseServerSideData<ConferenceIndex> output = IndexRepos.GetIndexPage(datatable);
            for (int i = 0; i < output.Data.Count; i++)
            {
                output.Data[i].RowNumber = datatable.Start + 1 + i;
                output.Data[i].CreatedDate = output.Data[i].Date.ToString("dd/MM/yyyy");
            }
            return Json(new { success = true, data = output.Data, draw = Request["draw"], recordsTotal = output.RecordsTotal, recordsFiltered = output.RecordsTotal }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            ViewBag.output = DetailRepos.GetDetailPageGuest(id, 1);
            return View();
        }
        [HttpPost]
        public ActionResult UpdateCriterias(string data, int request_id, string comment)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            DetailRepos.UpdateCriterias(data, request_id, CurrentAccount.AccountID(Session), comment);
            return Redirect("/ConferenceSponsor/Detail?id=" + request_id);
        }
        [HttpPost]
        public ActionResult RequestEdit(int request_id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            DetailRepos.RequestEdit(request_id);
            return Redirect("/ConferenceSponsor/Detail?id=" + request_id);
        }
        [HttpPost]
        public ActionResult UpdateCosts(string data, int request_id, string comment)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            DetailRepos.UpdateCosts(data, request_id, CurrentAccount.AccountID(Session), comment);
            return Redirect("/ConferenceSponsor/Detail?id=" + request_id);
        }
        [HttpPost]
        public JsonResult SubmitPolicy(HttpPostedFileBase decision_file, string valid_date, string decision_number, int request_id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            AlertModal<string> result = DetailRepos.SubmitPolicy(decision_file, valid_date, decision_number, request_id, CurrentAccount.AccountID(Session));
            return Json(result);
        }
        [HttpPost]
        public ActionResult UpdateReimbursement(string data, int request_id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            DetailRepos.SubmitReimbursement(data, request_id);
            return Redirect("/ConferenceSponsor/Detail?id=" + request_id);
        }
        [ChildActionOnly]
        public ActionResult CostMenu(int id)
        {
            ViewBag.id = id;
            ViewBag.CheckboxColumn = id == 2;
            ViewBag.ReimbursementColumn = id >= 3;
            return PartialView();
        }
    }
}