using BLL.ScienceManagement.ConferenceSponsor;
using ENTITIES.CustomModels;
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
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
            BaseDatatable datatable = new BaseDatatable(Request);
            string output = IndexRepos.GetIndexPageManager(datatable);
            JObject json = JObject.Parse(output);
            List<DataIndex> data = json["data"].ToObject<List<DataIndex>>();
            for (int i = 0; i < data.Count; i++)
            {
                data[i].RowNumber = datatable.Start + 1 + i;
                data[i].CreatedDate = data[i].Date.ToString("dd/MM/yyyy");
            }
            int recordsTotal = json.Value<int>("recordsTotal");
            return Json(new { success = true, data, draw = Request["draw"], recordsTotal, recordsFiltered = recordsTotal }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id)
        {
            ViewBag.id = id;
            //switch (id)
            //{
            //    case 
            //    default:
            //        break;
            //}
            return View();
        }
        [ChildActionOnly]
        public ActionResult CostMenu(int id)
        {
            ViewBag.id = id;
            ViewBag.CheckboxColumn = id == 2;
            ViewBag.ReimbursementColumn = id >= 3;
            return PartialView();
        }
        private class DataIndex
        {
            public int RowNumber { get; set; }
            public string PaperName { get; set; }
            public string ConferenceName { get; set; }
            public DateTime Date { get; set; }
            public string CreatedDate { get; set; }
            public string StatusName { get; set; }
            public string FullName { get; set; }
            public int StatusID { get; set; }
        }
    }
}