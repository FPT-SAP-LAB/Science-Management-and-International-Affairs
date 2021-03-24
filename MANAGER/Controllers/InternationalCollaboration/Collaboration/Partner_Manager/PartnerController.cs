using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.InternationalCollaboration.Collaboration.PartnerRepo;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity;
using MANAGER.Support;
using BLL.Authen;
using Newtonsoft.Json;
using BLL.ModelDAL;

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
            ViewBag.specializations = specializationRepo.GetSpecializations();
            return View();
        }

        [HttpPost]
        public ActionResult List(SearchPartner searchPartner)
        {
            try
            {
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
            ViewBag.specializations = specializationRepo.GetSpecializations();
            return View();
        }

        [HttpPost]
        public ActionResult List_Deleted(SearchPartner searchPartner)
        {
            try
            {
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
                return Json(new { alertModal.success }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw e;
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
            string partner_name, int country_id, string website, string address)
        {
            try
            {
                ViewBag.title = "THÊM ĐỐI TÁC";
                PartnerArticle partner_article = new PartnerArticle
                {
                    partner_name = partner_name,
                    country_id = country_id,
                    website = website,
                    address = address
                };

                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;
                }

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
                if (acc.account_id == 0)
                {
                    return Json(new
                    {
                        json = new AlertModal<string>(false, "Chưa đăng nhập không thể thêm bài"),
                        type = 0
                    });
                }
                else
                {
                    AlertModal<string> alertModal = partnerRePo.AddPartner(files_request, content,
                        partner_article, numberOfImage, acc.account_id);
                    return Json(new { json = alertModal, type = 1 });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { json = new AlertModal<string>(false, "Có lỗi xảy ra"), type = 2 });
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
            ViewBag.title = "Xem trước";
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
                ViewBag.specializations = specializationRepo.GetSpecializations();
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
            string partner_name, int country_id, string website, string address)
        {
            try
            {
                ViewBag.title = "CHI TIẾT ĐỐI TÁC";
                LoginRepo.User u = new LoginRepo.User();
                Account acc = new Account();
                if (Session["User"] != null)
                {
                    u = (LoginRepo.User)Session["User"];
                    acc = u.account;
                }
                PartnerArticle partner_article = new PartnerArticle
                {
                    partner_name = partner_name,
                    country_id = country_id,
                    website = website,
                    address = address
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
                if (acc.account_id == 0)
                {
                    return Json(new
                    {
                        json = new AlertModal<string>(false, "Chưa đăng nhập không thể thêm bài"),
                        type = 0
                    });
                }
                else
                {
                    AlertModal<string> alertModal = partnerRePo.EditPartner(files_request, content,
                        partner_article, numberOfImage, partner_id, acc.account_id);
                    return Json(new { json = alertModal, type = 1 });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Json(new { json = new AlertModal<string>(false, "Có lỗi xảy ra"), type = 2 });
            }
        }
    }
}