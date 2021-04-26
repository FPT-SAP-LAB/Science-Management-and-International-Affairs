using BLL.ModelDAL;
using BLL.ScienceManagement.ConferenceSponsor;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.Conference;
using MANAGER.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class ConferenceSponsorController : Controller
    {
        ConferenceSponsorIndexRepo IndexRepos;
        ConferenceSponsorDetailRepo DetailRepos;
        public ActionResult Index()
        {
            ViewBag.title = "DANH SÁCH ĐỀ NGHỊ ĐANG XỬ LÝ";
            return View();
        }
        public ActionResult History()
        {
            ViewBag.title = "LỊCH SỬ ĐỀ NGHỊ";
            return View();
        }

        [AjaxOnly, HttpPost]
        public JsonResult List(ConferenceSearch search)
        {
            IndexRepos = new ConferenceSponsorIndexRepo();
            BaseDatatable datatable = new BaseDatatable(Request);
            bool.TryParse(Request["is_history"], out bool is_history);
            BaseServerSideData<ConferenceIndex> output;
            if (is_history)
                output = IndexRepos.GetHistoryPage(datatable, search.SearchPaper, search.SearchConference);
            else
                output = IndexRepos.GetIndexPage(datatable, search);
            return Json(new ResultDatatable<ConferenceIndex>(output, Request));
        }

        public ActionResult Detail(int id)
        {
            QsUniversityRepo qsUniversityRepo = new QsUniversityRepo();
            DetailRepos = new ConferenceSponsorDetailRepo();

            string output = DetailRepos.GetDetailPageGuest(id, 1);
            if (output == null)
                return Redirect("/ConferenceSponsor");
            ViewBag.asideMinimize = true;
            ViewBag.output = output;
            string university = JObject.Parse(output)["Conference"]["QsUniversity"].ToString();
            ViewBag.ranking = qsUniversityRepo.GetRanking(university);
            return View();
        }
        [HttpPost]
        public ActionResult UpdateCriterias(string data, int request_id, string comment)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            DetailRepos.UpdateCriterias(data, request_id, CurrentAccount.AccountID(Session), comment);
            return Redirect("/ConferenceSponsor/Detail?id=" + request_id);
        }
        [HttpPost]
        public JsonResult RequestEdit(int request_id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            return Json(DetailRepos.RequestEdit(request_id));
        }
        [HttpPost]
        public ActionResult UpdateCosts(string data, int request_id, string comment)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            DetailRepos.UpdateCosts(data, request_id, CurrentAccount.AccountID(Session), comment);
            return Redirect("/ConferenceSponsor/Detail?id=" + request_id);
        }
        [HttpPost]
        public JsonResult SubmitPolicy(HttpPostedFileBase decision_file, string valid_date, string decision_number, int request_id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            AlertModal<string> result = DetailRepos.SubmitPolicy(decision_file, valid_date, decision_number, request_id);
            return Json(result);
        }
        [HttpPost]
        public JsonResult SubmitFiles(int request_id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            List<HttpPostedFileBase> files = new List<HttpPostedFileBase>();
            foreach (string key in Request.Files.AllKeys)
            {
                files.Add(Request.Files[key]);
            }
            if (files.Count == 0)
                return Json(new { success = false, content = "Không có file nào được up" });
            AlertModal<string> result = DetailRepos.SubmitFiles(files, request_id);
            return Json(result);
        }
        [HttpPost]
        public ActionResult UpdateReimbursement(string data, int request_id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            DetailRepos.SubmitReimbursement(data, request_id);
            return Redirect("/ConferenceSponsor/Detail?id=" + request_id);
        }
        [HttpPost]
        public ActionResult EndRequest(int request_id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            DetailRepos.EndRequest(request_id);
            return Redirect("/ConferenceSponsor/Detail?id=" + request_id);
        }
        [HttpGet]
        public ActionResult ExportRequest(int request_id)
        {
            ConferenceSponsorExportRepo exportRepo = new ConferenceSponsorExportRepo();
            byte[] Word = exportRepo.ExportRequest(request_id);
            if (Word == null)
                return Redirect("/ConferenceSponsor");
            else
                return File(Word, "application/vnd.ms-word", "Đơn-đề-nghị-hỗ-trợ-HNKH.docx");
        }
        [HttpGet]
        public ActionResult ExportAppointment(int request_id)
        {
            ConferenceSponsorExportRepo exportRepo = new ConferenceSponsorExportRepo();
            byte[] Word = exportRepo.ExportAppointment(request_id);
            if (Word == null)
                return Redirect("/ConferenceSponsor");
            else
                return File(Word, "application/vnd.ms-word", "Đề-nghị-cử-bán-bộ-đi-công-tác.docx");
        }
        [HttpPost]
        public JsonResult CancelRequest(int request_id)
        {
            DetailRepos = new ConferenceSponsorDetailRepo();
            return Json(DetailRepos.CancelRequest(request_id, CurrentAccount.AccountID(Session)));
        }
    }
}