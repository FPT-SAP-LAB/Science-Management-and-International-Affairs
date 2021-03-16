using BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOARepo;
using static BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement.PartnerMOARepo;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfAgreement
{
    public class PartnerMOAController : Controller
    {
        // GET: PartnerMOA
        private static PartnerMOARepo moa = new PartnerMOARepo();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewListMOAPartner(string partner_name, string nation, string specialization)
        {
            try
            {
                string id = Session["moa_detail_id"].ToString();
                string nation_name = nation is null ? "" : nation;
                string specialization_name = specialization is null ? "" : specialization;
                List<ListMOAPartner> moaList = moa.listAllMOAPartner(partner_name, nation_name, specialization_name, int.Parse(id));
                return Json(new { success = true, data = moaList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Get_MOA_History(string moa_partner_id)
        {
            try
            {
                List<PartnerHistory> historyList = moa.listMOUPartnerHistory(int.Parse(moa_partner_id));
                return Json(new { success = true, data = historyList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deletePartnerMOA(int moa_partner_id)
        {
            try
            {
                moa.deleteMOAPartner(moa_partner_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult Add_Moa_Partner(MOAPartnerInfo input)
        {
            try
            {
                string id = Session["moa_detail_id"].ToString();
                moa.addMOAPartner(input,int.Parse(id));
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Edit_Moa_Partner(MOAPartnerEdited input)
        {
            try
            {
                string id = Session["moa_detail_id"].ToString();
                moa.editMOAPartner(input, int.Parse(id));
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Get_Partner_Detail(int moa_partner_id)
        {
            try
            {
                CustomPartnerMOA obj = moa.getPartnerDetail(moa_partner_id);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}