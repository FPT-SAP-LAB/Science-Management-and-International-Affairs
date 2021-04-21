using BLL.ScienceManagement.ArticlePolicy;
using GUEST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers.ScienceManagement.ArticlePolicy
{
    public class ArticlePolicyController : Controller
    {
        private readonly ArticlePolicyIndexRepo IndexRepo = new ArticlePolicyIndexRepo();
        private readonly ArticlePolicyDetailRepo DetailRepo = new ArticlePolicyDetailRepo();
        private readonly List<PageTree> pagesTree = new List<PageTree>
            {
                new PageTree("Chính sách","/ArticlePolicy"),
            };
        // GET: Policy
        public ActionResult Index()
        {
            ViewBag.pagesTree = pagesTree;
            ViewBag.items = IndexRepo.ListGuest(LanguageResource.GetCurrentLanguageID());
            return View();
        }

        public ActionResult Detail(int id)
        {
            pagesTree.Add(new PageTree("Chi tiết", "#"));
            ViewBag.pagesTree = pagesTree;
            ViewBag.detail = DetailRepo.Detail(id, LanguageResource.GetCurrentLanguageID());
            return View();
        }
    }
}