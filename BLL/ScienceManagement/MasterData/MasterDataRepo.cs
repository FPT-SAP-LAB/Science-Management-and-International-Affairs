using CsvHelper;
using CsvHelper.Configuration;
using ENTITIES;
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
        public List<SpecializationLanguage> GetSpec(string language)
        {
            if (language == null) language = "";
            string sql = @"select sl.name, sl.specialization_id, sl.language_id
                            from [General].Specialization s join [Localization].SpecializationLanguage sl on s.specialization_id = sl.specialization_id
	                            join [Localization].Language l on sl.language_id = l.language_id
                            where l.language_name = @lang";
            List<SpecializationLanguage> list = db.Database.SqlQuery<SpecializationLanguage>(sql, new SqlParameter("lang", language)).ToList();
            return list;
        }

        public static BaseServerSideData<Scopu> GetListAllScopus(BaseDatatable baseDatatable, string name_search)
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

        public bool UpdateJournal(HttpPostedFileBase file_scopus, HttpPostedFileBase file_SCIE, HttpPostedFileBase file_SSCI)
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
                            Scopu temp = new Scopu
                            {
                                Sourcerecord_ID = worksheet.Cells[i, 1].Value.ToString(),
                                Source_Title_Medline_sourced_journals_are_indicated_in_Green = worksheet.Cells[i, 2].Value.ToString()
                            };
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
                        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            Delimiter = ",",
                            HasHeaderRecord = true,
                            BadDataFound = null
                        };

                        using (var csv = new CsvReader(reader, config))
                        {
                            int count = 1;
                            while (csv.Read())
                            {
                                if (csv.GetField(0) != "Journal title")
                                {
                                    SCIE temp = new SCIE
                                    {
                                        Journal_title = csv.GetField(0)
                                    };
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
                        CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            Delimiter = ",",
                            HasHeaderRecord = true,
                            BadDataFound = null
                        };

                        using (var csv = new CsvReader(reader, config))
                        {
                            int count = 1;
                            while (csv.Read())
                            {
                                if (csv.GetField(0) != "Journal title")
                                {
                                    SSCI temp = new SSCI
                                    {
                                        Journal_title = csv.GetField(0)
                                    };
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

        public static BaseServerSideData<CustomISI> GetListAllISI(BaseDatatable baseDatatable, string name_search)
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

        public List<PaperCriteria> GetPaperCriteria()
        {
            string sql = @"select pc.*
                            from SM_ScientificProduct.PaperCriteria pc join
	                            (select MAX(policy_id) as 'policy_id'
	                            from SM_ScientificProduct.PaperCriteria) as a on pc.policy_id = a.policy_id";
            List<PaperCriteria> list = db.Database.SqlQuery<PaperCriteria>(sql).ToList();
            return list;
        }

        public AuthorPaper GetMonry(AddAuthor item, int paper_id)
        {
            AuthorPaper ap = db.AuthorPapers
                .Where(x => x.paper_id == paper_id)
                .Where(x => x.people_id == item.people_id).FirstOrDefault();
            return ap;
        }

        public List<Office> GetOffice()
        {
            List<Office> list = db.Offices.ToList();
            return list;
        }

        public List<Area> GetAreas()
        {
            List<Area> list = db.Areas.ToList();
            return list;
        }

        public List<TitleWithName> GetTitle(string lang)
        {
            if (lang == null) lang = "";
            string sql = @"select tl.name, t.*
                            from [SM_MasterData].Title t join [Localization].TitleLanguage tl on t.title_id = tl.title_id
	                            join [Localization].Language l on l.language_id = tl.language_id
                            where l.language_name = @lang";
            List<TitleWithName> list = db.Database.SqlQuery<TitleWithName>(sql, new SqlParameter("lang", lang)).ToList();
            return list;
        }

        public List<ContractType> GetContract()
        {
            List<ContractType> list = db.ContractTypes.ToList();
            return list;
        }

        public List<AddAuthor> GetListPeopleFE()
        {
            string sql = @"select distinct po.mssv_msnv
                            from SM_ScientificProduct.Author po 
	                            join [General].Office ofi on po.office_id = ofi.office_id";
            List<AddAuthor> list = db.Database.SqlQuery<AddAuthor>(sql).ToList();
            return list;
        }

        public AddAuthor GetAuthor(string ms)
        {
            if (ms == null) ms = "";
            string sql = @"select ah.*, o.office_abbreviation
                            from SM_ScientificProduct.Author ah join General.Office o on ah.office_id = o.office_id
                            where ah.mssv_msnv = @ms
                            order by ah.people_id desc";
            AddAuthor item = db.Database.SqlQuery<AddAuthor>(sql, new SqlParameter("ms", ms)).FirstOrDefault();
            return item;
        }

        public List<PaperType> GetPaperType()
        {
            List<PaperType> list = db.PaperTypes.ToList();
            return list;
        }

        public ENTITIES.File AddFile(ENTITIES.File file)
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

        public List<Title2Name> GetListTitle_2Lang()
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

        public string UpdateTitle(int id, string tv, string ta)
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

        public int AddTitle(string tv, string ta)
        {
            TitleLanguage ck1 = db.TitleLanguages.Where(x => x.language_id == 1).Where(x => x.name == tv).FirstOrDefault();
            TitleLanguage ck2 = db.TitleLanguages.Where(x => x.language_id == 2).Where(x => x.name == ta).FirstOrDefault();
            if (ck1 != null || ck2 != null) return -1;
            if (tv == null || ta == null || tv.Trim() == "" || ta.Trim() == "") return 0;
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

        public string DeleteTitle(int id)
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
