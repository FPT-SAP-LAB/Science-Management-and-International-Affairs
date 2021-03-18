using BLL.ScienceManagement.Comment;
using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class PaperController : Controller
    {
        PaperRepo pr = new PaperRepo();
        MasterDataRepo md = new MasterDataRepo();
        CommentRepo cr = new CommentRepo();
        public ActionResult AddRequest()
        {
            ViewBag.title = "Đăng ký khen thưởng bài báo";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đăng ký khen thưởng bài báo","/Paper/AddRequest"),
            };
            ViewBag.pagesTree = pagesTree;

            string lang = "";
            if (Request.Cookies["language_name"] != null)
            {
                lang = Request.Cookies["language_name"].Value;
            }
            List<SpecializationLanguage> listSpec = md.getSpec(lang);
            ViewBag.listSpec = listSpec;

            List<PaperCriteria> listCriteria = md.getPaperCriteria();
            ViewBag.listCriteria = listCriteria;
            return View();
        }

        [HttpPost]
        public void AddPaper(Paper dataPaper)
        {
            return;
        }

        [HttpPost]
        public void AddRequest(RequestPaper item)
        {
            return;
        }

        [HttpPost]
        public void AddAuthor(List<AddAuthor> item)
        {
            return;
        }

        [HttpPost]
        public ActionResult Edit(string id, string editable)
        {
            ViewBag.title = "Chỉnh sửa khen thưởng bài báo";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Chỉnh sửa khen thưởng bài báo","/Paper/Edit"),
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.ckEdit = editable;

            DetailPaper item = pr.getDetail(id);
            ViewBag.Paper = item;

            int request_id = item.request_id;

            string lang = "";
            if (Request.Cookies["language_name"] != null)
            {
                lang = Request.Cookies["language_name"].Value;
            }
            List<SpecializationLanguage> listSpec = md.getSpec(lang);
            ViewBag.listSpec = listSpec;

            List<PaperCriteria> listCriteria = md.getPaperCriteria();
            ViewBag.listCriteria = listCriteria;

            List<ListCriteriaOfOnePaper> listCriteriaOne = pr.getCriteria(id);
            ViewBag.listCriteriaOne = listCriteriaOne;

            List<AuthorInfo> listAuthor = pr.getAuthorPaper(id);
            ViewBag.listAuthor = listAuthor;
            ViewBag.numberAuthor = listAuthor.Count();

            List<DetailComment> listCmt = cr.getComment(request_id);
            ViewBag.cmt = listCmt;

            return View();
        }

        //public ActionResult Pending()
        //{
        //    ViewBag.title = "Bài báo đang xử lý";
        //    var pagesTree = new List<PageTree>
        //    {
        //        new PageTree("Bài báo đang xử lý","/Paper/Pending"),
        //    };
        //    ViewBag.pagesTree = pagesTree;
        //    return View();
        //}
    }
}