using BLL.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace User.Controllers
{
    public class CitationController : Controller
    {
        CitationRepo cr = new CitationRepo();
        // GET: Citation
        public ActionResult List()
        {
            ViewBag.title = "Số trích dẫn";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Số trích dẫn","/Citation/List"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        public ActionResult AddRequest()
        {
            ViewBag.title = "Đề xuất khen thưởng số trích dẫn";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề xuất khen thưởng số trích dẫn","/Citation/AddRequest"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        public ActionResult Pending()
        {
            ViewBag.title = "Số trích dẫn đang xử lý";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Số trích dẫn đang xử lý","/Citation/Pending"),
            };
            ViewBag.pagesTree = pagesTree;
            List<ListOnePerson_Citation> list = cr.GetList("1");
            ViewBag.list = list;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].note = list[i].status_id + "_" + list[i].request_id;
            }
            return View();
        }

        public ActionResult Edit(string id, string editable)
        {
            ViewBag.title = "Chỉnh sửa số trích dẫn";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Chỉnh sửa số trích dẫn","/Citation/Edit"),
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.ckEdit = editable;
            return View();
        }
    }
}