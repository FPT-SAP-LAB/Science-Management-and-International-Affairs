using BLL.Authen;
using BLL.ScienceManagement.MasterData;
using ENTITIES;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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
        public JsonResult addItem(string criteria, HttpPostedFileBase file)
        {
            JObject @object = JObject.Parse(criteria);
            List<PaperCriteria> list = new List<PaperCriteria>();
            foreach (var item in @object["arr_cri"])
            {
                PaperCriteria temp = new PaperCriteria()
                {
                    name = (string)item["name"]
                };
                list.Add(temp);
            }

            LoginRepo.User u = new LoginRepo.User();
            Account acc = new Account();
            if (Session["User"] != null)
            {
                u = (LoginRepo.User)Session["User"];
                acc = u.account;
            }

            bool mess = md.addNewPolicy(file, list, acc);

            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deleteItem(string cri_id)
        {
            string mess = md.DeletePaperCriteria(cri_id);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }
    }
}