using BLL.ModelDAL;
using BLL.ScienceManagement.ArticlePolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENTITIES;
using MANAGER.Models;
using Newtonsoft.Json;

namespace MANAGER.Controllers.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyController : Controller
    {
        private readonly ArticlePolicyIndexRepo IndexRepo = new ArticlePolicyIndexRepo();
        private readonly ArticlePolicyAddRepo AddRepo = new ArticlePolicyAddRepo();
        private readonly ArticlePolicyDetailRepo DetailRepo = new ArticlePolicyDetailRepo();
        private readonly ArticlePolicyEditRepo EditRepo = new ArticlePolicyEditRepo();
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

        public ActionResult Detail(int id)
        {
            ViewBag.detail = DetailRepo.Detail(id, 1);
            return View();
        }

        public ActionResult Edit(int id)
        {
            PolicyTypeLanguageRepo policyTypeLanguageRepo = new PolicyTypeLanguageRepo();
            List<Language> languages = LanguageRepo.GetLanguages();
            var types = policyTypeLanguageRepo.PolicyTypeLanguages(languages[0].language_id)
                .Select(x => new
                {
                    x.policy_type_language_id,
                    x.policy_type_name,
                    value = x.policy_type_name
                }).ToList();
            ViewBag.types = JsonConvert.SerializeObject(types);
            ViewBag.languages = languages;
            ViewBag.detail = EditRepo.Detail(id);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(List<ArticleVersion> versions, List<int> types)
        {
            return Json(AddRepo.Add(versions, types, CurrentAccount.AccountID(Session)));
        }
    }
}