using BLL.ModelDAL;
using BLL.ScienceManagement.ArticlePolicy;
using ENTITIES;
using MANAGER.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

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
            int.TryParse(Request["language_id"], out int language_id);
            language_id = language_id == 0 ? 1 : language_id;
            ViewBag.detail = DetailRepo.Detail(id, language_id);
            return View();
        }

        public ActionResult Edit(int id)
        {
            PolicyTypeLanguageRepo policyTypeLanguageRepo = new PolicyTypeLanguageRepo();
            List<Language> languages = LanguageRepo.GetLanguages();
            var types = policyTypeLanguageRepo.PolicyTypeLanguages(languages[0].language_id);
            ViewBag.types = JsonConvert.SerializeObject(types);
            ViewBag.languages = languages;
            ViewBag.detail = EditRepo.Detail(id);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(List<ArticleVersion> versions, List<int> types, int id)
        {
            return Json(EditRepo.Edit(versions, types, id));
        }
    }
}