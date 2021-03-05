﻿using ENTITIES;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public string GetAddPageJson(string language_name)
        {
            var Countries = db.Countries.Select(x => new { x.country_id, x.country_name }).ToList()
                .Select(x => new Country
                {
                    country_id = x.country_id,
                    country_name = x.country_name
                }).ToList();
            var FormalityLanguages = db.FormalityLanguages.Where(x => x.Language.language_name.Equals(language_name))
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
            var TitleLanguages = db.TitleLanguages.Where(x => x.Language.language_name.Equals(language_name))
                .Select(x => new { x.title_id, x.name }).ToList()
                .Select(x => new TitleLanguage
                {
                    title_id = x.title_id,
                    name = x.name
                }).ToList();
            return JsonConvert.SerializeObject(new { Countries, FormalityLanguages, Offices, TitleLanguages });
        }
        public List<Info> GetAllProfileBy(string id)
        {
            id = id.ToUpper();
            List<string> currentID = new List<string>()
            {
                "HE130214"
            };
            bool IsExist = currentID.Any(x => x.Contains(id));
            List<Info> infos = new List<Info>();
            if (IsExist)
            {
                infos.Add(new Info("HE130214", "Đoàn Văn Thắng", 2, "Đai học FPT Hà Nội 2", 5, "Sinh viên"));
                infos.Add(new Info("HE130020", "Trần Thị Thúy Nguyên", 2, "Đai học FPT Hà Nội 2", 5, "Sinh viên"));
            }
            else
                infos.Add(new Info(id, "", 1, "", 1, ""));
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
                    formality_id = 1
                };
                Conferences.Add(c);
            }
            return Conferences;
        }
        public string AddConference(string input)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    DataTable dt = new DataTable();
                    //log.Debug("Creating a conference sponsor request");
                    //int a = int.Parse("43gg34+-");
                    JObject @object = JObject.Parse(input);

                    JToken conf = @object["Conference"];
                    int conference_id = conf["conference_id"].ToObject<int>();

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

                    RewardPolicy policy = db.RewardPolicies.Where(x => x.expired_date == null).FirstOrDefault();

                    ConferenceSupport support = new ConferenceSupport()
                    {
                        conference_id = conference.conference_id,
                        status_id = 1,
                        decision_id = null,
                        reward_policy_id = policy.reward_policy_id,
                        paper_file_id = 1, // Sẽ chỉnh sau khi xong upload file
                        account_id = 1, // Sẽ chỉnh sau khi xong tạo account
                        editable = false
                    };
                    db.ConferenceSupports.Add(support);
                    db.SaveChanges();

                    List<Cost> costs = @object["Cost"].ToObject<List<Cost>>();
                    foreach (var item in costs)
                    {
                        int total = int.Parse(dt.Compute(item.detail, "").ToString());
                        item.editable = false;
                        item.sponsoring_organization = "FPTU";
                        item.total = total;
                        item.conference_support_id = support.conference_support_id;
                    }
                    db.Costs.AddRange(costs);

                    List<ConferenceParticipant> participants = @object["ConferenceParticipant"].ToObject<List<ConferenceParticipant>>();
                    List<Person> Persons = @object["Persons"].ToObject<List<Person>>();
                    participants.ForEach(x => x.conference_support_id = support.conference_support_id);
                    List<string> codes = participants.Select(x => x.current_mssv_msnv).ToList();
                    List<int> title_ids = participants.Select(x => x.title_id).Distinct().ToList();
                    Dictionary<int, Title> IDTitlePairs = db.Titles.Where(x => title_ids.Contains(x.title_id))
                        .ToDictionary(x => x.title_id, x => x);
                    Dictionary<string, int> CodeIDPairs = db.People.Where(x => codes.Contains(x.mssv_msnv))
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

                            Persons[i].Titles.Add(IDTitlePairs[item.title_id]);
                            item.people_id = Persons[i].people_id;
                        }
                    }
                    db.ConferenceParticipants.AddRange(participants);
                    db.SaveChanges();
                    trans.Commit();
                    return JsonConvert.SerializeObject(new { success = true, message = "OK", id = support.conference_support_id });
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
            public string PeopleID { get; set; }
            public string Name { get; set; }
            public int OfficeID { get; set; }
            public string OfficeName { get; set; }
            public int TitleID { get; set; }
            public string TitleString { get; set; }
            public Info() { }
            public Info(string id, string name, int officeID, string officeName, int titleID, string titleString)
            {
                PeopleID = id;
                Name = name;
                OfficeID = officeID;
                OfficeName = officeName;
                TitleID = titleID;
                TitleString = titleString;
            }
        }
        private class ParticipantExtend : Participant
        {
            public string Name { get; set; }
        }
    }
}
