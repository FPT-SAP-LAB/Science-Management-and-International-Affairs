﻿using BLL.ModelDAL;
using ENTITIES.CustomModels;
using MANAGER.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class NotificationController : Controller
    {
        private readonly NotificationRepo notificationRepo = new NotificationRepo();

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List(int start)
        {
            try
            {
                List<Notification> Notis = notificationRepo.List(CurrentAccount.AccountID(Session), start, 1);
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