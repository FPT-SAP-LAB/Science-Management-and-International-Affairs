using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class InventionController : Controller
    {
        // GET: Invention
        //public ActionResult ListAll()
        //{
        //    ViewBag.title = "Bằng sáng chế";
        //    var pagesTree = new List<PageTree>
        //    {
        //        new PageTree("Bằng sáng chế","/Invention/ListAll"),
        //    };
        //    ViewBag.pagesTree = pagesTree;
        //    return View();
        //}

        public ActionResult AddRequest()
        {
            ViewBag.title = "Đăng ký khen thưởng bằng sáng chế";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đăng ký khen thưởng bằng sáng chế","/Invention/AddRequest"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        public ActionResult Edit()
        {
            ViewBag.title = "Chỉnh sửa khen thưởng bằng sáng chế";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Chỉnh sửa khen thưởng bằng sáng chế","/Invention/Edit"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}