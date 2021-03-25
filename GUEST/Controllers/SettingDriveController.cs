using ENTITIES.CustomModels;
using System.Collections.Generic;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class SettingDriveController : Controller
    {
        public ActionResult Index()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Cài đặt Drive","/SettingDrive"),
            };
            ViewBag.pagesTree = pagesTree;
            string Email = GlobalUploadDrive.CurrentAccount();
            if (Email == null) Email = "Cảnh báo, chưa đăng nhập";
            ViewBag.Email = Email;
            return View();
        }
        public ActionResult Login()
        {
            GlobalUploadDrive.InIt();
            return Redirect("/SettingDrive");
        }
        public ActionResult Logout()
        {
            GlobalUploadDrive.Logout();
            return Redirect("/SettingDrive");
        }
    }
}