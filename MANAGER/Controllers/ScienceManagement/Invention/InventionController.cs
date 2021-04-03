using BLL.ScienceManagement.Invention;
using BLL.ScienceManagement.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Invention;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class InventionController : Controller
    {
        InventionRepo ir = new InventionRepo();
        MasterDataRepo mrd = new MasterDataRepo();
        // GET: Invention
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

            Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadResearcherFile(file, name, 4, null);
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
    }
}