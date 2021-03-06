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
        public class ColorDeleteOutput
        {
            public List<ResponseMessage> Response { get; set; }
        }

        public class ResponseMessage
        {
            public string StyleNumber { get; set; }
            public string ColorLongDesc { get; set; }
            public string DistroDesc { get; set; }
            public string Status { get; set; }
        }

        public ActionResult Load_List()
        {
            List<Array> respmsg = new List<Array>();


            for (int i = 0; i < 100; i++)
            {
                string[] temp = { i + 1 + "", "SBI Graduate School", "Japan", "CNTT", "www"};
                respmsg.Add(temp);
                //respmsg.Add(new ResponseMessage { ColorLongDesc = i+"", StyleNumber = "124578", DistroDesc = "MARIO BRACKEN", Status = "SUCCESS" });
            }
            string sJSONResponse = JsonConvert.SerializeObject(respmsg);

            return Json(new { success = true, data = respmsg }, JsonRequestBehavior.AllowGet);
        }
    }
}