using BLL.InternationalCollaboration.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using System;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class AcademicCollaborationTypeController : Controller
    {
        private static AcademicCollaborationTypeRepo academicCollaborationTypeRepo = new AcademicCollaborationTypeRepo();

        // GET: AcademicCollaborationType
        public ActionResult List()
        {
            ViewBag.title = "QUẢN LÝ LOẠI HỢP TÁC HỌC THUẬT";
            return View();
        }

        public ActionResult listAcademicCollaborationType()
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<AcademicCollaborationType> baseServerSideData = academicCollaborationTypeRepo.getListAcademicCollaborationType(baseDatatable);
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
        public ActionResult addAcademicCollaborationType(string collab_type_name)
        {
            try
            {
                AlertModal<AcademicCollaborationType> alertModal = academicCollaborationTypeRepo.addAcademicCollaborationType(collab_type_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult getAcademicCollaborationType(int collab_type_id)
        {
            try
            {
                AlertModal<AcademicCollaborationType> alertModal = academicCollaborationTypeRepo.getAcademicCollaborationType(collab_type_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult editAcademicCollaborationType(int collab_type_id, string collab_type_name)
        {
            try
            {
                AlertModal<AcademicCollaborationType> alertModal = academicCollaborationTypeRepo.editAcademicCollaborationType(collab_type_id, collab_type_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult deleteAcademicCollaborationType(int collab_type_id)
        {
            try
            {
                AlertModal<AcademicCollaborationType> alertModal = academicCollaborationTypeRepo.deleteAcademicCollaborationType(collab_type_id);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}