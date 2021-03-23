using ENTITIES.CustomModels;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            GlobalUploadDrive.UpdateFile("1XLVjm1eu_M_7d76PcpQfvl20Pk-T0ljG");
            var pagesTree = new List<PageTree>
            {
                new PageTree("Bảng tin","/"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}
