using BLL.Authen;
using BLL.InternationalCollaboration.AcademicCollaborationRepository;
using BLL.InternationalCollaboration.MasterData;
using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities.SaveAcademicCollaborationEntities;
using MANAGER.Models;
using MANAGER.Support;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            ViewBag.languages = AcademicActivityTypeRepo.getLanguages().obj;
            return View();
        }

        [HttpPost]
        public ActionResult getListAcademicCollaboration(int direction, int collab_type_id, ObjectSearching_AcademicCollaboration obj_searching)
        {
            try
            {
                Session.Timeout = 120;
                Session["obj_searching"] = obj_searching;
                Session["direction"] = direction;
                Session["collab_type_id"] = collab_type_id;
                academicCollaborationRepo = new AcademicCollaborationRepo();
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<AcademicCollaboration_Ext> baseServerSideData = academicCollaborationRepo.AcademicCollaborations(direction, collab_type_id, obj_searching, baseDatatable);
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
        [HttpPost]
        public ActionResult ExportACExcel()
        {
            try
            {
                ObjectSearching_AcademicCollaboration obj_searching = (ObjectSearching_AcademicCollaboration)Session["obj_searching"];
                AcademicCollaborationExportRepo academicCollaborationRepo = new AcademicCollaborationExportRepo();
                System.IO.MemoryStream memoryStream = academicCollaborationRepo.ExportACExcel((int)Session["direction"], (int)Session["collab_type_id"], obj_searching);
                string downloadFile = "ACDownload.xlsx";
                string handle = Guid.NewGuid().ToString();
                TempData[handle] = memoryStream.ToArray();
                return Json(new { success = true, data = new { FileGuid = handle, FileName = downloadFile } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        public JsonResult getCountries(string country_name)
        {
            try
            {
                CountryRepo countryRepo = new CountryRepo();
                AlertModal<List<Country>> alertModal = new AlertModal<List<Country>>(countryRepo.GetCountries(country_name), true);
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
                AlertModal<List<Year>> alertModal = academicCollaborationRepo.YearSearching();
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
                AlertModal<List<Office>> alertModal = academicCollaborationRepo.Offices(office_name);
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
                AlertModal<List<AcademicCollaborationPartner_Ext>> alertModal = academicCollaborationRepo.Partners(partner_name);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public ActionResult getPartnersSearching(string partner_name)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<List<AcademicCollaborationPartner_Ext>> alertModal = academicCollaborationRepo.PartnersSearching(partner_name);
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
                AlertModal<List<AcademicCollaborationPerson_Ext>> alertModal = academicCollaborationRepo.People(person_name);
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
                AlertModal<List<CollaborationScope>> alertModal = academicCollaborationRepo.CollaborationScopes(collab_abbreviation_name);
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
                AlertModal<List<AcademicCollaborationStatu>> alertModal = academicCollaborationRepo.AcademicCollaborationStatus(status_type_specific);
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
                AlertModal<AcademicCollaborationPerson_Ext> alertModal = academicCollaborationRepo.Person(people_id, people_name);
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
                AlertModal<AcademicCollaborationPartner_Ext> alertModal = academicCollaborationRepo.Partner(partner_id, partner_name);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Auther(RightID = "10,11")]
        [HttpPost]
        public ActionResult saveAcademicCollaboration(HttpPostedFileBase evidence, string folder_name, int direction_id, int collab_type_id,
            string obj_person_stringify, string obj_partner_stringify, string obj_academic_collab_stringify)
        {
            try
            {
                //deserialize json obj string
                SaveAcadCollab_Person obj_person = JsonConvert.DeserializeObject<SaveAcadCollab_Person>(obj_person_stringify);
                SaveAcadCollab_Partner obj_partner = JsonConvert.DeserializeObject<SaveAcadCollab_Partner>(obj_partner_stringify);
                SaveAcadCollab_AcademicCollaboration obj_academic_collab = JsonConvert.DeserializeObject<SaveAcadCollab_AcademicCollaboration>(obj_academic_collab_stringify);

                academicCollaborationRepo = new AcademicCollaborationRepo();
                int account_id = CurrentAccount.AccountID(Session);
                if (account_id != 0)
                {
                    //upload file
                    if (GoogleDriveService.credential == null && GoogleDriveService.driveService == null)
                    {
                        AlertModal<string> alertModal1 = new AlertModal<string>(null, false, "Lỗi", "Vui lòng liên hệ với quản trị hệ thống để được cấp quyền.");
                        return Json(new { alertModal1.obj, alertModal1.success, alertModal1.title, alertModal1.content });
                    }
                    else
                    {
                        //Save academic collab
                        AlertModal<AcademicCollaboration_Ext> alertModal = academicCollaborationRepo.SaveAcademicCollaboration(direction_id, collab_type_id,
                            obj_person, obj_partner, obj_academic_collab, evidence, folder_name, account_id);
                        return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
                    }
                }
                else
                {
                    AlertModal<string> alertModal = new AlertModal<string>(null, false, "Lỗi", "Người dùng chưa đăng nhập vào hệ thống");
                    return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
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
                AlertModal<AcademicCollaboration_Ext> alertModal = academicCollaborationRepo.GetAcademicCollaboration(direction, collab_type_id, acad_collab_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Auther(RightID = "10,11")]
        [HttpPost]
        public ActionResult updateAcademicCollaboration(HttpPostedFileBase new_evidence, string file_action, string folder_name, int direction_id, int collab_type_id, string obj_person_stringify, string obj_partner_stringify, string obj_academic_collab_stringify)
        {
            try
            {
                //parse to Object
                SaveAcadCollab_Person obj_person = JsonConvert.DeserializeObject<SaveAcadCollab_Person>(obj_person_stringify);
                SaveAcadCollab_Partner obj_partner = JsonConvert.DeserializeObject<SaveAcadCollab_Partner>(obj_partner_stringify);
                SaveAcadCollab_AcademicCollaboration obj_academic_collab = JsonConvert.DeserializeObject<SaveAcadCollab_AcademicCollaboration>(obj_academic_collab_stringify);

                academicCollaborationRepo = new AcademicCollaborationRepo();
                int account_id = CurrentAccount.AccountID(Session);
                if (account_id != 0)
                {
                    if (GoogleDriveService.credential == null && GoogleDriveService.driveService == null)
                    {
                        AlertModal<string> alertModal1 = new AlertModal<string>(null, false, "Lỗi", "Vui lòng liên hệ với quản trị hệ thống để được cấp quyền.");
                        return Json(new { alertModal1.obj, alertModal1.success, alertModal1.title, alertModal1.content });
                    }
                    else
                    {
                        //Save academic collab
                        AlertModal<AcademicCollaboration_Ext> alertModal = academicCollaborationRepo.UpdateAcademicCollaboration(direction_id, collab_type_id,
                            obj_person, obj_partner, obj_academic_collab, new_evidence, file_action, folder_name, account_id);
                        return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
                    }
                }
                else
                {
                    AlertModal<string> alertModal = new AlertModal<string>(null, false, "Lỗi", "Người dùng chưa đăng nhập vào hệ thống");
                    return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Auther(RightID = "10,11")]
        //DELETE
        [HttpPost]
        public ActionResult deleteAcademicCollaboration(int acad_collab_id)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                AlertModal<string> alertModal = academicCollaborationRepo.DeleteAcademicCollaboration(acad_collab_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //VIEW STATUS HISTORY
        [HttpGet]
        public ActionResult getStatusHistories(int collab_id)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<StatusHistory> baseServerSideData = academicCollaborationRepo.GetStatusHistories(baseDatatable, collab_id);
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

        [Auther(RightID = "10,11")]
        //CHANGE STATUS HISTORY
        [HttpPost]
        public ActionResult changeStatus(int collab_id, HttpPostedFileBase evidence_file, string folder_name, string status_id, string note)
        {
            try
            {
                academicCollaborationRepo = new AcademicCollaborationRepo();
                int account_id = CurrentAccount.AccountID(Session);
                if (account_id != 0)
                {
                    AlertModal<string> alertModal = academicCollaborationRepo.ChangeStatus(collab_id, evidence_file, folder_name, status_id, note, account_id);
                    return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
                }
                else
                {
                    AlertModal<string> alertModal = new AlertModal<string>(null, false, "Lỗi", "Người dùng chưa đăng nhập vào hệ thống");
                    return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        //LONG-TERM CONTENT
        public ActionResult getLTContent(int collab_type_id, int language_id)
        {
            try
            {
                AcademicCollaborationLongRepo academicCollaborationLongRepo = new AcademicCollaborationLongRepo();
                AlertModal<AcademicCollaborationTypeLanguage> alertModal = academicCollaborationLongRepo.GetLTContent(collab_type_id, language_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Auther(RightID = "10,11")]
        //LONG-TERM UPDATE CONTENT
        [HttpPost]
        public ActionResult updateLTContent(int collab_type_id, int language_id, string description)
        {
            try
            {
                AcademicCollaborationLongRepo academicCollaborationLongRepo = new AcademicCollaborationLongRepo();
                AlertModal<string> alertModal = academicCollaborationLongRepo.UpdateLTContent(collab_type_id, language_id, description);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        //LONG-TERM GOING || COMING CONTENT
        public ActionResult getLTGCContent(int direction_id, int collab_type_id, int language_id)
        {
            try
            {
                AcademicCollaborationLongRepo academicCollaborationLongRepo = new AcademicCollaborationLongRepo();
                AlertModal<CollaborationTypeDirectionLanguage> alertModal = academicCollaborationLongRepo.GetLTGCContent(direction_id, collab_type_id, language_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Auther(RightID = "10,11")]
        //LONG-TERM GOING || COMING UPDATE CONTENT
        [HttpPost]
        public ActionResult updateLTGCContent(int collab_type_direction_id, int language_id, string description)
        {
            try
            {
                AcademicCollaborationLongRepo academicCollaborationLongRepo = new AcademicCollaborationLongRepo();
                AlertModal<string> alertModal = academicCollaborationLongRepo.UpdateLTGCContent(collab_type_direction_id, language_id, description);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /*--------------------------------------------------------SHORT TERM---------------------------------------------------------*/
        AcademicCollaborationShortRepo acShortRepo;
        AcademicCollaborationProgramRepo acProgramRepo;
        public ActionResult Shortterm_List()
        {
            ViewBag.title = "DANH SÁCH TRAO ĐỔI CÁN BỘ GIẢNG VIÊN";
            ViewBag.languages = AcademicActivityTypeRepo.getLanguages().obj;
            return View();
        }
        //---------------------------------------------------------procedure----------------------------------------------------------
        [HttpPost]
        public ActionResult GetProcedureList(int direction, string title)
        {
            try
            {
                acShortRepo = new AcademicCollaborationShortRepo();
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<ProcedureInfoManager> baseServerSideData = acShortRepo.GetListProcedure(baseDatatable, title, direction, 1);
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
                return Json(new { data = "" });
            }
        }

        [Auther(RightID = "11")]
        [HttpPost, ValidateInput(false)]
        public ActionResult AddProcedure(string procedure_title, string content, int direction,
            int numberOfImage, int partner_language_type)
        {
            try
            {
                acShortRepo = new AcademicCollaborationShortRepo();
                List<HttpPostedFileBase> files_request = new List<HttpPostedFileBase>();
                for (int i = 0; i < numberOfImage; i++)
                {
                    string label = "image_" + i;
                    files_request.Add(Request.Files[label]);
                }
                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;
                }
                if (acc.account_id == 0)
                {
                    AlertModal<string> json_false = new AlertModal<string>(false, "Chưa đăng nhập không thể thêm bài");
                    return Json(new { json_false.success, json_false.content });
                }
                AlertModal<string> json = acShortRepo.AddProcedure(files_request, procedure_title, direction,
                    content, numberOfImage, partner_language_type, acc.account_id);
                return Json(new { json.success, json.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                AlertModal<string> json_false = new AlertModal<string>(false, "Có lỗi xảy ra");
                return Json(new { json_false.success, json_false.content });
            }
        }

        [HttpPost]
        public ActionResult LoadEditProcedure(int procedure_id)
        {
            try
            {
                acShortRepo = new AcademicCollaborationShortRepo();
                ProcedureInfoManager procedureInfoManager = acShortRepo.LoadEditProcedure(procedure_id);
                ViewBag.procedure_id = procedure_id;
                return Json(new { json = procedureInfoManager });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                AlertModal<string> json = new AlertModal<string>(false, "Có lỗi xảy ra");
                return Json(new { json.success, json.content });
            }
        }

        [HttpPost]
        public ActionResult LoadContentDetailLanguage(int procedure_id, int language_id)
        {
            try
            {
                acShortRepo = new AcademicCollaborationShortRepo();
                ArticleVersion articleVersion = acShortRepo.GetContentLanguage(procedure_id, language_id);
                return Json(new { json = new AlertModal<string>(true, "Đổi ngôn ngữ thành công"), articleVersion });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new
                {
                    json = new AlertModal<string>(false, "Có lỗi xảy ra")
                });
            }
        }

        [Auther(RightID = "11")]
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEdit(int procedure_id, string content, int numberOfImage, string procedure_name, int language_id)
        {
            try
            {
                acShortRepo = new AcademicCollaborationShortRepo();
                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;
                }

                ProcedureInfoManager procedureInfoManager = new ProcedureInfoManager
                {
                    procedure_id = procedure_id,
                    procedure_name = procedure_name,
                    content = content,
                    language_id = language_id,
                };

                List<HttpPostedFileBase> files_request = new List<HttpPostedFileBase>();
                for (int i = 0; i < numberOfImage; i++)
                {
                    string label = "image_" + i;
                    files_request.Add(Request.Files[label]);
                }
                if (acc.account_id == 0)
                {
                    return Json(new
                    {
                        json = new AlertModal<string>(false, "Chưa đăng nhập không thể thêm bài")
                    });
                }
                else
                {
                    AlertModal<string> json = acShortRepo.SaveEditProcedure(files_request, procedureInfoManager, numberOfImage, acc.account_id);
                    return Json(new { json.success, json.content });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new
                {
                    json = new AlertModal<string>(false, "Có lỗi xảy ra")
                });
            }
        }

        [Auther(RightID = "11")]
        [HttpPost]
        public ActionResult DeleteProcedure(int procedure_id)
        {
            try
            {
                acShortRepo = new AcademicCollaborationShortRepo();
                AlertModal<string> json = acShortRepo.DeleteProcedure(procedure_id);
                return Json(new { json.success, json.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                AlertModal<string> json = new AlertModal<string>(false, "Có lỗi xảy ra");
                return Json(new { json.success, json.content });
            }
        }

        //---------------------------------------------------------program----------------------------------------------------------
        [HttpPost]
        public ActionResult GetProgramList(int direction, string title, int duration, int collab_type_id)
        {
            try
            {
                acProgramRepo = new AcademicCollaborationProgramRepo();
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<ProgramInfoManager> baseServerSideData =
                    acProgramRepo.GetListProgram(baseDatatable, title, duration, direction, collab_type_id);
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
                return Json(new { data = "" });
            }
        }

        [Auther(RightID = "10,11")]
        [HttpPost, ValidateInput(false)]
        public ActionResult AddProgram(int direction, int numberOfImage, string program_title, string collab_type,
            string program_partner, int program_language, string add_program_start_date, string add_program_end_date, string note, string content)
        {
            try
            {
                acProgramRepo = new AcademicCollaborationProgramRepo();
                List<HttpPostedFileBase> files_request = new List<HttpPostedFileBase>();
                for (int i = 0; i < numberOfImage; i++)
                {
                    string label = "image_" + i;
                    files_request.Add(Request.Files[label]);
                }
                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;
                }
                if (acc.account_id == 0)
                {
                    AlertModal<string> json_false = new AlertModal<string>(false, "Chưa đăng nhập không thể thêm bài");
                    return Json(new { json_false.success, json_false.content });
                }
                AlertModal<string> json = acProgramRepo.AddProgram(files_request, program_title, Int32.Parse(collab_type), direction,
                    content, numberOfImage, program_language, acc.account_id, program_partner, add_program_start_date, add_program_end_date, note);
                return Json(new { json.success, json.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                AlertModal<string> json_false = new AlertModal<string>(false, "Có lỗi xảy ra");
                return Json(new { json_false.success, json_false.content });
            }
        }

        [HttpPost]
        public ActionResult LoadEditProgram(int program_id)
        {
            try
            {
                acProgramRepo = new AcademicCollaborationProgramRepo();
                ProgramInfoManager programInfoManager = acProgramRepo.LoadEditProgram(program_id);
                return Json(new { json = programInfoManager });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                AlertModal<string> json = new AlertModal<string>(false, "Có lỗi xảy ra");
                return Json(new { json.success, json.content });
            }
        }

        [Auther(RightID = "10,11")]
        public ActionResult DeleteProgram(string article_id)
        {
            try
            {
                acProgramRepo = new AcademicCollaborationProgramRepo();
                AlertModal<string> json = acProgramRepo.DeleteProgram(Int32.Parse(article_id));
                return Json(new { json.success, json.content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                AlertModal<string> json_false = new AlertModal<string>(false, "Có lỗi xảy ra");
                return Json(new { json_false.success, json_false.content });
            }
        }

        [Auther(RightID = "10,11")]
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveEditProgram(string program_id, string content, int numberOfImage, string program_title,
            int program_language, string program_partner, string edit_program_start_date, string edit_program_end_date, string note, string direction)
        {
            try
            {
                acProgramRepo = new AcademicCollaborationProgramRepo();
                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;
                }

                List<HttpPostedFileBase> files_request = new List<HttpPostedFileBase>();
                for (int i = 0; i < numberOfImage; i++)
                {
                    string label = "image_" + i;
                    files_request.Add(Request.Files[label]);
                }
                if (acc.account_id == 0)
                {
                    return Json(new
                    {
                        json = new AlertModal<string>(false, "Chưa đăng nhập không thể thêm bài")
                    });
                }
                else
                {
                    AlertModal<string> json = acProgramRepo.SaveEditProgram(files_request, Int32.Parse(program_id), program_title, content, numberOfImage, program_language,
                        program_partner, edit_program_start_date, edit_program_end_date, note, Int32.Parse(direction), acc.account_id);
                    return Json(new { json.success, json.content });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new
                {
                    json = new AlertModal<string>(false, "Có lỗi xảy ra")
                });
            }
        }

        [HttpPost]
        public ActionResult LoadProgramDetailLanguage(string program_id, int language_id)
        {
            try
            {
                acProgramRepo = new AcademicCollaborationProgramRepo();
                ArticleVersion articleVersion = acProgramRepo.LoadProgramDetailLanguage(Int32.Parse(program_id), language_id);
                return Json(new { json = new AlertModal<string>(true, "Đổi ngôn ngữ thành công"), articleVersion });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new
                {
                    json = new AlertModal<string>(false, "Có lỗi xảy ra")
                });
            }
        }
    }
}
