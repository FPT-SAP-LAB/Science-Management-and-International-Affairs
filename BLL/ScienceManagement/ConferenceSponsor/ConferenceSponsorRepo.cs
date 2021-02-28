using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<Country> GetAllCountries()
        {
            var countries = db.Countries.Select(x => new { x.country_id, x.country_name }).ToList().Select(x => new Country
            {
                country_id = x.country_id,
                country_name = x.country_name
            }).ToList();
            return countries;
        }
        public List<Info> GetAllProfileBy(string id)
        {
            List<Info> infos = new List<Info>()
            {
                new Info("HE130214", "Đoàn Văn Thắng", 1, "FPTU", 1, "Hà Nội", 1, "Sinh viên"),
            };
            return infos;
        }
        public class Info
        {
            public string PeopleID { get; set; }
            public string Name { get; set; }
            public int WorkUnitID { get; set; }
            public string WorkUnitString { get; set; }
            public int AreaID { get; set; }
            public string AreaString { get; set; }
            public int TitleID { get; set; }
            public string TitleString { get; set; }
            public Info() { }
            public Info(string id, string name, int unitID, string unitString, int areaID, string areaString, int titleID, string titleString)
            {
                PeopleID = id;
                Name = name;
                WorkUnitID = unitID;
                WorkUnitString = unitString;
                AreaID = areaID;
                AreaString = areaString;
                TitleID = titleID;
                TitleString = titleString;
            }
        }
    }
}
