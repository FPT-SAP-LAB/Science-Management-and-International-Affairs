using BLL.ScienceManagement.MasterData;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.MasterData
{
    public class PaperCriteriaController : Controller
    {
        MasterDataRepo md = new MasterDataRepo();
        // GET: PaperCriteria
        public ActionResult List()
        {
            List<PaperCriteria> list = md.getPaperCriteria();
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public JsonResult getItem(string cri_id)
        {
            string pc = md.GetPaperCriteria(cri_id);
            return Json(new { success = "ss", pc = pc }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult updateItem(string cri_id, string name)
        {
            string mess = md.UpdatePaperCriteria(cri_id, name);
            return Json(new { success = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult addItem(string name)
        {
            int id = md.AddPaperCriteria(name);
            string mess = "ss";
            if (id == 0) mess = "ff";
            return Json(new { mess = mess, id = id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteItem(string cri_id)
        {
            string mess = md.DeletePaperCriteria(cri_id);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }
    }
}