using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}