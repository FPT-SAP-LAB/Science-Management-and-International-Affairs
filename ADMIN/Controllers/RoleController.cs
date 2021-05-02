using ENTITIES.CustomModels;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Admin;
using ADMIN.Support;

namespace ADMIN.Controllers
{
    [Auther(RightID = "0")]
    public class RoleController : Controller
    {
        private static RoleRepo repo = new RoleRepo();
        public ActionResult Index()
        {
            ViewBag.pageTitle = "Quản lí chức danh";
            ViewBag.HTright = repo.GetRightsByModule(1);
            ViewBag.QLright = repo.GetRightsByModule(2);
            return View();
        }
        [HttpPost]
        public ActionResult getDatatable()
        {
            List<RoleRepo.infoRole> data = repo.GetRoles();
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult add(RoleRepo.baseRole obj)
        {
            bool res = repo.Add(obj);
            if (res)
            {
                return Json("Thêm thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult delete(int role_id)
        {
            bool res = repo.Delete(role_id);
            if (res)
            {
                return Json("Xóa thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult edit(RoleRepo.infoRole obj)
        {
            bool res = repo.Edit(obj);
            if (res)
            {
                return Json("Chỉnh sửa thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult getRole(int role_id)
        {
            RoleRepo.infoRole data = repo.GetBaseRole(role_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult getRightByRole(int role_id)
        {
            List<RoleRepo.baseRight> data = repo.GetRightByRole(role_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult UpdateRight(int[] arrAccept, int role_id)
        {
            bool res = repo.UpdateRight(arrAccept, role_id);
            if (res) return Json("Chỉnh sửa thành công");
            else return Json(String.Empty);
        }
    }
}