//using BLL.Authen;
//using BLL.ScienceManagement.Comment;
//using BLL.ScienceManagement.Invention;
//using BLL.ScienceManagement.MasterData;
//using BLL.ScienceManagement.Paper;
//using ENTITIES;
//using ENTITIES.CustomModels;
//using ENTITIES.CustomModels.ScienceManagement.Comment;
//using ENTITIES.CustomModels.ScienceManagement.Invention;
//using ENTITIES.CustomModels.ScienceManagement.Paper;
//using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using User.Models;

//namespace GUEST.Controllers
//{
//    public class InventionController : Controller
//    {
//        InventionRepo ir = new InventionRepo();
//        CommentRepo cr = new CommentRepo();
//        MasterDataRepo md = new MasterDataRepo();

//        public ActionResult AddRequest()
//        {
//            ViewBag.title = "Đăng ký khen thưởng bằng sáng chế";
//            var pagesTree = new List<PageTree>
//            {
//                new PageTree("Đăng ký khen thưởng bằng sáng chế","/Invention/AddRequest"),
//            };
//            ViewBag.pagesTree = pagesTree;

//            List<Country> listCountry = ir.getCountry();
//            ViewBag.country = listCountry;

//            List<InventionType> listType = ir.getType();
//            ViewBag.type = listType;

//            return View();
//        }

//        [HttpPost]
//        public ActionResult Edit(string id, string editable)
//        {
//            ViewBag.title = "Chỉnh sửa khen thưởng bằng sáng chế";
//            var pagesTree = new List<PageTree>
//            {
//                new PageTree("Chỉnh sửa khen thưởng bằng sáng chế","/Invention/Edit"),
//            };
//            ViewBag.pagesTree = pagesTree;
//            ViewBag.ckEdit = editable;

//            DetailInvention item = ir.getDetail(id);
//            ViewBag.item = item;

//            int request_id = item.request_id;
//            List<DetailComment> listCmt = cr.GetComment(request_id);
//            ViewBag.cmt = listCmt;
//            ViewBag.id = id;

//            List<Country> listCountry = ir.getCountry();
//            ViewBag.listCountry = listCountry;
//            ViewBag.request_id = request_id;

//            List<InventionType> listType = ir.getType();
//            ViewBag.type = listType;
//            ViewBag.invenID = item.invention_id;

//            return View();
//        }

//        [HttpPost]
//        public JsonResult listAuthor(string id)
//        {
//            List<AuthorInfoWithNull> listAuthor = ir.getAuthor(id);
//            return Json(new { author = listAuthor }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public JsonResult addFile(HttpPostedFileBase file, string name)
//        {
//            Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadResearcherFile(file, name, 3, "hieumv0163@gmail.com");
//            ENTITIES.File fl = new ENTITIES.File
//            {
//                link = f.WebViewLink,
//                file_drive_id = f.Id,
//                name = name
//            };
//            string mess = ir.addFile(fl);
//            return Json(new { mess = mess, id = fl.file_id }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public JsonResult addInven(List<AddAuthor> people, Invention inven, string type, string kieuthuong)
//        {
//            PaperRepo pr = new PaperRepo();
//            LoginRepo.User u = new LoginRepo.User();
//            Account acc = new Account();
//            if (Session["User"] != null)
//            {
//                u = (LoginRepo.User)Session["User"];
//                acc = u.account;
//            }
//            BaseRequest b = pr.addBaseRequest(acc.account_id);

//            InventionType ip = ir.addInvenType(type);
//            inven.type_id = ip.invention_type_id;
//            Invention i = ir.addInven(inven);

//            string mess = ir.addAuthor(people, i.invention_id);
//            if (mess == "ss") mess = ir.addInvenRequest(b, i, kieuthuong);

//            return Json(new { mess = mess, id = i.invention_id }, JsonRequestBehavior.AllowGet);
//        }

//        [HttpPost]
//        public JsonResult editInven(List<AddAuthor> people, Invention inven, string type, string kieuthuong, string request)
//        {
//            InventionType ip = ir.addInvenType(type);
//            inven.type_id = ip.invention_type_id;

//            string mess = ir.editInven(inven);
//            if (mess == "ss") mess = ir.editRequest(request, kieuthuong);
//            if (mess == "ss")
//            {
//                mess = ir.updateAuthor(inven.invention_id, people);
//            }

//            return Json(new { mess = mess, id = inven.invention_id }, JsonRequestBehavior.AllowGet);
//        }
//    }
//}