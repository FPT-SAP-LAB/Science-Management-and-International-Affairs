using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MANAGER.Models;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class DetailOfAcademicActivityController : Controller
    {
        // GET: DetailOfAcademicActivity
        public ActionResult Index()
        {
            ViewBag.pageTitle = "Thông tin hoạt động học thuật";
            return View();
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
        public class QuantityByUnit
        {
            public string name { get; set; }
            public int quantity { get; set; }
        }
    }
}