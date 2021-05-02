using BLL;
using ENTITIES.CustomModels;
using GUEST.Models;
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
            HomeData result = homeRepo.GetHomeData(LanguageResource.GetCurrentLanguageID());
            Startup.BackgroundURL = homeRepo.GetBackground();
            if (result != null)
            {
                ViewBag.partner = result.Partner;
                ViewBag.images = result.Images;
                ViewBag.invention = result.Invention;
                ViewBag.scopusISI = result.ScopusISI;
                ViewBag.researcher = result.Researcher;
                ViewBag.articlePolicies = result.ArticlePolicies;
            }
            return View();
        }
    }
}
