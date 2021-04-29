using BLL.ModelDAL;
using BLL.ScienceManagement.Citation;
using BLL.ScienceManagement.Comment;
using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Citation;
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

            CitationTypeRepo citationTypeRepo = new CitationTypeRepo();
            ViewBag.citationTypes = citationTypeRepo.GetCitationTypes();

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

            List<CustomCitation> listCitation = cr.GetCitation(id);
            ViewBag.citation = listCitation;

            List<DetailComment> listCmt = crr.GetComment(Int32.Parse(id));
            ViewBag.cmt = listCmt;

            ViewBag.request_id = id;
            RequestCitation rc = cr.GetRequestCitation(id);
            ViewBag.ckEdit = rc.citation_status_id;

            CitationTypeRepo citationTypeRepo = new CitationTypeRepo();
            ViewBag.citationTypes = citationTypeRepo.GetCitationTypes();

            return View();
        }

        [HttpPost]
        public JsonResult AddCitation(List<Citation> citation, AddAuthor addAuthor)
        {
            CitationRequestAddRepo addRepo = new CitationRequestAddRepo();

            AlertModal<int> result = addRepo.AddRequestCitation(citation, addAuthor, CurrentAccount.AccountID(Session));
            return Json(new { result.success, id = result.obj, result.content });
        }

        [HttpPost]
        public JsonResult EditCitation(List<Citation> citation, AddAuthor addAuthor, int request_id)
        {
            CitationRequestEditRepo editRepo = new CitationRequestEditRepo();

            AlertModal<string> result = editRepo.EditRequestCitation(citation, addAuthor, CurrentAccount.AccountID(Session), request_id);
            return Json(result);
        }
    }
}
