using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;
using BLL.InternationalCollaboration.AcademicCollaborationRepository;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicCollaborationEntities;
using ENTITIES.CustomModels;
using GUEST.Support;

namespace GUEST.Controllers.InternationalCollaboration.AcademicCollaboration
{
    public class AcademicCollaborationController : Controller
    {
        private AcademicCollaborationGuestRepo guestRepo;
        private System.Resources.ResourceManager rm = Models.LanguageResource.GetResourceManager();
        // GET: AcademicCollaboration
        [Auther(RightID = "2,3,6")]
        public ActionResult Long_Term()
        {
            guestRepo = new AcademicCollaborationGuestRepo();
            int language = Models.LanguageResource.GetCurrentLanguageID();
            var pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("LongTerm"), "/AcademicCollaboration/Long_Term"),
            };
            ViewBag.Title = rm.GetString("LongTerm");
            ViewBag.YearSearching = guestRepo.yearSearching();
            ViewBag.TypeDescription = guestRepo.getTypeDescription(2, language);
            ViewBag.ListProgram = guestRepo.listProgram(0, 2, 0, null, 0, language);
            ViewBag.ListCountry = guestRepo.listCountry();
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        [HttpPost]
        [Auther(RightID = "2,3,6")]
        public ActionResult Load_More_List_Long_Term(int count, string countrystr, string partner, string yearstr)
        {
            guestRepo = new AcademicCollaborationGuestRepo();
            int country = 0;
            int year = 0;
            if (!String.IsNullOrEmpty(countrystr))
            {
                country = Int32.Parse(countrystr);
            }
            if (!String.IsNullOrEmpty(yearstr))
            {
                year = Int32.Parse(yearstr);
            }
            int language = Models.LanguageResource.GetCurrentLanguageID();
            List<ProgramInfo> data = guestRepo.listProgram(count, 2, country, partner, year, language);
            return Json(data);
        }
        [HttpPost]
        [Auther(RightID = "2,3,6")]
        public ActionResult Partner_Program(int collabtype, string partner, string yearstr, string countrystr)
        {
            guestRepo = new AcademicCollaborationGuestRepo();
            BaseDatatable baseDatatable = new BaseDatatable(Request);
            int language = Models.LanguageResource.GetCurrentLanguageID();
            int year = 0;
            int country = 0;
            if (!String.IsNullOrEmpty(yearstr))
            {
                year = Int32.Parse(yearstr);
            }
            if (!String.IsNullOrEmpty(countrystr))
            {
                country = Int32.Parse(countrystr);
            }
            BaseServerSideData<ProgramInfo> baseServerSideData = guestRepo.listPartnerProgram(baseDatatable, collabtype, partner, year, country, language);
            return Json(new
            {
                success = true,
                data = baseServerSideData.Data,
                draw = Request["draw"],
                recordsTotal = baseServerSideData.RecordsTotal,
                recordsFiltered = baseServerSideData.RecordsTotal
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Auther(RightID = "2,3,6")]
        public ActionResult FPT_Program(int collabtype, string yearstr)
        {
            guestRepo = new AcademicCollaborationGuestRepo();
            BaseDatatable baseDatatable = new BaseDatatable(Request);
            int year = 0;
            if (!String.IsNullOrEmpty(yearstr))
            {
                year = Int32.Parse(yearstr);
            }
            int language = Models.LanguageResource.GetCurrentLanguageID();
            BaseServerSideData<ProgramInfo> baseServerSideData = guestRepo.listFPTProgram(baseDatatable, collabtype, year, language);
            return Json(new
            {
                success = true,
                data = baseServerSideData.Data,
                draw = Request["draw"],
                recordsTotal = baseServerSideData.RecordsTotal,
                recordsFiltered = baseServerSideData.RecordsTotal
            }, JsonRequestBehavior.AllowGet);
        }
        [Auther(RightID = "2,3,6")]
        public ActionResult Short_Term()
        {
            guestRepo = new AcademicCollaborationGuestRepo();
            int language = Models.LanguageResource.GetCurrentLanguageID();
            var pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("ShortTerm"), "/AcademicCollaboration/Short_Term"),
            };
            ViewBag.Title = rm.GetString("ShortTerm");
            ViewBag.YearSearching = guestRepo.yearSearching();
            ViewBag.TypeDescription = guestRepo.getTypeDescription(1, language);
            ViewBag.ListProgram = guestRepo.listProgram(0, 1, 0, null, 0, language);
            ViewBag.ListCountry = guestRepo.listCountry();
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        [Auther(RightID = "2,3,6")]
        public ActionResult Load_More_List_Short_Term(int count, string countrystr, string partner, string yearstr)
        {
            guestRepo = new AcademicCollaborationGuestRepo();
            int country = 0;
            int year = 0;
            if (!String.IsNullOrEmpty(countrystr))
            {
                country = Int32.Parse(countrystr);
            }
            if (!String.IsNullOrEmpty(yearstr))
            {
                year = Int32.Parse(yearstr);
            }
            int language = Models.LanguageResource.GetCurrentLanguageID();
            List<ProgramInfo> data = guestRepo.listProgram(count, 1, country, partner, year, language);
            return Json(data);
        }
        [HttpPost]
        [Auther(RightID = "2,3,6")]
        public ActionResult Procedure_List(int direction, string title)
        {
            guestRepo = new AcademicCollaborationGuestRepo();
            BaseDatatable baseDatatable = new BaseDatatable(Request);
            int language = Models.LanguageResource.GetCurrentLanguageID();
            BaseServerSideData<ProcedureInfo> baseServerSideData = guestRepo.listProcedure(baseDatatable, title, direction, language);
            return Json(new
            {
                success = true,
                data = baseServerSideData.Data,
                draw = Request["draw"],
                recordsTotal = baseServerSideData.RecordsTotal,
                recordsFiltered = baseServerSideData.RecordsTotal
            }, JsonRequestBehavior.AllowGet);
        }
        [Auther(RightID = "2,3,6")]
        public ActionResult Program_Detail(string id)
        {
            guestRepo = new AcademicCollaborationGuestRepo();
            var pagesTree = new List<PageTree>();
            int language = Models.LanguageResource.GetCurrentLanguageID();
            ProgramInfo pi = guestRepo.programInfo(id, language);
            if (pi is null)
            {
                pi = new ProgramInfo()
                {
                    program_name = rm.GetString("EmptyContentName"),
                    content = rm.GetString("EmptyContentDetail")
                };
            }
            ViewBag.Program = pi;
            if (pi.collab_type_id == 2 & pi.direction_id == 1)
            {
                pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("LongTerm"), "/AcademicCollaboration/Long_Term"),
                new PageTree(rm.GetString("LongTermPartnerProgram"), "/AcademicCollaboration/Program_Detail"),
            };
            }
            else if (pi.collab_type_id == 2 & pi.direction_id == 2)
            {
                pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("LongTerm"), "/AcademicCollaboration/Long_Term"),
                new PageTree(rm.GetString("LongTermFPTProgram"), "/AcademicCollaboration/Program_Detail"),
            };
            }
            else if (pi.collab_type_id == 1 & pi.direction_id == 1)
            {
                pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("ShortTerm"), "/AcademicCollaboration/Short_Term"),
                new PageTree(rm.GetString("ShortTermPartnerProgram"), "/AcademicCollaboration/Program_Detail"),
            };
            }
            else if (pi.collab_type_id == 1 & pi.direction_id == 2)
            {
                pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("ShortTerm"), "/AcademicCollaboration/Short_Term"),
                new PageTree(rm.GetString("ShortTermFPTProgram"), "/AcademicCollaboration/Program_Detail"),
            };
            }
            else
            {
                pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("LongTerm"), "/AcademicCollaboration/Short_Term"),
            };
            }
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        [Auther(RightID = "2,3,6")]
        public ActionResult Procedure_Detail(int id)
        {

            guestRepo = new AcademicCollaborationGuestRepo();
            var pagesTree = new List<PageTree>();
            int language = Models.LanguageResource.GetCurrentLanguageID();
            ProcedureInfo pi = guestRepo.procedureInfo(id, language);
            if (pi is null)
            {
                pi = new ProcedureInfo()
                {
                    procedure_name = rm.GetString("EmptyContentName"),
                    content = rm.GetString("EmptyContentDetail")
                };
            }
            int type_procedure = pi.direction_id;
            ViewBag.Procedure = pi;
            switch (type_procedure)
            {
                case 1:
                    pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("ShortTerm"), "/AcademicCollaboration/Short_Term"),
                new PageTree(rm.GetString("PartnerProcedure"), "/AcademicCollaboration/Procedure_Detail"),
                //new PageTree("Đào tạo của đối tác", "/AcademicCollaboration/Procedure_Detail?id=" + id),
            };
                    break;
                case 2:
                    pagesTree = new List<PageTree>
            {
                new PageTree(rm.GetString("ShortTerm"), "/AcademicCollaboration/Short_Term"),
                new PageTree(rm.GetString("FPTProcedure"), "/AcademicCollaboration/Procedure_Detail"),
                //new PageTree("Đào tạo của FPT", "/AcademicCollaboration/Procedure_Detail?id=" + id),
            };
                    break;
            }
            ViewBag.pagesTree = pagesTree;
            return View();
        }
    }
}