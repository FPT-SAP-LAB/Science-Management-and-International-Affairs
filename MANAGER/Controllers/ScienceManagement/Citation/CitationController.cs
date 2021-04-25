using BLL.Authen;
using BLL.ScienceManagement.Citation;
using BLL.ScienceManagement.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using MANAGER.Support;
using OfficeOpenXml;
using System;
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

        //[HttpPost]
        [Auther(RightID = "16")]
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

            LoginRepo.User u = new LoginRepo.User();
            Account acc = new Account();
            if (Session["User"] != null)
            {
                u = (LoginRepo.User)Session["User"];
                acc = u.account;
            }
            ViewBag.acc = acc;

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

            List<string> listE = cr.getAuthorEmail();

            Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadDecisionFile(file, name, listE);
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

        [HttpPost]
        public JsonResult deleteRequest(int id)
        {
            string mess = cr.deleteRequest(id);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
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

            List<Citation_Appendix_1> list1 = cr.getListAppendix1();
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

            List<Citation_Appendix_2> list2 = cr.getListAppendix2();
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
            string savePath = HostingEnvironment.MapPath(Flocation);
            excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/download/Citation.xlsx")));

            return Json(new { mess = true, location = Flocation }, JsonRequestBehavior.AllowGet);
        }
    }
}