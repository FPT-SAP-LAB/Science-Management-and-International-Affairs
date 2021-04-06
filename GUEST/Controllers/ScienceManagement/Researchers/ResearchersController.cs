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
        const int ListGuestLenght = 20;
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        readonly List<PageTree> pagesTree = new List<PageTree>
            {
                new PageTree("Danh sách nghiên cứu viên", "/Researchers/List"),
            };
        public ActionResult List()
        {
            ViewBag.pagesTree = pagesTree;
            ////////////////////////////////////////////
            researcherListRepo = new ResearchersListRepo();
            BaseDatatable datatable = new BaseDatatable();
            datatable.Start = 0;
            datatable.Length = ListGuestLenght;
            datatable.SortColumnName = "name";
            datatable.SortDirection = "asc";
            BaseServerSideData<ResearcherList> list = researcherListRepo.GetList(datatable, "", "");
            ViewBag.list = list;
            ViewBag.initNumber = datatable.Length;
            ////////////////////////////////////////////
            return View();
        }
        public ActionResult GetList()
        {
            try
            {
                researcherListRepo = new ResearchersListRepo();
                BaseDatatable datatable = new BaseDatatable
                {
                    Start = Int32.Parse(Request["start"]),
                    Length = ListGuestLenght,
                    SortColumnName = "name",
                    SortDirection = "asc"
                };
                BaseServerSideData<ResearcherList> list = researcherListRepo.GetList(datatable, "", "");
                ViewBag.list = list;
                int initNumber = datatable.Start < list.RecordsTotal ? (list.RecordsTotal + datatable.Length) : list.RecordsTotal;
                return Json(new { success = true, list, initNumber });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }
        public ActionResult ViewInfo()
        {
            pagesTree.Add(new PageTree("Thông tin nghiên cứu viên", "#"));
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
            int id = Int32.Parse(Request.QueryString["id"]);
            pagesTree.Add(new PageTree("Thông tin nghiên cứu viên", "/Researchers/ViewInfo?id=" + id));
            pagesTree.Add(new PageTree("Chỉnh sửa thông tin", "#"));
            researcherDetailRepo = new ResearchersDetailRepo();
            if (CurrentAccount.getProfile(Session).people_id != id)
            {
                Response.Redirect("/ErrorPage/Error");
            }
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
