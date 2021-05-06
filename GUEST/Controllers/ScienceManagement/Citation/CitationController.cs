using BLL.ModelDAL;
using BLL.ScienceManagement.Citation;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using GUEST.Models;
using GUEST.Support;
using System.Collections.Generic;
using System.Web.Mvc;
using User.Models;

namespace User.Controllers
{
    public class CitationController : Controller
    {
        private readonly CitationRepo cr = new CitationRepo();

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
        public ActionResult Edit(string id)
        {
            ViewBag.title = "Chỉnh sửa số trích dẫn";
            pagesTree.Add(new PageTree("Chỉnh sửa số trích dẫn", "/Citation/Edit"));
            ViewBag.pagesTree = pagesTree;

            List<CustomCitation> listCitation = cr.GetCitation(id);
            ViewBag.citation = listCitation;

            ViewBag.request_id = id;
            RequestCitation rc = cr.GetRequestCitation(id);
            if (rc == null)
                return Redirect("/Citation/Pending");

            ViewBag.total_reward = rc.total_reward;

            ViewBag.ckEdit = rc.citation_status_id;

            CitationTypeRepo citationTypeRepo = new CitationTypeRepo();
            ViewBag.citationTypes = citationTypeRepo.GetCitationTypes();

            return View();
        }

        [HttpPost]
        [Auther(RightID = "29")]
        public JsonResult AddCitation(List<Citation> citation)
        {
            CitationRequestAddRepo addRepo = new CitationRequestAddRepo();

            AlertModal<int> result = addRepo.AddRequestCitation(citation, CurrentAccount.AccountID(Session));
            return Json(result);
        }

        [HttpPost]
        [Auther(RightID = "29")]
        public JsonResult EditCitation(List<Citation> citation, int request_id)
        {
            CitationRequestEditRepo editRepo = new CitationRequestEditRepo();

            AlertModal<string> result = editRepo.EditRequestCitation(citation, CurrentAccount.AccountID(Session), request_id);
            return Json(result);
        }
    }
}
