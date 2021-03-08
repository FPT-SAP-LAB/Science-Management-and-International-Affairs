using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;
using BLL.ScienceManagement.ConferenceSponsor;
using GUEST.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ENTITIES;

namespace GUEST.Controllers
{
    public class ConferenceSponsorController : Controller
    {
        readonly ConferenceSponsorRepo repos = new ConferenceSponsorRepo();
        // GET: ConferenceSponsor
        public ActionResult Index()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        [HttpGet]
        public ActionResult Add()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
                new PageTree("Thêm","/ConferenceSponsor/Add"),
            };
            string output = repos.GetAddPageJson(LanguageResource.GetCurrentLanguageName());
            DataAddPage data = JsonConvert.DeserializeObject<DataAddPage>(output);
            ViewBag.data = data;
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        [HttpPost]
        public string Add(string input)
        {
            string output = repos.AddConference(input);
            return output;
        }
        public ActionResult Detail(int id)
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
                new PageTree("Chi tiết","/ConferenceSponsor/Detail"),
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.id = id;
            return View();
        }
        [ChildActionOnly]
        public ActionResult CostMenu(int id)
        {
            ViewBag.id = id;
            ViewBag.CheckboxColumn = id == 2;
            ViewBag.ReimbursementColumn = id >= 3;
            ViewBag.EditAble = id == 2;
            return PartialView();
        }
        public JsonResult GetInformationPeopleWithID(string id)
        {
            var infos = repos.GetAllProfileBy(id);
            return Json(infos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetConferenceWithName(string name)
        {
            var confer = repos.GetAllConferenceBy(name);
            return Json(confer, JsonRequestBehavior.AllowGet);
        }
        public class DataAddPage
        {
            public List<string> ConferenceCriteriaLanguages { get; set; }
            public List<Country> Countries { get; set; }
            public List<FormalityLanguage> FormalityLanguages { get; set; }
            public List<Office> Offices { get; set; }
            public List<TitleLanguage> TitleLanguages { get; set; }
            public string link { get; set; }
        }
    }
}