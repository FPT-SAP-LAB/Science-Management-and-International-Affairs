using BLL.ScienceManagement.Paper;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.Paper
{
    public class ConditionPaperManagementController : Controller
    {
        PaperRepo pr = new PaperRepo();
        // GET: ConditionPaperManagement
        public ActionResult List()
        {
            List<CustomPaperPolicy> listPolicy = pr.getPolicy();
            ViewBag.policy = listPolicy;

            string link = pr.getLinkPolicy();
            ViewBag.link = link;

            return View();
        }

        [HttpPost]
        public JsonResult newPolicy(HttpPostedFileBase file, string criteria)
        {
            ENTITIES.File fl = new ENTITIES.File();
            try
            {
                JObject @object_criteria = JObject.Parse(criteria);
                List<CustomPaperPolicy> policy = new List<CustomPaperPolicy>();
                foreach (var item in @object_criteria["listCriteria"])
                {
                    CustomPaperPolicy temp = new CustomPaperPolicy()
                    {
                        tv = (string)item["name_tv"],
                        ta = (string)item["name_ta"]
                    };
                    policy.Add(temp);
                }

                Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadPolicyFile(file);
                fl.file_drive_id = f.Id;
                fl.link = f.WebViewLink;

                pr.addPolicy(fl, policy);

                return Json(new { mess = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                GoogleDriveService.DeleteFile(fl.file_drive_id);
                return Json(new { mess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult getPolicy(string id)
        {
            CustomPaperPolicy item = pr.getOnePolicy(id);
            bool mess = true;
            if (item == null) mess = false;
            return Json(new { item = item, mess = mess }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editPolicy(string id, string tv, string ta)
        {
            bool mess = pr.editPolicy(id, tv, ta);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }
    }
}