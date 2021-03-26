using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Conference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorDetailRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public string GetDetailPageGuest(int request_id, int language_id, int account_id = 0)
        {
            db.Configuration.LazyLoadingEnabled = false;
            ConferenceDetail Conference = (from r in db.BaseRequests
                                           join a in db.RequestConferences on r.request_id equals a.request_id
                                           join b in db.Conferences on a.conference_id equals b.conference_id
                                           join c in db.Countries on b.country_id equals c.country_id
                                           join d in db.Files on a.invitation_file_id equals d.file_id
                                           join e in db.Papers on a.paper_id equals e.paper_id
                                           join f in db.Files on e.file_id equals f.file_id
                                           join g in db.ConferenceStatus on a.status_id equals g.status_id
                                           join h in db.ConferenceStatusLanguages on g.status_id equals h.status_id
                                           join i in db.Formalities on b.formality_id equals i.formality_id
                                           join j in db.FormalityLanguages on i.formality_id equals j.formality_id
                                           join k in db.SpecializationLanguages on a.specialization_id equals k.specialization_id
                                           where h.language_id == language_id && j.language_id == language_id && k.language_id == language_id
                                           && (r.account_id == account_id || account_id == 0) && r.request_id == request_id
                                           select new ConferenceDetail
                                           {
                                               ConferenceName = b.conference_name,
                                               Website = b.website,
                                               KeynoteSpeaker = b.keynote_speaker,
                                               QsUniversity = b.qs_university,
                                               Co_organizedUnit = b.co_organized_unit,
                                               CreatedDate = r.created_date.Value,
                                               TimeEnd = b.time_end,
                                               TimeStart = b.time_start,
                                               AttendanceEnd = a.attendance_end,
                                               AttendanceStart = a.attendance_start,
                                               ConferenceID = a.conference_id,
                                               EditAble = a.editable,
                                               InvitationLink = d.link,
                                               PaperLink = f.link,
                                               PaperName = e.name,
                                               RequestID = a.request_id,
                                               CountryName = c.country_name,
                                               StatusName = h.name,
                                               StatusID = h.status_id,
                                               FormalityID = j.formality_id,
                                               FormalityName = j.name,
                                               Reimbursement = a.reimbursement,
                                               SpecializationName = k.name
                                           }).FirstOrDefault();
            if (Conference == null)
                return null;
            DecisionDetail DecisionDetail = null;
            if (Conference.StatusID >= 3 && Conference.FormalityID == 2)
            {
                DecisionDetail = (from a in db.RequestDecisions
                                  join b in db.Decisions on a.decision_id equals b.decision_id
                                  join c in db.Files on b.file_id equals c.file_id
                                  where a.request_id == request_id
                                  select new DecisionDetail
                                  {
                                      DecisionNumber = b.decision_number,
                                      Link = c.link,
                                      ValidDate = b.valid_date
                                  }).FirstOrDefault();
            }
            string Link = db.RequestConferencePolicies.Where(x => x.expired_date == null).Select(x => x.File).FirstOrDefault().link;
            List<ConferenceCriteria> Criterias = (from a in db.EligibilityCriterias
                                                  join b in db.ConferenceCriteriaLanguages on a.criteria_id equals b.criteria_id
                                                  where b.language_id == language_id && a.request_id == request_id
                                                  select new ConferenceCriteria
                                                  {
                                                      CriteriaID = a.criteria_id,
                                                      CriteriaName = b.name,
                                                      IsAccepted = a.is_accepted
                                                  }).ToList();
            List<ConferenceParticipantExtend> Participants = (from b in db.ConferenceParticipants
                                                              join c in db.TitleLanguages on b.title_id equals c.title_id
                                                              join d in db.People on b.people_id equals d.people_id
                                                              join e in db.Offices on b.office_id equals e.office_id
                                                              where b.request_id == request_id
                                                              select new ConferenceParticipantExtend
                                                              {
                                                                  ID = b.current_mssv_msnv,
                                                                  FullName = d.name,
                                                                  OfficeName = e.office_name,
                                                                  TitleName = c.name
                                                              }).ToList();
            for (int i = 0; i < Participants.Count; i++)
            {
                Participants[i].RowNumber = 1 + i;
            }
            var Costs = db.Costs.Where(x => x.request_id == request_id).ToList();
            var ApprovalProcesses = (from a in db.ApprovalProcesses
                                     join b in db.Accounts on a.account_id equals b.account_id
                                     join c in db.PositionLanguages.Where(x => x.language_id == language_id) on a.position_id equals c.position_id into Processes
                                     from d in Processes.DefaultIfEmpty()
                                     where a.request_id == request_id
                                     select new ConferenceApprovalProcess
                                     {
                                         CreatedDate = a.created_date,
                                         PositionName = d == null ? "Sinh viên" : d.name,
                                         FullName = b.full_name,
                                         Comment = a.comment
                                     }).ToList();
            return JsonConvert.SerializeObject(new { Conference, Participants, Costs, ApprovalProcesses, Link, Criterias, DecisionDetail });
        }
        public AlertModal<string> UpdateCriterias(string criterias, int request_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    var CriteriaIDs = JsonConvert.DeserializeObject<List<int>>(criterias);
                    var Request = db.RequestConferences.Find(request_id);
                    if (Request == null)
                        return new AlertModal<string>(false, "Đề nghị không tồn tại");
                    if (Request.status_id != 1)
                        return new AlertModal<string>(false, "Đề nghị đã đóng xét duyệt");
                    var ListCri = Request.EligibilityCriterias;
                    foreach (var item in ListCri)
                        if (CriteriaIDs.Contains(item.criteria_id)) item.is_accepted = true;
                    db.SaveChanges();
                    if (ListCri.All(x => x.is_accepted))
                        Request.status_id = 2;
                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true, "Cập nhật thành công");
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        public AlertModal<string> UpdateCosts(string costs, int request_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    var CostIDs = JsonConvert.DeserializeObject<List<int>>(costs);
                    var Request = db.RequestConferences.Find(request_id);
                    if (Request == null)
                        return new AlertModal<string>(false, "Đề nghị không tồn tại");
                    if (Request.status_id != 2)
                        return new AlertModal<string>(false, "Đề nghị đã đóng chi phí");
                    var ListCosts = Request.Costs;
                    foreach (var item in ListCosts)
                    {
                        if (CostIDs.Contains(item.cost_id))
                        {
                            item.is_accepted = true;
                            item.editable = false;
                        }
                        else item.editable = true;
                    }
                    db.SaveChanges();
                    if (ListCosts.All(x => x.is_accepted))
                    {
                        if (Request.Conference.formality_id == 2)
                            Request.status_id = 3;
                        else
                            Request.status_id = 4;
                    }
                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true, "Cập nhật thành công");
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        public AlertModal<string> RequestEdit(int request_id)
        {
            var Request = db.RequestConferences.Find(request_id);
            if (Request == null)
                return new AlertModal<string>(false, "Đề nghị không tồn tại");
            if (Request.status_id != 2)
                return new AlertModal<string>(false, "Đề nghị đã đóng chi phí");
            Request.editable = true;
            db.SaveChanges();
            return new AlertModal<string>(true, "Cập nhật thành công");
        }
        public AlertModal<string> SubmitPolicy(HttpPostedFileBase decision_file, string valid_date, string decision_number, int request_id, int account_id)
        {
            string DriveId = null;
            var temp = (from a in db.BaseRequests
                        join b in db.RequestConferences on a.request_id equals b.request_id
                        join c in db.Accounts on a.account_id equals c.account_id
                        join d in db.Conferences on b.conference_id equals d.conference_id
                        where c.account_id == account_id && a.request_id == request_id
                        select new
                        {
                            d.conference_name,
                            c.email,
                            b.status_id,
                            d.formality_id
                        }).FirstOrDefault();
            if (temp == null)
                return new AlertModal<string>(false, "Đề nghị không tồn tại");
            if (temp.status_id != 3)
                return new AlertModal<string>(false, "Trạng thái của quyết định không được phép đăng quyết định");
            if (temp.formality_id != 2)
                return new AlertModal<string>(false, "Loại quyết định không được phép đăng quyết định");
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    Google.Apis.Drive.v3.Data.File drive = GlobalUploadDrive.UploadResearcherFile(decision_file, temp.conference_name, 1, temp.email);
                    DriveId = drive.DriveId;
                    File file = new File
                    {
                        file_drive_id = drive.Id,
                        link = drive.WebViewLink,
                        name = decision_file.FileName
                    };
                    db.Files.Add(file);
                    db.SaveChanges();
                    Decision decision = new Decision
                    {
                        decision_number = decision_number,
                        valid_date = DateTime.ParseExact(valid_date, "dd/MM/yyyy", null),
                        file_id = file.file_id
                    };
                    db.Decisions.Add(decision);
                    db.SaveChanges();
                    db.RequestDecisions.Add(new RequestDecision
                    {
                        decision_id = decision.decision_id,
                        request_id = request_id
                    });
                    db.RequestConferences.Find(request_id).status_id = 4;
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    GlobalUploadDrive.DeleteFile(DriveId);
                    trans.Rollback();
                    return new AlertModal<string>(false, "Có lỗi xảy ra");
                }
            }
            return new AlertModal<string>(true);
        }
    }
}
