using BLL.InternationalCollaboration.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using MANAGER.Support;
using System;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class CollaborationStatusController : Controller
    {
        private static CollaborationStatusRepo collaborationStatusRepo = new CollaborationStatusRepo();

        [Auther(RightID = "12")]
        public ActionResult List()
        {
            ViewBag.title = "QUẢN LÝ TRẠNG THÁI THỎA THUẬN HỢP TÁC";
            return View();
        }

        public ActionResult listCollaborationStatus()
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<CollaborationStatu> baseServerSideData = collaborationStatusRepo.getListCollaborationStatu(baseDatatable);
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
        public ActionResult addCollaborationStatus(string mou_status_name)
        {
            try
            {
                AlertModal<CollaborationStatu> alertModal = collaborationStatusRepo.addCollaborationStatu(mou_status_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult getCollaborationStatus(int mou_status_id)
        {
            try
            {
                AlertModal<CollaborationStatu> alertModal = collaborationStatusRepo.getCollaborationStatu(mou_status_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult editCollaborationStatus(int mou_status_id, string mou_status_name)
        {
            try
            {
                AlertModal<CollaborationStatu> alertModal = collaborationStatusRepo.editCollaborationStatu(mou_status_id, mou_status_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult deleteCollaborationStatus(int mou_status_id)
        {
            try
            {
                AlertModal<CollaborationStatu> alertModal = collaborationStatusRepo.deleteCollaborationStatu(mou_status_id);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}