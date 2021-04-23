using BLL.InternationalCollaboration.Collaboration.PartnerRepo;
using BLL.ModelDAL;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.InternationalCollaboration.Collaboration.PartnerEntity;
using GUEST.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers.InternationalCollaboration.Collaboration.Partner
{
    public class PartnerController : Controller
    {
        private static readonly PartnerRepo partnerRePo = new PartnerRepo();
        private static readonly CountryRepo countryRepo = new CountryRepo();
        private static readonly SpecializationRepo specializationRepo = new SpecializationRepo();
        readonly System.Resources.ResourceManager rm = LanguageResource.GetResourceManager();
        // GET: Partner
        public ActionResult List()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("PartnerList"), "/Partner")
            };
            ViewBag.countries = countryRepo.GetCountries();
            ViewBag.specializations = specializationRepo.GetSpecializations(LanguageResource.GetCurrentLanguageID());
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        [HttpPost]
        public ActionResult List(SearchPartner searchPartner)
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                searchPartner.language = LanguageResource.GetCurrentLanguageID();
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
        public ActionResult Detail(int id)
        {
            try
            {
                var pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("PartnerList"), "/Partner/List"),
                new PageTree(rm.GetString("PartnerDetail"), "/Partner/Partner_Detail?" + id)
            };
                ViewBag.partnerArticle = partnerRePo.LoadEditPartnerGuest(id, LanguageResource.GetCurrentLanguageID());
                ViewBag.specializations_selected = partnerRePo.GetPartnerDetailSpec(id);
                ViewBag.pagesTree = pagesTree;
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}