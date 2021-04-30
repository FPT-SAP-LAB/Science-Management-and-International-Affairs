using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using MANAGER.Models;
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
    public class PaperController : Controller
    {
        private readonly PaperRepo pr = new PaperRepo();
        private readonly MasterDataRepo mdr = new MasterDataRepo();
        // GET: Paper
        [Auther(RightID = "16")]
        public ActionResult Pending()
        {
            ViewBag.title = "Danh sách bài báo đang chờ xét duyệt";
            List<PendingPaper_Manager> list = pr.ListPending();
            ViewBag.list = list;
            return View();
        }

        //[HttpPost]
        [Auther(RightID = "16")]
        public ActionResult Detail(string id)
        {
            ViewBag.title = "Chi tiết bài báo";
            DetailPaper paper = pr.GetDetail(id);
            ViewBag.paper = paper;

            List<ListCriteriaOfOnePaper> listCrite = pr.GetCriteria(id);
            ViewBag.crite = listCrite;

            List<SpecializationLanguage> listSpec = mdr.GetSpec("vi-VN");
            ViewBag.spec = listSpec;

            List<PaperType> listType = mdr.GetPaperType();
            ViewBag.type = listType;

            List<AuthorInfoWithNull> listAuthor = pr.GetAuthorPaper(id, "vi-VN");
            ViewBag.author = listAuthor;

            ViewBag.request_id = paper.request_id;

            Author p = pr.GetAuthorReceived_all(id);
            if (p == null) p = new Author();
            ViewBag.p = p;

            ViewBag.acc = CurrentAccount.Account(Session);

            List<PaperCriteria> listCriteria = mdr.GetPaperCriteria();
            ViewBag.listCriteria = listCriteria;

            return View();
        }

        [Auther(RightID = "18")]
        public ActionResult WaitDecision()
        {
            ViewBag.title = "Chờ quyết định khen thưởng (giảng viên)";

            List<WaitDecisionPaper> listWaitQT = pr.GetListWwaitDecision("2", 0);
            ViewBag.waitQT = listWaitQT;

            List<WaitDecisionPaper> listWaitTN = pr.GetListWwaitDecision("1", 0);
            ViewBag.waitTN = listWaitTN;

            return View();
        }

        [Auther(RightID = "18")]
        public ActionResult WaitDecision2()
        {
            ViewBag.title = "Chờ quyết định khen thưởng (nghiên cứu viên)";

            List<WaitDecisionPaper> listWaitQT = pr.GetListWwaitDecision2("2", 1);
            ViewBag.waitQT = listWaitQT;

            List<WaitDecisionPaper> listWaitTN = pr.GetListWwaitDecision2("1", 1);
            ViewBag.waitTN = listWaitTN;

            return View();
        }

        [Auther(RightID = "18")]
        [HttpPost]
        public JsonResult UpdateJournal()
        {
            bool mess = pr.UpdateJournal();
            string content = "Cập nhật thành công";
            if (!mess) content = "Cập nhật thất bại";
            return Json(new { mess, content }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult editPaper(DetailPaper paper, List<AuthorInfoWithNull> people, string id)
        {
            foreach (var item in people)
            {
                string temp = item.money_string;
                if (temp == null) temp = "0";
                else temp = temp.Replace(",", "");
                item.money_reward = Int32.Parse(temp);
            }
            string mess = pr.UpdateRewardPaper(paper);
            if (mess == "ss") mess = pr.UpdateAuthorReward(paper, people, id);
            if (mess == "ss") mess = pr.UpdateCriteria_ManagerCheck(paper.paper_id);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult exportExcel(int reseacher)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "Paper.xlsx";
            FileInfo file = new FileInfo(path + filename);
            ExcelPackage excelPackage = new ExcelPackage(file);
            ExcelWorkbook excelWorkbook = excelPackage.Workbook;

            List<Paper_Appendix_1> list1 = pr.GetListAppendix1_2("1", reseacher);
            ExcelWorksheet excelWorksheet1 = excelWorkbook.Worksheets[0];
            int i = 2;
            int count = 1;
            Paper_Appendix_1 temp = new Paper_Appendix_1();
            foreach (var item in list1)
            {
                if (item.author_name != temp.author_name && item.mssv_msnv != temp.mssv_msnv)
                {
                    excelWorksheet1.Cells[i, 1].Value = count;
                    excelWorksheet1.Cells[i, 2].Value = item.author_name;
                    excelWorksheet1.Cells[i, 3].Value = item.mssv_msnv;
                    excelWorksheet1.Cells[i, 4].Value = item.office_abbreviation;
                    count++;
                }
                excelWorksheet1.Cells[i, 5].Value = item.name;
                excelWorksheet1.Cells[i, 6].Value = item.journal_name;
                string note = item.sum + " tác giả, " + item.sumFE + " địa chỉ FPTU";
                excelWorksheet1.Cells[i, 7].Value = note;
                temp = item;
                i++;
            }

            List<Paper_Appendix_1> list2 = pr.GetListAppendix1_2("2", reseacher);
            ExcelWorksheet excelWorksheet2 = excelWorkbook.Worksheets[1];
            i = 2;
            count = 1;
            Paper_Appendix_1 temp2 = new Paper_Appendix_1();
            foreach (var item in list2)
            {
                if (item.author_name != temp2.author_name && item.mssv_msnv != temp2.mssv_msnv)
                {
                    excelWorksheet2.Cells[i, 1].Value = count;
                    excelWorksheet2.Cells[i, 2].Value = item.author_name;
                    excelWorksheet2.Cells[i, 3].Value = item.mssv_msnv;
                    excelWorksheet2.Cells[i, 4].Value = item.office_abbreviation;
                    count++;
                }
                excelWorksheet2.Cells[i, 5].Value = item.name;
                excelWorksheet2.Cells[i, 6].Value = item.journal_name;
                string note = item.sum + " tác giả, " + item.sumFE + " địa chỉ FPTU";
                excelWorksheet2.Cells[i, 7].Value = note;
                temp2 = item;
                i++;
            }

            List<Paper_Apendix_3> list3 = pr.GetListAppendix3_4("1", reseacher);
            ExcelWorksheet excelWorksheet3 = excelWorkbook.Worksheets[2];
            i = 2;
            count = 1;
            foreach (var item in list3)
            {
                excelWorksheet3.Cells[i, 1].Value = count;
                excelWorksheet3.Cells[i, 2].Value = item.name;
                excelWorksheet3.Cells[i, 3].Value = item.mssv_msnv;
                excelWorksheet3.Cells[i, 4].Value = item.office_abbreviation;
                CultureInfo cul = new CultureInfo("vi-VN");
                item.money_string = item.sum_money.ToString("C0", cul.NumberFormat);
                excelWorksheet3.Cells[i, 5].Value = item.money_string;
                excelWorksheet3.Cells[i, 6].Value = item.identification_file_link;
                i++;
                count++;
            }

            List<Paper_Apendix_3> list4 = pr.GetListAppendix3_4("2", reseacher);
            ExcelWorksheet excelWorksheet4 = excelWorkbook.Worksheets[3];
            i = 2;
            count = 1;
            foreach (var item in list4)
            {
                excelWorksheet4.Cells[i, 1].Value = count;
                excelWorksheet4.Cells[i, 2].Value = item.name;
                excelWorksheet4.Cells[i, 3].Value = item.mssv_msnv;
                excelWorksheet4.Cells[i, 4].Value = item.office_abbreviation;
                CultureInfo cul = new CultureInfo("vi-VN");
                item.money_string = item.sum_money.ToString("C0", cul.NumberFormat);
                excelWorksheet4.Cells[i, 5].Value = item.money_string;
                excelWorksheet4.Cells[i, 6].Value = item.identification_file_link;
                i++;
                count++;
            }

            string Flocation = "/Excel_template/download/Paper.xlsx";
            //string savePath = HostingEnvironment.MapPath(Flocation);
            excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/download/Paper.xlsx")));

            return Json(new { mess = true, location = Flocation }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult uploadDecision(HttpPostedFileBase file1, string number1, string date1, int reseacher)
        {
            string[] arr = date1.Split('/');
            string format = arr[1] + "/" + arr[0] + "/" + arr[2];
            DateTime date_format1 = DateTime.Parse(format);

            string name1 = "QD_" + number1 + "_" + date1;

            List<string> listE = pr.GetLstEmailAuthor(0);

            Google.Apis.Drive.v3.Data.File f1 = GoogleDriveService.UploadDecisionFile(file1, name1, listE);
            ENTITIES.File fl1 = new ENTITIES.File
            {
                link = f1.WebViewLink,
                file_drive_id = f1.Id,
                name = name1
            };

            ENTITIES.File myFile1 = mdr.AddFile(fl1);

            string mess = pr.UploadDecision(date_format1, myFile1.file_id, number1, myFile1.file_drive_id, reseacher);

            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult uploadDecision2(HttpPostedFileBase file1, string number1, string date1, int reseacher)
        {
            string[] arr = date1.Split('/');
            string format = arr[1] + "/" + arr[0] + "/" + arr[2];
            DateTime date_format1 = DateTime.Parse(format);

            string name1 = "QD_" + number1 + "_" + date1;

            List<string> listE = pr.GetLstEmailAuthor(1);

            Google.Apis.Drive.v3.Data.File f1 = GoogleDriveService.UploadDecisionFile(file1, name1, listE);
            ENTITIES.File fl1 = new ENTITIES.File
            {
                link = f1.WebViewLink,
                file_drive_id = f1.Id,
                name = name1
            };

            ENTITIES.File myFile1 = mdr.AddFile(fl1);

            string mess = pr.UploadDecision2(date_format1, myFile1.file_id, number1, myFile1.file_drive_id, reseacher);

            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult changeStatus(DetailPaper paper)
        {
            string mess = pr.ChangeStatus(paper);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult changeStatusManager(DetailPaper paper)
        {
            string mess = pr.ChangeStatusManager(paper);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteRequest(int id)
        {
            string mess = pr.DeleteRequest(id);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult waitVerify()
        {
            List<PendingPaper_Manager> list = pr.ListWaitVerify();
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public JsonResult confirmReqward(int request_id)
        {
            bool mess = pr.ConfirmReward(request_id);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editAuthorReqward(int request_id)
        {
            bool mess = pr.EditAuthorReward(request_id);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editCriteria(List<CustomCriteria> criteria, string paper_id)
        {
            string temp = pr.UpdateCriteria(criteria, paper_id);
            bool mess = true;
            if (temp == "ff") mess = false;
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }
    }
}