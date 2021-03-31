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
    public class ResearcherCandidateRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public BaseServerSideData<ResearcherCandidate> GetList(BaseDatatable baseDatatable, int account_id = 0, int language_id = 1)
        {
            var data = (from b in db.People 
                        join c in db.Profiles on b.people_id equals c.people_id
                        join d in db.Titles on c.title_id equals d.title_id
                        join e in db.TitleLanguages on d.title_id equals e.title_id
                        where e.language_id == 1 && (d.title_id == 1 || d.title_id == 2) && c.profile_page_active==false
                        select new ResearcherCandidate {
                            people_id=b.people_id,
                            name=b.name,
                            title=e.name,
                            paperNumber=(from m in db.AuthorPapers where m.people_id==b.people_id select m).Count()
                        });
            var res = data.OrderBy(baseDatatable.SortColumnName + " " + baseDatatable.SortDirection)
            .Skip(baseDatatable.Start).Take(baseDatatable.Length).ToList();
            int recordsTotal = data.Count();
            return new BaseServerSideData<ResearcherCandidate>(res, recordsTotal);
        }
    }
}
