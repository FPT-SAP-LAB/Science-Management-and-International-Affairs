using BLL.ScienceManagement.MasterData;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GUEST.Controllers.ScienceManagement
{
    public class PartialViewController : Controller
    {
        MasterDataRepo md = new MasterDataRepo();
        // GET: PartialView
        [ChildActionOnly]
        public ActionResult AddAuthor()
        {
            List<Office> listOff = md.getOffice();
            ViewBag.office = listOff;

            List<Area> listArea = md.GetAreas();
            ViewBag.area = listArea;

            string lang = "";
            if (Request.Cookies["language_name"] != null)
            {
                lang = Request.Cookies["language_name"].Value;
            }
            List<TitleWithName> listTitle = md.getTitle(lang);
            ViewBag.title = listTitle;

            List<ContractType> listContract = md.getContract();
            ViewBag.contract = listContract;

            List<AddAuthor> listPeople = md.getListPeopleFE();
            ViewBag.people = listPeople;

            return PartialView();
        }

        [HttpPost]
        public JsonResult fillData(AddAuthor item, int paper_id)
        {
            AddAuthor result = md.getAuthor(item.mssv_msnv);
            AuthorPaper ap = md.getMonry(item, paper_id);
            return Json(new { author = result, money = ap.money_reward }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult fillDataAdd(AddAuthor item)
        {
            AddAuthor result = md.getAuthor(item.mssv_msnv);
            string mess = "ss";
            if (result == null) mess = "ff";
            return Json(new { author = result, mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult EditAuthor()
        {
            List<Office> listOff = md.getOffice();
            ViewBag.office = listOff;

            List<Area> listArea = md.GetAreas();
            ViewBag.area = listArea;

            string lang = "";
            if (Request.Cookies["language_name"] != null)
            {
                lang = Request.Cookies["language_name"].Value;
            }
            List<TitleWithName> listTitle = md.getTitle(lang);
            ViewBag.title = listTitle;

            List<ContractType> listContract = md.getContract();
            ViewBag.contract = listContract;

            List<AddAuthor> listPeople = md.getListPeopleFE();
            ViewBag.people = listPeople;

            return PartialView();
        }
    }
}