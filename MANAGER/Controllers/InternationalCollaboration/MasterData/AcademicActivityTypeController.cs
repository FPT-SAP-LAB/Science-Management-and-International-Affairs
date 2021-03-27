using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.InternationalCollaboration.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.MasterData;
using MANAGER.Support;
using Newtonsoft.Json;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class AcademicActivityTypeController : Controller
    {
        private static AcademicActivityTypeRepo academicActivityTypeRepo = new AcademicActivityTypeRepo();

        [Auther(RightID = "4")]
        public ActionResult List()
        {
            ViewBag.title = "QUẢN LÝ LOẠI HOẠT ĐỘNG HỌC THUẬT";
            AlertModal<List<Language>> alertModal_Languages = academicActivityTypeRepo.getLanguages();
            ViewBag.languages = alertModal_Languages.obj;
            return View();
        }

        [HttpGet]
        public ActionResult listAcademicActivityType()
        {
            try
            {
                int language_id = Int32.Parse(Request["language_id"]);
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<AcademicActivityType_Ext> academicActivityTypes = academicActivityTypeRepo.getlistAcademicActivityType(baseDatatable, language_id);
                return Json(new
                {
                    success = true,
                    data = academicActivityTypes.Data,
                    draw = Request["draw"],
                    recordsTotal = academicActivityTypes.RecordsTotal,
                    recordsFiltered = academicActivityTypes.RecordsTotal
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public ActionResult getLanguages()
        {
            try
            {
                AlertModal<List<Language>> alertModal = academicActivityTypeRepo.getLanguages();
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult addAcademicActivityType(int language_id, string activity_type_name)
        {
            try
            {
                AlertModal<AcademicActivityType_Ext> alertModal = academicActivityTypeRepo.addAcademicActivityType(language_id, activity_type_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }

        [HttpPost]
        public ActionResult getAcademicActivityType(int language_id, int activity_type_id)
        {
            try
            {
                AlertModal<AcademicActivityType_Ext> alertModal = academicActivityTypeRepo.getAcademicActivityType(language_id, activity_type_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }

        [HttpPost]
        public ActionResult editAcademicActivtyType(int language_id, int activity_type_id, string activity_type_name)
        {
            try
            {
                AlertModal<AcademicActivityType_Ext> alertModal = academicActivityTypeRepo.editAcademicActivityType(language_id, activity_type_id, activity_type_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }

        [HttpPost]
        public ActionResult deleteAcademicActivityType(int language_id, int activity_type_id)
        {
            try
            {
                AlertModal<AcademicActivityType_Ext> alertModal = academicActivityTypeRepo.deleteAcademicActivityType(language_id, activity_type_id);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }
    }
}
