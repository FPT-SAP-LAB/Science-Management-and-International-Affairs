using BLL.ScienceManagement.Citation;
using BLL.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class CitationController : Controller
    {
        CitationRepo cr = new CitationRepo();
        MasterDataRepo mrd = new MasterDataRepo();
        // GET: Citation
        public ActionResult Pending()
        {
            ViewBag.title = "Danh sách trích dẫn đang chờ xét duyệt";
            List<PendingCitation_manager> listPending = cr.getListPending();
            ViewBag.pending = listPending;

            return View();
        }

        [HttpPost]
        public ActionResult Detail(string id)
        {
            ViewBag.title = "Chi tiết trích dẫn";
            List<ENTITIES.Citation> listCitation = cr.getCitation(id);
            ViewBag.citation = listCitation;

            AuthorInfo author = cr.getAuthor(id);
            ViewBag.author = author;

            List<TitleWithName> listTitle = mrd.getTitle("vi-VN");
            ViewBag.title = listTitle;

            ViewBag.request_id = id;

            return View();
        }

        public ActionResult WaitDecision()
        {
            ViewBag.title = "Các trích dẫn đang chờ quyết định";
            List<WaitDecisionCitation> list = cr.getListWait();
            ViewBag.wait = list;

            return View();
        }

        [HttpPost]
        public JsonResult editCitation(string request_id, string total)
        {
            string mess = cr.updateReward(request_id, total);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }
    }
}