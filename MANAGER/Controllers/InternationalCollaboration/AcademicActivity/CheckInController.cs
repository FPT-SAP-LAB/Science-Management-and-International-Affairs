using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MANAGER.Models;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class CheckInController : Controller
    {
        // GET: CheckIn
        public ActionResult List()
        {
            ViewBag.pageTitle = "Check-in hoạt động học thuật trong năm";
            return View();
        }
        public JsonResult add_CheckIn(string tu_cach, string name, string email, string don_vi, string co_so)
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
    }
}