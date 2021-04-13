using BLL.Authen;
using BLL.ScienceManagement.Comment;
using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using GUEST.Support;
using Newtonsoft.Json.Linq;
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

        [Auther(RightID = "26")]
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
        public JsonResult addFile(HttpPostedFileBase file, string name, DetailPaper paper)
        {
            LoginRepo.User u = new LoginRepo.User();
            Account acc = new Account();
            if (Session["User"] != null)
            {
                u = (LoginRepo.User)Session["User"];
                acc = u.account;
            }
            Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadResearcherFile(file, name, 2, acc.email);
            ENTITIES.File fl = new ENTITIES.File
            {
                link = f.WebViewLink,
                file_drive_id = f.Id,
                name = name
            };
            string mess = pr.addFile(fl);
            return Json(new { mess = mess, id = fl.file_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddPaper(HttpPostedFileBase file, string name, DetailPaper paper, string input)
        {
            if (paper == null)
            {
                paper = new DetailPaper();
                JObject @object = JObject.Parse(input);

                paper.name = (string)@object["name"];
                paper.date_string = (string)@object["date_string"];
                paper.link_doi = (string)@object["link_doi"];
                paper.link_scholar = (string)@object["link_scholar"];
                paper.paper_type_id = (int)@object["paper_type_id"];
                paper.journal_name = (string)@object["journal_name"];
                paper.vol = (string)@object["vol"];
                paper.page = (string)@object["page"];
                paper.company = (string)@object["company"];
                paper.index = (string)@object["index"];
                paper.note_domestic = (string)@object["note_domestic"];

                //paper = @object["DetailPaper"].ToObject<DetailPaper>();
            }

            if (file != null)
            {
                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;
                }
                Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadResearcherFile(file, name, 2, acc.email);
                ENTITIES.File fl = new ENTITIES.File
                {
                    link = f.WebViewLink,
                    file_drive_id = f.Id,
                    name = name
                };
                string mess = pr.addFile(fl);
                if (mess == "ss") paper.file_id = fl.file_id;
            }
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
            return Json(new { mess = mess, id = item.paper_id }, JsonRequestBehavior.AllowGet);
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
            string lang = "";
            if (Request.Cookies["language_name"] != null)
            {
                lang = Request.Cookies["language_name"].Value;
            }
            List<AuthorInfoWithNull> listAuthor = pr.getAuthorPaper(id, lang);
            string ms = pr.getAuthorReceived(id);
            if (ms == null) ms = "";
            return Json(new { author = listAuthor, ms = ms }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editPaperAuthorReward(List<AddAuthor> people, int paper_id)
        {
            string mess = pr.updateRewardAuthorAfterDecision(people, paper_id);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getDecision(int id)
        {
            List<string> link = pr.getDecisionLink(id);
            return Json(new { link = link }, JsonRequestBehavior.AllowGet);
        }
    }
}