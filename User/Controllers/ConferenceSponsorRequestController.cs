using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace User.Controllers
{
    public class ConferenceSponsorRequestController : Controller
    {
        // GET: ConferenceSponsorRequest
        [Route("conference-sponsor-request/create-request")]
        public ActionResult Index()
        {
            /////////////////////////////////////////////////////////////////////////////
            
            /////////////////////////////////////////////////////////////////////////////
            ViewBag.title = "Đề nghị hỗ trợ hội nghị";
            var pagesTree = new List<String> {"Trang chủ", "Đề nghị hỗ trợ hội nghị" };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}