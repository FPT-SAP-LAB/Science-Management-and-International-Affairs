using BLL.ScienceManagement.Comment;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using MANAGER.Models;
using System.Collections.Generic;
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
            return Json(CommentRepo.AddComment(request_id, CurrentAccount.AccountID(Session), content, CurrentAccount.RoleID(Session)));
        }
    }
}
