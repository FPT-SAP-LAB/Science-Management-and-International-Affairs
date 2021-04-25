using BLL.ScienceManagement.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using System;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.MasterData
{
    public class UpdateJournalController : Controller
    {
        // GET: UpdateJournal
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ListAll(string name_search)
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<Scopu> baseServerSideData = MasterDataRepo.getListAllScopus(baseDatatable, name_search);
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
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public ActionResult ListAll2(string name_search)
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<CustomISI> baseServerSideData = MasterDataRepo.getListAllISI(baseDatatable, name_search);
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
                Console.WriteLine(e.ToString());
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public ActionResult uploadJournal(HttpPostedFileBase file_scopus, HttpPostedFileBase file_SCIE, HttpPostedFileBase file_SSCI)
        {
            MasterDataRepo md = new MasterDataRepo();
            bool mess = md.updateJournal(file_scopus, file_SCIE, file_SSCI);
            return Json(new { mess }, JsonRequestBehavior.AllowGet);
        }
    }
}