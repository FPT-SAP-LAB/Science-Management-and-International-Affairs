using BLL.InternationalCollaboration.AcademicCollaboration;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicCollaboration
{
    public class AcademicCollaborationController : Controller
    {
        /*--------------------------------------------------------LONG TERM---------------------------------------------------------*/
        private static AcademicCollaborationRepo academicCollaborationRepo = new AcademicCollaborationRepo();

        // GET: AcademicCollaboration
        public ActionResult Longterm_List()
        {
            ViewBag.title = "DANH SÁCH ĐÀO TẠO SAU ĐẠI HỌC";
            //list for searching
            //country
            ViewBag.countries = academicCollaborationRepo.countries();
            //year 
            ViewBag.yearSearching = academicCollaborationRepo.yearSearching();
            //office
            ViewBag.offices = academicCollaborationRepo.offices();
            //partner
            ViewBag.partners = academicCollaborationRepo.partners();
            return View();
        }

        [HttpPost]
        public ActionResult getListAcademicCollaboration(int direction, int collab_type_id, ObjectSearching_AcademicCollaboration obj_searching)
        {
            BaseDatatable baseDatatable = new BaseDatatable(Request);
            BaseServerSideData<AcademicCollaboration_Ext> baseServerSideData = academicCollaborationRepo.academicCollaborations(direction, collab_type_id, obj_searching);
            return Json(new
            {
                success = true,
                data = baseServerSideData.Data,
                draw = Request["draw"],
                recordsTotal = baseServerSideData.RecordsTotal,
                recordsFiltered = baseServerSideData.RecordsTotal
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete_Longterm(string id)
        {
            try
            {
                string result = id;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        /*--------------------------------------------------------SHORT TERM---------------------------------------------------------*/

        public ActionResult Shortterm_List()
        {
            ViewBag.title = "DANH SÁCH TRAO ĐỔI CÁN BỘ GIẢNG VIÊN";
            return View();
        }

        public ActionResult Get_Status_History(string id)
        {
            try
            {
                string result = id;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}