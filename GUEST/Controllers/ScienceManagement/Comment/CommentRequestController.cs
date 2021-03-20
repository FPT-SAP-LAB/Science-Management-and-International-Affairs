using BLL.ScienceManagement.Comment;
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
        CommentRepo CommentRepo = new CommentRepo();
        [ChildActionOnly]
        public ActionResult Index(int request_id)
        {
            List<DetailComment> Comments = CommentRepo.GetComment(request_id);
            ViewBag.Comments = Comments;
            return View();
        }
        [HttpPost]
        public ActionResult Add(string content)
        {
            int account_id = 1;
        }
    }
}
