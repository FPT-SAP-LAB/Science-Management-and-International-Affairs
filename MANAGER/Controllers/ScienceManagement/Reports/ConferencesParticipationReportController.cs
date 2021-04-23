using BLL.ModelDAL;
using BLL.ScienceManagement.Report;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.Report;
using System;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.Reports
{
    public class ConferencesParticipationReportController : Controller
    {
        // GET: ConferencesParticipationReport
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult List()
        {
            ConferencesParticipationList repo = new ConferencesParticipationList();
            BaseDatatable datatable = new BaseDatatable(Request);
            BaseServerSideData<ConferencesParticipationReport> output = repo.ListConferences(datatable);
            for (int i = 0; i < output.Data.Count; i++)
            {
                output.Data[i].RowNumber = datatable.Start + 1 + i;
                output.Data[i].CreatedDate = output.Data[i].valid_date.ToString("dd/MM/yyyy");
            }
            return Json(new { success = true, data = output.Data, draw = Request["draw"], recordsTotal = output.RecordsTotal, recordsFiltered = output.RecordsTotal }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPeopleByName(String name)
        {
            PeopleRepo repo = new PeopleRepo();
            return Json(repo.GetPeopleName(name, 10), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeName(String name)
        {
            OfficeRepo repo = new OfficeRepo();
            return Json(repo.GetOfficeName(name, 10), JsonRequestBehavior.AllowGet);
        }
    }
}