using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
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
        // GET: BasicInfoMOU
        private static BasicInfoMOURepo mou = new BasicInfoMOURepo();

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
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult getBasicInfo()
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                MOUBasicInfo data = mou.getBasicInfoMOU(int.Parse(id));
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult ViewExMOU()
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                List<ExtraMOU> listExMOU = mou.listAllExtraMOU(int.Parse(id));
                return Json(new { success = true, data = listExMOU }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
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
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult editMOUBasicInfo(MOUBasicInfo input)
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                mou.editMOUBasicInfo(int.Parse(id), input);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult deleteExMOU(int mou_bonus_id)
        {
            try
            {
                mou.deleteExtraMOU(mou_bonus_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult ViewExMOUDetail(int mou_bonus_id)
        {
            try
            {
                string mou_id = Session["mou_detail_id"].ToString();
                ExMOUAdd mouObj = mou.getExtraMOUDetail(mou_bonus_id, int.Parse(mou_id));
                return Json(mouObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult getNewExtraMOUCode()
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                string ExMOUCode = mou.getNewExtraMOUCode(int.Parse(id));
                return Json(ExMOUCode);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult Add_Ex_Mou(ExMOUAdd input)
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                mou.addExtraMOU(input, int.Parse(id));
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Edit_Ex_Mou(ExMOUAdd input)
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                mou.editExtraMOU(input, int.Parse(id));
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}