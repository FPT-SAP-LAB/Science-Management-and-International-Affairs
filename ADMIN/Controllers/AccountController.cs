using ENTITIES.CustomModels;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Admin;
using ADMIN.Support;
using BLL.ModelDAL;

namespace ADMIN.Controllers
{
    [Auther(RightID = "0")]
    public class AccountController : Controller
    {
        private readonly AccountRepo repo = new AccountRepo();
        public ActionResult List()
        {
            ViewBag.pageTitle = "Quản lí tài khoản";
            ViewBag.HTright = repo.GetRightsByModule(1);
            ViewBag.QLright = repo.GetRightsByModule(2);
            ViewBag.Role = repo.GetRoles();
            ViewBag.Postions = PositionLanguageRepo.GetPositionLanguages(1);
            return View();
        }
        [HttpPost]
        public ActionResult getDatatable()
        {
            List<AccountRepo.extendAccount> data = repo.GetAccounts();
            return Json(new { success = true, data });
        }
        [HttpPost]
        public JsonResult add(AccountRepo.baseAccount obj)
        {
            bool res = repo.Add(obj);
            if (res)
            {
                return Json("Thêm thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult delete(int account_id)
        {
            string res = repo.Delete(account_id);
            if (res.Equals("ok"))
            {
                return Json(1);
            }
            else if (res.Equals("cons"))
            {
                return Json(2);
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult edit(AccountRepo.infoAccount obj)
        {
            string res = repo.Edit(obj);
            if (res.Equals("ok"))
            {
                return Json(1);
            }
            else if (res.Equals("cons"))
            {
                return Json(2);
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
            List<AccountRepo.baseRight> data = repo.GetRightByAccount(account_id);
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