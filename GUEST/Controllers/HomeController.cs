using BLL;
using ENTITIES.CustomModels;
using System.Collections.Generic;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeRepo homeRepo = new HomeRepo();
        public ActionResult Index()
        {
            ViewBag.pagesTree = new List<PageTree>();
            HomeData result = homeRepo.GetHomeData();
            Startup.BackgroundURL = homeRepo.GetBackground();
            if (result != null)
            {
                ViewBag.partner = result.Partner;
                ViewBag.images = result.Images;
            }
            return View();
        }
    }
}
