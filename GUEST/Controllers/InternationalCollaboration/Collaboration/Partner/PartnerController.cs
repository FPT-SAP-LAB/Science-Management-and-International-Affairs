using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace GUEST.Controllers.InternationalCollaboration.Collaboration.Partner
{
    public class PartnerController : Controller
    {
        // GET: Partner
        public ActionResult List()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Danh sách đối tác", "Partner")
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        //demo code back end thì xóa đi
        //------------------------------------------------

        class Person
        {
            private string partner_name_mou_code; // field
            private string country_name; // field
            private string specialization; // field
            private string website; // field

            public Person()
            {
            }
            public Person(string partner_name_mou_code, string country_name, string specialization, string website)
            {
                this.partner_name_mou_code = partner_name_mou_code;
                this.country_name = country_name;
                this.specialization = specialization;
                this.website = website;
            }

            public string Partner_name_mou_code   // property
            {
                get { return partner_name_mou_code; }   // get method
                set { partner_name_mou_code = value; }  // set method
            }
            public string Country_name   // property
            {
                get { return country_name; }   // get method
                set { country_name = value; }  // set method
            }
            public string Specialization   // property
            {
                get { return specialization; }   // get method
                set { specialization = value; }  // set method
            }
            public string Website   // property
            {
                get { return website; }   // get method
                set { website = value; }  // set method
            }


        }
        //------------------------------------------------

        public ActionResult Load_List()
        {
            List<Person> list = new List<Person>();


            for (int i = 0; i < 100; i++)
            {
                Person temp = new Person();
                if (i % 2 == 0)
                {
                    temp = new Person("2021/01:SBI Graduate School", "Japan", "CNNT", "www");
                }
                else
                {
                    temp = new Person("2021/01:SBI Graduate School", "Japan", "CNNT", "https://");

                }
                list.Add(temp);
            }


            return Json(new { success = true, data = list }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Partner_Detail(string id)
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Danh sách đối tác", "/Partner/List"),
                new PageTree("Thông tin chi tiết đối tác", "/Partner/Partner_Detail?" + id)
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}