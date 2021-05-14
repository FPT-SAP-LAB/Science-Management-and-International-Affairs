using BLL.InternationalCollaboration.AcademicActivity;
using MANAGER.Support;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class CheckInController : Controller
    {
        private static CheckInRepo repo = new CheckInRepo();
        public ActionResult List(int id)
        {
            ViewBag.pageTitle = "Check-in hoạt động học thuật trong năm";
            ViewBag.list_phase = repo.GetPhase(id);
            ViewBag.internalunit = repo.GetInternalUnit();
            return View();
        }
        [HttpPost]
        public ActionResult getDatatableByPhase(int phase_id)
        {
            List<CheckInRepo.dataParticipant> data = repo.GetParticipantByPhase(phase_id);
            return Json(new { success = true, data = data });
        }
        [Auther(RightID = "2")]
        [HttpPost]
        public JsonResult Checkin(int participant_id)
        {
            bool res = repo.Checkin(participant_id);
            if (res)
                return Json("Checkin thành công");
            else return Json(String.Empty);
        }
        [Auther(RightID = "2")]
        [HttpPost]
        public JsonResult Checkout(int participant_id)
        {
            bool res = repo.Checkout(participant_id);
            if (res)
                return Json("Thu hồi thành công");
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult getRoleByPhase(int phase_id)
        {
            List<CheckInRepo.PartiRole> data = repo.GetParticipantRolesByPhase(phase_id);
            return Json(data);
        }
        public JsonResult getAreaByUnit(int unit_id)
        {
            List<CheckInRepo.Area> data = repo.GetAreaByUnit(unit_id);
            return Json(data);
        }
        [Auther(RightID = "2")]
        [HttpPost]
        public JsonResult addParticipant(CheckInRepo.infoParticipant obj)
        {
            bool res = repo.AddParticipant(obj);
            if (res)
            {
                return Json("Thêm người tham dự thành công");
            }
            return Json(String.Empty);
        }
    }
}
