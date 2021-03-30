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
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Conference;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using BLL.ModelDAL;

namespace GUEST.Controllers
{
    public class ConferenceSponsorController : Controller
    {
        readonly ConferenceSponsorAddRepo AppRepos = new ConferenceSponsorAddRepo();
        readonly ConferenceSponsorIndexRepo IndexRepos = new ConferenceSponsorIndexRepo();
        readonly ConferenceSponsorDetailRepo DetailRepos = new ConferenceSponsorDetailRepo();
        readonly CountryRepo countryRepo = new CountryRepo();
        readonly SpecializationLanguageRepo specializationLanguageRepo = new SpecializationLanguageRepo();
        readonly FormalityLanguageRepo formalityLanguageRepo = new FormalityLanguageRepo();
        readonly ConferenceCriteriaLanguageRepo criteriaLanguageRepo = new ConferenceCriteriaLanguageRepo();
        readonly RequestConferencePolicyRepo policyRepo = new RequestConferencePolicyRepo();
        readonly OfficeRepo officeRepo = new OfficeRepo();
        readonly TitleLanguageRepo titleRepo = new TitleLanguageRepo();

        private readonly List<PageTree> pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
            };
        public ActionResult Index()
        {
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        [AjaxOnly]
        public JsonResult List()
        {
            BaseDatatable datatable = new BaseDatatable(Request);
            BaseServerSideData<ConferenceIndex> output = IndexRepos.GetIndexPage(datatable, CurrentAccount.AccountID(Session), LanguageResource.GetCurrentLanguageID());
            for (int i = 0; i < output.Data.Count; i++)
            {
                output.Data[i].RowNumber = datatable.Start + 1 + i;
                output.Data[i].CreatedDate = output.Data[i].Date.ToString("dd/MM/yyyy");
            }
            return Json(new { success = true, data = output.Data, draw = Request["draw"], recordsTotal = output.RecordsTotal, recordsFiltered = output.RecordsTotal }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Add()
        {
            int language_id = LanguageResource.GetCurrentLanguageID();
            pagesTree.Add(new PageTree("Thêm", "/ConferenceSponsor/Add"));
            string output = AppRepos.GetAddPageJson(CurrentAccount.AccountID(Session), language_id);
            DataAddPage data = JsonConvert.DeserializeObject<DataAddPage>(output);
            if (data.Profile == null)
                return Redirect("/");

            ViewBag.data = data;
            ViewBag.countries = countryRepo.GetCountries();
            ViewBag.pagesTree = pagesTree;
            ViewBag.SpecializationLanguages = specializationLanguageRepo.GetList(language_id);
            ViewBag.FormalityLanguages = formalityLanguageRepo.GetList(language_id);
            ViewBag.ConferenceCriteriaLanguages = criteriaLanguageRepo.GetCurrentList(language_id);
            ViewBag.Link = policyRepo.GetCurrentLink();
            ViewBag.Offices = officeRepo.GetList();
            ViewBag.TitleLanguages = titleRepo.GetList(language_id);
            return View();
        }
        public ActionResult Edit(int id)
        {
            int language_id = LanguageResource.GetCurrentLanguageID();
            pagesTree.Add(new PageTree("Chỉnh sửa", "/ConferenceSponsor/Edit?id=" + id));
            string output = DetailRepos.GetDetailPageGuest(id, LanguageResource.GetCurrentLanguageID(), CurrentAccount.AccountID(Session));
            if (output == null)
                return Redirect("/ConferenceSponsor");

            ViewBag.pagesTree = pagesTree;
            ViewBag.output = output;
            ViewBag.countries = countryRepo.GetCountries();
            ViewBag.SpecializationLanguages = specializationLanguageRepo.GetList(language_id);
            ViewBag.FormalityLanguages = formalityLanguageRepo.GetList(language_id);
            ViewBag.ConferenceCriteriaLanguages = criteriaLanguageRepo.GetCurrentList(language_id);
            ViewBag.Link = policyRepo.GetCurrentLink();
            ViewBag.Offices = officeRepo.GetList();
            ViewBag.TitleLanguages = titleRepo.GetList(language_id);
            return View();
        }
        [HttpPost]
        public string Add(string input, HttpPostedFileBase invite, HttpPostedFileBase paper)
        {
            string output = AppRepos.AddRequestConference(CurrentAccount.AccountID(Session), input, invite, paper);
            return output;
        }
        public ActionResult Detail(int id)
        {
            pagesTree.Add(new PageTree("Chi tiết", "/ConferenceSponsor/Detail?id=" + id));
            string output = DetailRepos.GetDetailPageGuest(id, LanguageResource.GetCurrentLanguageID(), CurrentAccount.AccountID(Session));
            if (output == null)
                return Redirect("/ConferenceSponsor");

            ViewBag.pagesTree = pagesTree;
            ViewBag.output = output;
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
        [AjaxOnly]
        public JsonResult GetInformationPeopleWithID(string id)
        {
            var infos = AppRepos.GetAllProfileBy(id, LanguageResource.GetCurrentLanguageID());
            return Json(infos, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        public JsonResult GetConferenceWithName(string name)
        {
            var confer = AppRepos.GetAllConferenceBy(name);
            return Json(confer, JsonRequestBehavior.AllowGet);
        }
        public class DataAddPage
        {
            public ProfileResearcher Profile { get; set; }
        }
    }
}