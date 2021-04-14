using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
namespace BLL.ScienceManagement.Researcher
{
    public class ResearchersDetailRepo
    {
        ResearchersBiographyRepo researcherBiographyRepo;
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public ResearcherDetail GetProfile(int id)
        {
            var profile = (
                from a in db.People
                join b in db.Profiles on a.people_id equals b.people_id
                where a.people_id == id
                select new ResearcherDetail
                {
                    id = a.people_id,
                    name = a.name,
                    dob = b.birth_date,
                    email = a.email,
                    phone = a.phone_number,
                    avatar = (from f in db.Profiles
                              join ff in db.Files on f.avatar_id equals ff.file_id
                              where f.people_id == a.people_id
                              select ff.link
                                      ).FirstOrDefault(),
                    website = b.website,
                    office = (from m in db.Offices where a.office_id == m.office_id select m.office_name).FirstOrDefault(),
                    gscholar = b.google_scholar,
                    cv = b.cv,
                    profile_page_active = b.profile_page_active
                }).FirstOrDefault();
            var interested_fields = (from a in db.Profiles
                                     from g in db.ResearchAreas.Where(x => x.Profiles.Contains(a))
                                     join h in db.ResearchAreaLanguages on g.research_area_id equals h.research_area_id
                                     where a.people_id == id && h.language_id == 1
                                     select new SelectField
                                     {
                                         id = h.research_area_id,
                                         name = h.name,
                                         selected = 1
                                     }).Union(from g in db.ResearchAreas
                                              join h in db.ResearchAreaLanguages on g.research_area_id equals h.research_area_id
                                              where !((from m in db.Profiles
                                                       from n in db.ResearchAreas
                                                       where m.ResearchAreas.Contains(n) && m.people_id == id
                                                       select n.research_area_id).Contains(h.research_area_id)) && h.language_id == 1
                                              select new SelectField
                                              {
                                                  id = h.research_area_id,
                                                  name = h.name,
                                                  selected = 0
                                              }).ToList<SelectField>();
            var title_fields = (from a in db.Profiles
                                from g in db.Titles.Where(x => x.Profiles.Contains(a))
                                join h in db.TitleLanguages on g.title_id equals h.title_id
                                where a.people_id == id && h.language_id == 1
                                select new SelectField
                                {
                                    id = h.title_id,
                                    name = h.name,
                                    selected = 1
                                }).Union(from g in db.Titles
                                         join h in db.TitleLanguages on g.title_id equals h.title_id
                                         where !((from m in db.Profiles
                                                  from n in db.Titles
                                                  where m.title_id == n.title_id && m.people_id == id
                                                  select n.title_id).Contains(h.title_id)) && h.language_id == 1
                                         select new SelectField
                                         {
                                             id = h.title_id,
                                             name = h.name,
                                             selected = 0
                                         }).ToList<SelectField>();
            var position_fields = (from a in db.PeoplePositions
                                   join g in db.Positions on a.position_id equals g.position_id
                                   join h in db.PositionLanguages on g.position_id equals h.position_id
                                   where a.people_id == id && h.language_id == 1
                                   select new SelectField
                                   {
                                       id = h.position_id,
                                       name = h.name,
                                       selected = 1
                                   }).Union(from g in db.Positions
                                            join h in db.PositionLanguages on g.position_id equals h.position_id
                                            where !((from m in db.PeoplePositions
                                                     where m.people_id == id
                                                     select m.position_id).Contains(h.position_id)) && h.language_id == 1
                                            select new SelectField
                                            {
                                                id = h.position_id,
                                                name = h.name,
                                                selected = 0
                                            }).ToList<SelectField>();
            var offices_fields = (from a in db.People
                                  join g in db.Offices on a.office_id equals g.office_id
                                  where a.people_id == id
                                  select new SelectField
                                  {
                                      id = g.office_id,
                                      name = g.office_name,
                                      selected = 1
                                  }).Union(from g in db.Offices
                                           where !((from m in db.People
                                                    join n in db.Offices on m.office_id equals n.office_id
                                                    where m.people_id == id
                                                    select n.office_id).Contains(g.office_id))
                                           select new SelectField
                                           {
                                               id = g.office_id,
                                               name = g.office_name,
                                               selected = 0
                                           }).ToList<SelectField>();
            var countries_fields = (from a in db.Profiles
                                    from g in db.Countries.Where(x => x.Profiles.Contains(a))
                                    where a.people_id == id
                                    select new SelectField
                                    {
                                        id = g.country_id,
                                        name = g.country_name,
                                        selected = 1
                                    }).Union(from g in db.Countries
                                             where !((from m in db.Profiles
                                                      from n in db.Countries.Where(x => x.Profiles.Contains(m))
                                                      where m.people_id == id
                                                      select n.country_id).Contains(g.country_id))
                                             select new SelectField
                                             {
                                                 id = g.country_id,
                                                 name = g.country_name,
                                                 selected = 0
                                             }).ToList<SelectField>();
            profile.interested_fields = interested_fields;
            profile.title_fields = title_fields;
            profile.position_fields = position_fields;
            profile.offices_fields = offices_fields;
            profile.countries_fields = countries_fields;
            return profile;
        }

        public ResearcherView GetDetailView(int id, int language_id)
        {
            researcherBiographyRepo = new ResearchersBiographyRepo();
            var profile = (
               from a in db.People
               join b in db.Profiles on a.people_id equals b.people_id
               where a.people_id == id
               select new ResearcherView
               {
                   id = a.people_id,
                   name = a.name,
                   dob = b.birth_date,
                   position_fields = (from a in db.Profiles
                                      //from g in db.Positions.Where(x => x.Profiles.Contains(a))
                                      join g in db.PeoplePositions on a.people_id equals g.people_id
                                      join h in db.PositionLanguages on g.position_id equals h.position_id
                                      where a.people_id == id && h.language_id == language_id
                                      select new SelectField
                                      {
                                          name = h.name
                                      }).ToList(),
                   interested_fields = (from a in db.Profiles
                                        from g in db.ResearchAreas.Where(x => x.Profiles.Contains(a))
                                        join h in db.ResearchAreaLanguages on g.research_area_id equals h.research_area_id
                                        where a.people_id == id && h.language_id == language_id
                                        select new SelectField
                                        {
                                            name = h.name
                                        }).ToList(),
                   title_fields = (from a in db.Profiles
                                   from g in db.Titles.Where(x => x.Profiles.Contains(a))
                                   join h in db.TitleLanguages on g.title_id equals h.title_id
                                   where a.people_id == id && h.language_id == language_id
                                   select new SelectField
                                   {
                                       name = h.name
                                   }).ToList(),
                   email = a.email,
                   phone = a.phone_number,
                   avatar = (from f in db.Profiles
                             join ff in db.Files on f.avatar_id equals ff.file_id
                             where f.people_id == a.people_id
                             select ff.link
                                      ).FirstOrDefault(),
                   website = b.website,
                   office = (from m in db.Offices where a.office_id == m.office_id select m.office_name).FirstOrDefault(),
                   gscholar = b.google_scholar,
                   cv = b.cv,
                   profile_page_active = b.profile_page_active
               }).FirstOrDefault();
            profile.awards = researcherBiographyRepo.GetAwards(id);
            profile.acadBiography = researcherBiographyRepo.GetAcadHistory(id);
            return profile;
        }
    }
}
