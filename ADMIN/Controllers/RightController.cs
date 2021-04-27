using ADMIN.Support;
using BLL.Admin;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using System;
using System.Web.Mvc;

namespace ADMIN.Controllers
{
    [Auther(RightID = "0")]
    public class RightController : Controller
    {
        private static RightRepo repo = new RightRepo();
        public ActionResult Index()
        {
            ViewBag.pageTitle = "Quản lí quyền hạn";
            ViewBag.module = repo.GetModules();
            return View();
        }
        [HttpPost]
        public ActionResult getDatatable()
        {
            BaseDatatable baseDatatable = new BaseDatatable(Request);
            BaseServerSideData<RightRepo.infoRight> data = repo.GetRights(baseDatatable);
            return Json(new
            {
                success = true,
                data = data.Data,
                draw = Request["draw"],
                recordsTotal = data.RecordsTotal,
                recordsFiltered = data.RecordsTotal
            });
        }
        [HttpPost]
        public JsonResult add(RightRepo.baseRight obj)
        {
            bool res = repo.Add(obj);
            if (res)
            {
                return Json("Thêm thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult delete(int right_id)
        {
            bool res = repo.Delete(right_id);
            if (res)
            {
                return Json("Xóa thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult edit(RightRepo.infoRight obj)
        {
            bool res = repo.Edit(obj);
            if (res)
            {
                return Json("Chỉnh sửa thành công");
            }
            else return Json(String.Empty);
        }
        [HttpPost]
        public JsonResult getRight(int right_id)
        {
            Right data = repo.GetBaseRight(right_id);
            return Json(data);
        }
    }
}