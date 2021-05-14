using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;
using BLL.InternationalCollaboration.AcademicActivity;
using GUEST.Support;

namespace GUEST.Controllers.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityController : Controller
    {
        private AcademicActivityGuestRepo guestRepo;
        private readonly System.Resources.ResourceManager rm = Models.LanguageResource.GetResourceManager();
        // GET: AcademicActivity
        public ActionResult Index()
        {
            guestRepo = new AcademicActivityGuestRepo();
            int language = Models.LanguageResource.GetCurrentLanguageID();
            ViewBag.title = rm.GetString("AcademicActivity");
            ViewBag.listActivity = guestRepo.GetBaseAA(0, new List<int>(), language, null, null);
            ViewBag.listActivityType = guestRepo.GetListType(language);
            ViewBag.YearSearching = guestRepo.YearSearching();
            var pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("AcademicActivity"),"/AcademicActivity/Index"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public ActionResult Detail(int id)
        {
            int language = Models.LanguageResource.GetCurrentLanguageID();
            guestRepo = new AcademicActivityGuestRepo();
            ViewBag.title = rm.GetString("AcademicActivity");
            var pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("AcademicActivity"),"/AcademicActivity"),
                new PageTree(rm.GetString("Detail"),"/AcademicActivity/Detail")
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.Detail = guestRepo.GetBaseAADetail(id, language);
            ViewBag.SubContent = guestRepo.GetSubContent(id, language);
            ViewBag.phase_id = guestRepo.GetPhaseCurrentByActivity(id);
            ViewBag.have_form = guestRepo.CheckForm(guestRepo.GetPhaseCurrentByActivity(id));
            return View();
        }

        [HttpPost]
        public ActionResult DetailContent(int content_id)
        {
            guestRepo = new AcademicActivityGuestRepo();
            if (content_id == 0)
            {
                return Json(new { content = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Maecenas ultricies mi eget mauris pharetra et ultrices neque ornare. Volutpat diam ut venenatis tellus. Faucibus a pellentesque sit amet porttitor. Mattis nunc sed blandit libero volutpat sed cras. A cras semper auctor neque vitae tempus. Euismod in pellentesque massa placerat duis ultricies lacus. Ipsum consequat nisl vel pretium lectus quam id leo in. Ut consequat semper viverra nam libero justo laoreet. Fusce id velit ut tortor pretium viverra suspendisse. Turpis nunc eget lorem dolor sed viverra ipsum nunc aliquet. Tortor condimentum lacinia quis vel eros donec ac odio. Felis eget nunc lobortis mattis aliquam faucibus purus in massa." });
            }
            else if (content_id == 1)
            {
                return Json(new { content = "Wibu" });
            }
            else
            {
                return Json(new { content = "Dương-san no Baka" });
            }
        }
        [HttpPost]
        public ActionResult LoadMoreList(int count, List<int> type, string search, int? year)
        {
            guestRepo = new AcademicActivityGuestRepo();
            int language = Models.LanguageResource.GetCurrentLanguageID();
            List<AcademicActivityGuestRepo.baseAA> data = guestRepo.GetBaseAA(count, type, language, search, year);
            return Json(data);
        }
        public ActionResult loadForm(int pid)
        {
            guestRepo = new AcademicActivityGuestRepo();
            ViewBag.title = "Đơn đăng kí tham dự";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Hoạt động học thuật","/AcademicActivity"),
                new PageTree("Đơn đăng kí tham dự","/AcademicActivity/Detail")
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.file_drive_id = guestRepo.GetImageActivity(pid);
            ViewBag.pid = pid;
            ViewBag.role = guestRepo.GetParticipantRoleByPhase(pid);
            ViewBag.office = guestRepo.GetOffices();
            ViewBag.outdate = guestRepo.CheckFormOutdate(pid);
            return View();
        }
        [HttpPost]
        public JsonResult getForm(int phase_id)
        {
            guestRepo = new AcademicActivityGuestRepo();
            AcademicActivityGuestRepo.fullForm data = guestRepo.GetForm(phase_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult sendForm(int fid, string answer, AcademicActivityGuestRepo.AnswerUnchange unchange)
        {
            guestRepo = new AcademicActivityGuestRepo();
            bool res = guestRepo.SendForm(fid, answer, unchange);
            if (res)
            {
                return Json("Gửi đơn đăng kí thành công");
            }
            return Json(String.Empty);
        }
    }
}