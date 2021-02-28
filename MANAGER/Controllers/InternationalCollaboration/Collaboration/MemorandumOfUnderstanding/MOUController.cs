using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class MOUController : Controller
    {
        // GET: MOU
        public ActionResult List()
        {
            ViewBag.pageTitle = "DANH SÁCH BIÊN BẢN GHI NHỚ";
            DateTime today = DateTime.Today;
            DateTime end_date = new DateTime(2021, 05, 20);
            TimeSpan value = end_date.Subtract(today);
            int duration = value.Duration().Days;
            return View(duration);
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

        public ActionResult Detail_MOA(string id)
        {
            ViewBag.pageTitle = "CHI TIẾT BIÊN BẢN THỎA THUẬN";
            return View();
        }
    }
}