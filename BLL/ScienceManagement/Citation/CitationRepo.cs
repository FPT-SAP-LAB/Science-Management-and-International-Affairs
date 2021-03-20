using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Citation
{
    public class CitationRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListOnePerson_Citation> GetList(string id)
        {
            string sql = @"select STRING_AGG(c.source, ',') AS 'source',SUM(c.count) as 'count', br.created_date, rc.status_id, rc.request_id
                            from [SM_Citation].Citation c join [SM_Citation].RequestHasCitation rhc on c.citation_id = rhc.citation_id
	                            join [SM_Citation].RequestCitation rc on rhc.request_id = rc.request_id
	                            join [SM_Request].BaseRequest br on br.request_id = rc.request_id
                            where br.account_id = @id
                            group by br.created_date, rc.status_id,  rc.request_id";
            List<ListOnePerson_Citation> list = new List<ListOnePerson_Citation>();
            list = db.Database.SqlQuery<ListOnePerson_Citation>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }

        public AuthorInfo getAuthor(string id)
        {
            AuthorInfo item = new AuthorInfo();
            string sql = @"select po.*, o.office_abbreviation, ct.contract_id, t.title_id, rc.total_reward, pro.bank_branch, pro.bank_number, pro.mssv_msnv, pro.tax_code, pro.identification_number
                            from [SM_Citation].Citation c join [SM_Citation].RequestHasCitation rhc on c.citation_id = rhc.citation_id
	                            join [SM_Citation].RequestCitation rc on rhc.request_id = rc.request_id
	                            join [General].People po on rc.people_id = po.people_id
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on pro.office_id = o.office_id
	                            join [General].Area a on o.area_id = a.area_id
	                            join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            join [SM_MasterData].ContractType ct on pc.contract_id = ct.contract_id
	                            join [SM_Researcher].PeopleTitle pt on po.people_id = pt.people_id
	                            join [SM_MasterData].Title t on pt.title_id = t.title_id
                            where rc.request_id = @id";
            item = db.Database.SqlQuery<AuthorInfo>(sql, new SqlParameter("id", id)).FirstOrDefault();
            return item;
        }

        public List<ENTITIES.Citation> getCitation(string id)
        {
            List<ENTITIES.Citation> list = new List<ENTITIES.Citation>();
            string sql = @"select c.*
                            from [SM_Citation].Citation c join [SM_Citation].RequestHasCitation rhc on c.citation_id = rhc.citation_id
	                            join [SM_Citation].RequestCitation rc on rhc.request_id = rc.request_id
                            where rc.request_id = @id";
            list = db.Database.SqlQuery<ENTITIES.Citation>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }
    }
}
