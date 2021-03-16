using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Invention;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Invention
{
    public class InventionRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public DetailInvention getDetail(string id)
        {
            DetailInvention item = new DetailInvention();
            string sql = @"select i.*, it.name as 'type_name', ri.reward_type, ri.total_reward, ri.request_id
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].InventionType it on i.type_id = it.invention_type_id
	                            join [SM_ScientificProduct].RequestInvention ri on i.invention_id = ri.invention_id
                            where i.invention_id = @id";
            item = db.Database.SqlQuery<DetailInvention>(sql, new SqlParameter("id", id)).FirstOrDefault();
            return item;
        }

        public List<Country> getCountry()
        {
            List<Country> list = new List<Country>();
            //string sql = "select * from [General].Country";
            list = db.Countries.ToList();
            return list;
        }

        public List<AuthorInfo> getAuthor(string id)
        {
            List<AuthorInfo> list = new List<AuthorInfo>();
            string sql = @"select po.*, tl.name as 'title_name', ct.name as 'contract_name', ai.money_reward, o.office_abbreviation, f.link
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].AuthorInvention ai on i.invention_id = ai.invention_id
	                            join [General].People po on ai.people_id = po.people_id
	                            join [SM_Researcher].PeopleTitle pt on po.people_id = pt.people_id
	                            join [SM_MasterData].Title t on pt.title_id = t.title_id
	                            join [Localization].TitleLanguage tl on t.title_id = tl.title_id
	                            join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            join [SM_MasterData].ContractType ct on pc.contract_id = ct.contract_id
	                            join [General].Office o on po.office_id = o.office_id
	                            join [General].[File] f on po.evidence = f.file_id
                            where i.invention_id = @id";
            list = db.Database.SqlQuery<AuthorInfo>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }
    }
}
