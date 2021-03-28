using BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement;
using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using OfficeOpenXml;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class MOUController : Controller
    {
        // GET: MOU
        readonly MOURepo mou = new MOURepo();
        readonly BasicInfoMOURepo mou_detail = new BasicInfoMOURepo();
        readonly PartnerMOURepo mou_partner = new PartnerMOURepo();
        readonly MOARepo moa = new MOARepo();
        public ActionResult List()
        {
            try
            {
                ViewBag.pageTitle = "DANH SÁCH BIÊN BẢN GHI NHỚ";
                mou.UpdateStatusMOU();
                ViewBag.listOffice = mou.GetOffice();
                ViewBag.newMOUCode = mou.getSuggestedMOUCode();
                ViewBag.listPartners = mou.GetPartners();
                ViewBag.listScopes = mou.GetCollaborationScopes();
                ViewBag.listSpe = mou.GetSpecializations();
                ViewBag.noti = mou.getNoti();
                return View();
            }
            catch (Exception ex)
            {
                return View(new HttpStatusCodeResult(400));
            }
        }
        public ActionResult ViewMOU(string partner_name, string contact_point_name, string mou_code)
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<ListMOU> listMOU = mou.listAllMOU(baseDatatable, partner_name, contact_point_name, mou_code);
                return Json(new { success = true, data = listMOU.Data, draw = Request["draw"], recordsTotal = listMOU.RecordsTotal, recordsFiltered = listMOU.RecordsTotal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult List_Deleted()
        {
            try
            {
                ViewBag.pageTitle = "DANH SÁCH BIÊN BẢN GHI NHỚ ĐÃ XÓA";
                return View();
            }
            catch (Exception ex)
            {
                return View(new HttpStatusCodeResult(400));
            }
        }
        public ActionResult ViewMOUDeleted(string partner_name, string contact_point_name, string mou_code)
        {
            try
            {
                List<ListMOU> listMOU = mou.listAllMOUDeleted(partner_name, contact_point_name, mou_code);
                return Json(new { success = true, data = listMOU }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult Delete_Mou(int mou_id)
        {
            try
            {
                mou.deleteMOU(mou_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult Add_Mou(MOUAdd input)
        {
            try
            {
                BLL.Authen.LoginRepo.User user = (BLL.Authen.LoginRepo.User)Session["User"];
                mou.addMOU(input,user);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CheckPartner(string partner_name)
        {
            try
            {
                CustomPartner partner = mou.CheckPartner(partner_name);
                return partner is null ? Json("") : Json(partner);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult ExportMOUExcel()
        {
            try
            {
                MemoryStream memoryStream = mou.ExportMOUExcel();
                string downloadFile = "MOUDownload.xlsx";
                string handle = Guid.NewGuid().ToString();
                TempData[handle] = memoryStream.ToArray();
                return Json(new { success = true, data = new { FileGuid = handle, FileName = downloadFile } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }
        public ActionResult Detail()
        {
            try
            {
                ViewBag.pageTitle = "CHI TIẾT BIÊN BẢN GHI NHỚ";
                string id = Session["mou_detail_id"].ToString();
                List<ENTITIES.Partner> partnerList = mou_detail.getPartnerExMOU(int.Parse(id));
                List<CollaborationScope> scopeList = mou_detail.GetScopesExMOU(int.Parse(id));
                ViewBag.scopeList = scopeList;
                ViewBag.partnerList = partnerList;

                //MOU Partner
                ViewBag.listSpeMOUPartner = mou_partner.getPartnerMOUSpe();
                ViewBag.listScopesMOUPartner = mou_partner.getPartnerMOUScope(int.Parse(id));
                ViewBag.listPartnerMOUPartner = mou_partner.GetPartners(int.Parse(id));

                //MOA
                ViewBag.newMOACode = moa.getSuggestedMOACode(int.Parse(id));
                ViewBag.listPartnersMOA = moa.GetMOAPartners(int.Parse(id));
                return View();
            }
            catch (Exception ex)
            {
                return View(new HttpStatusCodeResult(400));
            }
        }
        public ActionResult CheckDuplicatedPartnerInfo(string partner_name, string mou_start_date_string)
        {
            try
            {
                DuplicatePartnerInfo obj = mou.partnerInfoIsDuplicated(partner_name, mou_start_date_string);
                return obj == null ? Json("") : Json(obj);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult CheckDuplicatedMOUCode(string mou_code)
        {
            try
            {
                bool isDup = mou.getMOUCodeCheck(mou_code);
                return Json(isDup);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(400);
            }
        }
    }
}