using BLL.ScienceManagement.Citation;
using BLL.ScienceManagement.Comment;
using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using GUEST.Models;
using GUEST.Support;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using User.Models;

namespace User.Controllers
{
    public class CitationController : Controller
    {
        private readonly CitationRepo cr = new CitationRepo();
        private readonly MasterDataRepo md = new MasterDataRepo();
        private readonly CommentRepo crr = new CommentRepo();
        private readonly PaperRepo pr = new PaperRepo();

        private readonly List<PageTree> pagesTree = new List<PageTree>();
        // GET: Citation

        [Auther(RightID = "29")]
        public ActionResult List()
        {
            ViewBag.title = "Số trích dẫn";
            pagesTree.Add(new PageTree("Số trích dẫn", "/Citation/List"));
            ViewBag.pagesTree = pagesTree;

            return View();
        }

        [Auther(RightID = "29")]
        public ActionResult AddRequest()
        {
            ViewBag.title = "Đề xuất khen thưởng số trích dẫn";
            pagesTree.Add(new PageTree("Đề xuất khen thưởng số trích dẫn", "/Citation/AddRequest"));
            ViewBag.pagesTree = pagesTree;

            return View();
        }

        [Auther(RightID = "29")]
        public ActionResult Pending()
        {
            ViewBag.title = "Số trích dẫn đang xử lý";
            pagesTree.Add(new PageTree("Số trích dẫn đang xử lý", "/Citation/Pending"));
            ViewBag.pagesTree = pagesTree;

            List<ListOnePerson_Citation> list = cr.GetList(CurrentAccount.AccountID(Session));
            ViewBag.list = list;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].note = list[i].status_id + "_" + list[i].request_id;
            }
            return View();
        }

        [Auther(RightID = "29")]
        //[HttpPost]
        public ActionResult Edit(string id)
        {
            ViewBag.title = "Chỉnh sửa số trích dẫn";
            pagesTree.Add(new PageTree("Chỉnh sửa số trích dẫn", "/Citation/Edit"));
            ViewBag.pagesTree = pagesTree;

            AuthorInfo author = cr.GetAuthor(id);
            ViewBag.author = author;

            List<Citation> listCitation = cr.GetCitation(id);
            ViewBag.citation = listCitation;

            List<DetailComment> listCmt = crr.GetComment(Int32.Parse(id));
            ViewBag.cmt = listCmt;

            ViewBag.request_id = id;
            RequestCitation rc = cr.GetRequestCitation(id);
            ViewBag.ckEdit = rc.status_id;

            return View();
        }

        [HttpPost]
        public JsonResult addCitation(List<Citation> citation, List<AddAuthor> people)
        {
            Author author = cr.AddAuthor(people);

            cr.AddCitaion(citation);

            BaseRequest b = pr.addBaseRequest(CurrentAccount.AccountID(Session));
            cr.AddCitationRequest(b, author);
            string mess = cr.AddRequestHasCitation(citation, b);
            return Json(new { mess, id = b.request_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editCitation(List<Citation> citation, List<AddAuthor> people, string request_id)
        {
            //cr.addAuthor(people);
            List<Citation> oldcitation = cr.GetCitation(request_id);
            Author author = cr.EditAuthor(people);
            string mess = cr.EditCitation(oldcitation, citation, request_id, author);
            return Json(new { mess, id = request_id }, JsonRequestBehavior.AllowGet);
        }
    }
}
