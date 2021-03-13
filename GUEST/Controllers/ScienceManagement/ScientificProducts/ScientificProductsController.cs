using BLL.ScienceManagement.ScientificProduct;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers.ScientificProducts
{
    public class ScientificProductsController : Controller
    {
        ListProductRepo lpr = new ListProductRepo();
        ListProductOnePersonRepo lpo = new ListProductOnePersonRepo();
        // GET: ScientificProducts
        public ActionResult Index()
        {
            ViewBag.title = "Sản phẩm khoa học";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Sản phẩm khoa học","/ScientificProducts"),
            };
            ViewBag.pagesTree = pagesTree;

            List<ListProduct_JournalPaper> list = lpr.getList(new DataSearch());
            ViewBag.listJournal = list;
            List<ListProduct_ConferencePaper> list2 = lpr.getList2(new DataSearch());
            ViewBag.listConferen = list2;
            List<ListProdcut_Inven> listInven = lpr.getListInven(new DataSearch());
            ViewBag.listInven = listInven;

            return View();
        }

        [HttpPost]
        public JsonResult Search(DataSearch item)
        {
            List<ListProduct_JournalPaper> list = lpr.getList(item);
            List<ListProduct_ConferencePaper> list2 = lpr.getList2(item);
            List<ListProdcut_Inven> listInven = lpr.getListInven(item);
            return Json(new { Journal = list, Conference = list2, Invention = listInven }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchOnePerson(DataSearch item)
        {
            List<ListProduct_OnePerson> list = new List<ListProduct_OnePerson>();
            if (item.monthS == "paper")
            {
                list = lpo.getList(item);
            }
            else
            {
                list = lpo.getListInven(item);
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].note = list[i].status_id + "_" + list[i].paper_id + "_" + item.monthS;
            }
            return Json(new { OnePerson = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pending(string type)
        {
            ViewBag.title = "Sản phẩm khoa học đang chờ phê duyệt";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Sản phẩm khoa học đang chờ phê duyệt","/ScientificProducts/Pending"),
            };
            ViewBag.pagesTree = pagesTree;
            List<ListProduct_OnePerson> list = new List<ListProduct_OnePerson>();
            if (type == "paper")
            {
                list = lpo.getList(new DataSearch());
            }
            else
            {
                list = lpo.getListInven(new DataSearch());
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].note = list[i].status_id + "_" + list[i].paper_id + "_" + type;
            }
            ViewBag.list = list;
            ViewBag.type = type;
            return View();
        }
    }
}