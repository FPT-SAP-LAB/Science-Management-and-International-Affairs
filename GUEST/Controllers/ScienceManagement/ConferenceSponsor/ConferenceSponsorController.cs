//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using User.Models;
//using BLL.ScienceManagement.ConferenceSponsor;
//using GUEST.Models;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
//using ENTITIES;
//using ENTITIES.CustomModels;
//using ENTITIES.CustomModels.ScienceManagement.Conference;

//namespace GUEST.Controllers
//{
//    public class ConferenceSponsorController : Controller
//    {
//        readonly ConferenceSponsorAddRepo AppRepos = new ConferenceSponsorAddRepo();
//        readonly ConferenceSponsorIndexRepo IndexRepos = new ConferenceSponsorIndexRepo();
//        readonly ConferenceSponsorDetailRepo DetailRepos = new ConferenceSponsorDetailRepo();
//        // GET: ConferenceSponsor
//        public ActionResult Index()
//        {
//            var pagesTree = new List<PageTree>
//            {
//                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
//            };
//            ViewBag.pagesTree = pagesTree;
//            return View();
//        }
//        public JsonResult List()
//        {
//            BaseDatatable datatable = new BaseDatatable(Request);
//            BaseServerSideData<ConferenceIndex> output = IndexRepos.GetIndexPageGuest(datatable, 6, LanguageResource.GetCurrentLanguageID());
//            for (int i = 0; i < output.Data.Count; i++)
//            {
//                output.Data[i].RowNumber = datatable.Start + 1 + i;
//                output.Data[i].CreatedDate = output.Data[i].Date.ToString("dd/MM/yyyy");
//            }
//            return Json(new { success = true, data = output.Data, draw = Request["draw"], recordsTotal = output.RecordsTotal, recordsFiltered = output.RecordsTotal }, JsonRequestBehavior.AllowGet);
//        }
//        [HttpGet]
//        public ActionResult Add()
//        {
//            var pagesTree = new List<PageTree>
//            {
//                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
//                new PageTree("Thêm","/ConferenceSponsor/Add"),
//            };
//            string output = AppRepos.GetAddPageJson(LanguageResource.GetCurrentLanguageName());
//            DataAddPage data = JsonConvert.DeserializeObject<DataAddPage>(output);
//            ViewBag.data = data;
//            ViewBag.pagesTree = pagesTree;
//            return View();
//        }
//        [HttpPost]
//        public string Add(string input, HttpPostedFileBase invite, HttpPostedFileBase paper)
//        {
//            string output = AppRepos.AddRequestConference(input, invite, paper);
//            return output;
//        }
//        public ActionResult Detail(int id)
//        {
//            var pagesTree = new List<PageTree>
//            {
//                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
//                new PageTree("Chi tiết","/ConferenceSponsor/Detail"),
//            };
//            string output = DetailRepos.GetDetailPageGuest(id, 6, LanguageResource.GetCurrentLanguageID());
//            ViewBag.pagesTree = pagesTree;
//            ViewBag.output = output;
//            return View();
//        }
//        [ChildActionOnly]
//        public ActionResult CostMenu(int id)
//        {
//            ViewBag.id = id;
//            ViewBag.CheckboxColumn = id == 2;
//            ViewBag.ReimbursementColumn = id >= 3;
//            ViewBag.EditAble = id == 2;
//            return PartialView();
//        }
//        public JsonResult GetInformationPeopleWithID(string id)
//        {
//            var infos = AppRepos.GetAllProfileBy(id);
//            return Json(infos, JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult GetConferenceWithName(string name)
//        {
//            var confer = AppRepos.GetAllConferenceBy(name);
//            return Json(confer, JsonRequestBehavior.AllowGet);
//        }
//        public class DataAddPage
//        {
//            public List<string> ConferenceCriteriaLanguages { get; set; }
//            public List<Country> Countries { get; set; }
//            public List<FormalityLanguage> FormalityLanguages { get; set; }
//            public List<Office> Offices { get; set; }
//            public List<TitleLanguage> TitleLanguages { get; set; }
//            public string Link { get; set; }
//        }
//    }
//}