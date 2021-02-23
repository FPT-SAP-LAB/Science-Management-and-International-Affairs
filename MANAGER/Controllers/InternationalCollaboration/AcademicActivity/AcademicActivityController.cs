using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityController : Controller
    {
        // GET: AcademicActivity
        public ActionResult List()
        {
            ViewBag.pageTitle = "Danh sách hoạt động học thuật trong năm";
            return View();
        }
        public JsonResult delete_AcademicActivity(int id)
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
        public JsonResult add_AcademicActivity(string title, string category, string from, string to, string location)
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
        public JsonResult edit_AcademicActivity(string title, string category, string from, string to, string location)
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
        private class JsonError
        {
            public int code;
            public string err_content;
        }
    }
}