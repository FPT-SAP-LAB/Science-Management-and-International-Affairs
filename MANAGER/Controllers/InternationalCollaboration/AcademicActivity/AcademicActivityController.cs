﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MANAGER.Models;
using BLL.InternationalCollaboration.AcademicActivity;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityController : Controller
    {
        private static AcademicActivityRepo repo = new AcademicActivityRepo();
        public ActionResult List()
        {
            ViewBag.pageTitle = "Danh sách hoạt động học thuật trong năm";
            ViewBag.AAType = repo.getType();
            return View();
        }
        [HttpPost]
        public ActionResult getDatatable(int year)
        {
            List<AcademicActivityRepo.ListAA> data = repo.listAllAA(year);
            return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getBaseAA(int id)
        {
            AcademicActivityRepo.baseAA data = repo.GetbaseAA(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Detail(int id)
        {
            return View();
        }
        [HttpPost]
        public JsonResult delete_AcademicActivity(int id)
        {
            bool res = repo.deleteAA(id);
            if (res)
            {
                return Json("Đã xóa thành công", JsonRequestBehavior.AllowGet);
            }
            else return Json(String.Empty, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult add_AcademicActivity(AcademicActivityRepo.baseAA obj)
        {
            bool res = repo.AddAA(obj);
            if (res)
            {
                return Json("Đã thêm thành công", JsonRequestBehavior.AllowGet);
            }
            else return Json(String.Empty, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult edit_AcademicActivity(int id, int activity_type_id, string activity_name, string from, string to, string location)
        {

            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}