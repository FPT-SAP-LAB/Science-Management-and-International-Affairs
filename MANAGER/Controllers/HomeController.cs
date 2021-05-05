using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MANAGER.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
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
                return View();
            }
            else
            {
                return Redirect("/Authen/Login");
            }
        }
    }
}