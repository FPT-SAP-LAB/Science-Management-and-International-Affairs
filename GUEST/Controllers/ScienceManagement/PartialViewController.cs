using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GUEST.Controllers.ScienceManagement
{
    public class PartialViewController : Controller
    {
        // GET: PartialView
        [ChildActionOnly]
        public ActionResult AddAuthor()
        {
            return PartialView();
        }
    }
}