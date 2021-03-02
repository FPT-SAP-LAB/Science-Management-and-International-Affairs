using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOURepo;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class MOUController : Controller
    {
        // GET: MOU
        public static MOURepo mou;

        public ActionResult List()
        {
            ViewBag.pageTitle = "DANH SÁCH BIÊN BẢN GHI NHỚ";
            int duration = mou.getDuration();
            List<ListMOU> listMOU = mou.listAllMOU();

            return View();
        }

        private ActionResult getNotificationInfo()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete_Mou(string id)
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

        public ActionResult Detail(string id)
        {
            return View();
        }
    }
}