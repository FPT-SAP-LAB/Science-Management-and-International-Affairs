using BLL.Authen;
using BLL.ModelDAL;
using BLL.ScienceManagement.Researcher;
using ENTITIES;
using ENTITIES.CustomModels;
using GUEST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class AdditionalProfileController : Controller
    {
        private readonly List<PageTree> pagesTree = new List<PageTree>
            {
                new PageTree("Bổ sung thông tin","/AdditionalProfile"),
            };
        [HttpGet]
        public ActionResult Index()
        {
            OfficeRepo officeRepo = new OfficeRepo();
            TitleLanguageRepo titleRepo = new TitleLanguageRepo();
            CountryRepo countryRepo = new CountryRepo();

            Account account = CurrentAccount.Account(Session);
            if (account.account_id == 0)
                return Redirect("/");
            //string EmailDomain = account.email.Split('@').Last();
            //if (EmailDomain.Equals("fe.edu.vn"))
            //{
            //    ViewBag.Positions = PositionLanguageRepo.GetPositionLanguages(LanguageResource.GetCurrentLanguageID());
            //}

            ViewBag.Titles = titleRepo.GetList(LanguageResource.GetCurrentLanguageID());
            ViewBag.Offices = officeRepo.GetList();
            ViewBag.Countries = countryRepo.GetCountries();
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        [HttpPost]
        public JsonResult Add(HttpPostedFileBase identification, string person, string profile, string username)
        {
            AdditionalProfileRepo additionalProfileRepo = new AdditionalProfileRepo();
            AlertModal<int> result = additionalProfileRepo.Add(identification, person, profile, username, CurrentAccount.AccountID(Session));
            if (result.success)
            {
                LoginRepo repo = new LoginRepo();
                LoginRepo.User u = repo.GetAccount(result.obj, new List<int> { 0 });
                Session["User"] = u;
            }
            return Json(result);
        }
    }
}