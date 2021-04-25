using CsvHelper;
using CsvHelper.Configuration;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.MasterData;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

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

        public static BaseServerSideData<Scopu> getListAllScopus(BaseDatatable baseDatatable, string name_search)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    if (name_search == null) name_search = "";
                    db.Configuration.LazyLoadingEnabled = false;
                    List<Scopu> internalUnits = db.Database.SqlQuery<Scopu>("select * from [SM_ScientificProduct].[Scopus] " +
                                                                            "WHERE [Source_Title_Medline_sourced_journals_are_indicated_in_Green] like @name " +
                                                                        "ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                                                        " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY", new SqlParameter("name", "%" + name_search + "%")).ToList();
                    int recordsTotal = db.Database.SqlQuery<int>("select count(*) from [SM_ScientificProduct].[Scopus] WHERE [Source_Title_Medline_sourced_journals_are_indicated_in_Green] like @name", new SqlParameter("name", "%" + name_search + "%")).FirstOrDefault();
                    return new BaseServerSideData<Scopu>(internalUnits, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool updateJournal(HttpPostedFileBase file_scopus, HttpPostedFileBase file_SCIE, HttpPostedFileBase file_SSCI)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    if (file_scopus != null)
                    {
                        db.Database.ExecuteSqlCommand("delete from SM_ScientificProduct.Scopus");

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        var package = new ExcelPackage(file_scopus.InputStream);
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                        int rows = worksheet.Dimension.Rows;
                        //int columns = worksheet.Dimension.Columns;

                        for (int i = 2; i <= rows; i++)
                        {
                            Scopu temp = new Scopu();
                            temp.Sourcerecord_ID = worksheet.Cells[i, 1].Value.ToString();
                            temp.Source_Title_Medline_sourced_journals_are_indicated_in_Green = worksheet.Cells[i, 2].Value.ToString();
                            if (worksheet.Cells[i, 3].Value != null) temp.Print_ISSN = worksheet.Cells[i, 3].Value.ToString();
                            if (worksheet.Cells[i, 4].Value != null) temp.E_ISSN = worksheet.Cells[i, 4].Value.ToString();
                            temp.Active_or_Inactive = worksheet.Cells[i, 5].Value.ToString();

                            db.Scopus.Add(temp);
                        }

                        db.SaveChanges();
                    }

                    if (file_SCIE != null)
                    {
                        db.Database.ExecuteSqlCommand("truncate table SM_ScientificProduct.SCIE");

                        var reader = new StreamReader(file_SCIE.InputStream);
                        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
                        config.Delimiter = ",";
                        config.HasHeaderRecord = true;
                        config.BadDataFound = null;

                        using (var csv = new CsvReader(reader, config))
                        {
                            int count = 1;
                            while (csv.Read())
                            {
                                if (csv.GetField(0) != "Journal title")
                                {
                                    SCIE temp = new SCIE();
                                    temp.Journal_title = csv.GetField(0);
                                    if (csv.GetField(1) != "") temp.ISSN = csv.GetField(1);
                                    if (csv.GetField(2) != "") temp.eISSN = csv.GetField(2);
                                    temp.Publisher_name = csv.GetField(3);
                                    temp.Publisher_address = csv.GetField(4);
                                    if (csv.GetField(5) != "") temp.Languages = csv.GetField(5);
                                    if (csv.GetField(6) != "") temp.Web_of_Science_Categories = csv.GetField(6);
                                    temp.SCIE_id = count.ToString();
                                    count++;

                                    db.SCIEs.Add(temp);
                                }
                            }
                            db.SaveChanges();
                        }
                    }

                    if (file_SSCI != null)
                    {
                        db.Database.ExecuteSqlCommand("truncate table SM_ScientificProduct.SSCI");

                        var reader = new StreamReader(file_SSCI.InputStream);
                        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
                        config.Delimiter = ",";
                        config.HasHeaderRecord = true;
                        config.BadDataFound = null;

                        using (var csv = new CsvReader(reader, config))
                        {
                            int count = 1;
                            while (csv.Read())
                            {
                                if (csv.GetField(0) != "Journal title")
                                {
                                    SSCI temp = new SSCI();
                                    temp.Journal_title = csv.GetField(0);
                                    if (csv.GetField(1) != "") temp.ISSN = csv.GetField(1);
                                    if (csv.GetField(2) != "") temp.eISSN = csv.GetField(2);
                                    temp.Publisher_name = csv.GetField(3);
                                    temp.Publisher_address = csv.GetField(4);
                                    if (csv.GetField(5) != "") temp.Languages = csv.GetField(5);
                                    if (csv.GetField(6) != "") temp.Web_of_Science_Categories = csv.GetField(6);
                                    temp.SSCI_id = count.ToString();
                                    count++;

                                    db.SSCIs.Add(temp);
                                }
                            }
                            db.SaveChanges();
                        }
                    }

                    dbc.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return false;
                }
        }

        public static BaseServerSideData<CustomISI> getListAllISI(BaseDatatable baseDatatable, string name_search)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    if (name_search == null) name_search = "";
                    db.Configuration.LazyLoadingEnabled = false;
                    string sql = @"select Journal_title, ISSN, eISSN, 'SSCI' as type
                                    from SM_ScientificProduct.SSCI
                                    where Journal_title like @name
                                    union
                                    select Journal_title, ISSN, eISSN, 'SCIE' as type
                                    from SM_ScientificProduct.SCIE
                                    where Journal_title like @name
                                    ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                                    " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY";
                    List<CustomISI> internalUnits = db.Database.SqlQuery<CustomISI>(sql, new SqlParameter("name", "%" + name_search + "%")).ToList();

                    string count_sql = @"select count(*)
                                    from (select Journal_title, ISSN, eISSN, 'SSCI' as type
                                    from SM_ScientificProduct.SSCI
                                    where Journal_title like @name
                                    union
                                    select Journal_title, ISSN, eISSN, 'SCIE' as type
                                    from SM_ScientificProduct.SCIE
                                    where Journal_title like @name) as a";
                    int recordsTotal = db.Database.SqlQuery<int>(count_sql, new SqlParameter("name", "%" + name_search + "%")).FirstOrDefault();
                    return new BaseServerSideData<CustomISI>(internalUnits, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int AddPaperCriteria(string name)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    PaperCriteria ck = db.PaperCriterias.Where(x => x.name == name).FirstOrDefault();
                    if (ck != null) return -1;
                    else
                    {
                        PaperCriteria pc = new PaperCriteria
                        {
                            name = name,
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

        public bool addNewPolicy(HttpPostedFileBase file, List<PaperCriteria> list, Account acc)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadPolicyFile(file);
                    ENTITIES.File fl = new ENTITIES.File
                    {
                        link = f.WebViewLink,
                        file_drive_id = f.Id,
                        name = f.Name
                    };
                    db.Files.Add(fl);
                    db.SaveChanges();

                    string sql_getLastPolicyPaper = @"select MAX(p.policy_id) as 'policy_id', p.valid_date, p.expired_date, p.file_id, p.article_id, p.account_id, p.policy_type_id
                                                from SM_Request.Policy p
                                                where p.policy_type_id = 2 
                                                group by p.valid_date, p.expired_date, p.file_id, p.article_id, p.account_id, p.policy_type_id";
                    Policy p = db.Database.SqlQuery<Policy>(sql_getLastPolicyPaper).FirstOrDefault();
                    p.expired_date = DateTime.Now;
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();

                    Policy newPolicy = new Policy()
                    {
                        valid_date = DateTime.Now,
                        file_id = fl.file_id,
                        account_id = acc.account_id,
                        policy_type_id = 2
                    };
                    db.Policies.Add(newPolicy);
                    db.SaveChanges();

                    foreach (var item in list)
                    {
                        item.policy_id = newPolicy.policy_id;
                        db.PaperCriterias.Add(item);
                    }
                    db.SaveChanges();

                    dbc.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return false;
                }
        }

        public string DeletePaperCriteria(string cri_id)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    int criteria_id = Int32.Parse(cri_id);
                    PaperCriteria pc = db.PaperCriterias.Where(x => x.criteria_id == criteria_id && x.Policy.policy_type_id == 2 && x.Policy.expired_date == null).FirstOrDefault();
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
            string sql = @"select pc.*
                            from SM_ScientificProduct.PaperCriteria pc join
	                            (select MAX(policy_id) as 'policy_id'
	                            from SM_ScientificProduct.PaperCriteria) as a on pc.policy_id = a.policy_id";
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
            string sql = @"select distinct po.mssv_msnv
                            from SM_ScientificProduct.Author po 
	                            join [General].Office ofi on po.office_id = ofi.office_id";
            list = db.Database.SqlQuery<AddAuthor>(sql).ToList();
            return list;
        }

        public AddAuthor getAuthor(string ms)
        {
            AddAuthor item = new AddAuthor();
            string sql = @"select ah.*, o.office_abbreviation
                            from SM_ScientificProduct.Author ah join General.Office o on ah.office_id = o.office_id
                            where ah.mssv_msnv = @ms
                            order by ah.people_id desc";
            item = db.Database.SqlQuery<AddAuthor>(sql, new SqlParameter("ms", ms)).FirstOrDefault();
            return item;
        }

        public List<PaperType> getPaperType()
        {
            List<PaperType> list = db.PaperTypes.ToList();
            return list;
        }

        public ENTITIES.File addFile(ENTITIES.File file)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
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
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
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

        public List<Title2Name> getListTitle_2Lang()
        {
            string sql = @"select t.title_id, tv.name as 'tv', ta.name as 'ta'
                            from [SM_MasterData].Title t left join
		                            (select t.title_id, tl.name
		                            from [SM_MasterData].Title t join [Localization].TitleLanguage tl on t.title_id = tl.title_id
		                            where tl.language_id = 1) as tv on t.title_id = tv.title_id left join
		                            (select t.title_id, tl.name
		                            from [SM_MasterData].Title t join [Localization].TitleLanguage tl on t.title_id = tl.title_id
		                            where tl.language_id = 2) as ta on t.title_id = ta.title_id
                            where tv.name is not null ";
            List<Title2Name> list = db.Database.SqlQuery<Title2Name>(sql).ToList();
            return list;
        }

        public Title2Name GetTitleWithName(int id)
        {
            string sql = @"select t.title_id, tv.name as 'tv', ta.name as 'ta'
                            from [SM_MasterData].Title t left join
		                            (select t.title_id, tl.name
		                            from [SM_MasterData].Title t join [Localization].TitleLanguage tl on t.title_id = tl.title_id
		                            where tl.language_id = 1) as tv on t.title_id = tv.title_id left join
		                            (select t.title_id, tl.name
		                            from [SM_MasterData].Title t join [Localization].TitleLanguage tl on t.title_id = tl.title_id
		                            where tl.language_id = 2) as ta on t.title_id = ta.title_id
                            where tv.name is not null and t.title_id = @id";
            Title2Name t = db.Database.SqlQuery<Title2Name>(sql, new SqlParameter("id", id)).FirstOrDefault();
            return t;
        }

        public string updateTitle(int id, string tv, string ta)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    TitleLanguage tl = db.TitleLanguages.Where(x => x.title_id == id).Where(x => x.language_id == 1).FirstOrDefault();
                    if (tl == null)
                    {
                        tl = new TitleLanguage
                        {
                            language_id = 1,
                            title_id = id,
                            name = tv
                        };
                        db.TitleLanguages.Add(tl);
                    }
                    else
                    {
                        tl.name = tv;
                        db.Entry(tl).State = EntityState.Modified;
                    }
                    //db.SaveChanges();

                    TitleLanguage tl2 = db.TitleLanguages.Where(x => x.title_id == id).Where(x => x.language_id == 2).FirstOrDefault();
                    if (tl2 == null)
                    {
                        tl2 = new TitleLanguage
                        {
                            language_id = 2,
                            title_id = id,
                            name = ta
                        };
                        db.TitleLanguages.Add(tl2);
                    }
                    else
                    {
                        tl2.name = ta;
                        db.Entry(tl2).State = EntityState.Modified;
                    }

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

        public int addTitle(string tv, string ta)
        {
            TitleLanguage ck1 = db.TitleLanguages.Where(x => x.language_id == 1).Where(x => x.name == tv).FirstOrDefault();
            TitleLanguage ck2 = db.TitleLanguages.Where(x => x.language_id == 2).Where(x => x.name == ta).FirstOrDefault();
            if (ck1 != null || ck2 != null) return -1;

            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    Title t = new Title();
                    db.Titles.Add(t);
                    db.SaveChanges();

                    TitleLanguage tl1 = new TitleLanguage
                    {
                        language_id = 1,
                        name = tv,
                        title_id = t.title_id
                    };
                    db.TitleLanguages.Add(tl1);

                    TitleLanguage tl2 = new TitleLanguage
                    {
                        language_id = 2,
                        name = ta,
                        title_id = t.title_id
                    };
                    db.TitleLanguages.Add(tl2);
                    db.SaveChanges();
                    dbc.Commit();
                    return t.title_id;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return 0;
                }
        }

        public string deleteTitle(int id)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    string sql = @"delete from [Localization].TitleLanguage where title_id = @id
                                delete from [SM_MasterData].Title where title_id = @id";
                    db.Database.ExecuteSqlCommand(sql, new SqlParameter("id", id));
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
