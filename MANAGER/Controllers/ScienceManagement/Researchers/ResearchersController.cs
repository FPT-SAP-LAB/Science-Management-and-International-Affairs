using BLL.ScienceManagement.Researcher;
using BLL.ScienceManagement.ResearcherListRepo;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
namespace MANAGER.Controllers.ScienceManagement.Researchers
{
    public class ResearchersController : Controller
    {
        ResearchersListRepo researcherListRepo;
        ResearchersDetailRepo researcherDetailRepo;
        ResearchersBiographyRepo researcherBiographyRepo;
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
            List<AcadBiography> acadList = researcherBiographyRepo.GetBio(id);
            ViewBag.acadList = acadList;
            /////////////////////////////////////////////////////////////
            List<BaseRecord<WorkingProcess>> workList = researcherBiographyRepo.GetHistory(id);
            ViewBag.workList = workList;
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
            return View();
        }
        public ActionResult AddResearcher()
        {
            return View();
        }
    }
}