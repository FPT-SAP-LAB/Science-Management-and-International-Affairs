using BLL.InternationalCollaboration.Collaboration.MemorandumOfAgreement;
using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOU;
using MANAGER.Support;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class MOUController : Controller
    {
        // GET: MOU
        readonly MOURepo mou = new MOURepo();
        readonly BasicInfoMOURepo mou_detail = new BasicInfoMOURepo();
        readonly PartnerMOURepo mou_partner = new PartnerMOURepo();
        readonly MOARepo moa = new MOARepo();
        [Auther(RightID = "5")]
        public ActionResult List()
        {
            try
            {
                ViewBag.pageTitle = "DANH SÁCH BIÊN BẢN GHI NHỚ";
                mou.UpdateStatusMOU();
                ViewBag.listOffice = mou.GetOffice();
                //ViewBag.newMOUCode = mou.getSuggestedMOUCode();
                ViewBag.listScopes = mou.GetCollaborationScopes();
                ViewBag.listSpe = mou.GetSpecializations();
                ViewBag.listCountry = mou.GetCountries();
                //ViewBag.noti = mou.getNoti();
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(new HttpStatusCodeResult(400));
            }
        }
        public ActionResult ViewMOU(string partner_name, string contact_point_name, string mou_code)
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<ListMOU> listMOU = mou.ListAllMOU(baseDatatable, partner_name, contact_point_name, mou_code);
                return Json(new { success = true, data = listMOU.Data, draw = Request["draw"], recordsTotal = listMOU.RecordsTotal, recordsFiltered = listMOU.RecordsTotal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult GetNewMOUCode()
        {
            try
            {
                string new_mou_code = mou.GetSuggestedMOUCode();
                return Json(new_mou_code, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
                Console.WriteLine(ex.ToString());
                return View(new HttpStatusCodeResult(400));
            }
        }
        public ActionResult ViewMOUDeleted(string partner_name, string contact_point_name, string mou_code)
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<ListMOU> listMOU = mou.ListAllMOUDeleted(baseDatatable, partner_name, contact_point_name, mou_code);
                return Json(new { success = true, data = listMOU.Data, draw = Request["draw"], recordsTotal = listMOU.RecordsTotal, recordsFiltered = listMOU.RecordsTotal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "5")]
        public ActionResult Delete_Mou(int mou_id)
        {
            try
            {
                mou.DeleteMOU(mou_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "5")]
        public ActionResult Add_Mou(string input, HttpPostedFileBase evidence)
        {
            try
            {
                JObject inputObj = JObject.Parse(input);
                MOUAdd obj = inputObj.ToObject<MOUAdd>();
                BLL.Authen.LoginRepo.User user = (BLL.Authen.LoginRepo.User)Session["User"];
                //upload file to gg drive.
                Google.Apis.Drive.v3.Data.File f = new Google.Apis.Drive.v3.Data.File();
                if (evidence != null)
                {
                    f = mou.UploadEvidenceFile(evidence, obj.BasicInfo.mou_code, 1, false);
                }
                //update info in database
                mou.AddMOU(obj, user, f, evidence);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [HttpGet]
        public ActionResult GetPartner()
        {
            try
            {
                List<Partner> partners = mou.GetPartners();
                return partners is null ? Json("") : Json(partners);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        [Auther(RightID = "5")]
        public ActionResult ExportMOUExcel(string partner_name, string contact_point_name, string mou_code)
        {
            try
            {
                MemoryStream memoryStream = mou.ExportMOUExcel(partner_name, contact_point_name, mou_code);
                string downloadFile = "MOUDownload.xlsx";
                string handle = Guid.NewGuid().ToString();
                TempData[handle] = memoryStream.ToArray();
                return Json(new { success = true, data = new { FileGuid = handle, FileName = downloadFile } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
                if (Session["mou_detail_id"] is null)
                {
                    return Redirect("../MOU/List");
                }
                else
                {
                    string id = Session["mou_detail_id"].ToString();
                    List<ENTITIES.Partner> partnerList = mou_detail.GetPartnerExMOU(int.Parse(id));
                    List<CollaborationScope> scopeList = mou_detail.GetScopesExMOU(int.Parse(id));
                    ViewBag.scopeList = scopeList;
                    ViewBag.partnerList = partnerList;

                    //MOU Partner
                    ViewBag.listSpeMOUPartner = mou_partner.GetPartnerMOUSpe();
                    ViewBag.listScopesMOUPartner = mou_partner.GetPartnerMOUScope(int.Parse(id));
                    ViewBag.listPartnerMOUPartner = mou_partner.GetPartners(int.Parse(id));
                    ViewBag.listCountry = mou.GetCountries();

                    //MOA
                    //ViewBag.newMOACode = moa.getSuggestedMOACode(int.Parse(id));
                    ViewBag.listPartnersMOA = moa.GetMOAPartners(int.Parse(id));
                    return View();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(new HttpStatusCodeResult(400));
            }
        }
        public ActionResult GetNewMOACode()
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
                    string new_moa_code = moa.GetSuggestedMOACode(int.Parse(id));
                    return Json(new_moa_code, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        //chua ve class diagram
        public ActionResult CheckDuplicatedPartnerInfo(string partner_name, string mou_start_date_string)
        {
            try
            {
                DuplicatePartnerInfo obj = mou.PartnerInfoIsDuplicated(partner_name, mou_start_date_string);
                return obj == null ? Json("") : Json(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult CheckDuplicatedMOUCode(string mou_code)
        {
            try
            {
                bool isDup = mou.GetMOUCodeCheck(mou_code);
                return Json(isDup);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        public ActionResult checkIntersectPeriodMOUDate(List<PartnerInfo> PartnerInfo, string start_date, string end_date, int office_id)
        {
            try
            {
                IntersectPeriodMOUDate obj = mou.CheckIntersectPeriodMOUDate(PartnerInfo, start_date, end_date, office_id);
                return Json(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new HttpStatusCodeResult(400);
            }
        }
        //chua ve class diagram
    }
}