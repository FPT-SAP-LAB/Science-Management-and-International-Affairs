using BLL.InternationalCollaboration.AcademicActivity;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicActivity;
using MANAGER.Models;
using MANAGER.Support;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class DetailOfAcademicActivityController : Controller
    {
        DetailOfAcademicActivityRepo repo;
        FormRepo formRepo;
        AcademicActivityExpenseRepo expenseRepo;
        AcademicActivityPhaseRepo phaseRepo;
        AcademicActivityPartnerRepo partnerRepo;
        AcademicActivityExcelRepo excelRepo;
        [Auther(RightID = "3")]
        public ActionResult Index(int id)
        {
            repo = new DetailOfAcademicActivityRepo();
            phaseRepo = new AcademicActivityPhaseRepo();
            ViewBag.Title = "THÔNG TIN HOẠT ĐỘNG HỌC THUẬT";
            ViewBag.activity_id = id;
            ViewBag.types = repo.GetType(1);
            ViewBag.unit = repo.GetUnits();
            ViewBag.office = phaseRepo.GetOffices();
            return View();
        }
        [HttpPost]
        public JsonResult getDetail(int language_id, int activity_id)
        {
            repo = new DetailOfAcademicActivityRepo();
            DetailOfAcademicActivityRepo.SumDetail data = new DetailOfAcademicActivityRepo.SumDetail
            {
                baseDetail = repo.GetDetail(language_id, activity_id),
                subContent = repo.GetSubContents(language_id, activity_id),
                types = repo.GetType(language_id)
            };
            return Json(data);
        }
        [HttpPost]
        public JsonResult getDetailFirst(int language_id, int activity_id)
        {
            repo = new DetailOfAcademicActivityRepo();
            DetailOfAcademicActivityRepo.baseDetail baseDetail = repo.GetDetailFirst(language_id, activity_id);
            DetailOfAcademicActivityRepo.SumDetail data = new DetailOfAcademicActivityRepo.SumDetail
            {
                baseDetail = baseDetail,
                subContent = repo.GetSubContents(baseDetail.language_id, activity_id),
                types = repo.GetType(baseDetail.language_id)
            };
            return Json(data);
        }
        [Auther(RightID = "3")]
        [HttpPost, ValidateInput(false)]
        public JsonResult updateDetail(string obj)
        {
            try
            {
                DetailOfAcademicActivityRepo.InfoSumDetail infoSumDetail =
                    JsonConvert.DeserializeObject<DetailOfAcademicActivityRepo.InfoSumDetail>(obj);
                repo = new DetailOfAcademicActivityRepo();
                BLL.Authen.LoginRepo.User u = (BLL.Authen.LoginRepo.User)Session["User"];
                List<HttpPostedFileBase> list_image_main = new List<HttpPostedFileBase>();
                for (int i = 0; i < infoSumDetail.infoDetail.count_upload_main; i++)
                {
                    string label = "image_" + i;
                    list_image_main.Add(Request.Files[label]);
                }
                List<List<HttpPostedFileBase>> list_image_sub = new List<List<HttpPostedFileBase>>();
                foreach (var item in infoSumDetail.subContent)
                {
                    List<HttpPostedFileBase> list_image = new List<HttpPostedFileBase>();
                    if (item.number_of_image > 0)
                    {
                        for (int i = 0; i < item.number_of_image; i++)
                        {
                            string label = "image_" + item.sub_id + "_" + i;
                            list_image.Add(Request.Files[label]);
                        }
                        list_image_sub.Add(list_image);
                    }
                }

                bool res = repo.UpdateDetail(infoSumDetail, u, list_image_main, list_image_sub);
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
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult changeStatusAA(int activity_id, int status)
        {
            repo = new DetailOfAcademicActivityRepo();
            bool res = repo.ChangeStatusAA(activity_id, status);
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
            List<DetailOfAcademicActivityRepo.basePartner> data = repo.GetDatatableDTC(activity_id);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult getContent(int activity_partner_id)
        {
            repo = new DetailOfAcademicActivityRepo();
            DetailOfAcademicActivityRepo.ContactInfo data = repo.GetContact(activity_partner_id);
            return Json(data);
        }
        [HttpPost]
        public ActionResult getPhase(int language_id, int activity_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            List<AcademicActivityPhaseRepo.infoPhase> data = phaseRepo.GetPhase(language_id, activity_id);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult getDetailPhase(int language_id, int phase_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            AcademicActivityPhaseRepo.basePhase data = phaseRepo.GetDetailPhase(language_id, phase_id);
            return Json(data);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult addPhase(int language_id, int activity_id, AcademicActivityPhaseRepo.basePhase basePhase)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            int account_id = CurrentAccount.AccountID(Session);
            bool res = phaseRepo.AddPhase(language_id, activity_id, account_id, basePhase);
            if (res)
            {
                return Json("Thêm giai đoạn thành công");
            }
            else
            {
                return Json(String.Empty);
            }
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult deletePhase(int phase_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.DeletePhase(phase_id);
            if (res)
            {
                return Json("Xóa giai đoạn thành công");
            }
            else
            {
                return Json(String.Empty);
            }
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult editPhase(int language_id, AcademicActivityPhaseRepo.infoPhase data)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.EditPhase(language_id, data);
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
            List<AcademicActivityPhaseRepo.baseParticipantRole> data = phaseRepo.GetParticipantRoleByPhase(phase_id);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult getParticipantPlanByRole(int participant_role_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            AcademicActivityPhaseRepo.infoPlanParticipant data = phaseRepo.GetParticipantPlanByRole(participant_role_id);
            return Json(data);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult addParticipantRole(AcademicActivityPhaseRepo.baseParticipantRole baseParticipantRole, List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice, string check, string quantity, int phase_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.AddParticipantRole(baseParticipantRole, arrOffice, check, quantity, phase_id);
            if (res)
            {
                return Json("Thêm tư cách tham gia thành công");
            }
            else
                return Json(String.Empty);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult editParticipantRole(AcademicActivityPhaseRepo.baseParticipantRole baseParticipantRole, List<AcademicActivityPhaseRepo.basePlanParticipant> arrOffice, string check, string quantity, int phase_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.EditParticipantRole(baseParticipantRole, arrOffice, check, quantity, phase_id);
            if (res)
            {
                return Json("Chỉnh sửa tư cách tham gia thành công");
            }
            else
                return Json(String.Empty);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult deleteParticipantRole(int participant_role_id)
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            bool res = phaseRepo.DeleteParticipantRole(participant_role_id);
            if (res)
            {
                return Json("Xóa tư cách tham gia thành công");
            }
            else
                return Json(String.Empty);
        }
        public ActionResult RegisterForm(int pid)
        {
            ViewBag.phaseID = pid;
            ViewBag.pageTitle = "Mẫu đăng kí hoạt động học thuật";
            return View();
        }
        [HttpPost]
        public JsonResult getFormbyPhase(int phase_id)
        {
            formRepo = new FormRepo();
            DetailOfAcademicActivityRepo.baseForm data = formRepo.GetFormbyPhase(phase_id);
            return Json(data);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult updateForm(DetailOfAcademicActivityRepo.baseForm data, List<DetailOfAcademicActivityRepo.CustomQuestion> data_unchange)
        {
            formRepo = new FormRepo();
            bool res = formRepo.UpdateForm(data, data_unchange);
            if (res)
            {
                return Json("Lưu mẫu đăng ký thành công");
            }
            else
                return Json(String.Empty);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult deleteForm(int phase_id)
        {
            formRepo = new FormRepo();
            bool res = formRepo.DeleteForm(phase_id);
            if (res)
            {
                return Json("Xóa mẫu đăng ký thành công");
            }
            else
                return Json(String.Empty);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult sendEmailForm(FormRepo.EmailForm data)
        {
            formRepo = new FormRepo();
            bool res = formRepo.SendEmailForm(data);
            if (res)
            {
                return Json("Gửi email đến người đăng ký thành công");
            }
            else
                return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult getRealtimeParticipant(int phase_id)
        {
            formRepo = new FormRepo();
            FormRepo.viewResponse data = formRepo.GetResponse(phase_id);
            return Json(data);
        }
        public JsonResult getOffice()
        {
            phaseRepo = new AcademicActivityPhaseRepo();
            List<AcademicActivityPhaseRepo.baseOffice> data = phaseRepo.GetOffices();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult getDatatableKP(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            List<AcademicActivityExpenseRepo.infoExpense> data = expenseRepo.GetDatatableKP(activity_id);
            return Json(new { success = true, data = data });
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult addExpense(AcademicActivityExpenseRepo.baseExpense data)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            string res = expenseRepo.AddExpense(data);
            if (!String.IsNullOrEmpty(res))
            {
                return Json(res);
            }
            else return Json(String.Empty);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult deleteExpense(int activity_office_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            bool res = expenseRepo.DeleteExpense(activity_office_id);
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
            List<AcademicActivityExpenseRepo.infoExpenseEstimate> data = expenseRepo.GetDatatableKPDuTru(activity_office_id);
            return Json(new { success = true, data = data });
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult addExpenseDuTru(int activity_office_id, string activity_name, string data, HttpPostedFileBase img)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            bool res = expenseRepo.AddExpenseDuTru(activity_office_id, activity_name, data, img);
            if (res)
                return Json("Thành công");
            return Json(String.Empty);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult deleteExpenseType(int expense_category_id, int type)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            bool res = expenseRepo.DeleteExpenseType(expense_category_id, type);
            if (res)
                return Json("Thành công");
            return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult getExpenseType(int expense_category_id, int type)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            AcademicActivityExpenseRepo.infoExpenseEstimate data = expenseRepo.GetExpenseType(expense_category_id, type);
            return Json(data);
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult editExpenseDuTru(int activity_office_id, string activity_name, string data, HttpPostedFileBase img, string file_action)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            bool res = expenseRepo.EditExpenseDuTru(activity_office_id, activity_name, data, img, file_action);
            if (res)
                return Json("Thành công");
            return Json(String.Empty);
        }
        [HttpPost]
        public ActionResult getDatatableKPDieuChinh(int activity_office_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            List<AcademicActivityExpenseRepo.infoExpenseModified> data = expenseRepo.GetDatatableKPDieuChinh(activity_office_id);
            return Json(new { success = true, data = data });
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult editExpenseDieuChinh(int activity_office_id, string activity_name, string data, HttpPostedFileBase img, string file_action)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            bool res = expenseRepo.EditExpenseDieuChinh(activity_office_id, activity_name, data, img, file_action);
            if (res)
                return Json("Thành công");
            return Json(String.Empty);
        }
        [HttpPost]
        public ActionResult getDatatableKPThucTe(int activity_office_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            List<AcademicActivityExpenseRepo.infoExpenseModified> data = expenseRepo.GetDatatableKPThucTe(activity_office_id);
            return Json(new { success = true, data = data });
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult editExpenseThucTe(int activity_office_id, string activity_name, string data, HttpPostedFileBase img, string file_action)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            bool res = expenseRepo.EditExpenseThucTe(activity_office_id, activity_name, data, img, file_action);
            if (res)
                return Json("Thành công");
            return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult getTotal(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            AcademicActivityExpenseRepo.Statistic data = expenseRepo.GetTotal(activity_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult getStatisticOffice(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            List<AcademicActivityExpenseRepo.Statistic> data = expenseRepo.GetStatisticOffice(activity_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult getStatisticUnit(int activity_id)
        {
            expenseRepo = new AcademicActivityExpenseRepo();
            List<AcademicActivityExpenseRepo.Statistic> data = expenseRepo.GetStatisticUnit(activity_id);
            return Json(data);
        }
        [Auther(RightID = "3")]
        public JsonResult saveActivityPartner(HttpPostedFileBase evidence_file, string folder_name, string obj_activity_partner_stringify)
        {
            try
            {
                int account_id = CurrentAccount.AccountID(Session);
                partnerRepo = new AcademicActivityPartnerRepo();
                SaveActivityPartner activityPartner = JsonConvert.DeserializeObject<SaveActivityPartner>(obj_activity_partner_stringify);
                AlertModal<string> alertModal = partnerRepo.SaveActivityPartner(evidence_file, folder_name, activityPartner, account_id);
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
                AlertModal<ActivityPartner_Ext> alertModal = partnerRepo.GetActivityPartner(activity_partner_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult updateActivityPartner(HttpPostedFileBase evidence_file, string file_action, string folder_name, string obj_activity_partner_stringify)
        {
            try
            {
                partnerRepo = new AcademicActivityPartnerRepo();
                int account_id = CurrentAccount.AccountID(Session);
                SaveActivityPartner saveActivityPartner = JsonConvert.DeserializeObject<SaveActivityPartner>(obj_activity_partner_stringify);
                AlertModal<string> alertModal = partnerRepo.UpdateActivityPartner(evidence_file, file_action, folder_name, saveActivityPartner, account_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public JsonResult deleteActivityPartner(int activity_partner_id)
        {
            try
            {
                partnerRepo = new AcademicActivityPartnerRepo();
                AlertModal<string> alertModal = partnerRepo.DeleteActivityPartner(activity_partner_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [Auther(RightID = "3")]
        [HttpPost]
        public ActionResult ExportExcel(int type, int activity_id)
        {
            try
            {
                excelRepo = new AcademicActivityExcelRepo();
                switch (type)
                {
                    case 1:
                        MemoryStream memoryStream = excelRepo.ExportDuTruExcel(activity_id);
                        string downloadFile = "Kinh-phi-du-tru.xlsx";
                        string handle = Guid.NewGuid().ToString();
                        TempData[handle] = memoryStream.ToArray();
                        return Json(new { success = true, data = new { FileGuid = handle, FileName = downloadFile } }, JsonRequestBehavior.AllowGet);
                    case 2:
                        MemoryStream memoryStream2 = excelRepo.ExportDieuChinhExcel(activity_id);
                        string downloadFile2 = "Kinh-phi-dieu-chinh.xlsx";
                        string handle2 = Guid.NewGuid().ToString();
                        TempData[handle2] = memoryStream2.ToArray();
                        return Json(new { success = true, data = new { FileGuid = handle2, FileName = downloadFile2 } }, JsonRequestBehavior.AllowGet);
                    case 3:
                        MemoryStream memoryStream3 = excelRepo.ExportThucTeExcel(activity_id);
                        string downloadFile3 = "Kinh-phi-thuc-te.xlsx";
                        string handle3 = Guid.NewGuid().ToString();
                        TempData[handle3] = memoryStream3.ToArray();
                        return Json(new { success = true, data = new { FileGuid = handle3, FileName = downloadFile3 } }, JsonRequestBehavior.AllowGet);
                    case 4:
                        MemoryStream memoryStream4 = excelRepo.ExportTongHopExcel(activity_id);
                        string downloadFile4 = "Kinh-phi-tong-hop.xlsx";
                        string handle4 = Guid.NewGuid().ToString();
                        TempData[handle4] = memoryStream4.ToArray();
                        return Json(new { success = true, data = new { FileGuid = handle4, FileName = downloadFile4 } }, JsonRequestBehavior.AllowGet);
                    default:
                        return Json(new { success = true, data = new { } }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
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
    }
}