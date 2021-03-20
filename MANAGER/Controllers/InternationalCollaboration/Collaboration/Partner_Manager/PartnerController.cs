using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.InternationalCollaboration.Collaboration.PartnerRepo;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity;

namespace MANAGER.Controllers.InternationalCollaboration.Partner_Manager
{
    public class PartnerController : Controller
    {
        private static PartnerRepo partnerRePo = new PartnerRepo();
        // GET: Partner
        public ActionResult List()
        {
            ViewBag.title = "DANH SÁCH ĐỐI TÁC";
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            ViewBag.countries = db.Countries.ToList();
            ViewBag.specializations = db.Specializations.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult List(SearchPartner searchPartner)
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<PartnerList> baseServerSideData = partnerRePo.getListAll(baseDatatable);
                return Json(new
                {
                    success = true,
                    data = baseServerSideData.Data,
                    draw = Request["draw"],
                    recordsTotal = baseServerSideData.RecordsTotal,
                    recordsFiltered = baseServerSideData.RecordsTotal
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Load_History(int id)
        {
            try
            {
                PartnerHistoryList<PartnerHistory> partnerHistoryList = partnerRePo.getHistory(id);

                return Json(new
                {
                    list = partnerHistoryList.Data,
                    name = partnerHistoryList.Partner_name,
                    JsonRequestBehavior.AllowGet
                });
            }
            catch (Exception e)
            {
                return Json(new { data = e.Message });
            }
        }
        public ActionResult List_Deleted()
        {
            ViewBag.title = "DANH SÁCH ĐỐI TÁC ĐÃ XÓA";
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