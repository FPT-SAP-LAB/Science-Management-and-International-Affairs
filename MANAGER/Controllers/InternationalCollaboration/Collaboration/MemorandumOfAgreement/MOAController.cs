using BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement;
using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA;
using MANAGER.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOARepo;

namespace MANAGER.Controllers.InternationalCollaboration.MOA
{
    public class MOAController : Controller
    {
        // GET: Partner
        private static MOARepo moa = new MOARepo();
        private static BasicInfoMOARepo moa_detail = new BasicInfoMOARepo();
        private static PartnerMOARepo moa_partner = new PartnerMOARepo();
        //[Auther(RightID = "6")]
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
                ViewBag.partnerList = moa_detail.getPartnerExMOA(int.Parse(moa_id));
                ViewBag.newExMOACode = moa_detail.getNewExMOACode(int.Parse(moa_id));

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
                    List<ListMOA> listMOA = moa.listAllMOA(partner_name, moa_code, mou_id);
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
                    List<CustomScopesMOA> scopeList = moa.getMOAScope(int.Parse(mou_id), partner_id);
                    return Json(scopeList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult Delete_Moa(int moa_id)
        {
            try
            {
                moa.deleteMOA(moa_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult Add_Moa(MOAAdd input)
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
                    moa.addMOA(input, int.Parse(mou_id));
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}