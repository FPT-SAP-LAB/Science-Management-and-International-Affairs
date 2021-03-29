using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MANAGER.Models;
using BLL.InternationalCollaboration.AcademicActivity;
using MANAGER.Support;
using ENTITIES;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class DetailOfAcademicActivityController : Controller
    {
        DetailOfAcademicActivityRepo repo;
        FormRepo formRepo;
        AcademicActivityPhaseRepo phaseRepo;
        [Auther(RightID = "3")]
        public ActionResult Index(int id)
        {
            repo = new DetailOfAcademicActivityRepo();
            phaseRepo = new AcademicActivityPhaseRepo();
            ViewBag.Title = "Thông tin hoạt động học thuật";
            ViewBag.activity_id = id;
            ViewBag.types = repo.getType(1);
            ViewBag.unit = repo.getUnits();
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
            bool res = phaseRepo.editPhase(language_id,data);
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
            List<PlanParticipant> data = phaseRepo.getParticipantPlanByRole(participant_role_id);
            return Json(data);
        }
        public JsonResult add_Tucach(int id, string name, int quantity, bool by, List<QuantityByUnit> arr)
        {
            try
            {
                JsonError jerr = new JsonError()
                {
                    code = 1,
                    err_content = "Đã thêm thành công"
                };
                return Json(jerr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                JsonError jerr = new JsonError()
                {
                    code = 2,
                    err_content = "Có lỗi xảy ra. Vui lòng thử lại"
                };
                return Json(jerr, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult delete_Tucach(int id)
        {
            try
            {
                JsonError jerr = new JsonError()
                {
                    code = 1,
                    err_content = "Đã xóa thành công"
                };
                return Json(jerr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                JsonError jerr = new JsonError()
                {
                    code = 2,
                    err_content = "Có lỗi xảy ra. Vui lòng thử lại"
                };
                return Json(jerr, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult edit_Tucach(int id)
        {
            try
            {
                JsonError jerr = new JsonError()
                {
                    code = 1,
                    err_content = "Đã chỉnh sửa thành công"
                };
                return Json(jerr, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                JsonError jerr = new JsonError()
                {
                    code = 2,
                    err_content = "Có lỗi xảy ra. Vui lòng thử lại"
                };
                return Json(jerr, JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult updateForm(DetailOfAcademicActivityRepo.baseForm data)
        {
            formRepo = new FormRepo();
            bool res = formRepo.updateForm(data);
            if (res)
            {
                return Json("Lưu mẫu đăng ký thành công");
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
        public class QuantityByUnit
        {
            public string name { get; set; }
            public int quantity { get; set; }
        }
    }
}