using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOUBasicInfo;
using MANAGER.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.BasicInfoMOURepo;

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
                    MOUBasicInfo data = mou.getBasicInfoMOU(int.Parse(id));
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
                    List<ExtraMOU> listExMOU = mou.listAllExtraMOU(int.Parse(id));
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
        public ActionResult editMOUBasicInfo(MOUBasicInfo input)
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
                    mou.editMOUBasicInfo(int.Parse(id), input);
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
                mou.deleteExtraMOU(mou_bonus_id);
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
                    ExMOUAdd mouObj = mou.getExtraMOUDetail(mou_bonus_id, int.Parse(mou_id));
                    return Json(mouObj, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
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
                    string ExMOUCode = mou.getNewExtraMOUCode(int.Parse(id));
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
        public ActionResult Add_Ex_Mou(ExMOUAdd input)
        {
            try
            {
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    BLL.Authen.LoginRepo.User user = (BLL.Authen.LoginRepo.User)Session["User"];
                    string id = Session["mou_detail_id"].ToString();
                    mou.addExtraMOU(input, int.Parse(id), user);
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
        public ActionResult Edit_Ex_Mou(ExMOUAdd input)
        {
            try
            {
                BLL.Authen.LoginRepo.User user = (BLL.Authen.LoginRepo.User)Session["User"];
                mou.editExtraMOU(input, user);
                return Json("", JsonRequestBehavior.AllowGet);
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