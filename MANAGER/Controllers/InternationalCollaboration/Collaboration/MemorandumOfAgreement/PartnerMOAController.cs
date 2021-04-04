using BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement;
using ENTITIES;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOA;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfAgreement.MOAPartner;
using MANAGER.Support;
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
        [Auther(RightID = "7")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewListMOAPartner(string partner_name, string nation, string specialization)
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
                    string nation_name = nation is null ? "" : nation;
                    string specialization_name = specialization is null ? "" : specialization;
                    List<ListMOAPartner> moaList = moa.listAllMOAPartner(partner_name, nation_name, specialization_name, int.Parse(id));
                    return Json(new { success = true, data = moaList }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Get_MOA_History(string moa_partner_id)
        {
            try
            {
                List<PartnerHistory> historyList = moa.listMOAPartnerHistory(int.Parse(moa_partner_id));
                return Json(new { success = true, data = historyList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult deletePartnerMOA(int moa_partner_id)
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
                    moa.deleteMOAPartner(int.Parse(id), moa_partner_id);
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult Add_Moa_Partner(MOAPartnerInfo input)
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
                    moa.addMOAPartner(input, int.Parse(id));
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
                //return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Edit_Moa_Partner(MOAPartnerEdited input)
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
                    moa.editMOAPartner(input, int.Parse(id));
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
                //return Json("", JsonRequestBehavior.AllowGet);
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
                Console.WriteLine(ex.ToString());
                //return Json("", JsonRequestBehavior.AllowGet);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult GetMOAScopesByPartner(int partner_id)
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
                    List<CollaborationScope> scopeList = moa.getMOAScope(int.Parse(moa_id), partner_id, int.Parse(mou_id));
                    return Json(scopeList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult CheckMOAPartner(int partner_id, string start_date_string)
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
                    bool isInvalid = moa.MOAPartnerDateIsInvalid(int.Parse(mou_id), partner_id, start_date_string);
                    return Json(isInvalid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //return Json("", JsonRequestBehavior.AllowGet);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult CheckMOABonusExisted(int moa_partner_id)
        {
            try
            {
                bool isInvalid = moa.IsMOABonusExisted(moa_partner_id);
                return Json(isInvalid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //return Json("", JsonRequestBehavior.AllowGet);
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult CheckPartnerExistedInMOA(int partner_id)
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
                    string partner = moa.CheckPartnerExistedInMOA(int.Parse(id), partner_id);
                    return Json(partner);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult CheckPartnerExistedInEditMOA(int partner_id, int moa_partner_id)
        {
            try
            {
                string partner = moa.CheckPartnerExistedInMOA(moa_partner_id, partner_id);
                return Json(partner);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
    }
}