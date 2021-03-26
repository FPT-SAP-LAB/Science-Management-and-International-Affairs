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
        // GET: AcademicActivity
        public ActionResult Index()
        {
            ViewBag.title = "Hoạt động học thuật";

            ViewBag.listActivity = guestRepo.getBaseAA(0, new List<int>());
            var pagesTree = new List<PageTree>
            {
                new PageTree("Hoạt động học thuật","/AcademicActivity/Index"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public ActionResult Detail(int id)
        {
            ViewBag.title = "Hoạt động học thuật";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Hoạt động học thuật","/AcademicActivity"),
                new PageTree("Chi tiết","/AcademicActivity/Detail")
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.detail = guestRepo.getBaseAADetail(id);
            return View();
        }
        [HttpPost]
        public ActionResult LoadMoreList(int count, List<int> type)
        {
            List<AcademicActivityGuestRepo.baseAA> data = guestRepo.getBaseAA(count, type);
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