using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;

using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOUPartner;
using MANAGER.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.PartnerMOURepo;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class PartnerMOUController : Controller
    {
        private static PartnerMOURepo mou = new PartnerMOURepo();
        [Auther(RightID = "6")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get_MOU_History(string mou_partner_id)
        {
            try
            {
                List<PartnerHistory> historyList = mou.listMOUPartnerHistory(int.Parse(mou_partner_id));
                return Json(new { success = true, data = historyList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Get_Data_Partner_Detail(string mou_partner_id)
        {
            try
            {
                string result = mou_partner_id;
                ListMOUPartner mouObj = mou.getMOUPartnerDetail(int.Parse(mou_partner_id));
                return Json(mouObj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewListMOUPartner(string partner_name, string nation, string specialization)
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                string nation_name = nation is null ? "" : nation;
                string specialization_name = specialization is null ? "" : specialization;
                List<ListMOUPartner> mouList = mou.listAllMOUPartner(partner_name, nation_name, specialization_name, int.Parse(id));
                return Json(new { success = true, data = mouList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Add_Mou_Partner(PartnerInfo input)
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                mou.addMOUPartner(input, int.Parse(id));
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Edit_Mou_Partner(PartnerInfo input)
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                mou.addMOUPartner(input, int.Parse(id));
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deletePartnerMOU(int mou_bonus_id)
        {
            try
            {
                string id = Session["mou_detail_id"].ToString();
                mou.deleteMOUPartner(int.Parse(id), mou_bonus_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
    }
}