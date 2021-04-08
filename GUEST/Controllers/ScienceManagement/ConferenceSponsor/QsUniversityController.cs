using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using GUEST.Models;
using System.Web.Mvc;

namespace GUEST.Controllers.ScienceManagement.ConferenceSponsor
{
    public class QsUniversityController : Controller
    {
        private readonly QsUniversityRepo qsUniversityRepo = new QsUniversityRepo();
        [AjaxOnly]
        public JsonResult List(string university)
        {
            BaseDatatable datatable = new BaseDatatable()
            {
                Length = 10,
                Start = 0
            };
            BaseServerSideData<QsUniversity> output = qsUniversityRepo.List(datatable, university);
            return Json(output.Data, JsonRequestBehavior.AllowGet);
        }
    }
}