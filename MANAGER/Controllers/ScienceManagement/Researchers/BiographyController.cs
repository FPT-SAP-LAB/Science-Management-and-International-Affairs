using BLL.ScienceManagement.Researcher;
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
            }catch(Exception e)
            {
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
                return Json(new { success = false });
            }
        }
    }
}