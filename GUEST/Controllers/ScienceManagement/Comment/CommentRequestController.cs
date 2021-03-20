using BLL.ScienceManagement.Comment;
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
            int account_id = 6;
            return Json(CommentRepo.AddComment(request_id, account_id, content));
        }
    }
}
