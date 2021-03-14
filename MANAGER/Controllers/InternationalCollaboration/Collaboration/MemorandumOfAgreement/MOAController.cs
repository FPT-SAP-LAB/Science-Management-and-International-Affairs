using BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement;
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
        public ActionResult Detail_MOA(string id)
        {
            ViewBag.pageTitle = "CHI TIẾT BIÊN BẢN THỎA THUẬN";
            //string id = Session["moa_detail_id"].ToString();

            //List<ENTITIES.Partner> partnerList = mou_detail.getPartnerExMOU(int.Parse(id));
            //List<CollaborationScope> scopeList = mou_detail.GetScopesExMOU(int.Parse(id));
            //ViewBag.scopeList = scopeList;
            //ViewBag.partnerList = partnerList;

            ////MOA Partner
            //ViewBag.listSpeMOUPartner = mou_partner.getPartnerMOUSpe();
            //ViewBag.listScopesMOUPartner = mou_partner.getPartnerMOUScope();
            //ViewBag.listPartnerMOUPartner = mou_partner.GetPartners(int.Parse(id));
            return View();
        }
        public ActionResult ViewMOA(string partner_name, string moa_code)
        {
            try
            {
                string mou_id = Session["mou_detail_id"].ToString();
                List<ListMOA> listMOA = moa.listAllMOA(partner_name, moa_code, mou_id);
                return Json(new { success = true, data = listMOA }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        //public ActionResult Delete_Moa(int mou_id)
        //{
        //    try
        //    {
        //        mou.deleteMOU(mou_id);
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return new HttpStatusCodeResult(400);
        //    }
        //}
        //public ActionResult Add_Moa(MOUAdd input)
        //{
        //    try
        //    {
        //        mou.addMOU(input);
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}