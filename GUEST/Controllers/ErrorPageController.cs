using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class ErrorPageController : Controller
    {
        // GET: ErrorPage
        public ActionResult Error(int statusCode, Exception exception)
        {
            Response.StatusCode = statusCode;
            ViewBag.StatusCode = statusCode;
            ViewBag.exception = exception;
            var pagesTree = new List<PageTree>
            {
                new PageTree("","/"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}