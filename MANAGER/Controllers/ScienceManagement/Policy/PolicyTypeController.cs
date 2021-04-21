using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using MANAGER.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.Policy
{
    public class PolicyTypeController : Controller
    {
        private readonly PolicyTypeLanguageRepo typeRepo = new PolicyTypeLanguageRepo();
        public ActionResult Index(string language)
        {
            ViewBag.languages = LanguageRepo.GetLanguages();
            int.TryParse(language, out int language_id);
            language_id = language_id == 0 ? 1 : language_id;
            ViewBag.language_id = language_id;
            ViewBag.PolicyTypeLanguages = typeRepo.PolicyTypeLanguages(language_id);
            return View();
        }
        [AjaxOnly]
        public JsonResult Edit(int id, string name)
        {
            AlertModal<string> result = typeRepo.Edit(id, name);
            return Json(result);
        }
        [AjaxOnly]
        public JsonResult Add(List<PolicyTypeLanguage> types)
        {
            AlertModal<string> result = typeRepo.Add(types, CurrentAccount.AccountID(Session));
            return Json(result);
        }
    }
}