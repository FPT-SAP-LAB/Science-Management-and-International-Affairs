using BLL.Authen;
using BLL.InternationalCollaboration.Collaboration.PartnerRepo;
using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity;
using MANAGER.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.Partner_Manager
{
    public class PartnerController : Controller
    {
        private static readonly PartnerRepo partnerRePo = new PartnerRepo();
        private static readonly CountryRepo countryRepo = new CountryRepo();
        private static readonly SpecializationRepo specializationRepo = new SpecializationRepo();
        //[Auther(RightID = "8")]

        public ActionResult List()
        {
            ViewBag.title = "DANH SÁCH ĐỐI TÁC";
            ViewBag.countries = countryRepo.GetCountries();
            ViewBag.specializations = specializationRepo.GetSpecializations(1);
            return View();
        }

        [HttpPost]
        public ActionResult List(SearchPartner searchPartner)
        {
            try
            {
                Session.Timeout = 120;
                Session["searchPartner"] = searchPartner;
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<PartnerList> baseServerSideData = partnerRePo.GetListAll(baseDatatable, searchPartner);
                return Json(new
                {
                    success = true,
                    data = baseServerSideData.Data,
                    draw = Request["draw"],
                    recordsTotal = baseServerSideData.RecordsTotal,
                    recordsFiltered = baseServerSideData.RecordsTotal
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = e.Message });
            }
        }

        public ActionResult List_Deleted()
        {
            ViewBag.title = "DANH SÁCH ĐỐI TÁC ĐÃ XÓA";
            ViewBag.countries = countryRepo.GetCountries();
            ViewBag.specializations = specializationRepo.GetSpecializations(1);
            return View();
        }

        [HttpPost]
        public ActionResult List_Deleted(SearchPartner searchPartner)
        {
            try
            {
                Session.Timeout = 120;
                Session["searchPartner"] = searchPartner;
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<PartnerList> baseServerSideData = partnerRePo.GetListAll(baseDatatable, searchPartner);
                return Json(new
                {
                    success = true,
                    data = baseServerSideData.Data,
                    draw = Request["draw"],
                    recordsTotal = baseServerSideData.RecordsTotal,
                    recordsFiltered = baseServerSideData.RecordsTotal
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Load_History(int id)
        {
            try
            {
                PartnerHistoryList<PartnerHistory> partnerHistoryList = partnerRePo.GetHistory(id);

                return Json(new
                {
                    list = partnerHistoryList.Data,
                    name = partnerHistoryList.Partner_name,
                    JsonRequestBehavior.AllowGet
                });
            }
            catch (Exception e)
            {
                return Json(new { data = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                AlertModal<string> alertModal = partnerRePo.DeletePartner(id);
                return Json(new { alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                AlertModal<string> alertModal = new AlertModal<string>(null, false, "Thất bại", "Có lỗi xảy ra khi xóa");
                return Json(new { alertModal.success, alertModal.title, alertModal.content }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Add()
        {
            ViewBag.title = "THÊM ĐỐI TÁC";
            ViewBag.countries = countryRepo.GetCountries();
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Add(HttpPostedFileBase image, string content, int numberOfImage,
            string partner_name, int country_id, string website, string address, int partner_language_type)
        {
            try
            {
                ViewBag.title = "THÊM ĐỐI TÁC";
                PartnerArticle partner_article = new PartnerArticle
                {
                    partner_name = partner_name,
                    country_id = country_id,
                    website = website,
                    address = address,
                    partner_language_type = partner_language_type
                };

                List<HttpPostedFileBase> files_request = new List<HttpPostedFileBase>();
                for (int i = 0; i < numberOfImage; i++)
                {
                    string label = "image_" + i;
                    files_request.Add(Request.Files[label]);
                }
                if (image != null)
                {
                    files_request.Add(image);
                }
                int account_id = CurrentAccount.AccountID(Session);
                if (account_id == 0)
                {
                    return Json(new
                    {
                        json = new AlertModal<string>(false, "Chưa đăng nhập không thể thêm bài")
                    });
                }
                else
                {
                    AlertModal<string> json = partnerRePo.AddPartner(files_request, image, content,
                        partner_article, numberOfImage, account_id);
                    return Json(new { json.success, json.content });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                AlertModal<string> json = new AlertModal<string>(false, "Có lỗi xảy ra");
                return Json(new { json.success, json.content });
            }
        }

        public ActionResult Pass_Content(string content, string website, string address, string avata)
        {
            try
            {
                Session.Timeout = 120;
                Session["content"] = content;
                Session["address"] = address;
                Session["website"] = website;
                Session["avata"] = avata;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Preview()
        {
            ViewBag.title = "XEM TRƯỚC";
            ViewBag.content = Session["content"];
            ViewBag.address = Session["address"];
            ViewBag.website = Session["website"];
            ViewBag.avata = Session["avata"];
            return View();
        }

        [HttpPost]
        public ActionResult Detail(int id)
        {
            try
            {
                ViewBag.title = "CHI TIẾT ĐỐI TÁC";
                ViewBag.partnerArticle = partnerRePo.LoadEditPartner(id);
                ViewBag.countries = countryRepo.GetCountries();
                ViewBag.specializations = specializationRepo.GetSpecializations(1);
                ViewBag.specializations_selected = partnerRePo.GetPartnerDetailSpec(id);
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        [HttpPost]
        public ActionResult LoadContentDetailLanguage(int partner_id, int partner_language_type)
        {
            try
            {
                string content = partnerRePo.GetContentLanguage(partner_id, partner_language_type);
                return Json(new { json = new AlertModal<string>(true, "Đổi ngôn ngữ thành công"), content });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new
                {
                    json = new AlertModal<string>(false, "Có lỗi xảy ra")
                });
            }
        }

        [HttpPost]
        public ActionResult Detail_History(int id)
        {
            try
            {
                PartnerHistoryList<PartnerHistory> partnerHistoryList = partnerRePo.GetHistory(id);

                return Json(new
                {
                    data = partnerHistoryList.Data,
                    JsonRequestBehavior.AllowGet
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { success = false });
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Save_Edit(HttpPostedFileBase image, string content, int numberOfImage, int partner_id,
            string partner_name, int country_id, string website, string address, int partner_language_type)
        {
            try
            {
                ViewBag.title = "CHI TIẾT ĐỐI TÁC";

                PartnerArticle partner_article = new PartnerArticle
                {
                    partner_name = partner_name,
                    country_id = country_id,
                    website = website,
                    address = address,
                    partner_language_type = partner_language_type
                };

                List<HttpPostedFileBase> files_request = new List<HttpPostedFileBase>();
                for (int i = 0; i < numberOfImage; i++)
                {
                    string label = "image_" + i;
                    files_request.Add(Request.Files[label]);
                }
                if (image != null)
                {
                    files_request.Add(image);
                }
                int account_id = CurrentAccount.AccountID(Session);
                if (account_id == 0)
                {
                    return Json(new
                    {
                        json = new AlertModal<string>(false, "Chưa đăng nhập không thể thêm bài")
                    });
                }
                else
                {
                    AlertModal<string> json = partnerRePo.EditPartner(files_request, image, content,
                            partner_article, numberOfImage, partner_id, account_id);
                    return Json(new { json.success, json.content });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                AlertModal<string> json = new AlertModal<string>(false, "Có lỗi xảy ra");
                return Json(new { json.success, json.content });
            }
        }

        public ActionResult ExportPartnerExcel()
        {
            try
            {
                SearchPartner searchPartner = (SearchPartner)Session["searchPartner"];
                MemoryStream memoryStream = partnerRePo.ExportPartnerExcel(searchPartner);
                string downloadFile = "PartnerDownload.xlsx";
                string handle = Guid.NewGuid().ToString();
                TempData[handle] = memoryStream.ToArray();
                return Json(new { success = true, data = new { FileGuid = handle, FileName = downloadFile } }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
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
    }
}