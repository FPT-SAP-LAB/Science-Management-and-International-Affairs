using BLL.InternationalCollaboration.AcademicActivity;
using MANAGER.Models;
using MANAGER.Support;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityController : Controller
    {
        AcademicActivityRepo repo;
        DetailOfAcademicActivityRepo detailRepo;
        [Auther(RightID = "2")]
        public ActionResult List()
        {
            repo = new AcademicActivityRepo();
            ViewBag.pageTitle = "DANH SÁCH HOẠT ĐỘNG HỌC THUẬT TRONG NĂM";
            ViewBag.AAType = repo.getType(1);
            return View();
        }
        [HttpPost]
        public ActionResult getDatatable(int year)
        {
            repo = new AcademicActivityRepo();
            List<AcademicActivityRepo.ListAA> data = repo.listAllAA(year);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult getBaseAA(int id)
        {
            repo = new AcademicActivityRepo();
            detailRepo = new DetailOfAcademicActivityRepo();
            AcademicActivityRepo.baseAA baseAA = repo.GetbaseAA(id);
            AcademicActivityRepo.AAtypes data = new AcademicActivityRepo.AAtypes
            {
                baseAA = baseAA,
                types = detailRepo.getType(baseAA.language_id)
            };
            return Json(data);
        }
        [HttpPost]
        public JsonResult getBaseAALanguage(int id, int language_id)
        {
            repo = new AcademicActivityRepo();
            detailRepo = new DetailOfAcademicActivityRepo();
            AcademicActivityRepo.baseAA baseAA = repo.GetbaseAALanguage(id, language_id);
            AcademicActivityRepo.AAtypes data = new AcademicActivityRepo.AAtypes
            {
                baseAA = baseAA == null ? new AcademicActivityRepo.baseAA() : baseAA,
                types = detailRepo.getType(language_id)
            };
            return Json(data);
        }
        public JsonResult resetCategory(int language_id)
        {
            detailRepo = new DetailOfAcademicActivityRepo();
            List<ENTITIES.AcademicActivityTypeLanguage> data = detailRepo.getType(language_id);
            return Json(data);
        }
        [Auther(RightID = "2")]
        [HttpPost]
        public JsonResult delete_AcademicActivity(int id)
        {
            repo = new AcademicActivityRepo();
            bool res = repo.deleteAA(id);
            if (res)
            {
                return Json("Đã xóa thành công", JsonRequestBehavior.AllowGet);
            }
            else return Json(String.Empty);
        }
        [Auther(RightID = "2")]
        [HttpPost]
        public JsonResult add_AcademicActivity(AcademicActivityRepo.baseAA obj, int language_id)
        {
            try
            {
                repo = new AcademicActivityRepo();
                BLL.Authen.LoginRepo.User u = (BLL.Authen.LoginRepo.User)Session["User"];
                bool res = repo.AddAA(obj, language_id, u);
                if (res)
                {
                    return Json("Đã thêm thành công", JsonRequestBehavior.AllowGet);
                }
                else return Json(String.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(String.Empty);
            }
        }
        [Auther(RightID = "2")]
        [HttpPost]
        public JsonResult edit_AcademicActivity(int id, int activity_type_id, string activity_name, string location, string from, string to, int language_id, HttpPostedFileBase img)
        {
            repo = new AcademicActivityRepo();
            BLL.Authen.LoginRepo.User u = (BLL.Authen.LoginRepo.User)Session["User"];
            bool res = repo.updateBaseAAA(id, activity_type_id, activity_name, location, from, to, language_id, img, u);
            if (res)
            {
                return Json("Đã chỉnh sửa thành công", JsonRequestBehavior.AllowGet);
            }
            else return Json(String.Empty);
        }
        [Auther(RightID = "2")]
        public JsonResult clone(AcademicActivityRepo.cloneBase obj)
        {
            repo = new AcademicActivityRepo();
            int account_id = CurrentAccount.AccountID(Session);
            bool res = repo.clone(obj, account_id);
            if (res)
            {
                return Json("Đã sao chép thành công", JsonRequestBehavior.AllowGet);
            }
            else return Json(String.Empty);
        }
    }
}