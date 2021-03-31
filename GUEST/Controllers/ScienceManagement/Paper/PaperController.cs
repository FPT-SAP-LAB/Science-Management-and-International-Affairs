using BLL.Authen;
using BLL.ScienceManagement.Comment;
using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public JsonResult AddPaper(DetailPaper paper)
        {
            paper.publish_date = DateTime.ParseExact(paper.date_string, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Paper p = pr.addPaper(paper);
            if (p != null) return Json(new { id = p.paper_id, mess = "ss" }, JsonRequestBehavior.AllowGet);
            else return Json(new { mess = "ff" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddRequest(RequestPaper request, string daidien)
        {
            LoginRepo.User u = new LoginRepo.User();
            Account acc = new Account();
            if (Session["User"] != null)
            {
                u = (LoginRepo.User)Session["User"];
                acc = u.account;
            }
            BaseRequest b = pr.addBaseRequest(acc.account_id);
            string mess = pr.addRequestPaper(b.request_id, request, daidien);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddAuthor(List<AddAuthor> people, string paper_id)
        {
            string mess = pr.addAuthor(people, paper_id);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddCriteria(List<CustomCriteria> criteria, string paper_id)
        {
            string mess = pr.addCriteria(criteria, paper_id);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
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

            //List<DetailComment> listCmt = cr.GetComment(request_id);
            //ViewBag.cmt = listCmt;
            ViewBag.request_id = request_id;
            ViewBag.id = id;

            return View();
        }
        [HttpPost]
        public JsonResult editPaper(string paper_id, Paper paper)
        {
            string mess = pr.updatePaper(paper_id, paper);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editRequest(RequestPaper item)
        {
            string mess = pr.updateRequest(item);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editCriteria(List<CustomCriteria> criteria, string paper_id)
        {
            string mess = pr.updateCriteria(criteria, paper_id);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editAuthor(List<AddAuthor> people, string paper_id)
        {
            string mess = pr.updateAuthor(people, paper_id);
            return Json(new { mess = mess, id = paper_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult listAuthor(string id)
        {
            List<AuthorInfoWithNull> listAuthor = pr.getAuthorPaper(id);
            string ms = pr.getAuthorReceived(id);
            if (ms == null) ms = "";
            return Json(new { author = listAuthor, ms = ms }, JsonRequestBehavior.AllowGet);
        }
    }
}