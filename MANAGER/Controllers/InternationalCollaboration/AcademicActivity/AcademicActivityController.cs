﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MANAGER.Models;
using BLL.InternationalCollaboration.AcademicActivity;
using MANAGER.Support;

namespace MANAGER.Controllers.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityController : Controller
    {
        private static AcademicActivityRepo repo = new AcademicActivityRepo();
        [Auther(RightID = "2")]
        public ActionResult List()
        {
            ViewBag.pageTitle = "Danh sách hoạt động học thuật trong năm";
            ViewBag.AAType = repo.getType(1);
            return View();
        }
        [HttpPost]
        public ActionResult getDatatable(int year)
        {
            List<AcademicActivityRepo.ListAA> data = repo.listAllAA(year);
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult getBaseAA(int id)
        {
            AcademicActivityRepo.baseAA data = repo.GetbaseAA(id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult delete_AcademicActivity(int id)
        {
            bool res = repo.deleteAA(id);
            if (res)
            {
                return Json("Đã xóa thành công", JsonRequestBehavior.AllowGet);
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult add_AcademicActivity(AcademicActivityRepo.baseAA obj)
        {
            try
            {
                BLL.Authen.LoginRepo.User u = (BLL.Authen.LoginRepo.User)Session["User"];
                bool res = repo.AddAA(obj, 1, u);
                if (res)
                {
                    return Json("Đã thêm thành công", JsonRequestBehavior.AllowGet);
                }
                else return Json(String.Empty);
            }
            catch (Exception e)
            {
                return Json(String.Empty);
            }
        }
        [HttpPost]
        public JsonResult edit_AcademicActivity(int id, int activity_type_id, string activity_name, string location, string from, string to)
        {
            bool res = repo.updateBaseAA(id, activity_type_id, activity_name, location, from, to, 1);
            if (res)
            {
                return Json("Đã chỉnh sửa thành công", JsonRequestBehavior.AllowGet);
            }
            else return Json(String.Empty);
        }
        public JsonResult clone(AcademicActivityRepo.cloneBase obj)
        {
            bool res = repo.clone(obj);
            if (res)
            {
                return Json("Đã sao chép thành công", JsonRequestBehavior.AllowGet);
            }
            else return Json(String.Empty);
        }
    }
}