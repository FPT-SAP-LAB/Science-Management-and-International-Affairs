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
        readonly ConferenceSponsorIndexRepo IndexRepos = new ConferenceSponsorIndexRepo();
        readonly ConferenceSponsorDetailRepo DetailRepos = new ConferenceSponsorDetailRepo();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
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
            string output = DetailRepos.GetDetailPageGuest(id, 1);
            ViewBag.output = output;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateCriterias(string criterias, int request_id)
        {
            DetailRepos.UpdateCriterias(criterias, request_id);
            return Redirect("/ConferenceSponsor");
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