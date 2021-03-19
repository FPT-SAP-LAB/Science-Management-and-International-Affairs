using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Paper
{
    public class PaperRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public DetailPaper getDetail(string id)
        {
            DetailPaper item = new DetailPaper();
            string sql = @"select p.*, rp.type, rp.reward_type, rp.total_reward, rp.specialization_id, rp.request_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
                            where p.paper_id = @id";
            item = db.Database.SqlQuery<DetailPaper>(sql, new SqlParameter("id", id)).FirstOrDefault();
            return item;
        }

        public List<ListCriteriaOfOnePaper> getCriteria(string id)
        {
            List<ListCriteriaOfOnePaper> list = new List<ListCriteriaOfOnePaper>();
            string sql = @"select pc.name, pwc.*
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].PaperWithCriteria pwc on p.paper_id = pwc.paper_id
	                            join [SM_ScientificProduct].PaperCriteria pc on pwc.criteria_id = pc.criteria_id
                            where p.paper_id = @id";
            list = db.Database.SqlQuery<ListCriteriaOfOnePaper>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }

        public List<AuthorInfo> getAuthorPaper(string id)
        {
            List<AuthorInfo> list = new List<AuthorInfo>();
            string sql = @"select po.*, tl.name as 'title_name', ct.name as 'contract_name', ap.money_reward, o.office_abbreviation, f.link, pro.bank_branch, pro.bank_number, pro.mssv_msnv, pro.tax_code, pro.identification_number, pro.office_id, pc.contract_id, t.title_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [General].People po on ap.people_id = po.people_id
	                            join [SM_Researcher].PeopleTitle pt on po.people_id = pt.people_id
	                            join [SM_MasterData].Title t on pt.title_id = t.title_id
	                            join [Localization].TitleLanguage tl on t.title_id = tl.title_id
	                            join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            join [SM_MasterData].ContractType ct on pc.contract_id = ct.contract_id
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on pro.office_id = o.office_id
	                            join [General].[File] f on pro.identification_file_id = f.file_id
                            where p.paper_id = @id";
            list = db.Database.SqlQuery<AuthorInfo>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }
    }
}
