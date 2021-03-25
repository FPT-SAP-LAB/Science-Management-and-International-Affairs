using BLL.Authen;
using BLL.InternationalCollaboration.AcademicCollaborationRepository;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities.SaveAcademicCollaborationEntities;
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
        AcademicCollaborationRepo academicCollaborationRepo;

        // GET: AcademicCollaboration
        public ActionResult Longterm_List()
        {
            ViewBag.title = "DANH SÁCH ĐÀO TẠO SAU ĐẠI HỌC";
            return View();
        }

        [HttpPost]
        public ActionResult getListAcademicCollaboration(int direction, int collab_type_id, ObjectSearching_AcademicCollaboration obj_searching)
        {
            academicCollaborationRepo = new AcademicCollaborationRepo();
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
            academicCollaborationRepo = new AcademicCollaborationRepo();
            AlertModal<List<Country>> alertModal = academicCollaborationRepo.countries(country_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getYears()
        {
            academicCollaborationRepo = new AcademicCollaborationRepo();
            AlertModal<YearSearching> alertModal = academicCollaborationRepo.yearSearching();
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getOffices(string office_name)
        {
            academicCollaborationRepo = new AcademicCollaborationRepo();
            AlertModal<List<Office>> alertModal = academicCollaborationRepo.offices(office_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getPartners(string partner_name)
        {
            academicCollaborationRepo = new AcademicCollaborationRepo();
            AlertModal<List<AcademicCollaborationPartner_Ext>> alertModal = academicCollaborationRepo.partners(partner_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        //add person
        [HttpGet]
        public ActionResult getPeople(string person_name)
        {
            academicCollaborationRepo = new AcademicCollaborationRepo();
            AlertModal<List<AcademicCollaborationPerson_Ext>> alertModal = academicCollaborationRepo.people(person_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getCollabScopes(string collab_abbreviation_name)
        {
            academicCollaborationRepo = new AcademicCollaborationRepo();
            AlertModal<List<CollaborationScope>> alertModal = academicCollaborationRepo.collaborationScopes(collab_abbreviation_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult getAcadCollabStatus(int status_type_specific)
        {
            academicCollaborationRepo = new AcademicCollaborationRepo();
            AlertModal<List<AcademicCollaborationStatu>> alertModal = academicCollaborationRepo.academicCollaborationStatus(status_type_specific);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult checkPerson(int people_id, string people_name)
        {
            academicCollaborationRepo = new AcademicCollaborationRepo();
            AlertModal<AcademicCollaborationPerson_Ext> alertModal = academicCollaborationRepo.person(people_id, people_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
        }

        [HttpPost]
        public ActionResult checkPartner(int partner_id, string partner_name)
        {
            academicCollaborationRepo = new AcademicCollaborationRepo();
            AlertModal<AcademicCollaborationPartner_Ext> alertModal = academicCollaborationRepo.partner(partner_id, partner_name);
            return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
        }

        [HttpPost]
        public ActionResult uploadEvidenceFile(HttpPostedFileBase evidence, string folder_name)
        {
            Google.Apis.Drive.v3.Data.File f = GlobalUploadDrive.UploadIAFile(evidence, folder_name, 4, false);
            return Json(new { evidence_link = f.WebViewLink });
        }

        [HttpPost]
        public ActionResult saveAcademicCollaboration(int direction_id, int collab_type_id, SaveAcadCollab_Person obj_person, SaveAcadCollab_Partner obj_partner, SaveAcadCollab_AcademicCollaboration obj_academic_collab)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;
                }
                else
                {
                    AlertModal<AcademicCollaboration_Ext> alertModal1 = new AlertModal<AcademicCollaboration_Ext>(null, false, "Lỗi", "Người dùng chưa đăng nhập.");
                    return Json(new { alertModal1.obj, alertModal1.success, alertModal1.title, alertModal1.content });
                }
                AlertModal<AcademicCollaboration_Ext> alertModal = academicCollaborationRepo.saveAcademicCollaboration(direction_id, collab_type_id, obj_person, obj_partner, obj_academic_collab, acc.account_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

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
