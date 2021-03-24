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
        public ProfileResearcher GetProfile(int id)
        {
            var data = (
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
                select new ProfileResearcher
                {
                    id = a.people_id,
                    name = a.name,
                    dob = b.birth_date,
                    country = c.country_name,
                    email = a.email,
                    phone = a.phone_number,
                    title = e.name,
                    office = f.office_name,
                    position = h.name,
                    avatar = w.link,
                    website = b.website,
                    gscholar = b.google_scholar,
                    cv = b.cv
                });
             return (ProfileResearcher) data;
        }
    }
}
