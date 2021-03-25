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
    public class SpecializationController : Controller
    {
        // GET: Specialization
        private static SpecializationRepo specializationRepo = new SpecializationRepo();
        public ActionResult listSpecializations()
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<Specialization> baseServerSideData = specializationRepo.getListSpecialization(baseDatatable);
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
        public ActionResult deleteSpecialization(int spe_id)
        {
            try
            {
                AlertModal<Specialization> alertModal = specializationRepo.deleteSpecialization(spe_id);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpPost]
        public ActionResult addSpecialization(string spe_name)
        {
            try
            {
                AlertModal<Specialization> alertModal = specializationRepo.addSpecialization(spe_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult getSpecialization(int spe_id)
        {
            try
            {
                AlertModal<Specialization> alertModal = specializationRepo.getSpecialization(spe_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult editSpecialization(int spe_id, string spe_name)
        {
            try
            {
                AlertModal<Specialization> alertModal = specializationRepo.editSpecialization(spe_id, spe_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}