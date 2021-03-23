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
    public class AccountController : Controller
    {
        private static AccountRepo repo = new AccountRepo();
        public ActionResult List()
        {
            ViewBag.pageTitle = "Quản lí tài khoản";
            ViewBag.HTright = repo.getRightsByModule(1);
            ViewBag.QLright = repo.getRightsByModule(2);
            ViewBag.Role = repo.getRoles();
            return View();
        }
        [HttpPost]
        public ActionResult getDatatable()
        {
            List<AccountRepo.extendAccount> data = repo.getAccounts();
            return Json(new { success = true, data = data });
        }
        [HttpPost]
        public JsonResult add(AccountRepo.baseAccount obj)
        {
            bool res = repo.add(obj);
            if (res)
            {
                return Json("Thêm thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult delete(int account_id)
        {
            bool res = repo.delete(account_id);
            if (res)
            {
                return Json("Xóa thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult edit(AccountRepo.infoAccount obj)
        {
            bool res = repo.edit(obj);
            if (res)
            {
                return Json("Chỉnh sửa thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult getAccount(int account_id)
        {
            AccountRepo.baseAccount data = repo.GetBaseAccount(account_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult getRightByAccount(int account_id)
        {
            List<AccountRepo.baseRight> data = repo.getRightByAccount(account_id);
            return Json(data);
        }
        [HttpPost]
        public JsonResult UpdateRight(int[] arrAccept, int account_id)
        {
            bool res = repo.UpdateRight(arrAccept, account_id);
            if (res) return Json("Chỉnh sửa thành công");
            else return Json(String.Empty);
        }
    }
}