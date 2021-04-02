using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MANAGER.Models;
using BLL.InternationalCollaboration.AcademicActivity;
using MANAGER.Support;
using ENTITIES;
using Newtonsoft.Json;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicActivity;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class DetailOfAcademicActivityController : Controller
    {
        DetailOfAcademicActivityRepo repo;
        FormRepo formRepo;
        AcademicActivityExpenseRepo expenseRepo;
        AcademicActivityPhaseRepo phaseRepo;
        AcademicActivityPartnerRepo partnerRepo;
        [Auther(RightID = "3")]
        public ActionResult Index(int id)
        {
            repo = new DetailOfAcademicActivityRepo();
            phaseRepo = new AcademicActivityPhaseRepo();
            ViewBag.Title = "Thông tin hoạt động học thuật";
            ViewBag.activity_id = id;
            ViewBag.types = repo.getType(1);
            ViewBag.unit = repo.getUnits();
            ViewBag.office = phaseRepo.getOffices();
            return View();
        }
        [HttpPost]
        public JsonResult getDetail(int language_id, int activity_id)
        {
            repo = new DetailOfAcademicActivityRepo();
            DetailOfAcademicActivityRepo.SumDetail data = new DetailOfAcademicActivityRepo.SumDetail
            {
                baseDetail = repo.getDetail(language_id, activity_id),
                subContent = repo.getSubContents(language_id, activity_id),
                types = repo.getType(language_id)
            };
            return Json(data);
        }
        [HttpPost]
        public JsonResult updateDetail(DetailOfAcademicActivityRepo.InfoSumDetail obj)
        {
            try
            {
                repo = new DetailOfAcademicActivityRepo();
                BLL.Authen.LoginRepo.User u = (BLL.Authen.LoginRepo.User)Session["User"];
                bool res = repo.updateDetail(obj, u);
                if (res)
                {
                    return Json("Cập nhật thành công");
                }
                else
                    return Json(String.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(String.Empty);
            }
        }
        [HttpPost]
        public JsonResult changeStatusAA(int activity_id, int status)
        {
            repo = new DetailOfAcademicActivityRepo();
            bool res = repo.changeStatusAA(activity_id, status);
            if (res)
            {
                return Json(status == 2 ? "Đăng bài thành công" : "Thu hồi thành công");
            }
            else
            {
                return Json(String.Empty);
            }
        }
        [HttpPost]
        public ActionResult getDatatableDTC(int activity_id)
        {
            repo = new DetailOfAcademicActivityRepo();
            List<DetailOfAcademicActivityRepo.basePartner> data = repo.getDatatableDTC(activity_id);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult getContent(int activity_partner_id)
        {
            repo = new DetailOfAcademicActivityRepo();
            DetailOfAcademicActivityRepo.ContactInfo data = repo.getContact(activity_partner_id);
            return Json(data);
        }
        [HttpPost]
        public ActionResult getPhase(int language_id, int activity_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            List<AcademicActivityPhaseRepo.infoPhase> data = phaseRepo.getPhase(language_id, activity_id);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult getDetailPhase(int language_id, int phase_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            AcademicActivityPhaseRepo.basePhase data = phaseRepo.getDetailPhase(language_id, phase_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult addPhase(int language_id, int activity_id, AcademicActivityPhaseRepo.basePhase basePhase)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            int account_id = CurrentAccount.AccountID(Session);
            bool res = phaseRepo.addPhase(language_id, activity_id, account_id, basePhase);
            if (res)
            {
                return Json("Thêm giai đoạn thành công");
            }
            else
            {
                return Json(String.Empty);
            }
        }
        [HttpPost]
        public JsonResult deletePhase(int phase_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.deletePhase(phase_id);
            if (res)
            {
                return Json("Xóa giai đoạn thành công");
            }
            else
            {
                return Json(String.Empty);
            }
        }
        [HttpPost]
        public JsonResult editPhase(int language_id, AcademicActivityPhaseRepo.infoPhase data)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.editPhase(language_id, data);
            if (res)
            {
                return Json("Chỉnh sửa giai đoạn thành công");
            }
            else
            {
                return Json(String.Empty);
            }
        }
        [HttpPost]
        public ActionResult getParticipantRoleByPhase(int phase_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            List<AcademicActivityPhaseRepo.baseParticipantRole> data = phaseRepo.getParticipantRoleByPhase(phase_id);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult getParticipantPlanByRole(int participant_role_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            AcademicActivityPhaseRepo.infoPlanParticipant data = phaseRepo.getParticipantPlanByRole(participant_role_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult addParticipantRole(AcademicActivityPhaseRepo.baseParticipantRole baseParticipantRole, List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice, string check, string quantity, int phase_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.addParticipantRole(baseParticipantRole, arrOffice, check, quantity, phase_id);
            if (res)
            {
                return Json("Thêm tư cách tham gia thành công");
            }
            else
                return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult editParticipantRole(AcademicActivityPhaseRepo.baseParticipantRole baseParticipantRole, List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice, string check, string quantity, int phase_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.editParticipantRole(baseParticipantRole, arrOffice, check, quantity, phase_id);
            if (res)
            {
                return Json("Chỉnh sửa tư cách tham gia thành công");
            }
            else
                return Json(String.Empty);
        }
        public JsonResult deleteParticipantRole(int participant_role_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.deleteParticipantRole(participant_role_id);
            if (res)
            {
                return Json("Xóa tư cách tham gia thành công");
            }
            else
                return Json(String.Empty);
        }
        public ActionResult RegisterForm(int id, int lid)
        {
            ViewBag.phaseID = id;
            ViewBag.language_id = lid;
            ViewBag.pageTitle = "Mẫu đăng kí hoạt động học thuật";
            return View();
        }
        [HttpPost]
        public JsonResult getFormbyPhase(int phase_id)
        {
            formRepo = new FormRepo();
            DetailOfAcademicActivityRepo.baseForm data = formRepo.getFormbyPhase(phase_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult updateForm(DetailOfAcademicActivityRepo.baseForm data, List<DetailOfAcademicActivityRepo.CustomQuestion> data_unchange)
        {
            formRepo = new FormRepo();
            bool res = formRepo.updateForm(data, data_unchange);
            if (res)
            {
                return Json("Lưu mẫu đăng ký thành công");
            }
            else
                return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult deleteForm(int phase_id)
        {
            formRepo = new FormRepo();
            bool res = formRepo.deleteForm(phase_id);
            if (res)
            {
                return Json("Xóa mẫu đăng ký thành công");
            }
            else
                return Json(String.Empty);
        }
        public JsonResult getOffice()
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            List<AcademicActivityPhaseRepo.baseOffice> data = phaseRepo.getOffices();
            return Json(data);
        }
        [HttpPost]
        public ActionResult getDatatableKP(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            List<AcademicActivityExpenseRepo.infoExpense> data = expenseRepo.getDatatableKP(activity_id);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult addExpense(AcademicActivityExpenseRepo.baseExpense data)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            string res = expenseRepo.addExpense(data);
            if (!String.IsNullOrEmpty(res))
            {
                return Json(res);
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult deleteExpense(int activity_office_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            bool res = expenseRepo.deleteExpense(activity_office_id);
            if (res)
            {
                return Json("Xóa mục kinh phí thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public ActionResult getDatatableKPDuTru(int activity_office_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            List<AcademicActivityExpenseRepo.infoExpenseEstimate> data = expenseRepo.getDatatableKPDuTru(activity_office_id);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult addExpenseDuTru(int activity_office_id, string activity_name, AcademicActivityExpenseRepo.infoExpenseEstimate data, HttpPostedFileBase img)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            bool res = expenseRepo.addExpenseDuTru(activity_office_id, activity_name, data, img);
            if (res)
                return Json("Thêm mục kinh phí dự trù thành công");
            return Json(String.Empty);
        }
        public JsonResult saveActivityPartner(HttpPostedFileBase evidence_file, string folder_name, string obj_activity_partner_stringify)
        {
            try
            {
                partnerRepo = new AcademicActivityPartnerRepo();
                SaveActivityPartner activityPartner = JsonConvert.DeserializeObject<SaveActivityPartner>(obj_activity_partner_stringify);
                AlertModal<string> alertModal = partnerRepo.saveActivityPartner(evidence_file, folder_name, activityPartner);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpGet]
        public JsonResult getActivityPartner(int activity_partner_id)
        {
            try
            {
                partnerRepo = new AcademicActivityPartnerRepo();
                AlertModal<ActivityPartner_Ext> alertModal = partnerRepo.getActivityPartner(activity_partner_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpPost]
        public JsonResult updateActivityPartner(HttpPostedFileBase evidence_file, string folder_name, string obj_activity_partner_stringify)
        {
            try
            {
                partnerRepo = new AcademicActivityPartnerRepo();
                SaveActivityPartner saveActivityPartner = JsonConvert.DeserializeObject<SaveActivityPartner>(obj_activity_partner_stringify);
                AlertModal<string> alertModal = partnerRepo.updateActivityPartner(evidence_file, folder_name, saveActivityPartner);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpPost]
        public JsonResult deleteActivityPartner(int activity_partner_id)
        {
            try
            {
                partnerRepo = new AcademicActivityPartnerRepo();
                AlertModal<string> alertModal = partnerRepo.deleteActivityPartner(activity_partner_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}