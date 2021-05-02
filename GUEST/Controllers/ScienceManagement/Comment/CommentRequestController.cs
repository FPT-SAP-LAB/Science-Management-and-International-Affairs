using BLL.ModelDAL;
using BLL.ScienceManagement.Comment;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using GUEST.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GUEST.Controllers.ScienceManagement.Comment
{
    public class CommentRequestController : Controller
    {
        readonly CommentRepo commentRepo = new CommentRepo();
        readonly BaseRequestRepo requestRepo = new BaseRequestRepo();
        public ActionResult Index(int request_id)
        {
            List<DetailComment> Comments = commentRepo.GetComment(request_id);
            ViewBag.Comments = Comments;
            ViewBag.request_id = request_id;
            ViewBag.IsEnded = requestRepo.IsEnded(request_id);
            return PartialView();
        }
        [HttpPost]
        public JsonResult Add(int request_id, string content)
        {
            return Json(commentRepo.AddComment(request_id, CurrentAccount.AccountID(Session), content, CurrentAccount.RoleID(Session), false, Request.UrlReferrer.AbsolutePath));
        }
    }
}
