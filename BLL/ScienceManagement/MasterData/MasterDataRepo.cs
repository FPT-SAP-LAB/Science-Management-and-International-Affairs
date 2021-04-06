using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public int AddPaperCriteria(string name)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                PaperCriteria ck = db.PaperCriterias.Where(x => x.name == name).Where(x => x.status == "active").FirstOrDefault();
                if (ck != null) return -1;
                else
                {
                    PaperCriteria pc = new PaperCriteria
                    {
                        name = name,
                        status = "active"
                    };
                    db.PaperCriterias.Add(pc);
                    db.SaveChanges();
                    dbc.Commit();
                    return pc.criteria_id;
                }
            }
            catch (Exception e)
            {
                dbc.Rollback();
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public string DeletePaperCriteria(string cri_id)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                int criteria_id = Int32.Parse(cri_id);
                PaperCriteria pc = db.PaperCriterias.Where(x => x.criteria_id == criteria_id).FirstOrDefault();
                pc.status = "inactive";
                db.Entry(pc).State = EntityState.Modified;
                db.SaveChanges();
                dbc.Commit();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                return "ff";
            }
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

        public AuthorPaper getMonry(AddAuthor item, int paper_id)
        {
            AuthorPaper ap = db.AuthorPapers
                .Where(x => x.paper_id == paper_id)
                .Where(x => x.people_id == item.people_id).FirstOrDefault();
            return ap;
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
	                            join [General].Office ofi on po.office_id = ofi.office_id";
            list = db.Database.SqlQuery<AddAuthor>(sql).ToList();
            return list;
        }

        public AddAuthor getAuthor(string ms)
        {
            AddAuthor item = new AddAuthor();
            string sql = @"select po.*, pc.contract_id, pro.title_id, o.office_abbreviation, pro.mssv_msnv, pro.bank_branch, pro.bank_number, pro.tax_code, pro.identification_number, pro.is_reseacher
                            from [General].People po join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on po.office_id = o.office_id
                            where pro.mssv_msnv = @ms";
            item = db.Database.SqlQuery<AddAuthor>(sql, new SqlParameter("ms", ms)).FirstOrDefault();
            return item;
        }

        public List<PaperType> getPaperType()
        {
            List<PaperType> list = db.PaperTypes.ToList();
            return list;
        }

        public File addFile(File file)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                db.Files.Add(file);
                db.SaveChanges();
                dbc.Commit();
                return file;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();

                return null;
            }
        }

        public string GetPaperCriteria(string id)
        {
            int criteria_id = Int32.Parse(id);
            string name = (from a in db.PaperCriterias where a.criteria_id == criteria_id select a.name).FirstOrDefault();
            return name;
        }

        public string UpdatePaperCriteria(string id, string name)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                int criteria_id = Int32.Parse(id);
                PaperCriteria pc = db.PaperCriterias.Where(x => x.criteria_id == criteria_id).FirstOrDefault();
                pc.name = name;
                db.Entry(pc).State = EntityState.Modified;
                db.SaveChanges();
                dbc.Commit();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                return "ff";
            }
        }
    }
}
