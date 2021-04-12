using BLL.ScienceManagement.Researcher;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.Researchers
{
    public class BiographyController : Controller
    {
        ResearchersBiographyRepo researcherBiographyRepo;
        public ActionResult AddNewAcadEvent()
        {
            try
            {
                researcherBiographyRepo = new ResearchersBiographyRepo();
                string data = Request["data"];
                researcherBiographyRepo.AddNewAcadEvent(data);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }
        public ActionResult AddWorkEvent()
        {
            try
            {
                researcherBiographyRepo = new ResearchersBiographyRepo();
                string data = Request["data"];
                researcherBiographyRepo.AddNewWorkEvent(data);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }
        public ActionResult EditWorkEvent()
        {
            try
            {
                researcherBiographyRepo = new ResearchersBiographyRepo();
                string data = Request["data"];
                researcherBiographyRepo.EditWorkEvent(data);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }
        public ActionResult EditAcadEvent()
        {
            try
            {
                researcherBiographyRepo = new ResearchersBiographyRepo();
                string data = Request["data"];
                researcherBiographyRepo.EditAcadEvent(data);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }
        public ActionResult DeleteAcadEvent()
        {
            try
            {
                researcherBiographyRepo = new ResearchersBiographyRepo();
                string data = Request["data"];
                researcherBiographyRepo.DeleteAcadEvent(data);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }
        public ActionResult DeleteWorkEvent()
        {
            try
            {
                researcherBiographyRepo = new ResearchersBiographyRepo();
                string data = Request["data"];
                researcherBiographyRepo.DeleteWorkEvent(data);
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }
    }
}