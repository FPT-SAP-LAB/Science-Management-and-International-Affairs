using ENTITIES.CustomModels;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Admin;

namespace ADMIN.Controllers
{
    public class RoleController : Controller
    {
        private static RoleRepo repo = new RoleRepo();
        public ActionResult Index()
        {
            ViewBag.pageTitle = "Quản lí chức danh";
            ViewBag.HTright = repo.getRightsByModule(1);
            ViewBag.QLright = repo.getRightsByModule(2);
            return View();
        }
        [HttpPost]
        public ActionResult getDatatable()
        {
            List<RoleRepo.infoRole> data = repo.getRoles();
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult add(RoleRepo.baseRole obj)
        {
            bool res = repo.add(obj);
            if (res)
            {
                return Json("Thêm thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult delete(int role_id)
        {
            bool res = repo.delete(role_id);
            if (res)
            {
                return Json("Xóa thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult edit(Role obj)
        {
            bool res = repo.edit(obj);
            if (res)
            {
                return Json("Chỉnh sửa thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult getRole(int role_id)
        {
            Role data = repo.GetBaseRole(role_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult getRightByRole(int role_id)
        {
            List<RoleRepo.baseRight> data = repo.getRightByRole(role_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult UpdateRight(int[] arrAccept,int role_id)
        {
            //bool res = repo.UpdateRight(arrAccept,role_id);
            return Json(String.Empty);
        }
    }
}