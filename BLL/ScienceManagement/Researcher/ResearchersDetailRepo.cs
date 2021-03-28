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
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public ResearcherDetail GetProfile(int id)
        {
            var profile = (
                from a in db.People
                join b in db.Profiles on a.people_id equals b.people_id
                join c in db.Countries on b.country_id equals c.country_id
                from d in db.Titles.Where(x => b.Titles.Contains(x))
                join e in db.TitleLanguages on d.title_id equals e.title_id
                join f in db.Offices on b.office_id equals f.office_id
                from g in db.Positions.Where(x => a.Profile.Positions.Contains(x))
                join h in db.PositionLanguages on g.position_id equals h.position_id
                join w in db.Files on b.avatar_id equals w.file_id
                where h.language_id == 1 && e.language_id == 1 && a.people_id == id
                select new ResearcherDetail
                {
                    id = a.people_id,
                    name = a.name,
                    dob = b.birth_date,
                    country = c.country_name,
                    email = a.email,
                    phone = a.phone_number,
                    title_id = e.title_id,
                    office = f.office_name,
                    position_id = h.position_id,
                    position_name = h.name,
                    title_name = h.name,
                    avatar = w.link,
                    website = b.website,
                    gscholar = b.google_scholar,
                    cv = b.cv
                }).FirstOrDefault();
            var interested_fields = (from a in db.Profiles
                                     from g in db.ResearchAreas.Where(x => x.Profiles.Contains(a))
                                     join h in db.ResearchAreaLanguages on g.research_area_id equals h.research_area_id
                                     where a.people_id == id
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
                                                       select n.research_area_id).Contains(h.research_area_id))
                                              select new SelectField
                                              {
                                                  id = h.research_area_id,
                                                  name = h.name,
                                                  selected = 0
                                              }).ToList<SelectField>();
            var title_fields = (from a in db.Profiles
                                from g in db.Titles.Where(x => x.Profiles.Contains(a))
                                join h in db.TitleLanguages on g.title_id equals h.title_id
                                where a.people_id == id
                                select new SelectField
                                {
                                    id = h.title_id,
                                    name = h.name,
                                    selected = 1
                                }).Union(from g in db.Titles
                                         join h in db.TitleLanguages on g.title_id equals h.title_id
                                         where !((from m in db.Profiles
                                                  from n in db.Titles
                                                  where m.Titles.Contains(n) && m.people_id == id
                                                  select n.title_id).Contains(h.title_id))
                                         select new SelectField
                                         {
                                             id = h.title_id,
                                             name = h.name,
                                             selected = 0
                                         }).ToList<SelectField>();
            var position_fields = (from a in db.Profiles
                                   from g in db.Positions.Where(x => x.Profiles.Contains(a))
                                   join h in db.PositionLanguages on g.position_id equals h.position_id
                                   where a.people_id == id
                                   select new SelectField
                                   {
                                       id = h.position_id,
                                       name = h.name,
                                       selected = 1
                                   }).Union(from g in db.Positions
                                            join h in db.PositionLanguages on g.position_id equals h.position_id
                                            where !((from m in db.Profiles
                                                     from n in db.Positions
                                                     where m.Positions.Contains(n) && m.people_id == id
                                                     select n.position_id).Contains(h.position_id))
                                            select new SelectField
                                            {
                                                id = h.position_id,
                                                name = h.name,
                                                selected = 0
                                            }).ToList<SelectField>();
            var offices_fields = (from a in db.Profiles
                                  from g in db.Offices.Where(x => x.Profiles.Contains(a))
                                  where a.people_id == id
                                  select new SelectField
                                  {
                                      id = g.office_id,
                                      name = g.office_name,
                                      selected = 1
                                  }).Union(from g in db.Offices
                                           where !((from m in db.Profiles
                                                    from n in db.Offices.Where(x => x.Profiles.Contains(m))
                                                    where m.people_id == id
                                                    select n.office_id).Contains(g.office_id))
                                           select new SelectField
                                           {
                                               id = g.office_id,
                                               name = g.office_name,
                                               selected = 0
                                           }).ToList<SelectField>();
            profile.interested_fields = interested_fields;
            profile.title_fields = title_fields;
            profile.position_fields = position_fields;
            profile.offices_fields = offices_fields;
            return profile;
        }
    }
}
