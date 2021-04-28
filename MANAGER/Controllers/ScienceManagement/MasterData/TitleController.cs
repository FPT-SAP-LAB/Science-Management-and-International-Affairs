using BLL.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.MasterData
{
    public class TitleController : Controller
    {
        private readonly MasterDataRepo md = new MasterDataRepo();
        // GET: Title
        public ActionResult List()
        {
            List<Title2Name> list = md.GetListTitle_2Lang();
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public JsonResult getItem(int cri_id)
        {
            Title2Name pc = md.GetTitleWithName(cri_id);
            return Json(new { success = "ss", pc }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult updateItem(int cri_id, string tv, string ta)
        {
            string mess = md.UpdateTitle(cri_id, tv, ta);
            return Json(new { success = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult addItem(string tv, string ta)
        {
            int id = md.AddTitle(tv, ta);
            string mess = "ss";
            if (id == 0) mess = "ff";
            return Json(new { mess, id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteItem(int cri_id)
        {
            string mess = md.DeleteTitle(cri_id);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }
    }
}