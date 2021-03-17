using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers.InternationalCollaboration.AcademicCollaboration
{
    public class AcademicCollaborationController : Controller
    {
        // GET: AcademicCollaboration
        public ActionResult Long_Term()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đào tạo sau đại học", "/AcademicCollaboration/Long_Term"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}