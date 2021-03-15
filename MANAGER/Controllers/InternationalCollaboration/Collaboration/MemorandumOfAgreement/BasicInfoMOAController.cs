using BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfAgreement
{
    public class BasicInfoMOAController : Controller
    {
        // GET: BasicInfoMOA
        private static BasicInfoMOARepo moa = new BasicInfoMOARepo();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PassDataToMOADetail(int id)
        {
            try
            {
                Session["moa_detail_id"] = id;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult getBasicInfo()
        {
            try
            {
                string id = Session["moa_detail_id"].ToString();
                //MOABasicInfo data = moa.getBasicInfoMOA(int.Parse(id));
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
    }
}