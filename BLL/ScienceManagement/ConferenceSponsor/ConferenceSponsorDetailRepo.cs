using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Conference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                                           join h in db.ConferenceStatusLanguages on a.status_id equals h.status_id
                                           join j in db.FormalityLanguages on b.formality_id equals j.formality_id
                                           join k in db.SpecializationLanguages on a.specialization_id equals k.specialization_id
                                           where h.language_id == language_id && j.language_id == language_id && k.language_id == language_id
                                           && (r.account_id == account_id || account_id == 0) && r.request_id == request_id
                                           select new ConferenceDetail
                                           {
                                               ConferenceName = b.conference_name,
                                               Website = b.website,
                                               KeynoteSpeaker = b.keynote_speaker,
                                               QsUniversity = b.qs_university,
                                               OrganizedUnit = b.organized_unit,
                                               Co_organizedUnit = b.co_organized_unit,
                                               CreatedDate = r.created_date.Value,
                                               FinishedDate = r.finished_date,
                                               TimeEnd = b.time_end,
                                               TimeStart = b.time_start,
                                               AttendanceEnd = a.attendance_end,
                                               AttendanceStart = a.attendance_start,
                                               ConferenceID = a.conference_id,
                                               EditAble = a.editable,
                                               InvitationLink = d.link,
                                               InvitationFileName = d.name,
                                               PaperLink = f.link,
                                               PaperFileName = f.name,
                                               PaperName = e.name,
                                               FolderLink = a.folder_id == null ? null : a.File1.link,
                                               RequestID = a.request_id,
                                               CountryID = c.country_id,
                                               CountryName = c.country_name,
                                               StatusName = h.name,
                                               StatusID = h.status_id,
                                               FormalityID = j.formality_id,
                                               FormalityName = j.name,
                                               Reimbursement = a.reimbursement,
                                               SpecializationID = k.specialization_id,
                                               SpecializationName = k.name,
                                               AccountID = account_id
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
            PolicyRepo policyRepo = new PolicyRepo();
            string Link = policyRepo.GetCurrentLink(1, db);
            List<ConferenceCriteria> Criterias = (from a in db.EligibilityConditions
                                                  join b in db.ConferenceConditionLanguages on a.condition_id equals b.condition_id
                                                  where b.language_id == language_id && a.request_id == request_id
                                                  select new ConferenceCriteria
                                                  {
                                                      CriteriaID = a.condition_id,
                                                      CriteriaName = b.name,
                                                      IsAccepted = a.is_accepted
                                                  }).ToList();
            ConferenceParticipantExtend Participants = (from b in db.ConferenceParticipants
                                                        join c in db.TitleLanguages on b.title_id equals c.title_id
                                                        join e in db.Offices on b.office_id equals e.office_id
                                                        where b.request_id == request_id && c.language_id == language_id
                                                        select new ConferenceParticipantExtend
                                                        {
                                                            ID = b.mssv_msnv,
                                                            FullName = b.name,
                                                            OfficeName = e.office_name,
                                                            OfficeID = e.office_id,
                                                            TitleName = c.name,
                                                            TitleID = c.title_id,
                                                            Email = b.email
                                                        }).FirstOrDefault();
            var Costs = db.Costs.Where(x => x.request_id == request_id).ToList();
            //  Account microsoft sẽ không có profile, account FU sẽ ưu tiên lấy chức vụ => sinh viên
            var ApprovalProcesses = (from a in db.ApprovalProcesses
                                     join b in db.Accounts on a.account_id equals b.account_id
                                     join e in db.Profiles on b.account_id equals e.account_id into Processes
                                     from d in Processes.DefaultIfEmpty()
                                     where a.request_id == request_id
                                     select new ConferenceApprovalProcess
                                     {
                                         CreatedDate = a.created_date,
                                         PositionName = d != null ?
                                         (d.PeoplePositions.Count == 0 ? "Sinh viên"
                                         : db.PeoplePositions.FirstOrDefault().Position.PositionLanguages.Where(x => x.language_id == language_id).FirstOrDefault().name
                                         )
                                         : db.PositionLanguages.Where(x => x.Position.position_id == b.position_id && x.language_id == language_id).FirstOrDefault().name,
                                         FullName = d == null ? b.full_name : d.Person.name,
                                         Comment = a.comment
                                     }).ToList();

            double Budget = 30000000 - db.ConferenceParticipants
                .Where(x => x.RequestConference.BaseRequest.finished_date.Value.Year == DateTime.Now.Year && x.mssv_msnv.Equals(Participants.ID))
                .Select(x => x.RequestConference.reimbursement).ToList().Sum();
            return JsonConvert.SerializeObject(new { Conference, Participants, Costs, ApprovalProcesses, Link, Criterias, DecisionDetail, Budget });
        }
        public AlertModal<string> UpdateCriterias(string criterias, int request_id, int account_id, string comment)
        {
            int? position_id = PositionRepo.GetPositionIdByAccountId(db, account_id);
            if (position_id == null)
                return new AlertModal<string>(false, "Tài khoản chưa có chức vụ");

            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    var CriteriaIDs = JsonConvert.DeserializeObject<List<int>>(criterias);
                    if (CriteriaIDs.Count == 0)
                        return new AlertModal<string>(true);

                    var Request = db.RequestConferences.Find(request_id);
                    if (Request == null)
                        return new AlertModal<string>(false, "Đề nghị không tồn tại");
                    if (Request.status_id != 1)
                        return new AlertModal<string>(false, "Đề nghị đã đóng xét duyệt");
                    var ListCri = Request.EligibilityConditions.Where(x => !x.is_accepted && CriteriaIDs.Contains(x.condition_id));

                    if (ListCri.Count() > 0)
                    {
                        foreach (var item in ListCri)
                        {
                            item.is_accepted = true;
                        }
                        db.SaveChanges();
                        if (Request.EligibilityConditions.All(x => x.is_accepted))
                        {
                            Request.status_id = 2;
                            Request.Conference.is_verified = true;
                        }

                        ApprovalProcessRepo.Add(db, account_id, DateTime.Now, position_id, request_id, comment);

                        db.SaveChanges();
                        trans.Commit();
                    }
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                    return new AlertModal<string>(false);
                }
            }
        }
        public AlertModal<string> UpdateCosts(string costs, int request_id, int account_id, string comment)
        {
            int? position_id = PositionRepo.GetPositionIdByAccountId(db, account_id);
            if (position_id == null)
                return new AlertModal<string>(false, "Tài khoản chưa có chức vụ");

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

                    ApprovalProcessRepo.Add(db, account_id, DateTime.Now, position_id, request_id, comment);

                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true, "Cập nhật thành công");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                    throw;
                }
            }
        }
        public AlertModal<string> RequestEdit(int request_id)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    NotificationRepo notificationRepo = new NotificationRepo(db);

                    var Request = db.RequestConferences.Find(request_id);
                    Account account = Request.BaseRequest.Account;

                    if (Request == null)
                        return new AlertModal<string>(false, "Đề nghị không tồn tại");
                    if (Request.status_id >= 2)
                        return new AlertModal<string>(false, "Đề nghị đã đóng chỉnh sửa");
                    Request.editable = true;
                    int notification_id = notificationRepo.AddByAccountID(account.account_id, 4, "/ConferenceSponsor/Detail?id=" + Request.request_id, false);
                    trans.Commit();

                    return new AlertModal<string>(notification_id.ToString(), true, "Cập nhật thành công");
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    Console.WriteLine(e.ToString());
                }
            }
            return new AlertModal<string>(false);
        }
        public AlertModal<string> SubmitPolicy(HttpPostedFileBase decision_file, string valid_date, string decision_number, int request_id)
        {
            string DriveId = null;
            var temp = (from a in db.BaseRequests
                        join b in db.RequestConferences on a.request_id equals b.request_id
                        join c in db.Accounts on a.account_id equals c.account_id
                        join d in db.Conferences on b.conference_id equals d.conference_id
                        where a.request_id == request_id
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
                    Google.Apis.Drive.v3.Data.File drive = GoogleDriveService.UploadResearcherFile(decision_file, temp.conference_name, 1, temp.email);
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
                    GoogleDriveService.DeleteFile(DriveId);
                    trans.Rollback();
                    return new AlertModal<string>(false, "Có lỗi xảy ra");
                }
            }
            return new AlertModal<string>(true);
        }
        public AlertModal<string> SubmitFiles(List<HttpPostedFileBase> files, int request_id)
        {
            RequestConference request = db.RequestConferences.Find(request_id);
            if (request == null)
                return new AlertModal<string>(false, "Đề nghị không tồn tại");
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                List<string> ids = new List<string>();
                try
                {
                    string parentID;
                    if (request.folder_id == null)
                    {
                        parentID = GoogleDriveService.GetFile(request.File.file_drive_id).Parents[0];
                        string parentLink = GoogleDriveService.GetFile(parentID).WebViewLink;
                        File file = new File
                        {
                            file_drive_id = parentID,
                            link = parentLink
                        };
                        request.File1 = file;
                        db.SaveChanges();
                    }
                    else
                    {
                        parentID = request.File1.file_drive_id;
                    }
                    foreach (var item in files)
                    {
                        Google.Apis.Drive.v3.Data.File temp = GoogleDriveService.UploadFile(item.FileName, item.InputStream, item.ContentType, parentID);
                        ids.Add(temp.Id);
                    }
                    trans.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    Console.WriteLine(e.ToString());
                    foreach (var item in ids)
                    {
                        GoogleDriveService.DeleteFile(item);
                    }
                }
            }
            return new AlertModal<string>(false);
        }
        public AlertModal<string> SubmitReimbursement(string reimbursement_string, int request_id)
        {
            reimbursement_string = reimbursement_string.Replace(",", "");
            if (!int.TryParse(reimbursement_string, out int reimbursement))
                return new AlertModal<string>(false, "Tiền hoàn ứng không hợp lệ");
            if (reimbursement <= 0)
                return new AlertModal<string>(false, "Tiền hoàn ứng không hợp lệ");
            RequestConference request = db.RequestConferences.Find(request_id);
            if (request == null)
                return new AlertModal<string>(false, "Đề nghị không tồn tại");
            if (request.status_id != 4)
                return new AlertModal<string>(false, "Đề nghị không được nhập hoàn ứng");

            try
            {
                request.reimbursement = reimbursement;
                db.SaveChanges();
                return new AlertModal<string>(true, "Cập nhật thành công");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<string>(false, "Có lỗi xảy ra");
            }
        }
        public AlertModal<string> EndRequest(int request_id)
        {
            RequestConference request = db.RequestConferences.Find(request_id);
            if (request == null)
                return new AlertModal<string>(false, "Đề nghị không tồn tại");
            if (request.status_id != 4)
                return new AlertModal<string>(false, "Đề nghị không được phép kết thúc");

            try
            {
                request.status_id = 5;
                request.BaseRequest.finished_date = DateTime.Now;
                db.SaveChanges();
                return new AlertModal<string>(true, "Cập nhật thành công");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<string>(false, "Có lỗi xảy ra");
            }
        }
    }
}
