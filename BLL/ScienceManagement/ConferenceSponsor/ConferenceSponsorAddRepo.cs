using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorAddRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public string GetAddPageJson(int account_id, int language_id)
        {
            var Link = db.RequestConferencePolicies.Where(x => x.expired_date == null).Select(x => x.File.link).FirstOrDefault();
            var Profile = (from a in db.Profiles
                           join b in db.Accounts on a.account_id equals b.account_id
                           join c in db.Offices on a.office_id equals c.office_id
                           join d in db.People on a.people_id equals d.people_id
                           where b.account_id == account_id
                           select new ProfileResearcher
                           {
                               ID = a.mssv_msnv,
                               FullName = d.name,
                               Email = b.email,
                               OfficeID = c.office_id,
                               TitleID = a.Titles.FirstOrDefault().title_id
                           }).FirstOrDefault();
            var ConferenceCriteriaLanguages = (from a in db.RequestConferencePolicies
                                               join b in db.Criteria on a.policy_id equals b.policy_id
                                               join c in db.ConferenceCriteriaLanguages on b.criteria_id equals c.criteria_id
                                               where a.expired_date == null && c.language_id == language_id
                                               select c.name).ToList();
            var Countries = db.Countries.Select(x => new { x.country_id, x.country_name }).ToList()
                .Select(x => new Country
                {
                    country_id = x.country_id,
                    country_name = x.country_name
                }).ToList();
            var FormalityLanguages = db.FormalityLanguages.Where(x => x.language_id == language_id)
                .Select(x => new { x.formality_id, x.name }).ToList()
                .Select(x => new FormalityLanguage
                {
                    formality_id = x.formality_id,
                    name = x.name
                }).ToList();
            var Offices = db.Offices.Select(x => new { x.office_id, x.office_name }).ToList()
                .Select(x => new Office
                {
                    office_id = x.office_id,
                    office_name = x.office_name
                }).ToList();
            var TitleLanguages = db.TitleLanguages.Where(x => x.language_id == language_id)
                .Select(x => new { x.title_id, x.name }).ToList()
                .Select(x => new TitleLanguage
                {
                    title_id = x.title_id,
                    name = x.name
                }).ToList();
            return JsonConvert.SerializeObject(new { Countries, FormalityLanguages, Offices, TitleLanguages, ConferenceCriteriaLanguages, Link, Profile });
        }
        public List<Info> GetAllProfileBy(string id, int language_id)
        {
            var infos = (from a in db.Profiles
                           join b in db.People on a.people_id equals b.people_id
                           join c in db.Offices on a.office_id equals c.office_id
                           join d in db.TitleLanguages on a.Titles.FirstOrDefault().title_id equals d.title_id
                           where a.mssv_msnv.Contains(id) && d.language_id == language_id
                           select new Info
                           {
                               Email = b.email,
                               MS = a.mssv_msnv,
                               Name = b.name,
                               OfficeID = c.office_id,
                               OfficeName = c.office_name,
                               TitleID = a.Titles.FirstOrDefault().title_id,
                               TitleString = d.name,
                           }).Take(10).ToList();
            if (infos.Count == 0)
            {
                Info info = new Info()
                {
                    Email = "",
                    MS = id.ToUpper(),
                    Name = "",
                    OfficeID = 1,
                    OfficeName = "",
                    TitleID = 1,
                    TitleString = ""
                };
                infos = new List<Info>() { info };
            }
            return infos;
        }
        public List<Conference> GetAllConferenceBy(string name)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var Conferences = db.Conferences.Where(x => x.conference_name.Contains(name)).ToList();
            Conferences.DefaultIfEmpty();
            if (Conferences.Count == 0)
            {
                Conference c = new Conference
                {
                    conference_id = 0,
                    conference_name = name,
                    website = "",
                    keynote_speaker = "",
                    qs_university = "",
                    country_id = 1,
                    time_start = DateTime.Today,
                    time_end = DateTime.Today,
                    formality_id = 1,
                    co_organized_unit = ""
                };
                Conferences.Add(c);
            }
            return Conferences;
        }
        public string AddRequestConference(int account_id, string input, HttpPostedFileBase invite, HttpPostedFileBase paper)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    Account account = db.Accounts.Find(account_id);  // Sẽ chỉnh sau khi xong tạo account
                    DataTable dt = new DataTable();
                    JObject @object = JObject.Parse(input);

                    JToken conf = @object["Conference"];

                    Conference conference = conf.ToObject<Conference>();
                    Conference temp = db.Conferences.Find(conference.conference_id);
                    if (temp != null)
                    {
                        conference = temp;
                    }
                    else
                    {
                        db.Conferences.Add(conference);
                        db.SaveChanges();
                    }

                    List<HttpPostedFileBase> InputFiles = new List<HttpPostedFileBase> { paper, invite };

                    List<string> Links = GlobalUploadDrive.UploadResearcherFile(InputFiles, conference.conference_name, 1, "doanvanthang4271@gmail.com");

                    RequestConferencePolicy policy = db.RequestConferencePolicies.Where(x => x.expired_date == null).FirstOrDefault();

                    BaseRequest @base = new BaseRequest()
                    {
                        account_id = account.account_id,
                        created_date = DateTime.Now,
                        finished_date = null
                    };
                    db.BaseRequests.Add(@base);

                    File Fpaper = new File()
                    {
                        link = Links[0],
                        name = paper.FileName
                    };
                    File Finvite = new File()
                    {
                        link = Links[1],
                        name = invite.FileName
                    };
                    db.Files.Add(Fpaper);
                    db.Files.Add(Finvite);
                    db.SaveChanges();

                    RequestConference support = new RequestConference()
                    {
                        request_id = @base.request_id,
                        conference_id = conference.conference_id,
                        status_id = 1,
                        policy_id = policy.policy_id,
                        editable = false,
                        reimbursement = 0,
                        attendance_start = DateTime.Parse(@object["attendance_start"].ToString()),
                        attendance_end = DateTime.Parse(@object["attendance_end"].ToString()),
                        invitation_file_id = Finvite.file_id,
                        paper_id = Fpaper.file_id,
                    };
                    db.RequestConferences.Add(support);
                    db.SaveChanges();

                    List<Cost> costs = @object["Cost"].ToObject<List<Cost>>();
                    foreach (var item in costs)
                    {
                        int total = int.Parse(dt.Compute(item.detail, "").ToString());
                        item.editable = false;
                        item.sponsoring_organization = "FPTU";
                        item.total = total;
                        item.request_id = support.request_id;
                    }
                    db.Costs.AddRange(costs);

                    List<ConferenceParticipant> participants = @object["ConferenceParticipant"].ToObject<List<ConferenceParticipant>>();
                    List<Person> Persons = @object["Persons"].ToObject<List<Person>>();
                    participants.ForEach(x => x.request_id = support.request_id);
                    List<string> codes = participants.Select(x => x.current_mssv_msnv).ToList();
                    List<int> title_ids = participants.Select(x => x.title_id).Distinct().ToList();
                    Dictionary<int, Title> IDTitlePairs = db.Titles.Where(x => title_ids.Contains(x.title_id))
                        .ToDictionary(x => x.title_id, x => x);
                    Dictionary<string, int> CodeIDPairs = db.Profiles.Where(x => codes.Contains(x.mssv_msnv))
                        .ToDictionary(x => x.mssv_msnv, x => x.people_id);
                    for (int i = 0; i < participants.Count; i++)
                    {
                        var item = participants[i];
                        if (CodeIDPairs.ContainsKey(item.current_mssv_msnv))
                            item.people_id = CodeIDPairs[item.current_mssv_msnv];
                        else
                        {
                            db.People.Add(Persons[i]);
                            db.SaveChanges();

                            //Persons[i].Titles.Add(IDTitlePairs[item.title_id]);
                            item.people_id = Persons[i].people_id;
                        }
                    }
                    db.ConferenceParticipants.AddRange(participants);
                    db.SaveChanges();
                    trans.Commit();
                    return JsonConvert.SerializeObject(new { success = true, message = "OK", id = support.request_id });
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    //log.Error(ex);
                    return JsonConvert.SerializeObject(new { success = false, });
                }
            }
        }
        public class Info
        {
            public string MS { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public int OfficeID { get; set; }
            public string OfficeName { get; set; }
            public int TitleID { get; set; }
            public string TitleString { get; set; }
        }
    }
}
