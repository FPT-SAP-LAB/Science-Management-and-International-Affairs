using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class PaperController : Controller
    {
        PaperRepo pr = new PaperRepo();
        MasterDataRepo mdr = new MasterDataRepo();
        // GET: Paper
        public ActionResult Pending()
        {
            ViewBag.title = "Danh sách bài báo đang chờ xét duyệt";
            List<PendingPaper_Manager> list = pr.listPending();
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public ActionResult Detail(string id)
        {
            ViewBag.title = "Chi tiết bài báo";
            DetailPaper paper = pr.getDetail(id);
            ViewBag.paper = paper;

            List<ListCriteriaOfOnePaper> listCrite = pr.getCriteria(id);
            ViewBag.crite = listCrite;

            List<SpecializationLanguage> listSpec = mdr.getSpec("vi-VN");
            ViewBag.spec = listSpec;

            List<PaperType> listType = mdr.getPaperType();
            ViewBag.type = listType;

            List<AuthorInfoWithNull> listAuthor = pr.getAuthorPaper(id);
            ViewBag.author = listAuthor;

            ViewBag.request_id = paper.request_id;

            return View();
        }

        public ActionResult WaitDecision()
        {
            ViewBag.title = "Chờ quyết định khen thưởng";

            List<WaitDecisionPaper> listWaitQT = pr.getListWwaitDecision("Quocte");
            ViewBag.waitQT = listWaitQT;

            List<WaitDecisionPaper> listWaitTN = pr.getListWwaitDecision("Trongnuoc");
            ViewBag.waitTN = listWaitTN;

            return View();
        }

        public JsonResult editPaper(DetailPaper paper, List<AuthorInfoWithNull> people)
        {
            foreach (var item in people)
            {
                string temp = item.money_string;
                temp = temp.Replace(",", "");
                item.money_reward = Int32.Parse(temp);
            }
            string mess = pr.updateRewardPaper(paper);
            if (mess == "ss") mess = pr.updateAuthorReward(paper, people);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult exportExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "Paper.xlsx";
            FileInfo file = new FileInfo(path + filename);
            ExcelPackage excelPackage = new ExcelPackage(file);
            ExcelWorkbook excelWorkbook = excelPackage.Workbook;

            List<Paper_Appendix_1> list1 = pr.getListAppendix1_2("Trongnuoc");
            ExcelWorksheet excelWorksheet1 = excelWorkbook.Worksheets[0];
            int i = 2;
            int count = 1;
            Paper_Appendix_1 temp = new Paper_Appendix_1();
            foreach (var item in list1)
            {
                if (item.author_name != temp.author_name)
                {
                    excelWorksheet1.Cells[i, 1].Value = count;
                    excelWorksheet1.Cells[i, 2].Value = item.author_name;
                    excelWorksheet1.Cells[i, 3].Value = item.mssv_msnv;
                    excelWorksheet1.Cells[i, 4].Value = item.office_abbreviation;
                    count++;
                }
                excelWorksheet1.Cells[i, 5].Value = item.name;
                excelWorksheet1.Cells[i, 6].Value = item.company;
                string note = item.sum + " tác giả, " + item.sumFE + " địa chỉ FPTU";
                excelWorksheet1.Cells[i, 7].Value = note;
                temp = item;
                i++;
            }

            List<Paper_Appendix_1> list2 = pr.getListAppendix1_2("Quocte");
            ExcelWorksheet excelWorksheet2 = excelWorkbook.Worksheets[1];
            i = 2;
            count = 1;
            Paper_Appendix_1 temp2 = new Paper_Appendix_1();
            foreach (var item in list2)
            {
                if (item.author_name != temp2.author_name)
                {
                    excelWorksheet2.Cells[i, 1].Value = count;
                    excelWorksheet2.Cells[i, 2].Value = item.author_name;
                    excelWorksheet2.Cells[i, 3].Value = item.mssv_msnv;
                    excelWorksheet2.Cells[i, 4].Value = item.office_abbreviation;
                    count++;
                }
                excelWorksheet2.Cells[i, 5].Value = item.name;
                excelWorksheet2.Cells[i, 6].Value = item.company;
                string note = item.sum + " tác giả, " + item.sumFE + " địa chỉ FPTU";
                excelWorksheet2.Cells[i, 7].Value = note;
                temp2 = item;
                i++;
            }

            List<Paper_Apendix_3> list3 = pr.getListAppendix3_4("Trongnuoc");
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
                i++;
                count++;
            }

            List<Paper_Apendix_3> list4 = pr.getListAppendix3_4("Quocte");
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
                i++;
                count++;
            }

            string Flocation = "/Excel_template/download/Paper.xlsx";
            string savePath = HostingEnvironment.MapPath(Flocation);
            excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/download/Paper.xlsx")));

            return Json(new { mess = true, location = Flocation }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult uploadDecision(HttpPostedFileBase file1, string number1, string date1, 
                                         HttpPostedFileBase file2, string number2, string date2)
        {
            string[] arr = date1.Split('/');
            string format = arr[1] + "/" + arr[0] + "/" + arr[2];
            DateTime date_format1 = DateTime.Parse(format);

            arr = date2.Split('/');
            format = arr[1] + "/" + arr[0] + "/" + arr[2];
            DateTime date_format2 = DateTime.Parse(format);

            string name1 = "QD_" + number1 + "_" + date1;
            string name2 = "QD_" + number2 + "_" + date2;

            Google.Apis.Drive.v3.Data.File f1 = GoogleDriveService.UploadResearcherFile(file1, name1, 4, null);
            ENTITIES.File fl1 = new ENTITIES.File
            {
                link = f1.WebViewLink,
                file_drive_id = f1.Id,
                name = name1
            };

            Google.Apis.Drive.v3.Data.File f2 = GoogleDriveService.UploadResearcherFile(file2, name2, 4, null);
            ENTITIES.File fl2 = new ENTITIES.File
            {
                link = f2.WebViewLink,
                file_drive_id = f2.Id,
                name = name2
            };

            ENTITIES.File myFile1 = mdr.addFile(fl1);
            ENTITIES.File myFile2 = mdr.addFile(fl2);

            string mess = pr.uploadDecision(date_format1, myFile1.file_id, number1, myFile1.file_drive_id, 
                                            date_format2, myFile2.file_id, number2, myFile2.file_drive_id);

            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }
    }
}