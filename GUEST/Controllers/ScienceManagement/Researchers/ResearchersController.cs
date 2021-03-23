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
            Random rand = new Random();
            List<string> imgs = new List<string>(new string[]
            {
                "https://2.pik.vn/2021a5e55746-085b-4e95-b1ba-29f8cc1ee953.jpg",
                "https://2.pik.vn/2021cab49afe-0dbf-4b48-b42b-0eaa4bd60179.jpg",
                "https://2.pik.vn/20217c6a2670-4843-4def-8690-087bf06b4a61.jpg",
                "https://2.pik.vn/202171fbc1d7-2f00-4b65-ba44-e3d6565eb655.jpg",
                "https://2.pik.vn/20213166d763-230c-40f5-a13b-b614bf75b02d.jpg",
                "https://2.pik.vn/2021f4a23177-e585-4fbf-b6ba-ddab70a664a1.jpg",
                "https://2.pik.vn/2021795bbdf6-6fb5-4601-8ae3-91f360b84587.jpg",
                "https://2.pik.vn/2021b64863ac-8173-43d1-a2e1-367548689138.jpg",
                "https://2.pik.vn/202131187ccc-42db-4c50-a696-c741967614bb.jpg"
            });
            var list = (from p in db.Profiles
                        join rp in db.People on p.people_id equals rp.people_id
                        join o in db.Offices on p.office_id equals o.office_id
                        select new Researchers_ListView
                        {
                            name = rp.name,
                            avatar_id = p.avatar_id,
                            email = rp.email,
                            google_scholar = p.google_scholar,
                            website = p.website,
                            office_name = o.office_name,
                        }).ToList<Researchers_ListView>();
            foreach (var r in list)
            {
                r.avatar_img = imgs.ElementAt(rand.Next(imgs.Count));
            }
            list.Add(new Researchers_ListView { name = "Bùi Ngọc Anh", avatar_id = 0, avatar_img = "https://2.pik.vn/20217e3790eb-97e1-4121-b6f8-81235ac0fa9a.jpg", email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHN" });
            list.Add(new Researchers_ListView { name = "Đoàn Văn Thắng", avatar_id = 0, avatar_img = "https://2.pik.vn/2021cab49afe-0dbf-4b48-b42b-0eaa4bd60179.jpg", email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHCM" });
            list.Add(new Researchers_ListView { name = "Nguyễn Bá Sơn", avatar_id = 0, avatar_img = "https://2.pik.vn/2021f4a23177-e585-4fbf-b6ba-ddab70a664a1.jpg", email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FPoly HN" });
            list.Add(new Researchers_ListView { name = "Phạm Đặng Dũng", avatar_id = 0, avatar_img = "https://2.pik.vn/2021795bbdf6-6fb5-4601-8ae3-91f360b84587.jpg", email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHCM" });
            list.Add(new Researchers_ListView { name = "Phan Lạc Dương", avatar_id = 0, avatar_img = "https://2.pik.vn/2021b64863ac-8173-43d1-a2e1-367548689138.jpg", email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FPTU Cần Thơ" });
            list.Add(new Researchers_ListView { name = "Đoàn Thị Thuý Nguyên", avatar_id = 0, avatar_img = "https://2.pik.vn/2021a5e55746-085b-4e95-b1ba-29f8cc1ee953.jpg", email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHN" });
            list.Add(new Researchers_ListView { name = "Nguyễn Văn Sơn", avatar_id = 0, avatar_img = "https://2.pik.vn/202171fbc1d7-2f00-4b65-ba44-e3d6565eb655.jpg", email = "anhbn@fe.edu.vn", google_scholar = "#", website = "#", office_name = "FUHN" });

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
        public ActionResult EditInfo()
        {
            var pagesTree = new List<PageTree>
           {
               new PageTree("Trang cá nhân", "/Researchers/ViewInfo"),
               new PageTree("Chỉnh sửa thông tin", "/Researchers/EditInfo"),
           }; 
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public class Researchers_ListView
        {
            public string name { get; set; }
            public int? avatar_id { get; set; }

            public string avatar_img { get; set; }

            public string email { get; set; }
            public string google_scholar { get; set; }
            public string website { get; set; }
            public string office_name { get; set; }
        }
    }
}