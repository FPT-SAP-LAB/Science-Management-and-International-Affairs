using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using MANAGER.Models;

namespace MANAGER.Controllers.ScienceManagement.Policy
{
    public class CriteriaManagementController : Controller
    {
        private readonly PaperCriteriaRepo PaperCriteriaRepo = new PaperCriteriaRepo();
        public ActionResult Index()
        {
            ViewBag.paperCriterias = PaperCriteriaRepo.GetCurrentPaperCriterias();
            return View();
        }

        [AjaxOnly]
        public JsonResult Edit(int id, string name)
        {
            AlertModal<string> result = PaperCriteriaRepo.Edit(id, name);
            return Json(result);
        }

        public ActionResult Add()
        {
            ViewBag.paperCriterias = PaperCriteriaRepo.GetCurrentPaperCriterias();
            return View();
        }

        [HttpPost]
        public JsonResult Add(HttpPostedFileBase file, string paperCriterias)
        {
            AlertModal<string> result = PaperCriteriaRepo.Add(file, paperCriterias, CurrentAccount.AccountID(Session));
            return Json(result);
        }
    }
}