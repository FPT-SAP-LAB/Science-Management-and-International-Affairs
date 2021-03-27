using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;
using BLL.InternationalCollaboration.AcademicActivity;

namespace GUEST.Controllers.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityController : Controller
    {
        private static AcademicActivityGuestRepo guestRepo = new AcademicActivityGuestRepo();
        private System.Resources.ResourceManager rm = Models.LanguageResource.GetResourceManager();
        // GET: AcademicActivity
        public ActionResult Index()
        {
            int language = Models.LanguageResource.GetCurrentLanguageID(); ;
            ViewBag.title = rm.GetString("AcademicActivity");
            ViewBag.listActivity = guestRepo.getBaseAA(0, new List<int>(), language, null);
            ViewBag.listActivityType = guestRepo.getListType(language);

            var pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("AcademicActivity"),"/AcademicActivity/Index"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public ActionResult Detail(int id)
        {
            ViewBag.title = "Hoạt động học thuật";
            var pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("AcademicActivity"),"/AcademicActivity"),
                new PageTree("Chi tiết","/AcademicActivity/Detail")
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.detail = guestRepo.getBaseAADetail(id);
            return View();
        }
        [HttpPost]
        public ActionResult LoadMoreList(int count, List<int> type, string search)
        {
            int language = Models.LanguageResource.GetCurrentLanguageID();
            List<AcademicActivityGuestRepo.baseAA> data = guestRepo.getBaseAA(count, type, language, search);
            return Json(data);
        }
        public ActionResult loadForm(int fid)
        {
            ViewBag.title = "Đơn đăng kí tham dự";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Hoạt động học thuật","/AcademicActivity"),
                new PageTree("Đơn đăng kí tham dự","/AcademicActivity/Detail")
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.fid = fid;
            return View();
        }
    }
}