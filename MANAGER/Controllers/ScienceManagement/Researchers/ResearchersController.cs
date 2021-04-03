using BLL.ScienceManagement.Researcher;
using BLL.ScienceManagement.ResearcherListRepo;
using ENTITIES;
using ENTITIES.CustomModels;
using BLL.ModelDAL;
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
        ResearcherCandidateRepo researcherCandidate;
        // GET: Researchers

        public ActionResult List()
        {
            TitleLanguageRepo ra = new TitleLanguageRepo();
            OfficeRepo o = new OfficeRepo();
            int lang_id = 1;
            List<TitleLanguage> listTitleLanguage = ra.GetList(lang_id);
            List<Office> listOffices = o.GetList();
            ViewBag.listTitleLanguage = listTitleLanguage;
            ViewBag.listOffices = listOffices;
            return View();
        }
        public JsonResult GetList()
        {
            try
            {
                string coso = (Request["coso"]);
                string name = (Request["name"]);
                researcherListRepo = new ResearchersListRepo();
                BaseDatatable datatable = new BaseDatatable(Request);
                BaseServerSideData<ResearcherList> output = researcherListRepo.GetList(datatable, coso, name);
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
        public JsonResult GetListCandidate()
        {
            try
            {
                string name = (Request["name"]);
                string coso = (Request["coso"]);
                string chucdanh = (Request["chucdanh"]);
                researcherCandidate = new ResearcherCandidateRepo();
                BaseDatatable datatable = new BaseDatatable(Request);
                BaseServerSideData<ResearcherCandidate> output = researcherCandidate.GetList(datatable, name, chucdanh, coso);
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
            if (profile.profile_page_active==false)
            {
                ViewBag.profile_page_active = false;
            }
            else
            {
                ViewBag.profile_page_active = true;
                ViewBag.profile = profile;
            }
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
            researcherDetailRepo = new ResearchersDetailRepo();
            int id = Int32.Parse(Request.QueryString["id"]);
            ResearcherDetail profile = researcherDetailRepo.GetProfile(id);
            ViewBag.profile = profile;
            if (profile.profile_page_active) {
                ViewBag.profile_page_active = true;
                return View();
            }
            else
            {
                ViewBag.profile_page_active = false;
            }
            ///////////////////////////////////////////////////////////////////
            researcherBiographyRepo = new ResearchersBiographyRepo();
            ///////////////////////////////////////////////////////////////
            List<AcadBiography> acadList = researcherBiographyRepo.GetAcadHistory(id);
            ViewBag.acadList = acadList;
            /////////////////////////////////////////////////////////////
            List<SelectField> listAcadDegree = researcherBiographyRepo.getAcadDegrees();
            List<BaseRecord<WorkingProcess>> workList = researcherBiographyRepo.GetWorkHistory(id);
            List<SelectField> listWorkHistory = researcherBiographyRepo.getTitles();
            ///////////////////////////////////////////////////////////////
            List<ResearcherPublications> publications = researcherBiographyRepo.GetPublications(id);
            ViewBag.publications = publications;
            ///////////////////////////////////////////////////////////////
            List<ResearcherPublications> conferences = researcherBiographyRepo.GetConferencePublic(id);
            ///////////////////////////////////////////////////////////////
            List<BaseRecord<Award>> awards = researcherBiographyRepo.GetAwards(id);
            ViewBag.awards = awards;
            ViewBag.conferences = conferences;
            ViewBag.workList = workList;
            ViewBag.listAcadDegree = listAcadDegree;
            ViewBag.listWorkHistory = listWorkHistory;
            return View();
        }
        public JsonResult UpdateProfilePage()
        {
            string data = Request["status"];
            int id = Int32.Parse(Request["id"]);
            bool status=data=="1"?true:false;
            researcherCandidate = new ResearcherCandidateRepo();
            bool update = researcherCandidate.UpdateProfilePage(id, status);
            return Json(new { success=update});
        }
    }
}