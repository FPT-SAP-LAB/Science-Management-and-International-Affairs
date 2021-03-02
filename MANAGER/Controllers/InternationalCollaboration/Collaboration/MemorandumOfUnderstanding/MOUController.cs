using BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BLL.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding.MOURepo;

namespace MANAGER.Controllers.InternationalCollaboration.Collaboration.MemorandumOfUnderstanding
{
    public class MOUController : Controller
    {
        // GET: MOU
        private static MOURepo mou;

        public ActionResult List()
        {
            ViewBag.pageTitle = "DANH SÁCH BIÊN BẢN GHI NHỚ";
            int duration = mou.getDuration();
            List<ListMOU> listMOU = mou.listAllMOU();
            NotificationInfo noti = mou.getNoti();
            mou.UpdateStatusMOU();
            return View();
        }

        public ActionResult ListDeleted()
        {
            ViewBag.pageTitle = "DANH SÁCH BIÊN BẢN GHI NHỚ ĐÃ XÓA";
            int duration = mou.getDuration();
            List<ListMOU> listMOU = mou.listAllMOUDeleted();
            return View();
        }

        public ActionResult Delete_Mou(int mou_id)
        {
            try
            {
                mou.deleteMOU(mou_id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }

        public ActionResult Add_Mou(MOUAdd input)
        {
            try
            {
                mou.addMOU(input);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }

        public ActionResult ExportMOUExcel()
        {
            try
            {
                mou.ExportMOUExcel();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }

        public ActionResult getDataAddingMOU()
        {
            try
            {
                List<ENTITIES.InternalUnit> unitList = mou.GetInternalUnit();
                List<ENTITIES.Partner> partnerList = mou.GetPartners();
                List<ENTITIES.Specialization> speList = mou.GetSpecializations();
                List<ENTITIES.CollaborationScope> scopeList = mou.GetCollaborationScopes();
                string suggestedMOUCode = mou.getSuggestedMOUCode();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }

        public ActionResult checkPartner()
        {
            try
            {
                mou.ExportMOUExcel();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(400);
            }
        }

        public ActionResult Detail(string id)
        {
            return View();
        }
    }
}