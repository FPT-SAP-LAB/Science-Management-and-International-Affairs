using BLL.InternationalCollaboration.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using MANAGER.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class AcademicCollaborationStatusController : Controller
    {
        private static AcademicCollaborationStatusRepo academicCollaborationStatusRepo = new AcademicCollaborationStatusRepo();

        public ActionResult List()
        {
            List<StatusType> statusTypes = new List<StatusType>
            {
                new StatusType(1, "Dài hạn"),
                new StatusType(2, "Ngắn hạn"),
                new StatusType(3, "Dài hạn và ngắn hạn")
            };
            ViewBag.statusTypes = statusTypes;
            ViewBag.title = "QUẢN LÝ TRẠNG THÁI HOẠT ĐỘNG HỢP TÁC";
            return View();
        }

        public ActionResult listAcademicCollaborationStatus()
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<AcademicCollaborationStatu_Ext> baseServerSideData = academicCollaborationStatusRepo.getListAcademicCollaborationStatu(baseDatatable);
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
        public ActionResult addAcademicCollaborationStatus(string collab_status_name, int status_type)
        {
            try
            {
                AlertModal<AcademicCollaborationStatu> alertModal = academicCollaborationStatusRepo.addAcademicCollaborationStatu(collab_status_name, status_type);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }

        [HttpPost]
        public ActionResult getAcademicCollaborationStatus(int collab_status_id)
        {
            try
            {
                AlertModal<AcademicCollaborationStatu> alertModal = academicCollaborationStatusRepo.getAcademicCollaborationStatu(collab_status_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }

        [HttpPost]
        public ActionResult editAcademicCollaborationStatus(int collab_status_id, string collab_status_name, int status_type)
        {
            try
            {
                AlertModal<AcademicCollaborationStatu> alertModal = academicCollaborationStatusRepo.editAcademicCollaborationStatu(collab_status_id, collab_status_name, status_type);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new AlertModal<string>(false));
            }
        }

        [HttpPost]
        public ActionResult deleteAcademicCollaborationStatus(int collab_status_id)
        {
            try
            {
                AlertModal<AcademicCollaborationStatu> alertModal = academicCollaborationStatusRepo.deleteAcademicCollaborationStatu(collab_status_id);
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