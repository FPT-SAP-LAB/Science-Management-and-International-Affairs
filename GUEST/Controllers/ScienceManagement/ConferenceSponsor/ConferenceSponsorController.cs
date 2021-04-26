using BLL.ModelDAL;
using BLL.ScienceManagement.ConferenceSponsor;
using ENTITIES;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.Conference;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using GUEST.Models;
using GUEST.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class ConferenceSponsorController : Controller
    {
        readonly ConferenceSponsorAddRepo AppRepos = new ConferenceSponsorAddRepo();
        readonly ConferenceSponsorDetailRepo DetailRepos = new ConferenceSponsorDetailRepo();
        readonly CountryRepo countryRepo = new CountryRepo();
        readonly SpecializationLanguageRepo specializationLanguageRepo = new SpecializationLanguageRepo();
        readonly FormalityLanguageRepo formalityLanguageRepo = new FormalityLanguageRepo();
        readonly ConferenceCriteriaLanguageRepo criteriaLanguageRepo = new ConferenceCriteriaLanguageRepo();
        readonly PolicyRepo policyRepo = new PolicyRepo();
        readonly OfficeRepo officeRepo = new OfficeRepo();
        readonly TitleLanguageRepo titleRepo = new TitleLanguageRepo();

        private readonly List<PageTree> pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
            };

        [Auther(RightID = "27")]
        public ActionResult Index()
        {
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        [AjaxOnly, HttpPost]
        public JsonResult List(ConferenceSearch search)
        {
            ConferenceSponsorIndexRepo IndexRepos = new ConferenceSponsorIndexRepo();
            BaseDatatable datatable = new BaseDatatable(Request);
            BaseServerSideData<ConferenceIndex> output = IndexRepos.GetIndexPage(datatable, search,
                CurrentAccount.AccountID(Session), LanguageResource.GetCurrentLanguageID());

            return Json(new ResultDatatable<ConferenceIndex>(output, Request));
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
            ViewBag.Link = policyRepo.GetCurrentLink(1);
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
            ViewBag.Link = policyRepo.GetCurrentLink(1);
            ViewBag.Offices = officeRepo.GetList();
            ViewBag.TitleLanguages = titleRepo.GetList(language_id);
            return View();
        }
        [HttpPost]
        public JsonResult Edit(string input, HttpPostedFileBase invite, HttpPostedFileBase paper, int request_id)
        {
            ConferenceSponsorEditRepo EditRepo = new ConferenceSponsorEditRepo();
            AlertModal<int> output = EditRepo.EditRequestConference(CurrentAccount.AccountID(Session), input, invite, paper, request_id);
            return Json(output);
        }
        public ActionResult Detail(int id)
        {
            pagesTree.Add(new PageTree("Chi tiết", "/ConferenceSponsor/Detail?id=" + id));
            ViewBag.pagesTree = pagesTree;

            QsUniversityRepo qsUniversityRepo = new QsUniversityRepo();

            string output = DetailRepos.GetDetailPageGuest(id, LanguageResource.GetCurrentLanguageID(), CurrentAccount.AccountID(Session));
            if (output == null)
                return Redirect("/ConferenceSponsor");

            ViewBag.output = output;
            string university = JObject.Parse(output)["Conference"]["QsUniversity"].ToString();
            ViewBag.ranking = qsUniversityRepo.GetRanking(university);
            return View();
        }
        [AjaxOnly]
        public JsonResult UpdateCost(int request_id, List<Cost> costs)
        {
            CostRepo costRepo = new CostRepo();
            int Times = costRepo.Update(request_id, CurrentAccount.AccountID(Session), costs);
            if (Times == 0)
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            else
                return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult CancelRequest(int request_id)
        {
            return Json(DetailRepos.CancelRequest(request_id, CurrentAccount.AccountID(Session)));
        }
        //[ChildActionOnly]
        //public ActionResult CostMenu(int id)
        //{
        //    ViewBag.id = id;
        //    ViewBag.CheckboxColumn = id == 2;
        //    ViewBag.ReimbursementColumn = id >= 3;
        //    ViewBag.EditAble = id == 2;
        //    return PartialView();
        //}
        [AjaxOnly]
        public JsonResult GetInformationPeopleWithID(string id)
        {
            var infos = AppRepos.GetAllProfileBy(id, LanguageResource.GetCurrentLanguageID());
            return Json(infos, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        public JsonResult GetConferenceWithName(string name, int page)
        {
            var confer = AppRepos.GetAllConferenceBy(name, page);
            return Json(confer, JsonRequestBehavior.AllowGet);
        }
        public class DataAddPage
        {
            public ProfileResearcher Profile { get; set; }
        }
    }
}