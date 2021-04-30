using BLL.ScienceManagement.Citation;
using BLL.ScienceManagement.MasterData;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using MANAGER.Models;
using MANAGER.Support;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Hosting;
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

        [HttpPost]
        public JsonResult exportExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "Citation.xlsx";
            FileInfo file = new FileInfo(path + filename);
            ExcelPackage excelPackage = new ExcelPackage(file);
            ExcelWorkbook excelWorkbook = excelPackage.Workbook;

            List<Citation_Appendix_1> list1 = cr.GetListAppendix1();
            ExcelWorksheet excelWorksheet1 = excelWorkbook.Worksheets[0];
            int i = 2;
            int count = 1;
            foreach (var item in list1)
            {
                excelWorksheet1.Cells[i, 1].Value = count;
                excelWorksheet1.Cells[i, 2].Value = item.name;
                excelWorksheet1.Cells[i, 3].Value = item.mssv_msnv;
                excelWorksheet1.Cells[i, 4].Value = item.office_abbreviation;
                excelWorksheet1.Cells[i, 5].Value = item.sum_scopus;
                excelWorksheet1.Cells[i, 6].Value = item.sum_scholar;
                count++;
                i++;
            }

            List<Citation_Appendix_2> list2 = cr.GetListAppendix2();
            ExcelWorksheet excelWorksheet2 = excelWorkbook.Worksheets[1];
            i = 2;
            count = 1;
            foreach (var item in list2)
            {
                excelWorksheet2.Cells[i, 1].Value = count;
                excelWorksheet2.Cells[i, 2].Value = item.name;
                excelWorksheet2.Cells[i, 3].Value = item.mssv_msnv;
                excelWorksheet2.Cells[i, 4].Value = item.office_abbreviation;
                excelWorksheet2.Cells[i, 5].Value = item.total_reward;
                count++;
                i++;
            }

            string Flocation = "/Excel_template/download/Citation.xlsx";
            //string savePath = HostingEnvironment.MapPath(Flocation);
            excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/download/Citation.xlsx")));

            return Json(new { mess = true, location = Flocation }, JsonRequestBehavior.AllowGet);
        }
    }
}