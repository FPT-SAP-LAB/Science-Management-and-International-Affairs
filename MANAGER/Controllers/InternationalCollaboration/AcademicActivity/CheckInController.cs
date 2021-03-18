using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENTITIES;
using BLL.InternationalCollaboration.AcademicActivity;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class CheckInController : Controller
    {
        private static CheckInRepo repo = new CheckInRepo();
        public ActionResult List(int id)
        {
            ViewBag.pageTitle = "Check-in hoạt động học thuật trong năm";
            ViewBag.list_phase = repo.getPhase(id);
            ViewBag.internalunit = repo.GetInternalUnit();
            return View();
        }
        [HttpPost]
        public ActionResult getDatatableByPhase(int phase_id)
        {
            List<CheckInRepo.dataParticipant> data = repo.getParticipantByPhase(phase_id);
            return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Checkin(int participant_id)
        {
            bool res = repo.Checkin(participant_id);
            if (res)
                return Json("Checkin thành công", JsonRequestBehavior.AllowGet);
            else return Json(String.Empty, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Checkout(int participant_id)
        {
            bool res = repo.Checkout(participant_id);
            if (res)
                return Json("Thu hồi thành công", JsonRequestBehavior.AllowGet);
            else return Json(String.Empty, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getRoleByPhase(int phase_id)
        {
            List<CheckInRepo.PartiRole> data = repo.GetParticipantRolesByPhase(phase_id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getAreaByUnit(int unit_id)
        {
            List<CheckInRepo.Area> data = repo.getAreaByUnit(unit_id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult addParticipant(CheckInRepo.infoParticipant obj)
        {
            bool res = repo.addParticipant(obj);
            if (res)
            {
                return Json("Thêm người tham dự thành công", JsonRequestBehavior.AllowGet);
            }
            return Json(String.Empty, JsonRequestBehavior.AllowGet);
        }
    }
}
