using BLL.ScienceManagement.Invention;
using ENTITIES;
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

            List<AuthorInfoWithNull> listAuthor = ir.getAuthor(id);
            ViewBag.author = listAuthor;

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
    }
}