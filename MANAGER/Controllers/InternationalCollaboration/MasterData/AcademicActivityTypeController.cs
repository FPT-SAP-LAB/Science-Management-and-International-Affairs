using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using BLL.InternationalCollaboration.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using Newtonsoft.Json;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class AcademicActivityTypeController : Controller
    {
        private static AcademicActivityTypeRepo academicActivityTypeRepo = new AcademicActivityTypeRepo();

        // GET: AcademicActivityType
        public ActionResult List()
        {
            ViewBag.pageTitle = "QUẢN LÝ LOẠI HOẠT ĐỘNG HỌC THUẬT";
            return View();
        }

        public ActionResult listAcademicActivityType()
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                List<AcademicActivityType> academicActivityTypes = academicActivityTypeRepo.getlistAcademicActivityType(baseDatatable);
                return Json(new { success = true, data = academicActivityTypes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }
        }
    }
}
