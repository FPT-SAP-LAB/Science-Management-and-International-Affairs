using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using GUEST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.ConferenceSponsor
{
    public class QsUniversityController : Controller
    {
        private readonly QsUniversityRepo qsUniversityRepo = new QsUniversityRepo();
        // GET: QsUniversity
        public ActionResult Index()
        {
            return View();
        }

        [AjaxOnly]
        public JsonResult List()
        {
            BaseDatatable datatable = new BaseDatatable(Request);
            BaseServerSideData<QsUniversity> output = qsUniversityRepo.List(datatable);
            return Json(new { success = true, data = output.Data, draw = Request["draw"], recordsTotal = output.RecordsTotal, recordsFiltered = output.RecordsTotal }, JsonRequestBehavior.AllowGet);
        }
    }
}