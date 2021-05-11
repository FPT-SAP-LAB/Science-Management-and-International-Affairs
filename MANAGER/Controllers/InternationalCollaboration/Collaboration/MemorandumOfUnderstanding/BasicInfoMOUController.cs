using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOUBasicInfo;
using MANAGER.Support;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class BasicInfoMOUController : Controller
    {
        private static BasicInfoMOURepo mou = new BasicInfoMOURepo();
        [Auther(RightID = "6")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PassDataToMOUDetail(int id)
        {
            try
            {
                Session["mou_detail_id"] = id;
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
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    string id = Session["mou_detail_id"].ToString();
                    MOUBasicInfo data = mou.GetBasicInfoMOU(int.Parse(id));
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult ViewExMOU()
        {
            try
            {
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    string id = Session["mou_detail_id"].ToString();
                    List<ExtraMOU> listExMOU = mou.ListAllExtraMOU(int.Parse(id));
                    return Json(new { success = true, data = listExMOU }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult getOffice()
        {
            try
            {
                List<Office> listOffice = mou.GetOffice();
                return Json(listOffice);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "6")]
        public ActionResult editMOUBasicInfo(string input, int old_file_number, int new_file_number, HttpPostedFileBase evidence)
        {
            try
            {
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    //MOUBasicInfo input;
                    JObject inputObj = JObject.Parse(input);
                    MOUBasicInfo obj = inputObj.ToObject<MOUBasicInfo>();
                    string id = Session["mou_detail_id"].ToString();
                    mou.EditMOUBasicInfo(int.Parse(id), obj, evidence, old_file_number, new_file_number);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "6")]
        public ActionResult deleteExMOU(int mou_bonus_id)
        {
            try
            {
                mou.DeleteExtraMOU(mou_bonus_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult ViewExMOUDetail(int mou_bonus_id)
        {
            try
            {
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    string mou_id = Session["mou_detail_id"].ToString();
                    ExMOUAdd mouObj = mou.GetExtraMOUDetail(mou_bonus_id, int.Parse(mou_id));
                    return Json(mouObj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        //chưa có class diagram
        public ActionResult getNewExtraMOUCode()
        {
            try
            {
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    string id = Session["mou_detail_id"].ToString();
                    string ExMOUCode = mou.GetNewExtraMOUCode(int.Parse(id));
                    return Json(ExMOUCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "6")]
        public ActionResult Add_Ex_Mou(string input, HttpPostedFileBase evidence)
        {
            try
            {
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    JObject inputObj = JObject.Parse(input);
                    ExMOUAdd obj = inputObj.ToObject<ExMOUAdd>();
                    //ExMOUAdd input
                    BLL.Authen.LoginRepo.User user = (BLL.Authen.LoginRepo.User)Session["User"];
                    string id = Session["mou_detail_id"].ToString();
                    mou.AddExtraMOU(obj, int.Parse(id), user, evidence);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [Auther(RightID = "6")]
        public ActionResult Edit_Ex_Mou(string input, int old_file_number_ex_mou, int new_file_number_ex_mou, HttpPostedFileBase evidence)
        {
            try
            {
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    JObject inputObj = JObject.Parse(input);
                    ExMOUAdd obj = inputObj.ToObject<ExMOUAdd>();
                    BLL.Authen.LoginRepo.User user = (BLL.Authen.LoginRepo.User)Session["User"];
                    string id = Session["mou_detail_id"].ToString();
                    mou.EditExtraMOU(obj, user, old_file_number_ex_mou, new_file_number_ex_mou, evidence, int.Parse(id));
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getFullScope()
        {
            try
            {
                List<CollaborationScope> listScopes = mou.GetFullScopesExMOU();
                return Json(listScopes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
    }
}