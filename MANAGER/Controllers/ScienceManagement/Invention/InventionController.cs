using BLL.ScienceManagement.Invention;
using BLL.ScienceManagement.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Invention;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using MANAGER.Support;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class InventionController : Controller
    {
        InventionRepo ir = new InventionRepo();
        MasterDataRepo mrd = new MasterDataRepo();
        // GET: Invention
        [Auther(RightID = "16")]
        public ActionResult Pending()
        {
            ViewBag.title = "Danh sách bằng sáng chế đang chờ xét duyệt";
            List<PendingInvention_Manager> listPending = ir.getListPending();
            ViewBag.pending = listPending;

            return View();
        }

        [HttpPost]
        public ActionResult Detail(string id)
        {
            ViewBag.title = "Chi tiết bằng sáng chế";
            DetailInvention item = ir.getDetail(id);
            ViewBag.item = item;

            List<Country> listCountry = ir.getCountry();
            ViewBag.country = listCountry;

            List<AuthorInfoWithNull> listAuthor = ir.getAuthor(id, "vi-VN");
            ViewBag.author = listAuthor;

            ViewBag.request_id = item.request_id;

            return View();
        }

        [Auther(RightID = "18")]
        public ActionResult WaitDecision()
        {
            ViewBag.title = "Chờ quyết định khen thưởng";
            List<WaitDecisionInven> list = ir.getListWaitDecision();
            ViewBag.list = list;
            return View();
        }

        public JsonResult editInven(DetailInvention inven, List<AuthorInfoWithNull> people)
        {
            foreach (var item in people)
            {
                string temp = item.money_string;
                temp = temp.Replace(",", "");
                item.money_reward = Int32.Parse(temp);
            }
            string mess = ir.updateRewardInven(inven);
            if (mess == "ss") mess = ir.updateAuthorReward(inven, people);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult uploadDecision(HttpPostedFileBase file, string number, string date)
        {
            string[] arr = date.Split('/');
            string format = arr[1] + "/" + arr[0] + "/" + arr[2];
            DateTime date_format = DateTime.Parse(format);

            string name = "QD_" + number + "_" + date;

            List<string> listE = ir.getAuthorEmail();

            Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadDecisionFile(file, name, listE);
            ENTITIES.File fl = new ENTITIES.File
            {
                link = f.WebViewLink,
                file_drive_id = f.Id,
                name = name
            };

            ENTITIES.File myFile = mrd.addFile(fl);
            string mess = ir.uploadDecision(date_format, myFile.file_id, number, myFile.file_drive_id);

            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult changeStatus(DetailInvention inven)
        {
            string mess = ir.changeStatus(inven);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult changeStatusManager(DetailInvention inven)
        {
            string mess = ir.changeStatusManager(inven);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteRequest(int id)
        {
            string mess = ir.deleteRequest(id);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult exportExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string path = HostingEnvironment.MapPath("/Excel_template/");
            string filename = "Invention.xlsx";
            FileInfo file = new FileInfo(path + filename);
            ExcelPackage excelPackage = new ExcelPackage(file);
            ExcelWorkbook excelWorkbook = excelPackage.Workbook;

            List<Invention_Appendix_1> list1 = ir.getListAppendix1();
            ExcelWorksheet excelWorksheet1 = excelWorkbook.Worksheets[0];
            int i = 2;
            int count = 1;
            Invention_Appendix_1 temp1 = new Invention_Appendix_1();
            foreach (var item in list1)
            {
                if (item.author_name != temp1.author_name && item.mssv_msnv != temp1.mssv_msnv)
                {
                    excelWorksheet1.Cells[i, 1].Value = count;
                    excelWorksheet1.Cells[i, 2].Value = item.author_name;
                    excelWorksheet1.Cells[i, 3].Value = item.mssv_msnv;
                    excelWorksheet1.Cells[i, 4].Value = item.office_abbreviation;
                    count++;
                }
                excelWorksheet1.Cells[i, 5].Value = item.type_name;
                excelWorksheet1.Cells[i, 6].Value = item.no;
                excelWorksheet1.Cells[i, 7].Value = item.name;
                temp1 = item;
                i++;
            }

            List<Invention_Appendix_2> list2 = ir.getListAppendix2();
            ExcelWorksheet excelWorksheet2 = excelWorkbook.Worksheets[1];
            i = 2;
            count = 1;
            foreach (var item in list2)
            {
                excelWorksheet2.Cells[i, 1].Value = count;
                excelWorksheet2.Cells[i, 2].Value = item.author_name;
                excelWorksheet2.Cells[i, 3].Value = item.mssv_msnv;
                excelWorksheet2.Cells[i, 4].Value = item.office_abbreviation;
                excelWorksheet2.Cells[i, 5].Value = item.money_reward;
                count++;
                i++;
            }

            string Flocation = "/Excel_template/download/Invention.xlsx";
            string savePath = HostingEnvironment.MapPath(Flocation);
            excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/download/Invention.xlsx")));

            return Json(new { mess = true, location = Flocation }, JsonRequestBehavior.AllowGet);
        }
    }
}