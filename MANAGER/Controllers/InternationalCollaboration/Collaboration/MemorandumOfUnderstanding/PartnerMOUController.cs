using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class PartnerMOUController : Controller
    {
        // GET: PartnerMOU
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get_MOU_History(string id)
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

        public ActionResult Get_Data_Partner_Detail(string id)
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
    }
}