using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.InternationalCollaboration.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using Newtonsoft.Json;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class AcademicActivityExpenseTypeController : Controller
    {
        private static AcademicActivityExpenseTypeRepo activityExpenseTypeRepo = new AcademicActivityExpenseTypeRepo();

        // GET: AcademicActivityExpenseType
        public ActionResult List()
        {
            ViewBag.title = "QUẢN LÝ LOẠI KINH PHÍ";
            return View();
        }

        public ActionResult listAcademicActivityExpenseType()
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<ActivityExpenseType> baseServerSideData = activityExpenseTypeRepo.getListActivityExpenseType(baseDatatable);
                return Json(new
                {
                    success = true,
                    data = baseServerSideData.Data,
                    draw = Request["draw"],
                    recordsTotal = baseServerSideData.RecordsTotal,
                    recordsFiltered = baseServerSideData.RecordsTotal
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult addAcademicActivityExpenseType(string expense_type_name)
        {
            try
            {
                AlertModal<ActivityExpenseType> alertModal = activityExpenseTypeRepo.addAcademicActivityExpenseType(expense_type_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }

        [HttpPost]
        public ActionResult getAcademicActivityExpenseType(int expense_type_id)
        {
            try
            {
                AlertModal<ActivityExpenseType> alertModal = activityExpenseTypeRepo.getActivityExpenseType(expense_type_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }

        [HttpPost]
        public ActionResult editAcademicActivityExpenseType(int expense_type_id, string expense_type_name)
        {
            try
            {
                AlertModal<ActivityExpenseType> alertModal = activityExpenseTypeRepo.editActivityExpenseType(expense_type_id, expense_type_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }

        [HttpPost]
        public ActionResult deleteAcademicActivityExpenseType(int expense_type_id)
        {
            try
            {
                AlertModal<ActivityExpenseType> alertModal = activityExpenseTypeRepo.deleteActivityExpenseType(expense_type_id);
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
