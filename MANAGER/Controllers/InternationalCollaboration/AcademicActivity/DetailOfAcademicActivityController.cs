using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MANAGER.Models;
using BLL.InternationalCollaboration.AcademicActivity;
using MANAGER.Support;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class DetailOfAcademicActivityController : Controller
    {
        private static DetailOfAcademicActivityRepo repo = new DetailOfAcademicActivityRepo();
        [Auther(RightID = "3")]
        public ActionResult Index(int id)
        {
            ViewBag.Title = "Thông tin hoạt động học thuật";
            ViewBag.activity_id = id;
            ViewBag.types = repo.getType();
            return View();
        }
        [HttpPost]
        public JsonResult getDetail(int language_id,int activity_id)
        {
            SumDetail data = new SumDetail
            {
                baseDetail = repo.getDetail(language_id, activity_id),
                subContent = repo.getSubContents(language_id, activity_id)
            };
            return Json(data);
        }
        public JsonResult add_Phase(int id, string name, string from, string to)
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
        public JsonResult delete_Phase(int id)
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
        public JsonResult edit_Phase(int id, string name, string from, string to)
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
        public ActionResult RegisterForm(int id)
        {
            ViewBag.phaseID = id;
            ViewBag.pageTitle = "Mẫu đăng kí hoạt động học thuật";
            return View();
        }
        public class QuantityByUnit
        {
            public string name { get; set; }
            public int quantity { get; set; }
        }
        public class SumDetail
        {
            public DetailOfAcademicActivityRepo.baseDetail baseDetail { get; set; }
            public List<DetailOfAcademicActivityRepo.subContent> subContent { get; set; }
        }
    }
}