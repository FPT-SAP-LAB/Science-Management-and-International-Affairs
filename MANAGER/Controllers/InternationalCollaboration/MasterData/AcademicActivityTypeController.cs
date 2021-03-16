using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.InternationalCollaboration.MasterData;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class AcademicActivityTypeController : Controller
    {
        private AcademicActivityTypeRepo academicActivityTypeRepo = new AcademicActivityTypeRepo();

        // GET: AcademicActivityType
        public ActionResult List()
        {
            ViewBag.pageTitle = "QUẢN LÝ LOẠI HOẠT ĐỘNG HỌC THUẬT";
            return View();
        }

        public ActionResult listAcademicActivityType()
        {
            List<ENTITIES.AcademicActivityType> academicActivityTypes = academicActivityTypeRepo.getlistAcademicActivityType();
            return Json(new { academicActivityTypes, JsonRequestBehavior.AllowGet });
        }


    }
}