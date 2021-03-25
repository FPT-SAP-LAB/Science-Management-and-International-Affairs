using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Authen;
using BLL.ScienceManagement.ResearcherListRepo;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
namespace MANAGER.Controllers.ScienceManagement.Researchers
{
    public class ResearchersController : Controller
    {
        ResearchersListRepo researcherListRepo;
        // GET: Researchers

        public ActionResult List()
        {
            return View();
        }
        public JsonResult GetList()
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
        public ActionResult ViewInfo()
        {
            return View();
        }
        public ActionResult Biography()
        {
            return View();
        }
        public ActionResult Publications()
        {
            return View();
        }
        public ActionResult Rewards()
        {
            return View();
        }
        public ActionResult AddResearcher()
        {
            return View();
        }
    }
}