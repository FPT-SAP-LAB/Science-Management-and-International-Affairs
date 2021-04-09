using BLL.ModelDAL;
using ENTITIES.CustomModels;
using GUEST.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationRepo notificationRepo = new NotificationRepo();
        private readonly List<PageTree> pagesTree = new List<PageTree>
            {
                new PageTree("Cài đặt thông báo","/Notification"),
            };

        public ActionResult Index()
        {
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public JsonResult List(int start)
        {
            try
            {
                List<Notification> Notis = notificationRepo.List(CurrentAccount.AccountID(Session), start, LanguageResource.GetCurrentLanguageID());
                Notis.ForEach(x => x.StringDate = x.CreatedDate.ToString("HH:mm dd/MM/yyyy"));
                return Json(new { success = true, content = Notis }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false, content = "Không thể lấy được dữ liệu thông báo" });
            }
        }
    }
}