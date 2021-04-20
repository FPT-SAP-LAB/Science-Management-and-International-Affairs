using BLL.ModelDAL;
using BLL.ScienceManagement.ArticlePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENTITIES;
using MANAGER.Models;

namespace MANAGER.Controllers.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyController : Controller
    {
        private readonly ArticlePolicyIndexRepo IndexRepo = new ArticlePolicyIndexRepo();
        private readonly ArticlePolicyAddRepo AddRepo = new ArticlePolicyAddRepo();
        public ActionResult Index()
        {
            int.TryParse(Request["language"], out int language_id);
            language_id = language_id == 0 ? 1 : language_id;
            ViewBag.items = IndexRepo.List(language_id);
            return View();
        }
        public ActionResult Add()
        {
            PolicyTypeLanguageRepo policyTypeLanguageRepo = new PolicyTypeLanguageRepo();
            List<Language> languages = LanguageRepo.GetLanguages();
            ViewBag.types = policyTypeLanguageRepo.PolicyTypeLanguages(languages[0].language_id);
            ViewBag.languages = languages;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(List<ArticleVersion> versions, List<int> types)
        {
            return Json(AddRepo.Add(versions, types, CurrentAccount.AccountID(Session)));
        }
    }
}