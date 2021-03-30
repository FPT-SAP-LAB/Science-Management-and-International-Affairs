using BLL.ScienceManagement.Researcher;
using BLL.ScienceManagement.ResearcherListRepo;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using MANAGER.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
namespace MANAGER.Controllers.ScienceManagement.Researchers
{
    public class ResearchersController : Controller
    {
        ResearchersListRepo researcherListRepo;
        ResearchersDetailRepo researcherDetailRepo;
        ResearchersBiographyRepo researcherBiographyRepo;
        EditResearcherInfoRepo researcherEditResearcherInfo;
        // GET: Researchers

        public ActionResult List()
        {
            return View();
        }
        public JsonResult GetList()
        {
            try
            {
                researcherListRepo = new ResearchersListRepo();
                BaseDatatable datatable = new BaseDatatable(Request);
                BaseServerSideData<ResearcherList> output = researcherListRepo.GetList(datatable);
                for (int i = 0; i < output.Data.Count; i++)
                {
                    output.Data[i].rowNum = datatable.Start + 1 + i;
                }
                return Json(new { success = true, data = output.Data, draw = Request["draw"], recordsTotal = output.RecordsTotal, recordsFiltered = output.RecordsTotal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }
        public ActionResult ViewInfo()
        {
            researcherDetailRepo = new ResearchersDetailRepo();
            int id = Int32.Parse(Request.QueryString["id"]);
            ResearcherDetail profile = researcherDetailRepo.GetProfile(id);
            ViewBag.profile = profile;
            return View();
        }

        public ActionResult Biography()
        {
            researcherDetailRepo = new ResearchersDetailRepo();
            researcherBiographyRepo = new ResearchersBiographyRepo();
            int id = Int32.Parse(Request.QueryString["id"]);
            ResearcherDetail profile = researcherDetailRepo.GetProfile(id);
            ViewBag.profile = profile;
            ///////////////////////////////////////////////////////////////
            List<AcadBiography> acadList = researcherBiographyRepo.GetAcadHistory(id);
            ViewBag.acadList = acadList;
            /////////////////////////////////////////////////////////////
            List<SelectField> listAcadDegree = researcherBiographyRepo.getAcadDegrees();
            List<BaseRecord<WorkingProcess>> workList = researcherBiographyRepo.GetWorkHistory(id);
            List<SelectField> listWorkHistory = researcherBiographyRepo.getTitles();
            ViewBag.workList = workList;
            ViewBag.listAcadDegree = listAcadDegree;
            ViewBag.listWorkHistory = listWorkHistory;
            return View();
        }
        public ActionResult Publications()
        {
            researcherDetailRepo = new ResearchersDetailRepo();
            researcherBiographyRepo = new ResearchersBiographyRepo();
            int id = Int32.Parse(Request.QueryString["id"]);
            ResearcherDetail profile = researcherDetailRepo.GetProfile(id);
            ViewBag.profile = profile;
            ///////////////////////////////////////////////////////////////
            List<ResearcherPublications> publications = researcherBiographyRepo.GetPublications(id);
            ViewBag.publications = publications;
            ///////////////////////////////////////////////////////////////
            List<ResearcherPublications> conferences = researcherBiographyRepo.GetConferencePublic(id);
            ViewBag.conferences = conferences;
            return View();
        }
        public ActionResult Rewards()
        {
            researcherDetailRepo = new ResearchersDetailRepo();
            researcherBiographyRepo = new ResearchersBiographyRepo();
            int id = Int32.Parse(Request.QueryString["id"]);
            ResearcherDetail profile = researcherDetailRepo.GetProfile(id);
            ViewBag.profile = profile;
            ///////////////////////////////////////////////////////////////
            List<BaseRecord<Award>> awards = researcherBiographyRepo.GetAwards(id);
            ViewBag.awards = awards;
            return View();
        }

        public ActionResult EditProfilePhoto()
        {
            researcherEditResearcherInfo = new EditResearcherInfoRepo();
            var uploadfile = Request.Files["imageInput"];
            int people_id = Int32.Parse(Request.Form["people_id"]);
            Account account = CurrentAccount.Account(Session);
            var file = GoogleDriveService.UploadProfileMedia(uploadfile, account.email);
            int res = researcherEditResearcherInfo.EditResearcherProfilePicture(file, people_id);
            return Json(new { res = res });
        }
        public ActionResult EditResearcher()
        {
            researcherEditResearcherInfo = new EditResearcherInfoRepo();
            string data = Request["info"];
            researcherEditResearcherInfo.EditResearcherProfile(data);
            return null;
        }
        public ActionResult AddResearcher()
        {
            return View();
        }
    }
}