using BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA;
using MANAGER.Support;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.MOA
{
    public class MOAController : Controller
    {
        // GET: Partner
        private static MOARepo moa = new MOARepo();
        private static BasicInfoMOARepo moa_detail = new BasicInfoMOARepo();
        private static PartnerMOARepo moa_partner = new PartnerMOARepo();
        [Auther(RightID = "7")]
        public ActionResult Detail_MOA()
        {
            ViewBag.pageTitle = "CHI TIẾT BIÊN BẢN THỎA THUẬN";
            if (Session["mou_detail_id"] is null || Session["moa_detail_id"] is null)
            {
                return Redirect("../MOU/List");
            }
            else
            {
                string moa_id = Session["moa_detail_id"].ToString();
                string mou_id = Session["mou_detail_id"].ToString();
                //ViewBag.scopeList = moa_detail.GetScopesExMOA(int.Parse(moa_id), int.Parse(mou_id));
                ViewBag.partnerList = moa_detail.GetPartnerExMOA(int.Parse(moa_id));
                ViewBag.newExMOACode = moa_detail.GetNewExMOACode(int.Parse(moa_id));

                ////MOA Partner
                ViewBag.listScopesMOAPartner = moa_partner.getPartnerMOAScope(int.Parse(moa_id), int.Parse(mou_id));
                ViewBag.listPartnerMOAPartner = moa_partner.getPartnerMOA(int.Parse(mou_id));
                return View();
            }
        }
        public ActionResult ViewMOA(string partner_name, string moa_code)
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
                    List<ListMOA> listMOA = moa.ListAllMOA(partner_name, moa_code, mou_id);
                    return Json(new { success = true, data = listMOA }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult CheckPartner(int partner_id)
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
                    CustomPartnerMOA partner = moa.CheckPartner(int.Parse(mou_id), partner_id);
                    return partner is null ? Json("") : Json(partner);
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult GetMOAScopesByPartner(int partner_id)
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
                    List<CustomScopesMOA> scopeList = moa.GetMOAScope(int.Parse(mou_id), partner_id);
                    return Json(scopeList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "7")]
        public ActionResult Delete_Moa(int moa_id)
        {
            try
            {
                moa.DeleteMOA(moa_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "7")]
        public ActionResult Add_Moa(string input, HttpPostedFileBase evidence)
        {
            try
            {
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    //MOAAdd input
                    JObject inputObj = JObject.Parse(input);
                    MOAAdd obj = inputObj.ToObject<MOAAdd>();
                    BLL.Authen.LoginRepo.User user = (BLL.Authen.LoginRepo.User)Session["User"];
                    string mou_id = Session["mou_detail_id"].ToString();

                    moa.AddMOA(obj, int.Parse(mou_id), user, evidence);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckDuplicatedMOACode(string moa_code)
        {
            try
            {
                bool isDup = moa.GetMOACodeCheck(moa_code);
                return Json(isDup);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult checkDuplicatePartnersMOA(List<MOAPartnerInfo> MOAPartnerInfo)
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
                    bool isDup = moa.CheckDuplicatePartnersMOA(MOAPartnerInfo, int.Parse(mou_id));
                    return Json(isDup);
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