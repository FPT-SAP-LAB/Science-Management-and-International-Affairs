using BLL.ScienceManagement.DecisionHistory;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.Decision
{
    public class DecisionHistoryController : Controller
    {
        DecisionRepo dr = new DecisionRepo();
        // GET: Decision
        public ActionResult History()
        {
            List<ENTITIES.Decision> list = dr.GetListDecision("");
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public ActionResult List(string search)
        {
            if (search == null) search = "";
            try
            {
                List<ENTITIES.Decision> list = dr.GetListDecision(search);
                return Json(new { list, mess = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { mess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult deleteItem(int cri_id)
        {
            string mess = dr.DeleteDecision(cri_id);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }
    }
}