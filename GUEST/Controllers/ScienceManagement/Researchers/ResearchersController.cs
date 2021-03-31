using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;
using ENTITIES;
using BLL.ScienceManagement.ConferenceSponsor;
using GUEST.Models;
using System.Collections;
using BLL.ScienceManagement.Researcher;
using BLL.ScienceManagement.ResearcherListRepo;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using Newtonsoft.Json.Linq;

namespace GUEST.Controllers.ScienceManagement.Researchers
{
    public class ResearchersController : Controller
    {
        ResearchersListRepo researcherListRepo;
        ResearchersDetailRepo researcherDetailRepo;
        ResearchersBiographyRepo researcherBiographyRepo;
        EditResearcherInfoRepo researcherEditResearcherInfo;
        // GET: Reseachers
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public ActionResult List()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Nghiên cứu viên", "/Researchers"),
                new PageTree("Danh sách nghiên cứu viên", "/Researchers/List"),
            };
            ViewBag.pagesTree = pagesTree;
            ////////////////////////////////////////////
            researcherListRepo = new ResearchersListRepo();
            BaseDatatable datatable = new BaseDatatable();
            datatable.Start = 0;
            datatable.Length = 20;
            datatable.SortColumnName = "name";
            datatable.SortDirection = "asc";
            BaseServerSideData<ResearcherList> list = researcherListRepo.GetList(datatable);
            ViewBag.list = list;
            ////////////////////////////////////////////
            return View();
        }
        public ActionResult GetList()
        {
            researcherListRepo = new ResearchersListRepo();
            BaseDatatable datatable = new BaseDatatable();
            string request = Request["request"];
            var requestJson = JObject.Parse(request);
            datatable.Start = (int)requestJson["request"]["start"];
            datatable.Start = (int)requestJson["request"]["length"];
            BaseServerSideData<ResearcherList> list = researcherListRepo.GetList(datatable);
            ViewBag.list = list;
            return Json(new { list = list });
        }
        public ActionResult ViewInfo()
        {
            var pagesTree = new List<PageTree>
           {
               new PageTree("Nghiên cứu viên", "/Researchers"),
               new PageTree("Thông tin nghiên cứu viên", "/Researchers/ViewInfo"),
           };
            researcherDetailRepo = new ResearchersDetailRepo();
            int id = Int32.Parse(Request.QueryString["id"]);
            ResearcherDetail profile = researcherDetailRepo.GetDetailView(id);
            ViewBag.profile = profile;
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public ActionResult GetPublications()
        {
            researcherBiographyRepo = new ResearchersBiographyRepo();
            int id = Int32.Parse(Request.QueryString["id"]);
            List<ResearcherPublications> researcher_public = researcherBiographyRepo.GetPublications(id);
            return Json(new { success = true, data = researcher_public, draw = Request["draw"], recordsTotal = researcher_public.Count, recordsFiltered = researcher_public.Count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditInfo()
        {
            var pagesTree = new List<PageTree>
           {
               new PageTree("Trang cá nhân", "/Researchers/ViewInfo"),
               new PageTree("Chỉnh sửa thông tin", "/Researchers/EditInfo"),
           };
            researcherDetailRepo = new ResearchersDetailRepo();
            int id = Int32.Parse(Request.QueryString["id"]);
            ResearcherDetail profile = researcherDetailRepo.GetProfile(id);
            ViewBag.profile = profile;
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public ActionResult EditResearcher()
        {
            //Chỗ này cần thêm chỉ có user đang đăng nhập trùng với profile này mới sửa được
            researcherEditResearcherInfo = new EditResearcherInfoRepo();
            string data = Request["info"];
            researcherEditResearcherInfo.EditResearcherProfile(data);
            return null;
        }
    }
}
