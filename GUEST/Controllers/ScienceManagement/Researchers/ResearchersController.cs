using BLL.ScienceManagement.Researcher;
using BLL.ScienceManagement.ResearcherListRepo;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using GUEST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using User.Models;

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
            int languageId = LanguageResource.GetCurrentLanguageID();
            researcherListRepo = new ResearchersListRepo();
            BaseDatatable datatable = new BaseDatatable
            {
                Start = 0,
                Length = ListGuestLenght,
                SortColumnName = "name",
                SortDirection = "asc"
            };
            BaseServerSideData<ResearcherList> list = researcherListRepo.GetList(datatable, "", "", languageId);
            ViewBag.list = list;
            ViewBag.initNumber = datatable.Length;
            ////////////////////////////////////////////
            return View();
        }
        public ActionResult GetList()
        {
            try
            {
                int languageId = LanguageResource.GetCurrentLanguageID();
                researcherListRepo = new ResearchersListRepo();
                BaseDatatable datatable = new BaseDatatable
                {
                    Start = int.Parse(Request["start"]),
                    Length = ListGuestLenght,
                    SortColumnName = "name",
                    SortDirection = "asc"
                };
                BaseServerSideData<ResearcherList> list = researcherListRepo.GetList(datatable, "", "", languageId);
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
            int id = int.Parse(Request.QueryString["id"]);
            ResearcherDetail profile = researcherDetailRepo.GetDetailView(id, LanguageResource.GetCurrentLanguageID());
            ViewBag.profile = profile;
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public ActionResult GetPublications()
        {
            researcherBiographyRepo = new ResearchersBiographyRepo();
            int id = int.Parse(Request.QueryString["id"]);
            List<ResearcherPublications> researcher_public = researcherBiographyRepo.GetPublications(id);
            return Json(new { success = true, data = researcher_public, draw = Request["draw"], recordsTotal = researcher_public.Count, recordsFiltered = researcher_public.Count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditInfo()
        {
            var pagesTree = new List<PageTree>
           {
               new PageTree("Trang cá nhân", "/Researchers/ViewInfo"),
               new PageTree("Chỉnh sửa thông tin", "#"),
           };
            int languageId = LanguageResource.GetCurrentLanguageID();
            researcherDetailRepo = new ResearchersDetailRepo();
            researcherBiographyRepo = new ResearchersBiographyRepo();
            int id = int.Parse(Request.QueryString["id"]);
            pagesTree.Add(new PageTree("Thông tin nghiên cứu viên", "/Researchers/ViewInfo?id=" + id));
            pagesTree.Add(new PageTree("Chỉnh sửa thông tin", "#"));
            researcherDetailRepo = new ResearchersDetailRepo();
            if (CurrentAccount.GetProfile(Session).people_id != id)
            {
                Response.Redirect("/ErrorPage/Error");
            }
            ResearcherDetail profile = researcherDetailRepo.GetProfile(id, languageId);
            List<SelectField> listAcadDegree = researcherBiographyRepo.getAcadDegrees();
            List<SelectField> listTitles = researcherBiographyRepo.getTitles(languageId);
            ViewBag.listAcadDegree = listAcadDegree;
            ViewBag.listTitles = listTitles;
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
        public JsonResult getAcadList()
        {
            try
            {
                int languageId = LanguageResource.GetCurrentLanguageID();
                researcherBiographyRepo = new ResearchersBiographyRepo();
                int id = int.Parse(Request.QueryString["id"]);
                List<AcadBiography> acadList = researcherBiographyRepo.GetAcadHistory(id, languageId);
                return Json(new { success = true, data = acadList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getWorkList()
        {
            try
            {
                researcherBiographyRepo = new ResearchersBiographyRepo();
                int id = int.Parse(Request.QueryString["id"]);
                List<BaseRecord<WorkingProcess>> workList = researcherBiographyRepo.GetWorkHistory(id);
                workList = workList.Select(x => { x.records.Profile = null; return x; }).ToList();
                return Json(new
                {
                    success = true,
                    data = (from a in workList
                            select new
                            {
                                a.index,
                                a.records.id,
                                a.records.pepple_id,
                                a.records.work_unit,
                                a.records.title,
                                a.records.start_year,
                                a.records.end_year
                            })
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditProfilePhoto()
        {
            researcherEditResearcherInfo = new EditResearcherInfoRepo();
            var uploadfile = Request.Files["imageInput"];
            int people_id = int.Parse(Request.Form["people_id"]);
            Account account = CurrentAccount.Account(Session);
            var file = GoogleDriveService.UploadProfileMedia(uploadfile, account.email);
            int res = researcherEditResearcherInfo.EditResearcherProfilePicture(file, people_id);
            return Json(new { res });
        }
        public JsonResult GetAwards()
        {
            researcherBiographyRepo = new ResearchersBiographyRepo();
            int id = int.Parse(Request.QueryString["id"]);
            ///////////////////////////////////////////////////////////////
            List<BaseRecord<Award>> awards = researcherBiographyRepo.GetAwards(id);
            return Json(new
            {
                success = true,
                data = (from a in awards
                        select new
                        {
                            id = a.records.award_id,
                            a.index,
                            a.records.competion_name,
                            a.records.rank,
                            award_time = a.records.award_time != null ? a.records.award_time.Value.ToString("dd/MM/yyyy") : ""
                        })
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
