using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Partner
{
    public class PartnerController : Controller
    {
        // GET: Partner
        public ActionResult List()
        {
            ViewBag.pageTitle = "DANH SÁCH ĐỐI TÁC";
            return View();
        }

        public ActionResult Delete(string id)
        {
            try
            {
                string result = id;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Detail()
        {
            ViewBag.pageTitle = "CHI TIẾT ĐỐI TÁC";
            return View();
        }
    }
}