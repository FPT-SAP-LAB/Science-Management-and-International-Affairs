using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using MANAGER.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.ConferenceSponsor
{
    public class ConditionManagementController : Controller
    {
        private readonly ConferenceCriteriaLanguageRepo criteriaLanguageRepo = new ConferenceCriteriaLanguageRepo();
        public ActionResult Index(string language)
        {
            ViewBag.languages = LanguageRepo.GetLanguages();
            int.TryParse(language, out int language_id);
            language_id = language_id == 0 ? 1 : language_id;
            ViewBag.language_id = language_id;
            ViewBag.ConferenceCriteriaLanguages = criteriaLanguageRepo.GetCurrentList(language_id);
            return View();
        }
        [AjaxOnly]
        public JsonResult Edit(int id, string name)
        {
            AlertModal<string> result = criteriaLanguageRepo.Edit(id, name);
            return Json(result);
        }
        public ActionResult Add()
        {
            ViewBag.languages = LanguageRepo.GetLanguages();
            IEnumerable<ConferenceConditionLanguage> result = criteriaLanguageRepo.GetAll();
            ViewBag.ConferenceCriteriaLanguages = result;
            ViewBag.DistinctList = result.Select(x => x.condition_id).Distinct().ToList();
            return View();
        }
        [HttpPost]
        public JsonResult Add(HttpPostedFileBase file, string policies)
        {
            AlertModal<string> result = criteriaLanguageRepo.Add(file, policies, CurrentAccount.AccountID(Session));
            return Json(result);
        }
    }
}