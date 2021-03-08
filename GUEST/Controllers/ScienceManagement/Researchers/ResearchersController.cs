using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;
using ENTITIES;
using BLL.ScienceManagement.ConferenceSponsor;
using GUEST.Models;
using System.Collections;

namespace GUEST.Controllers.ScienceManagement.Researchers
{
    public class ResearchersController : Controller
    {
        // GET: Reseachers
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public ActionResult List()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Nghiên cứu viên", "/Researchers"),
                new PageTree("Danh sách nghiên cứu viên", "/Researchers/List"),
            };
            ViewBag.pagesTree = pagesTree;
            ////////////////////////////////////////////
            var list = (from p in db.Profiles
                        join rp in db.People on p.people_id equals rp.people_id
                        join o in db.Offices on rp.office_id equals o.office_id
                        select new Researchers_ListView
                        {
                            name = p.name,
                            avatar_id = p.avatar_id,
                            email = p.email,
                            google_scholar = p.google_scholar,
                            website = p.website,
                            office_name = o.office_name
                        }).ToList<Researchers_ListView>();
            list.Add(new Researchers_ListView { name = "Bùi Ngọc Anh", avatar_id = 0, email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHN" });
            list.Add(new Researchers_ListView { name = "Nguyễn Phi Hùng", avatar_id = 0, email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHCM" });
            list.Add(new Researchers_ListView { name = "Nguyễn Bá Sơn", avatar_id = 0, email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FPoly HN" });
            list.Add(new Researchers_ListView { name = "Trần Thị A", avatar_id = 0, email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHCM" });
            list.Add(new Researchers_ListView { name = "Nguyễn Văn B", avatar_id = 0, email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FPTU Cần Thơ" });
            list.Add(new Researchers_ListView { name = "Trần văn C", avatar_id = 0, email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHN" });
            list.Add(new Researchers_ListView { name = "Trần văn C", avatar_id = 0, email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHN" });
            //var list = query.ToList<Researchers_ListView>();

            ViewBag.list = list;
            ////////////////////////////////////////////
            return View();
        }

        public ActionResult ViewInfo()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Nghiên cứu viên", "/Researchers"),
                new PageTree("Thông tin nghiên cứu viên", "/Researchers/ViewInfo"),
            };
            ViewBag.researcher_avt = "https://2.pik.vn/20217382d096-d98d-473d-acb6-9dc98a16f45b.jpg";
            ViewBag.researcher_name = "PGS. TS Phạm Hùng Quý";
            ViewBag.researcher_email = "quyph@fe.edu.vn";
            ViewBag.researcher_majors = "Giảng viên –  Nghiên cứu viên Toán học";
            ViewBag.researcher_workplace = "FPTU Hà Nội";
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public class Researchers_ListView
        {
            public string name { get; set; }
            public int? avatar_id { get; set; }
            public string email { get; set; }
            public string google_scholar { get; set; }
            public string website { get; set; }
            public string office_name { get; set; }
        }
    }
}
