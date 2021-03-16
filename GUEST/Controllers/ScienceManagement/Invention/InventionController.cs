﻿using BLL.ScienceManagement.Comment;
using BLL.ScienceManagement.Invention;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using ENTITIES.CustomModels.ScienceManagement.Invention;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers
{
    public class InventionController : Controller
    {
        InventionRepo ir = new InventionRepo();
        CommentRepo cr = new CommentRepo();

        public ActionResult AddRequest()
        {
            ViewBag.title = "Đăng ký khen thưởng bằng sáng chế";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đăng ký khen thưởng bằng sáng chế","/Invention/AddRequest"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(string id, string editable)
        {
            ViewBag.title = "Chỉnh sửa khen thưởng bằng sáng chế";
            var pagesTree = new List<PageTree>
            {
                new PageTree("Chỉnh sửa khen thưởng bằng sáng chế","/Invention/Edit"),
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.ckEdit = editable;

            DetailInvention item = ir.getDetail(id);
            ViewBag.item = item;

            int request_id = item.request_id;
            List<DetailComment> listCmt = cr.getComment(request_id);
            ViewBag.cmt = listCmt;

            List<Country> listCountry = ir.getCountry();
            ViewBag.listCountry = listCountry;

            List<AuthorInfo> listAuthor = ir.getAuthor(id);
            ViewBag.listAuthor = listAuthor;
            ViewBag.numberAuthor = listAuthor.Count();

            return View();
        }
    }
}