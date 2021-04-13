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
            var pagesTree = new List<PageTree>
            {
                new PageTree("","/"),
            };
            ViewBag.pagesTree = pagesTree;
            if (Response.HeadersWritten)
                return View();
            Response.StatusCode = statusCode;
            ViewBag.StatusCode = statusCode;
            ViewBag.exception = exception;
            return View();
        }
    }
}