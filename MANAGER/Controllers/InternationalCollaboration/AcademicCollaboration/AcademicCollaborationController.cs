using BLL.InternationalCollaboration.AcademicCollaboration;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration;
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
            return View();
        }

        [HttpPost]
        public ActionResult getListAcademicCollaboration(int direction, int collab_type_id, ObjectSearching_AcademicCollaboration obj_searching)
        {
            BaseDatatable baseDatatable = new BaseDatatable(Request);
            BaseServerSideData<AcademicCollaboration_Ext> baseServerSideData = academicCollaborationRepo.academicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);
            return Json(new
            {
                success = true,
                data = baseServerSideData.Data,
                draw = Request["draw"],
                recordsTotal = baseServerSideData.RecordsTotal,
                recordsFiltered = baseServerSideData.RecordsTotal
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getCountries(string country_name)
        {
            AlertModal<List<Country>> alertModal = academicCollaborationRepo.countries(country_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getYears()
        {
            AlertModal<YearSearching> alertModal = academicCollaborationRepo.yearSearching();
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getOffices(string office_name)
        {
            AlertModal<List<Office>> alertModal = academicCollaborationRepo.offices(office_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getPartners(string partner_name)
        {
            AlertModal<List<AcademicCollaborationPartner_Ext>> alertModal = academicCollaborationRepo.partners(partner_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        //add person
        [HttpGet]
        public ActionResult getPeople(string person_name)
        {
            AlertModal<List<AcademicCollaborationPerson_Ext>> alertModal = academicCollaborationRepo.people(person_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getCollabScopes(string collab_abbreviation_name)
        {
            AlertModal<List<CollaborationScope>> alertModal = academicCollaborationRepo.collaborationScopes(collab_abbreviation_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getAcadCollabStatus(int status_type_specific)
        {
            AlertModal<List<AcademicCollaborationStatu>> alertModal = academicCollaborationRepo.academicCollaborationStatus(status_type_specific);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult checkPerson(int people_id, string people_name)
        {
            AlertModal<AcademicCollaborationPerson_Ext> alertModal = academicCollaborationRepo.person(people_id, people_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
        }

        [HttpPost]
        public ActionResult checkPartner(int partner_id, string partner_name)
        {
            AlertModal<AcademicCollaborationPartner_Ext> alertModal = academicCollaborationRepo.partner(partner_id, partner_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
        }

        [HttpPost]
        public ActionResult addPerson(Person person)
        {
            return Json(new { });
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
