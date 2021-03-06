﻿using BLL.ScienceManagement.Dashboard;
using ENTITIES.CustomModels.ScienceManagement.Dashboard;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class HomeController : Controller
    {
        private readonly string[] Criterias = new string[] { "Q1", "Q2", "Q3", "Q4" };
        public ActionResult Index(int? year)
        {
            if (Request.IsAuthenticated && Session["User"] != null)
            {
                int this_year = DateTime.Today.Year;

                List<int> year_select = new List<int>
                {
                    this_year + 2,
                    this_year + 1,
                    this_year
                };
                for (int i = 1; i <= (this_year - 2015); i++)
                {
                    year_select.Add(this_year - i);
                }
                ViewBag.year_select = year_select;

                DashboardRepo dashboardRepo = new DashboardRepo();
                PaperByOffice paperByOffice = dashboardRepo.GetPaperByOffices(Criterias, year);
                Dictionary<string, int> specialiNumPaper = dashboardRepo.GetPaperBySpecializations(year);
                ViewBag.paperByOffice = paperByOffice;
                ViewBag.specialiNumPaper = specialiNumPaper;

                return View();
            }
            else
            {
                return Redirect("/Authen/Login");
            }
        }
    }
}