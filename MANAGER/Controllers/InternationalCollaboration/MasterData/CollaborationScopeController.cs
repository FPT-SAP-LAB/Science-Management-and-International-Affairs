using BLL.InternationalCollaboration.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using MANAGER.Support;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class CollaborationScopeController : Controller
    {
        private static CollaborationScopeRepo collaborationStatusRepo = new CollaborationScopeRepo();

        [Auther(RightID = "9")]
        public ActionResult List()
        {
            ViewBag.title = "QUẢN LÝ PHẠM VI HỢP TÁC";
            return View();
        }

        public ActionResult listCollaborationScope()
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<CollaborationScope> baseServerSideData = collaborationStatusRepo.getListCollaborationScope(baseDatatable);
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
        public ActionResult listCollaborationScopeUpdated()
        {
            try
            {
                List<CollaborationScope> collaborationScopes = collaborationStatusRepo.getListCollaborationScopeUpdated();
                return Json(collaborationScopes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public ActionResult addCollaborationScope(string scope_name, string scope_abbreviation)
        {
            try
            {
                AlertModal<CollaborationScope> alertModal = collaborationStatusRepo.addCollaborationScope(scope_name, scope_abbreviation);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult getCollaborationScope(int scope_id)
        {
            try
            {
                AlertModal<CollaborationScope> alertModal = collaborationStatusRepo.getCollaborationScope(scope_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult editCollaborationScope(int scope_id, string scope_name, string scope_abbreviation)
        {
            try
            {
                AlertModal<CollaborationScope> alertModal = collaborationStatusRepo.editCollaborationScope(scope_id, scope_name, scope_abbreviation);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult deleteCollaborationScope(int scope_id)
        {
            try
            {
                AlertModal<CollaborationScope> alertModal = collaborationStatusRepo.deleteCollaborationScope(scope_id);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}