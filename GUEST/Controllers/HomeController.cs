using BLL.InternationalCollaboration.Collaboration.PartnerRepo;
using ENTITIES.CustomModels;
using System.Collections.Generic;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class HomeController : Controller
    {
        PartnerRepo partnerRepo = new PartnerRepo();
        public ActionResult Index()
        {
            var pagesTree = new List<PageTree>();
            ViewBag.pagesTree = pagesTree;
            ViewBag.partner = partnerRepo.GetPartnerWidget();
            return View();
        }
    }
}
