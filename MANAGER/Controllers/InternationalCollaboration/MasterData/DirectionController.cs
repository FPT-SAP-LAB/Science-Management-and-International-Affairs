using BLL.InternationalCollaboration.MasterData;
using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.MasterData
{
    public class DirectionController : Controller
    {
        private static DirectionRepo directionRepo = new DirectionRepo();

        // GET: Direction
        public ActionResult List()
        {
            ViewBag.title = "QUẢN LÝ CHIỀU HỢP TÁC HỌC THUẬT";
            return View();
        }

        public ActionResult listDirection()
        {
            try
            {
                BaseDatatable baseDatatable = new BaseDatatable(Request);
                BaseServerSideData<Direction> baseServerSideData = directionRepo.getListDirection(baseDatatable);
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
        public ActionResult addDirection(string direction_name)
        {
            try
            {
                AlertModal<Direction> alertModal = directionRepo.addDirection(direction_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult getDirection(int direction_id)
        {
            try
            {
                AlertModal<Direction> alertModal = directionRepo.getDirection(direction_id);
                return Json(new { alertModal.obj, alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult editDirection(int direction_id, string direction_name)
        {
            try
            {
                AlertModal<Direction> alertModal = directionRepo.editDirection(direction_id, direction_name);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public ActionResult deleteDirection(int direction_id)
        {
            try
            {
                AlertModal<Direction> alertModal = directionRepo.deleteDirection(direction_id);
                return Json(new { alertModal.success, alertModal.title, alertModal.content });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}