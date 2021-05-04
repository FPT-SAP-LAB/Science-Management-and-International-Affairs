using BLL.GuestConfiguration;
using MANAGER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.GuestConfiguration
{
    public class HomeImageConfigurationController : Controller
    {
        // GET: HomeImageConfiguration
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Update()
        {
            HomeImageConfigurationRepo imageConfigurationRepo = new HomeImageConfigurationRepo();

            HttpPostedFileBase wallpaper = null;
            List<HttpPostedFileBase> banner = new List<HttpPostedFileBase>();
            foreach (string key in Request.Files.AllKeys)
            {
                if (key.Equals("wallpaper"))
                    wallpaper = Request.Files[key];
                else
                    banner.Add(Request.Files[key]);
            }
            var output = imageConfigurationRepo.Update(wallpaper, banner, CurrentAccount.AccountID(Session), new List<int> { });
            return Json(output);
        }
    }
}