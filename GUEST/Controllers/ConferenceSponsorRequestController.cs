using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class ConferenceSponsorRequestController : Controller
    {
        // GET: ConferenceSponsorRequest
        public ActionResult Index()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsorRequest"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        public ActionResult Add()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsorRequest"),
                new PageTree("Thêm","/ConferenceSponsorRequest/Add"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
    public class JsonError
    {
        public int code;
        public string err_content;
    }
}