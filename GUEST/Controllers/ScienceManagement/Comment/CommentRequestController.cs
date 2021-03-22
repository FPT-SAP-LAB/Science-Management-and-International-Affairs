using BLL.Authen;
using BLL.ScienceManagement.Comment;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GUEST.Controllers.ScienceManagement.Comment
{
    public class CommentRequestController : Controller
    {
        readonly CommentRepo CommentRepo = new CommentRepo();
        public ActionResult Index(int request_id)
        {
            List<DetailComment> Comments = CommentRepo.GetComment(request_id);
            ViewBag.Comments = Comments;
            ViewBag.request_id = request_id;
            return PartialView();
        }
        [HttpPost]
        public JsonResult Add(int request_id, string content)
        {
            LoginRepo.User u = new LoginRepo.User();
            Account acc = new Account();
            if (Session["User"] != null)
            {
                u = (LoginRepo.User)Session["User"];
                acc = u.account;
            }
            int account_id = acc.account_id;
            return Json(CommentRepo.AddComment(request_id, account_id, content));
        }
    }
}
