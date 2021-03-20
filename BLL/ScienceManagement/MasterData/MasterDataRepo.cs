using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.MasterData
{
    public class MasterDataRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<SpecializationLanguage> getSpec(string language)
        {
            List<SpecializationLanguage> list = new List<SpecializationLanguage>();
            string sql = @"select sl.name, sl.specialization_id, sl.language_id
                            from [General].Specialization s join [Localization].SpecializationLanguage sl on s.specialization_id = sl.specialization_id
	                            join [Localization].Language l on sl.language_id = l.language_id
                            where l.language_name = @lang";
            list = db.Database.SqlQuery<SpecializationLanguage>(sql, new SqlParameter("lang", language)).ToList();
            return list;
        }

        public List<PaperCriteria> getPaperCriteria()
        {
            List<PaperCriteria> list = new List<PaperCriteria>();
            string sql = @"select c.*
                            from [SM_ScientificProduct].PaperCriteria c
                            where c.status = 'active'";
            list = db.Database.SqlQuery<PaperCriteria>(sql).ToList();
            return list;
        }

        public List<Office> getOffice()
        {
            List<Office> list = db.Offices.ToList();
            return list;
        }

        public List<Area> GetAreas()
        {
            List<Area> list = db.Areas.ToList();
            return list;
        }

        public List<TitleWithName> getTitle(string lang)
        {
            List<TitleWithName> list = new List<TitleWithName>();
            string sql = @"select tl.name, t.*
                            from [SM_MasterData].Title t join [Localization].TitleLanguage tl on t.title_id = tl.title_id
	                            join [Localization].Language l on l.language_id = tl.language_id
                            where l.language_name = @lang";
            list = db.Database.SqlQuery<TitleWithName>(sql, new SqlParameter("lang", lang)).ToList();
            return list;
        }

        public List<ContractType> getContract()
        {
            List<ContractType> list = db.ContractTypes.ToList();
            return list;
        }

        public List<AddAuthor> getListPeopleFE()
        {
            List<AddAuthor> list = new List<AddAuthor>();
            string sql = @"select pro.mssv_msnv, po.name, po.people_id
                            from [General].People po 
	                            join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            join [SM_MasterData].ContractType ct on pc.contract_id = ct.contract_id
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].[File] f on pro.identification_file_id = f.file_id
	                            join [General].Office ofi on pro.office_id = ofi.office_id";
            list = db.Database.SqlQuery<AddAuthor>(sql).ToList();
            return list;
        }

        public AddAuthor getAuthor(string ms)
        {
            AddAuthor item = new AddAuthor();
            string sql = @"select po.*, pc.contract_id, pt.title_id, o.office_abbreviation, pro.mssv_msnv, pro.bank_branch, pro.bank_number, pro.tax_code, pro.identification_number
                            from [General].People po join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            join [SM_Researcher].PeopleTitle pt on po.people_id = pt.people_id
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on pro.office_id = o.office_id
                            where pro.mssv_msnv = @ms";
            item = db.Database.SqlQuery<AddAuthor>(sql, new SqlParameter("ms", ms)).FirstOrDefault();
            return item;
        }
    }
}
