using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
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
            string Flocation = "/Excel_template/download/Paper.xlsx";
            string savePath = HostingEnvironment.MapPath(Flocation);
            excelPackage.SaveAs(new FileInfo(HostingEnvironment.MapPath("/Excel_template/download/Paper.xlsx")));

            return Json(new { mess = true, location = Flocation }, JsonRequestBehavior.AllowGet);
        }
    }
}