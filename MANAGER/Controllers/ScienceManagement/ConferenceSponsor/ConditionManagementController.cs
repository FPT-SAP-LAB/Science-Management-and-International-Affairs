using BLL.ModelDAL;
using ENTITIES.CustomModels;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.ConferenceSponsor
{
    public class ConditionManagementController : Controller
    {
        ConferenceCriteriaLanguageRepo criteriaLanguageRepo = new ConferenceCriteriaLanguageRepo();
        public ActionResult Index(string language)
        {
            ViewBag.languages = LanguageRepo.GetLanguages();
            int.TryParse(language, out int language_id);
            language_id = language_id == 0 ? 1 : language_id;
            ViewBag.language_id = language_id;
            ViewBag.ConferenceCriteriaLanguages = criteriaLanguageRepo.GetCurrentList(language_id);
            return View();
        }
        public JsonResult Edit(int id, string name)
        {
            AlertModal<string> result = criteriaLanguageRepo.Edit(id, name);
            return Json(result);
        }
        public ActionResult Add()
        {
            return View();
        }
    }
}