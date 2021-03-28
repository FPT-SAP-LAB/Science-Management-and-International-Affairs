using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class SettingDriveController : Controller
    {
        public ActionResult Index()
        {
            string Email = GoogleDriveService.CurrentAccount();
            if (Email == null) Email = "Cảnh báo, chưa đăng nhập";
            ViewBag.Email = Email;
            return View();
        }
        public ActionResult Login()
        {
            GoogleDriveService.InIt();
            return Redirect("/SettingDrive");
        }
        public ActionResult Logout()
        {
            GoogleDriveService.Logout();
            return Redirect("/SettingDrive");
        }
    }
}