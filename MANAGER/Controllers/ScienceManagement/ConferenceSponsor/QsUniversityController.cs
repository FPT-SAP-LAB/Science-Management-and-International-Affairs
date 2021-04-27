using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using MANAGER.Models;
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
        public JsonResult List(string university)
        {
            BaseDatatable datatable = new BaseDatatable(Request);
            BaseServerSideData<QsUniversity> output = qsUniversityRepo.List(datatable, university);
            return Json(new ResultDatatable<QsUniversity>(output, Request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(HttpPostedFileBase ListUniversity)
        {
            AlertModal<string> result = qsUniversityRepo.Add(ListUniversity);
            return Json(result);
        }
    }
}