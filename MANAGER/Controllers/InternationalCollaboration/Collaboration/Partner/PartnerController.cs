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
        public ActionResult List_Deleted()
        {
            ViewBag.pageTitle = "DANH SÁCH ĐỐI TÁC ĐÃ XÓA";
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

        public ActionResult Add()
        {
            ViewBag.pageTitle = "THÊM ĐỐI TÁC";
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add(HttpPostedFileBase image, string content, int numberOfImage)
        {
            List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();
            for (int i = 0; i < numberOfImage; i++)
            {
                string label = "image_" + i;
                files.Add(Request.Files[label]);
            }
            ViewBag.pageTitle = "THÊM ĐỐI TÁC";
            return View();
        }

        public ActionResult pass_content(string content, string website, string address, string imgInp)
        {
            try
            {
                Session.Timeout = 120;
                Session["content"] = content;
                Session["address"] = address;
                Session["website"] = website;
                Session["imgInp"] = imgInp;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Preview()
        {
            ViewBag.title = "Xem trước";
            ViewBag.content = Session["content"];
            ViewBag.address = Session["address"];
            ViewBag.website = Session["website"];
            ViewBag.imgInp = Session["imgInp"];
            return View();
        }

        public ActionResult Detail()
        {
            ViewBag.pageTitle = "CHI TIẾT ĐỐI TÁC";
            return View();
        }
    }
}