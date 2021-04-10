using BLL.ScienceManagement.Citation;
using BLL.ScienceManagement.MasterData;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using MANAGER.Support;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        [Auther(RightID = "16")]
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
            ViewBag.ctitle = listTitle;

            ViewBag.request_id = id;

            int status = cr.getStatus(id);
            ViewBag.status = status;

            return View();
        }

        [Auther(RightID = "18")]
        public ActionResult WaitDecision()
        {
            ViewBag.title = "Các trích dẫn đang chờ quyết định";
            List<WaitDecisionCitation> list = cr.getListWait();
            foreach (var item in list)
            {
                CultureInfo cul = new CultureInfo("vi-VN");
                item.total_reward_string = item.total_reward.ToString("C0", cul.NumberFormat);
            }
            ViewBag.wait = list;

            return View();
        }

        [HttpPost]
        public JsonResult editCitation(string request_id, string total)
        {
            string mess = cr.updateReward(request_id, total);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult uploadDecision(HttpPostedFileBase file, string number, string date)
        {
            string[] arr = date.Split('/');
            string format = arr[1] + "/" + arr[0] + "/" + arr[2];
            DateTime date_format = DateTime.Parse(format);

            string name = "QD_" + number + "_" + date;

            Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadResearcherFile(file, name, 4, null);
            ENTITIES.File fl = new ENTITIES.File
            {
                link = f.WebViewLink,
                file_drive_id = f.Id,
                name = name
            };

            ENTITIES.File myFile = mrd.addFile(fl);
            string mess = cr.uploadDecision(date_format, myFile.file_id, number, myFile.file_drive_id);

            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult changeStatus(string request_id)
        {
            string mess = cr.changeStatus(request_id);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }
    }
}