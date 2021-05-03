﻿using BLL.ScienceManagement.Citation;
using BLL.ScienceManagement.MasterData;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using MANAGER.Models;
using MANAGER.Support;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class CitationController : Controller
    {
        private readonly CitationRepo cr = new CitationRepo();
        private readonly MasterDataRepo mrd = new MasterDataRepo();
        // GET: Citation
        [Auther(RightID = "16")]
        public ActionResult Pending()
        {
            ViewBag.title = "Danh sách trích dẫn đang chờ xét duyệt";
            List<PendingCitation_manager> listPending = cr.GetListPending();
            ViewBag.pending = listPending;

            return View();
        }

        //[HttpPost]
        [Auther(RightID = "16")]
        public ActionResult Detail(string id)
        {
            List<CustomCitation> listCitation = cr.GetCitation(id);
            ViewBag.citation = listCitation;

            ViewBag.request_id = id;
            RequestCitation rc = cr.GetRequestCitation(id);
            ViewBag.total_reward = rc.total_reward;

            List<TitleWithName> listTitle = mrd.GetTitle("vi-VN");
            ViewBag.ctitle = listTitle;

            int status = rc.citation_status_id;
            ViewBag.status = status;

            ViewBag.acc = CurrentAccount.Account(Session);

            return View();
        }

        [Auther(RightID = "18")]
        public ActionResult WaitDecision()
        {
            ViewBag.title = "Các trích dẫn đang chờ quyết định";
            List<WaitDecisionCitation> list = cr.GetListWait();
            foreach (var item in list)
            {
                CultureInfo cul = new CultureInfo("vi-VN");
                item.total_reward_string = item.total_reward.ToString("C0", cul.NumberFormat);
            }
            ViewBag.wait = list;

            return View();
        }

        [HttpPost]
        public JsonResult UpdateReward(string request_id, string total)
        {
            return Json(cr.UpdateReward(request_id, total));
        }

        [HttpPost]
        public JsonResult UploadDecision(HttpPostedFileBase file, string number, string date)
        {
            return Json(cr.UploadDecision(file, number, date));
        }

        [HttpPost]
        public JsonResult changeStatus(string request_id)
        {
            string mess = cr.ChangeStatus(request_id);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteRequest(int id)
        {
            string mess = cr.DeleteRequest(id);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportExcel()
        {
            CitationRequestExportRepo exportRepo = new CitationRequestExportRepo();
            byte[] Excel = exportRepo.ExportExcel();
            if (Excel == null)
                return Redirect("/Citation/WaitDecision");
            else
                return File(Excel, "application/vnd.ms-excel", "Danh-sách-chờ quyết định.xlsx");
        }
    }
}