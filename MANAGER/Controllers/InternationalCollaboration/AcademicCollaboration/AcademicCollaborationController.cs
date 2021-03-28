using BLL.Authen;
using BLL.InternationalCollaboration.AcademicCollaborationRepository;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities.SaveAcademicCollaborationEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicCollaboration
{
    public class AcademicCollaborationController : Controller
    {
        /*--------------------------------------------------------LONG TERM ACADEMIC COLLABORATION---------------------------------------------------------*/
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
            try
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
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public JsonResult getCountries(string country_name)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<List<Country>> alertModal = academicCollaborationRepo.countries(country_name);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult getYears()
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<YearSearching> alertModal = academicCollaborationRepo.yearSearching();
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult getOffices(string office_name)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<List<Office>> alertModal = academicCollaborationRepo.offices(office_name);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult getPartners(string partner_name)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<List<AcademicCollaborationPartner_Ext>> alertModal = academicCollaborationRepo.partners(partner_name);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //add person
        [HttpGet]
        public ActionResult getPeople(string person_name)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<List<AcademicCollaborationPerson_Ext>> alertModal = academicCollaborationRepo.people(person_name);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult getCollabScopes(string collab_abbreviation_name)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<List<CollaborationScope>> alertModal = academicCollaborationRepo.collaborationScopes(collab_abbreviation_name);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult getAcadCollabStatus(int status_type_specific)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<List<AcademicCollaborationStatu>> alertModal = academicCollaborationRepo.academicCollaborationStatus(status_type_specific);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult checkPerson(int people_id, string people_name)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<AcademicCollaborationPerson_Ext> alertModal = academicCollaborationRepo.person(people_id, people_name);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult checkPartner(int partner_id, string partner_name)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<AcademicCollaborationPartner_Ext> alertModal = academicCollaborationRepo.partner(partner_id, partner_name);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult saveAcademicCollaboration(HttpPostedFileBase evidence, string folder_name, int direction_id, int collab_type_id, string obj_person_stringify, string obj_partner_stringify, string obj_academic_collab_stringify)
        {
            try
            {
                //parse to Object
                SaveAcadCollab_Person obj_person = JsonConvert.DeserializeObject<SaveAcadCollab_Person>(obj_person_stringify);
                SaveAcadCollab_Partner obj_partner = JsonConvert.DeserializeObject<SaveAcadCollab_Partner>(obj_partner_stringify);
                SaveAcadCollab_AcademicCollaboration obj_academic_collab = JsonConvert.DeserializeObject<SaveAcadCollab_AcademicCollaboration>(obj_academic_collab_stringify);

                academicCollaborationRepo = new AcademicCollaborationRepo();
                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;

                    //upload file
                    if (GoogleDriveService.credential == null && GoogleDriveService.driveService == null)
                    {
                        AlertModal<string> alertModal1 = new AlertModal<string>(null, false, "Lỗi", "Vui lòng liên hệ với quản trị hệ thống để được cấp quyền.");
                        return Json(new { alertModal1.obj, alertModal1.success, alertModal1.title, alertModal1.content });
                    }
                    else
                    {
                        //upload file
                        Google.Apis.Drive.v3.Data.File f = new Google.Apis.Drive.v3.Data.File();
                        if (evidence != null)
                        {
                            f = academicCollaborationRepo.uploadEvidenceFile(evidence, folder_name, 4, false);
                        }
                        //Save academic collab
                        AlertModal<AcademicCollaboration_Ext> alertModal = academicCollaborationRepo.saveAcademicCollaboration(direction_id, collab_type_id,
                            obj_person, obj_partner, obj_academic_collab, f, evidence, acc.account_id);
                        return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
                    }
                }
                else
                {
                    AlertModal<AcademicCollaboration_Ext> alertModal1 = new AlertModal<AcademicCollaboration_Ext>(null, false, "Lỗi", "Người dùng chưa đăng nhập.");
                    return Json(new { alertModal1.obj, alertModal1.success, alertModal1.title, alertModal1.content });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //EDIT
        [HttpGet]
        public ActionResult getAcademicCollaboration(int direction, int collab_type_id, int acad_collab_id)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<AcademicCollaboration_Ext> alertModal = academicCollaborationRepo.getAcademicCollaboration(direction, collab_type_id, acad_collab_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult updateAcademicCollaboration(string old_evidence_stringify, HttpPostedFileBase new_evidence, string folder_name, int direction_id, int collab_type_id, string obj_person_stringify, string obj_partner_stringify, string obj_academic_collab_stringify)
        {
            try
            {
                //parse to Object
                File old_evidence = JsonConvert.DeserializeObject<File>(old_evidence_stringify);
                SaveAcadCollab_Person obj_person = JsonConvert.DeserializeObject<SaveAcadCollab_Person>(obj_person_stringify);
                SaveAcadCollab_Partner obj_partner = JsonConvert.DeserializeObject<SaveAcadCollab_Partner>(obj_partner_stringify);
                SaveAcadCollab_AcademicCollaboration obj_academic_collab = JsonConvert.DeserializeObject<SaveAcadCollab_AcademicCollaboration>(obj_academic_collab_stringify);

                academicCollaborationRepo = new AcademicCollaborationRepo();
                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;

                    if (GoogleDriveService.credential == null && GoogleDriveService.driveService == null)
                    {
                        AlertModal<string> alertModal1 = new AlertModal<string>(null, false, "Lỗi", "Vui lòng liên hệ với quản trị hệ thống để được cấp quyền.");
                        return Json(new { alertModal1.obj, alertModal1.success, alertModal1.title, alertModal1.content });
                    }
                    else
                    {
                        Google.Apis.Drive.v3.Data.File f = academicCollaborationRepo.updateEvidenceFile(old_evidence, new_evidence, folder_name, 4, false);
                        //Save academic collab
                        AlertModal<AcademicCollaboration_Ext> alertModal = academicCollaborationRepo.updateAcademicCollaboration(direction_id, collab_type_id,
                            obj_person, obj_partner, obj_academic_collab, f, old_evidence, new_evidence, acc.account_id);
                        return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
                    }
                }
                else
                {
                    AlertModal<AcademicCollaboration_Ext> alertModal1 = new AlertModal<AcademicCollaboration_Ext>(null, false, "Lỗi", "Người dùng chưa đăng nhập.");
                    return Json(new { alertModal1.obj, alertModal1.success, alertModal1.title, alertModal1.content });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //DELETE
        [HttpPost]
        public ActionResult deleteAcademicCollaboration(int acad_collab_id)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<string> alertModal = academicCollaborationRepo.deleteAcademicCollaboration(acad_collab_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //CHANGE STATUS HISTORY
        [HttpPost]
        public ActionResult changeStatus(int collab_id, HttpPostedFileBase evidence_file, string folder_name, int status_id, string note)
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

                    AlertModal<string> alertModal = academicCollaborationRepo.changeStatus(collab_id, evidence_file, folder_name, status_id, note, acc.account_id);
                    return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
                } else
                {
                    AlertModal<string> alertModal1 = new AlertModal<string>(null, false, "Lỗi", "Người dùng chưa đăng nhập.");
                    return Json(new { alertModal1.obj, alertModal1.success, alertModal1.title, alertModal1.content });
                }
            } catch (Exception e)
            {
                throw e;
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
