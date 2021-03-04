using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
                infos.Add(new Info("HE130214", "Đoàn Văn Thắng", 2, "Đai học FPT Hà Nội 2", 5, "Sinh viên"));
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
    }
}
