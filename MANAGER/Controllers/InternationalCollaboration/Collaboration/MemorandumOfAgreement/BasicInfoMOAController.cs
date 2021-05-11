using BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement;
using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOABasicInfo;
using MANAGER.Support;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfAgreement
{
    public class BasicInfoMOAController : Controller
    {
        // GET: BasicInfoMOA
        private static BasicInfoMOARepo moa = new BasicInfoMOARepo();
        [Auther(RightID = "7")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PassDataToMOADetail(int id)
        {
            try
            {
                Session["moa_detail_id"] = id;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult getBasicInfo()
        {
            try
            {
                if (Session["moa_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    string id = Session["moa_detail_id"].ToString();
                    MOABasicInfo data = moa.GetBasicInfoMOA(int.Parse(id));
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult ViewExMOA()
        {
            try
            {
                if (Session["moa_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    string id = Session["moa_detail_id"].ToString();
                    List<ExtraMOA> listExMOA = moa.ListAllExtraMOA(int.Parse(id));
                    return Json(new { success = true, data = listExMOA }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "7")]
        public ActionResult editMOABasicInfo(string input, int old_file_number, int new_file_number, HttpPostedFileBase evidence)
        {
            try
            {
                if (Session["moa_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    //MOABasicInfo input
                    string id = Session["moa_detail_id"].ToString();
                    JObject inputObj = JObject.Parse(input);
                    MOABasicInfo obj = inputObj.ToObject<MOABasicInfo>();
                    moa.EditMOABasicInfo(int.Parse(id), obj, evidence, old_file_number, new_file_number);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "7")]
        public ActionResult deleteExMOA(int moa_bonus_id)
        {
            try
            {
                moa.DeleteExtraMOA(moa_bonus_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult ViewExMOADetail(int moa_bonus_id)
        {
            try
            {
                if (Session["moa_detail_id"] is null || Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    string moa_id = Session["moa_detail_id"].ToString();
                    string mou_id = Session["mou_detail_id"].ToString();
                    ExMOAAdd mouObj = moa.GetExtraMOADetail(int.Parse(moa_id), moa_bonus_id, int.Parse(mou_id));
                    return Json(mouObj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "7")]
        public ActionResult Add_Ex_Moa(string input, HttpPostedFileBase evidence)
        {
            try
            {
                if (Session["moa_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    JObject inputObj = JObject.Parse(input);
                    ExMOAAdd obj = inputObj.ToObject<ExMOAAdd>();
                    BLL.Authen.LoginRepo.User user = (BLL.Authen.LoginRepo.User)Session["User"];
                    string id = Session["moa_detail_id"].ToString();
                    moa.AddExtraMOA(obj, int.Parse(id), user, evidence);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [Auther(RightID = "7")]
        public ActionResult Edit_Ex_Moa(string input, int old_file_number_ex_moa, int new_file_number_ex_moa, HttpPostedFileBase evidence)
        {
            try
            {
                if (Session["moa_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    JObject inputObj = JObject.Parse(input);
                    ExMOAAdd obj = inputObj.ToObject<ExMOAAdd>();
                    BLL.Authen.LoginRepo.User user = (BLL.Authen.LoginRepo.User)Session["User"];
                    string id = Session["moa_detail_id"].ToString();
                    moa.EditExtraMOA(obj, user, old_file_number_ex_moa, new_file_number_ex_moa, evidence, int.Parse(id));
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getNewExMOAScopesList(int partner_id)
        {
            try
            {
                if (Session["mou_detail_id"] is null || Session["moa_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    string moa_id = Session["moa_detail_id"].ToString();
                    string mou_id = Session["mou_detail_id"].ToString();
                    List<CollaborationScope> data = moa.GetScopesExMOA(int.Parse(moa_id), int.Parse(mou_id), partner_id);
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
    }
}