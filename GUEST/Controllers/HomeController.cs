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
            GlobalUploadDrive.FindFirstFolder("SonNT69");
            var pagesTree = new List<PageTree>
            {
                new PageTree("Bảng tin","/"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}