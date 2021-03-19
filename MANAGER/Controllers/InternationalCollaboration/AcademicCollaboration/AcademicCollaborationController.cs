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
            //list for searching
            
            //country
            //year 
            //office
            //partner

            ViewBag.title = "DANH SÁCH ĐÀO TẠO SAU ĐẠI HỌC";
            return View();
        }

        [HttpPost]
        public ActionResult getListAcademicCollaboration(int direction, int collab_status_type)
        {
            BaseDatatable baseDatatable = new BaseDatatable(Request);
            BaseServerSideData<AcademicCollaboration_Ext> baseServerSideData = academicCollaborationRepo.academicCollaborations(direction, collab_status_type);
            return Json(new {
                success = true,
                data = baseServerSideData.Data,
                draw = Request["draw"],
                recordsTotal = baseServerSideData.RecordsTotal,
                recordsFiltered = baseServerSideData.RecordsTotal
            });
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