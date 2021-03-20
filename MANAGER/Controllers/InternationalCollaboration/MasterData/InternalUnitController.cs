using BLL.InternationalCollaboration.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class InternalUnitController : Controller
    {
        private static InternalUnitRepo collaborationStatusRepo = new InternalUnitRepo();

        // GET: InternalUnit
        public ActionResult List()
        {
            ViewBag.title = "QUẢN LÝ ĐƠN VỊ NỘI BỘ";
            return View();
        }

        public ActionResult listInternalUnit()
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<InternalUnit> baseServerSideData = collaborationStatusRepo.getListInternalUnit(baseDatatable);
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
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult addInternalUnit(string unit_name, string unit_abbreviation)
        {
            try
            {
                AlertModal<InternalUnit> alertModal = collaborationStatusRepo.addInternalUnit(unit_name, unit_abbreviation);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult getInternalUnit(int unit_id)
        {
            try
            {
                AlertModal<InternalUnit> alertModal = collaborationStatusRepo.getInternalUnit(unit_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult editInternalUnit(int unit_id, string unit_name, string unit_abbreviation)
        {
            try
            {
                AlertModal<InternalUnit> alertModal = collaborationStatusRepo.editInternalUnit(unit_id, unit_name, unit_abbreviation);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult deleteInternalUnit(int unit_id)
        {
            try
            {
                AlertModal<InternalUnit> alertModal = collaborationStatusRepo.deleteInternalUnit(unit_id);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}