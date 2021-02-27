using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class ConferenceSponsorController : Controller
    {
        // GET: ConferenceSponsor
        public ActionResult Index()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        public ActionResult Add()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
                new PageTree("Thêm","/ConferenceSponsor/Add"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public ActionResult Detail(int id)
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
                new PageTree("Chi tiết","/ConferenceSponsor/Detail"),
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.id = id;
            return View();
        }
        [ChildActionOnly]
        public ActionResult CostMenu(int id)
        {
            ViewBag.id = id;
            ViewBag.CheckboxColumn = id == 2;
            ViewBag.ReimbursementColumn = id >= 3;
            ViewBag.EditAble = id == 2;
            return PartialView();
        }
        public JsonResult GetInformationPeopleWithID(string id)
        {
            List<Info> infos = new List<Info>()
            {
                new Info("HE130214", "Đoàn Văn Thắng", 1, "FPTU", 1, "Hà Nội", 1, "Sinh viên")
            };
            return Json(infos, JsonRequestBehavior.AllowGet);
        }
        private class Info
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public int WorkUnitID { get; set; }
            public string WorkUnitString { get; set; }
            public int AreaID { get; set; }
            public string AreaString { get; set; }
            public int TitleID { get; set; }
            public string TitleString { get; set; }
            public Info() { }
            public Info(string id, string name, int unitID, string unitString, int areaID, string areaString, int titleID, string titleString)
            {
                ID = id;
                Name = name;
                WorkUnitID = unitID;
                WorkUnitString = unitString;
                AreaID = areaID;
                AreaString = areaString;
                TitleID = titleID;
                TitleString = titleString;
            }
        }
    }
}