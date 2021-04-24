using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
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
            SubscribeLanguageRepo subscribeRepo = new SubscribeLanguageRepo();
            ViewBag.pagesTree = pagesTree;
            ViewBag.subscribes = subscribeRepo.GetSubscribes(CurrentAccount.AccountID(Session), LanguageResource.GetCurrentLanguageID());
            return View();
        }

        public ActionResult Update(List<NotificationSubscribe> subscribes)
        {
            SubscribeLanguageRepo subscribeRepo = new SubscribeLanguageRepo();
            return Json(subscribeRepo.UpdateSubscribe(subscribes, CurrentAccount.AccountID(Session)));
        }

        [AjaxOnly]
        public JsonResult List(int start)
        {
            try
            {
                BaseServerSideData<Notification> Notis = notificationRepo.List(CurrentAccount.AccountID(Session), start, LanguageResource.GetCurrentLanguageID());
                return Json(new { success = true, content = Notis.Data, unread = Notis.RecordsTotal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false, content = "Không thể lấy được dữ liệu thông báo" });
            }
        }
        public ActionResult Read(int id)
        {
            return Redirect(notificationRepo.Read(id));
        }
    }
}