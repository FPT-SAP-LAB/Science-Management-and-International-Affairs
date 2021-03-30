using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorEditRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public string EditRequestConference(int account_id, string input, HttpPostedFileBase invite, HttpPostedFileBase paper, int request_id)
        {
            db = new ScienceAndInternationalAffairsEntities();

            RequestConference request = db.RequestConferences.Where(x => x.request_id == request_id && x.BaseRequest.account_id == account_id).FirstOrDefault();
            if (request == null)
                return JsonConvert.SerializeObject(new { success = false, message = "Đề nghị không tồn tại" });
            if (request.status_id >= 2 || !request.editable)
                return JsonConvert.SerializeObject(new { success = false, message = "Đề nghị không thể chỉnh sửa" });

            List<string> FileIDs = new List<string>();
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    DataTable dt = new DataTable();
                    JObject @object = JObject.Parse(input);

                    Conference conference = @object["Conference"].ToObject<Conference>();
                    Conference temp = db.Conferences.Find(conference.conference_id);
                    if (temp != null)
                    {
                        conference.conference_id = temp.conference_id;
                    }
                    else
                    {
                        conference.is_verified = false;
                        db.Conferences.Add(conference);
                        db.SaveChanges();
                        request.conference_id = conference.conference_id;
                    }

                    if (invite != null)
                    {
                        GoogleDriveService.UpdateFile(invite.FileName, invite.InputStream, invite.ContentType, request.File.file_drive_id);
                        request.File.name = invite.FileName;
                    }
                    if (paper != null)
                    {
                        GoogleDriveService.UpdateFile(paper.FileName, paper.InputStream, paper.ContentType, request.Paper.File.file_drive_id);
                        request.Paper.File.name = paper.FileName;
                    }

                    request.editable = false;
                    request.attendance_start = DateTime.Parse(@object["attendance_start"].ToString());
                    request.attendance_end = DateTime.Parse(@object["attendance_end"].ToString());
                    request.specialization_id = int.Parse(@object["specialization_id"].ToString());

                    db.Costs.RemoveRange(request.Costs);
                    List<Cost> costs = @object["Cost"].ToObject<List<Cost>>();
                    foreach (var item in costs)
                    {
                        int total = int.Parse(dt.Compute(item.detail.Replace(",", ""), "").ToString());
                        item.editable = false;
                        item.sponsoring_organization = "FPTU";
                        item.total = total;
                        item.request_id = request.request_id;
                    }
                    db.Costs.AddRange(costs);

                    ConferenceParticipant participant = @object["ConferenceParticipant"].ToObject<ConferenceParticipant>();
                    participant.request_id = request_id;
                    Person person = @object["Persons"].ToObject<Person>();
                    Profile profile = db.Profiles.Where(x => x.mssv_msnv == participant.current_mssv_msnv).FirstOrDefault();
                    if (profile == null)
                    {
                        db.People.Add(person);
                        db.SaveChanges();

                        profile = new Profile()
                        {
                            mssv_msnv = participant.current_mssv_msnv,
                            office_id = participant.office_id,
                            people_id = person.people_id,
                        };
                        profile.Titles.Add(db.Titles.Find(participant.title_id));
                        db.Profiles.Add(profile);
                    }
                    else
                    {
                        participant.office_id = profile.office_id;
                        participant.people_id = profile.people_id;
                        participant.title_id = profile.Titles.First().title_id;
                    }

                    db.ConferenceParticipants.RemoveRange(request.ConferenceParticipants);
                    db.ConferenceParticipants.Add(participant);

                    foreach (var item in request.EligibilityCriterias)
                    {
                        item.is_accepted = false;
                    }
                    db.SaveChanges();

                    trans.Commit();
                    return JsonConvert.SerializeObject(new { success = true, message = "OK", id = request.request_id });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                    foreach (var item in FileIDs)
                    {
                        GoogleDriveService.DeleteFile(item);
                    }
                    return JsonConvert.SerializeObject(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
        }
    }
}
