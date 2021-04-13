using Aspose.Cells;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using Microsoft.VisualBasic.FileIO;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.ComponentModel;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Data.Entity.Validation;

namespace BLL.ScienceManagement.Paper
{
    public class PaperRepo
    {
        public DetailPaper getDetail(string id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DetailPaper item = new DetailPaper();
            string sql = @"select p.*, rp.type, rp.reward_type, rp.total_reward, rp.specialization_id, rp.request_id, rp.status_id, f.link as 'link_file'
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
							left join [General].[File] f on p.file_id = f.file_id
                            where p.paper_id = @id";
            item = db.Database.SqlQuery<DetailPaper>(sql, new SqlParameter("id", Int32.Parse(id))).FirstOrDefault();
            return item;
        }

        public List<ListCriteriaOfOnePaper> getCriteria(string id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select pc.name, pwc.*
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].PaperWithCriteria pwc on p.paper_id = pwc.paper_id
	                            join [SM_ScientificProduct].PaperCriteria pc on pwc.criteria_id = pc.criteria_id
                            where p.paper_id = @id";
            List<ListCriteriaOfOnePaper> list = db.Database.SqlQuery<ListCriteriaOfOnePaper>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }

        public Author getAuthorReceived_all(string id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            int paper_id = Int32.Parse(id);
            Author p = (from a in db.RequestPapers
                        join b in db.Authors on a.author_received_reward equals b.people_id
                        where a.paper_id == paper_id
                        select b).FirstOrDefault();
            return p;
        }

        public string addFile(ENTITIES.File f)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            try
            {
                db.Files.Add(f);
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public List<AuthorInfoWithNull> getAuthorPaper(string id, string lang)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select ah.people_id, ah.name, ah.email,ah.office_id, ah.bank_branch, ah.bank_number,ah.tax_code, ah.identification_number,ah.mssv_msnv, ah.contract_id, title.name as 'title_name', ct.name as 'contract_name', o.office_abbreviation, o.office_id as 'office_id_string', ah.title_id as 'title_id_string', case when ah.is_reseacher is null then cast(0 as bit) else ah.is_reseacher end as 'is_reseacher', ap.money_reward, ah.identification_file_link
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].Author ah on ah.people_id = ap.people_id
	                            left join (select ah.people_id, tl.name
				                            from [SM_ScientificProduct].Author ah join [Localization].TitleLanguage tl on ah.title_id = tl.title_id
					                            join [Localization].Language l on tl.language_id = l.language_id
				                            where l.language_name = @lang) as title on ah.people_id = title.people_id
	                            left join [SM_MasterData].ContractType ct on ah.contract_id = ct.contract_id
	                            left join [General].Office o on o.office_id = ah.office_id
                            where p.paper_id = @id";
            List<AuthorInfoWithNull> list = db.Database.SqlQuery<AuthorInfoWithNull>(sql, new SqlParameter("id", id), new SqlParameter("lang", lang)).ToList();
            foreach (var item in list)
            {
                item.title_string = item.title_name;
                if (item.is_reseacher == true) item.title_string += ", Nghiên cứu viên";
                if (item.title_id_string != null) item.title_id = item.title_id_string.Value;
            }
            return list;
        }

        public bool updateJournal()
        {
            int count = 1;
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            Scimagojr sci;
            try
            {
                //download file csv
                string url = "https://www.scimagojr.com/journalrank.php?out=xls";

                string name = RandomString(10);
                string path = @"D:\" + name;
                Directory.CreateDirectory(path);

                string savePath = path + "\\insert.csv";

                WebClient client = new WebClient();
                client.DownloadFile(url, savePath);

                //update db
                db.Database.ExecuteSqlCommand("delete from [SM_ScientificProduct].Scimagojr");
                using (var reader = new StreamReader(savePath))
                {
                    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture);
                    config.Delimiter = ";";
                    config.HasHeaderRecord = true;
                    config.BadDataFound = null;
                    using (var csv = new CsvReader(reader, config))
                    {
                        while (csv.Read())
                        {
                            if (csv.GetField(0) != "Rank")
                            {
                                sci = new Scimagojr();
                                sci.Rank = csv.GetField(0);
                                sci.Sourceid = csv.GetField(1);
                                sci.Title = csv.GetField(2);
                                sci.Type = csv.GetField(3);
                                sci.Issn = csv.GetField(4);
                                sci.SJR = csv.GetField(5);
                                sci.SJR_Best_Quartile = csv.GetField(6);
                                sci.H_index = csv.GetField(7);
                                sci.Total_Docs_2019 = csv.GetField(8);
                                sci.Total_Docs_3years = csv.GetField(9);
                                sci.Total_Refs = csv.GetField(10);
                                sci.Total_Cites_3years = csv.GetField(11);
                                sci.Citable_Docs_3years = csv.GetField(12);
                                sci.Cites_Doc_2years = csv.GetField(13);
                                sci.Ref_Doc = csv.GetField(14);
                                sci.Country = csv.GetField(15);
                                sci.Region = csv.GetField(16);
                                sci.Publisher = csv.GetField(17);
                                sci.Coverage = csv.GetField(18);
                                sci.Categories = csv.GetField(19);

                                db.Scimagojrs.Add(sci);
                                count++;
                            }
                        }
                        db.SaveChanges();

                        //delete folder
                        System.IO.DirectoryInfo di = new DirectoryInfo(path);
                        di.Delete(true);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public DataTable ToDataTable(IEnumerable<dynamic> items)
        {
            var data = items.ToArray();
            if (data.Count() == 0) return null;

            var dt = new DataTable();
            foreach (var key in ((IDictionary<string, object>)data[0]).Keys)
            {
                dt.Columns.Add(key);
            }
            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }

        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public List<AuthorInfoWithNull> getAuthorPaper_FE(string id, string lang)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select ah.people_id, ah.name, ah.email,ah.office_id, ah.bank_branch, ah.bank_number,ah.tax_code, ah.identification_number,ah.mssv_msnv, ah.contract_id, title.name as 'title_name', ct.name as 'contract_name', o.office_abbreviation, o.office_id as 'office_id_string', ah.title_id as 'title_id_string', case when ah.is_reseacher is null then cast(0 as bit) else cast(1 as bit) end as 'is_reseacher'
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].Author ah on ah.people_id = ap.people_id
	                            left join (select ah.people_id, tl.name
				                            from [SM_ScientificProduct].Author ah join [Localization].TitleLanguage tl on ah.title_id = tl.title_id
					                            join [Localization].Language l on tl.language_id = l.language_id
				                            where l.language_name = @lang) as title on ah.people_id = title.people_id
	                            left join [SM_MasterData].ContractType ct on ah.contract_id = ct.contract_id
	                            left join [General].Office o on o.office_id = ah.office_id
                            where p.paper_id = @id and ah.mssv_msnv is not null";
            List<AuthorInfoWithNull> list = db.Database.SqlQuery<AuthorInfoWithNull>(sql, new SqlParameter("id", id), new SqlParameter("lang", lang)).ToList();
            foreach (var item in list)
            {
                item.title_string = item.title_name;
                if (item.is_reseacher == true) item.title_string += ", Nghiên cứu viên";
                if (item.title_id_string != null) item.title_id = item.title_id_string.Value;
            }
            return list;
        }

        public string deleteRequest(int id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                RequestPaper rp = db.RequestPapers.Where(x => x.request_id == id).FirstOrDefault();
                rp.status_id = 1;
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

        public List<string> getDecisionLink(int id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            try
            {
                string sql = @"select f.link
                                from [SM_ScientificProduct].RequestPaper rp join [SM_Request].RequestDecision rd on rd.request_id = rp.request_id
	                                join [SM_Request].Decision d on rd.decision_id = d.decision_id
	                                join [General].[File] f on f.file_id = d.file_id
                                where rp.paper_id = @id";
                List<string> link = db.Database.SqlQuery<string>(sql, new SqlParameter("id", id)).ToList();
                return link;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ENTITIES.Paper addPaper(DetailPaper item)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    ENTITIES.Paper paper = new ENTITIES.Paper
                    {
                        name = item.name,
                        publish_date = item.publish_date,
                        link_doi = item.link_doi,
                        link_scholar = item.link_scholar,
                        journal_name = item.journal_name,
                        page = item.page,
                        vol = item.vol,
                        company = item.company,
                        index = item.index,
                        paper_type_id = item.paper_type_id,
                        note_domestic = item.note_domestic
                    };
                    if (item.file_id != null) paper.file_id = item.file_id;
                    db.Papers.Add(paper);
                    db.SaveChanges();
                    //string sql = @"select p.*
                    //            from [SM_ScientificProduct].Paper p
                    //            where p.name = @name and p.publish_date = @date
                    //            order by p.paper_id desc";
                    //ENTITIES.Paper p = db.Database.SqlQuery<ENTITIES.Paper>(sql, new SqlParameter("name", item.name), new SqlParameter("date", item.publish_date)).FirstOrDefault();
                    dbc.Commit();
                    dbc.Dispose();
                    return paper;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    dbc.Dispose();
                    return null;
                }
            }
        }

        public BaseRequest addBaseRequest(int account_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                BaseRequest b = new BaseRequest
                {
                    account_id = account_id,
                    created_date = DateTime.Today
                };
                db.BaseRequests.Add(b);
                db.SaveChanges();
                //string sql = @"select b.*
                //            from [SM_Request].BaseRequest b 
                //            where b.account_id = @id
                //            order by b.request_id desc";
                //BaseRequest ba = db.Database.SqlQuery<BaseRequest>(sql, new SqlParameter("id", account_id)).FirstOrDefault();
                dbc.Commit();
                dbc.Dispose();
                return b;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                return null;
            }
        }

        public string addRequestPaper(int request_id, RequestPaper r, string daidien)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    r.request_id = request_id;
                    r.status_id = 3;
                    r.total_reward = 0;
                    if (r.reward_type == "Canhan")
                    {
                        Author author = (from a in db.Authors
                                         join b in db.AuthorPapers on a.people_id equals b.people_id
                                         where a.mssv_msnv == daidien && b.paper_id == r.paper_id
                                         select a).FirstOrDefault();
                        //Person p = (from a in db.People
                        //            join b in db.Profiles on a.people_id equals b.people_id
                        //            where b.mssv_msnv == daidien
                        //            select a).FirstOrDefault();
                        r.author_received_reward = author.people_id;
                    }
                    db.RequestPapers.Add(r);
                    db.SaveChanges();
                    dbc.Commit();
                    dbc.Dispose();
                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    dbc.Dispose();
                    return null;
                }
            }
        }

        public string updateRewardAuthorAfterDecision(List<AddAuthor> people, int paper_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                foreach (var item in people)
                {
                    string temp = item.money_string.Replace(",", "");
                    int money = Int32.Parse(temp);
                    AuthorPaper ap = db.AuthorPapers.Where(x => x.people_id == item.people_id)
                                                    .Where(x => x.paper_id == paper_id)
                                                    .FirstOrDefault();
                    ap.money_reward = money;
                }
                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                return "ff";
            }
        }

        public string getAuthorReceived(string id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            int paper_id = Int32.Parse(id);
            string ms = (from a in db.RequestPapers
                         join b in db.Authors on a.author_received_reward equals b.people_id
                         where a.paper_id == paper_id
                         select b.mssv_msnv).FirstOrDefault();
            return ms;
        }

        public string addAuthor(List<AddAuthor> list, string paper_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    List<Author> listAuthor = new List<Author>();
                    int paper_id_int = Int32.Parse(paper_id);
                    foreach (var item in list)
                    {
                        Author author = new Author
                        {
                            name = item.name,
                            email = item.email
                        };
                        if (item.office_id != 0)
                        {
                            author.office_id = item.office_id;
                            author.bank_number = item.bank_number;
                            author.bank_branch = item.bank_branch;
                            author.tax_code = item.tax_code;
                            author.identification_number = item.identification_number;
                            author.mssv_msnv = item.mssv_msnv;
                            author.is_reseacher = item.is_reseacher;
                            author.title_id = item.title_id;
                            author.contract_id = 1;
                            author.identification_file_link = item.identification_file_link;
                        }
                        db.Authors.Add(author);
                        listAuthor.Add(author);
                    }
                    db.SaveChanges();

                    foreach (var item in listAuthor)
                    {
                        AuthorPaper ap = new AuthorPaper
                        {
                            people_id = item.people_id,
                            paper_id = paper_id_int,
                            money_reward = 0,
                            money_reward_in_decision = 0
                        };
                        db.AuthorPapers.Add(ap);
                    }
                    db.SaveChanges();

                    dbc.Commit();
                    dbc.Dispose();
                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    dbc.Dispose();
                    return "ff";
                }
            }
        }

        public string changeStatus(DetailPaper paper)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                RequestPaper rp = db.RequestPapers.Where(x => x.request_id == paper.request_id).FirstOrDefault();
                rp.status_id = 5;
                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                dbc.Rollback();
                dbc.Dispose();
                return "ff";
            }
        }

        public string changeStatusManager(DetailPaper paper)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                RequestPaper rp = db.RequestPapers.Where(x => x.request_id == paper.request_id).FirstOrDefault();
                rp.status_id = 3;
                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                dbc.Rollback();
                dbc.Dispose();
                return "ff";
            }
        }

        public string uploadDecision(DateTime date_format1, int file_id1, string number1, string file_drive_id1, int research)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                Decision decision = new Decision
                {
                    valid_date = date_format1,
                    file_id = file_id1,
                    decision_number = number1
                };
                db.Decisions.Add(decision);
                db.SaveChanges();

                List<WaitDecisionPaper> list = getListWait_UploadQDGV(0);
                List<WaiDecisionHaveReseacher> list2 = getListHaveReseacher();

                foreach (var item in list)
                {
                    RequestDecision request = new RequestDecision
                    {
                        request_id = item.request_id,
                        decision_id = decision.decision_id
                    };
                    db.RequestDecisions.Add(request);

                    RequestPaper rc = db.RequestPapers.Where(x => x.request_id == item.request_id).FirstOrDefault();
                    rc.status_id = 2;
                }

                foreach (var item in list2)
                {
                    RequestPaper rc = db.RequestPapers.Where(x => x.request_id == item.request_id).FirstOrDefault();
                    rc.status_id = 6;
                }

                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                GoogleDriveService.DeleteFile(file_drive_id1);
                return "ff";
            }
        }

        public string uploadDecision2(DateTime date_format1, int file_id1, string number1, string file_drive_id1, int research)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                Decision decision = new Decision
                {
                    valid_date = date_format1,
                    file_id = file_id1,
                    decision_number = number1
                };
                db.Decisions.Add(decision);
                db.SaveChanges();

                List<WaitDecisionPaper> list = getListWait_UploadNCV(1);
                List<WaiDecisionHaveReseacher> list2 = getListHaveReseacher();

                foreach (var item in list)
                {
                    RequestDecision request = new RequestDecision
                    {
                        request_id = item.request_id,
                        decision_id = decision.decision_id
                    };
                    db.RequestDecisions.Add(request);

                    RequestPaper rc = db.RequestPapers.Where(x => x.request_id == item.request_id).FirstOrDefault();
                    rc.status_id = 2;
                }

                foreach (var item in list2)
                {
                    RequestPaper rc = db.RequestPapers.Where(x => x.request_id == item.request_id).FirstOrDefault();
                    rc.status_id = 7;
                }

                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                GoogleDriveService.DeleteFile(file_drive_id1);
                return "ff";
            }
        }

        public List<WaiDecisionHaveReseacher> getListHaveReseacher()
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select a.*
                            from (select p.paper_id, rp.request_id, p.name, count(rd.decision_id) as 'sum'
		                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
			                            join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
			                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
			                            left join [SM_Request].RequestDecision rd on rd.request_id = rp.request_id
		                            where rp.status_id in (4) and ah.is_reseacher = 0 and ah.office_id is not null
		                            group by p.paper_id, rp.request_id, p.name) as a
                            join (select p.paper_id, rp.request_id, p.name, count(rd.decision_id) as 'sum'
		                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
			                            join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
			                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
			                            left join [SM_Request].RequestDecision rd on rd.request_id = rp.request_id
		                            where rp.status_id in (4) and ah.is_reseacher = 1 and ah.office_id is not null
		                            group by p.paper_id, rp.request_id, p.name) as b on a.request_id = b.request_id 
                            where a.sum < 2";
            List<WaiDecisionHaveReseacher> list = db.Database.SqlQuery<WaiDecisionHaveReseacher>(sql).ToList();
            return list;
        }

        public List<WaitDecisionPaper> getListWait_UploadQDGV(int reseacher)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select p.name, p.company, po.name as 'author_name', pro.mssv_msnv, o.office_abbreviation, count(ap.people_id) as 'note', rp.request_id, p.paper_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join [SM_Request].BaseRequest br on rp.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on o.office_id = po.office_id
								join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
                            where rp.status_id in (4, 7) and ah.is_reseacher = @reseacher
                            group by p.name, p.company, po.name, pro.mssv_msnv, o.office_abbreviation, rp.request_id, p.paper_id";
            List<WaitDecisionPaper> list = db.Database.SqlQuery<WaitDecisionPaper>(sql, new SqlParameter("reseacher", reseacher)).ToList();
            return list;
        }

        public List<WaitDecisionPaper> getListWait_UploadNCV(int reseacher)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select p.name, p.company, po.name as 'author_name', pro.mssv_msnv, o.office_abbreviation, count(ap.people_id) as 'note', rp.request_id, p.paper_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join [SM_Request].BaseRequest br on rp.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on o.office_id = po.office_id
								join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
                            where rp.status_id in (4, 6) and ah.is_reseacher = @reseacher
                            group by p.name, p.company, po.name, pro.mssv_msnv, o.office_abbreviation, rp.request_id, p.paper_id";
            List<WaitDecisionPaper> list = db.Database.SqlQuery<WaitDecisionPaper>(sql, new SqlParameter("reseacher", reseacher)).ToList();
            return list;
        }

        public int addPeople(string name, string mail, Nullable<int> office_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    Person p = new Person
                    {
                        name = name,
                        email = mail
                    };
                    if (office_id != 0) p.office_id = office_id;
                    db.People.Add(p);
                    db.SaveChanges();
                    string sql = @"select people_id
                            from [General].People
                            where email = @mail";
                    int result = db.Database.SqlQuery<int>(sql, new SqlParameter("mail", mail)).FirstOrDefault();
                    dbc.Commit();
                    dbc.Dispose();
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    dbc.Dispose();
                    return 0;
                }
            }
        }

        public void addProfile(AddAuthor a)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    Profile pro = new Profile
                    {
                        people_id = a.people_id,
                        //bank_number = a.bank_number,
                        bank_branch = a.bank_branch,
                        tax_code = a.tax_code,
                        identification_number = a.identification_number,
                        //office_id = a.office_id,
                        mssv_msnv = a.mssv_msnv,
                        title_id = a.title_id
                    };
                    db.Profiles.Add(pro);
                    db.SaveChanges();

                    string sql = @"insert into [SM_Researcher].PeopleContract values (@people, @contract)";
                    db.Database.ExecuteSqlCommand(sql, new SqlParameter("people", a.people_id), new SqlParameter("contract", a.contract_id));

                    db.SaveChanges();
                    dbc.Commit();
                    dbc.Dispose();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    dbc.Dispose();
                    return;
                }
            }
        }

        public string addCriteria(List<CustomCriteria> criteria, string paper_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    if (criteria == null) return "ss";
                    string temp = "";
                    foreach (var item in criteria)
                    {
                        temp += "," + item.name;
                    }
                    temp = temp.Substring(1);
                    string[] listCriName = temp.Split(',');

                    String strAppend = "";
                    List<SqlParameter> listParam = new List<SqlParameter>();
                    for (int i = 0; i < listCriName.Length; i++)
                    {
                        SqlParameter param = new SqlParameter("@idParam" + i, listCriName[i]);
                        listParam.Add(param);
                        string paramName = "@idParam" + i;
                        strAppend += paramName + ",";
                    }
                    strAppend = strAppend.ToString().Remove(strAppend.LastIndexOf(","), 1);
                    string sql = @"select pc.*
                                from [SM_ScientificProduct].PaperCriteria pc
                                where pc.name in (" + strAppend + ")";
                    List<CustomCriteria> list = db.Database.SqlQuery<CustomCriteria>(sql, listParam.ToArray()).ToList();

                    int paper_id_int = Int32.Parse(paper_id);
                    ENTITIES.Paper paper = db.Papers.Where(x => x.paper_id == paper_id_int).FirstOrDefault();

                    foreach (var item in criteria)
                    {
                        foreach (var cri in list)
                        {
                            if (cri.name == item.name)
                            {
                                item.criteria_id = cri.criteria_id;
                                if (item.name == "ISI")
                                {
                                    SCIE scie = db.SCIEs.Where(x => x.Journal_title == paper.journal_name).FirstOrDefault();
                                    SSCI ssci = db.SSCIs.Where(x => x.Journal_title == paper.journal_name).FirstOrDefault();
                                    if (scie != null || ssci != null)
                                    {
                                        item.check = true;
                                    }
                                }
                                else if (item.name == "Scopus")
                                {
                                    Scopu scopu = db.Scopus.Where(x => x.Source_Title_Medline_sourced_journals_are_indicated_in_Green == paper.journal_name).FirstOrDefault();
                                    if (scopu != null)
                                    {
                                        if (scopu.Active_or_Inactive == "Active") item.check = true;
                                    }
                                }
                                else
                                {
                                    item.check = false;
                                }
                            }
                        }
                        PaperWithCriteria pwc = new PaperWithCriteria
                        {
                            paper_id = paper_id_int,
                            criteria_id = item.criteria_id,
                            link = item.link,
                            check = item.check,
                            manager_check = false
                        };
                        db.PaperWithCriterias.Add(pwc);
                    }
                    db.SaveChanges();
                    dbc.Commit();
                    dbc.Dispose();
                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    dbc.Dispose();
                    return "ff";
                }
            }
        }

        public string updatePaper(string paper_id, ENTITIES.Paper paper)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                int id = Int32.Parse(paper_id);
                ENTITIES.Paper p = db.Papers.Where(x => x.paper_id == id).FirstOrDefault();
                p.name = paper.name;
                p.publish_date = paper.publish_date;
                p.link_doi = paper.link_doi;
                p.link_scholar = paper.link_scholar;
                p.journal_name = paper.journal_name;
                p.page = paper.page;
                p.vol = paper.vol;
                p.company = paper.company;
                p.index = paper.index;
                p.paper_type_id = paper.paper_type_id;
                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                return "ff";
            }
        }

        public string updateRequest(RequestPaper item, string daidien)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                RequestPaper rp = (from a in db.RequestPapers
                                   join b in db.BaseRequests on a.request_id equals b.request_id
                                   where a.paper_id == item.paper_id
                                   orderby b.created_date descending
                                   select a).FirstOrDefault();
                rp.specialization_id = item.specialization_id;
                rp.type = item.type;
                rp.reward_type = item.reward_type;
                rp.status_id = 3;

                if (rp.reward_type == "Canhan")
                {
                    Author author = (from a in db.Authors
                                     join b in db.AuthorPapers on a.people_id equals b.people_id
                                     where a.mssv_msnv == daidien && b.paper_id == rp.paper_id
                                     select a).FirstOrDefault();
                    rp.author_received_reward = author.people_id;
                }

                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                return "ff";
            }
        }

        public string updateCriteria(List<CustomCriteria> criteria, string paper_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                db.Database.ExecuteSqlCommand("delete from [SM_ScientificProduct].PaperWithCriteria where paper_id = @id", new SqlParameter("id", paper_id));
                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                string mess = addCriteria(criteria, paper_id);
                return mess;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                return "ff";
            }
        }

        public string updateAuthor(List<AddAuthor> people, string paper_id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                List<AddAuthor> list = new List<AddAuthor>();
                //db.Database.ExecuteSqlCommand("delete from[SM_ScientificProduct].AuthorPaper where paper_id = @id", new SqlParameter("id", paper_id));
                //string mess = addAuthor(people, paper_id);
                foreach (var item in people)
                {
                    if (item.people_id == 0)
                    {
                        list.Add(item);
                    }
                    else
                    {
                        Author author = db.Authors.Where(x => x.people_id == item.people_id).FirstOrDefault();
                        author.name = item.name;
                        author.email = item.email;
                        if (item.office_id == 0 || item.office_id == null)
                        {
                            author.office_id = null;
                        }
                        else
                        {
                            author.office_id = item.office_id;
                            author.bank_number = item.bank_number;
                            author.bank_branch = item.bank_branch;
                            author.tax_code = item.tax_code;
                            author.identification_number = item.identification_number;
                            author.mssv_msnv = item.mssv_msnv;
                            author.is_reseacher = item.is_reseacher;
                            author.title_id = item.title_id;
                            author.contract_id = 1;
                            author.identification_file_link = item.identification_file_link;
                        }
                        db.Entry(author).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                addAuthor(list, paper_id);
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                return "ff";
            }
        }

        public List<PendingPaper_Manager> listPending()
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            List<PendingPaper_Manager> list = new List<PendingPaper_Manager>();
            string sql = @"select p.name, a.email, br.created_date, p.paper_id, rp.status_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join [SM_Request].BaseRequest br on rp.request_id = br.request_id
	                            join [General].Account a on br.account_id = a.account_id
                            where rp.status_id = 3 or rp.status_id = 5";
            list = db.Database.SqlQuery<PendingPaper_Manager>(sql).ToList();
            return list;
        }

        public string updateRewardPaper(DetailPaper paper)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                RequestPaper p = db.RequestPapers.Where(x => x.request_id == paper.request_id).FirstOrDefault();
                p.total_reward = paper.total_reward;
                p.status_id = 4;
                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                return "ff";
            }
        }

        public string updateAuthorReward(DetailPaper paper, List<AuthorInfoWithNull> people, string id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                foreach (var item in people)
                {
                    AuthorPaper ap = db.AuthorPapers
                                        .Where(x => x.paper_id == paper.paper_id)
                                        .Where(x => x.people_id == item.people_id)
                                        .FirstOrDefault();
                    ap.money_reward = item.money_reward;
                    ap.money_reward_in_decision = item.money_reward;
                }
                if (paper.reward_type == "Canhan")
                {
                    int people_id = Int32.Parse(id);
                    AuthorPaper ap = db.AuthorPapers
                                       .Where(x => x.paper_id == paper.paper_id)
                                       .Where(x => x.people_id == people_id)
                                       .FirstOrDefault();
                    ap.money_reward_in_decision = paper.total_reward;
                }

                db.SaveChanges();
                dbc.Commit();
                dbc.Dispose();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                dbc.Dispose();
                return "ff";
            }
        }

        public string updateCriteria_ManagerCheck(int id)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                List<PaperWithCriteria> list = db.PaperWithCriterias.Where(x => x.paper_id == id).ToList();
                foreach (var item in list)
                {
                    item.manager_check = true;
                    db.Entry(item).State = EntityState.Modified;
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

        public List<WaitDecisionPaper> getListWwaitDecision(string type, int reseacher)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select p.name, p.journal_name, po.name as 'author_name', pro.mssv_msnv, o.office_abbreviation, a.note, rp.request_id, p.paper_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join [SM_Request].BaseRequest br on rp.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on o.office_id = po.office_id
								join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
								join (select p.paper_id, count(ap.people_id) as 'note'
										from SM_ScientificProduct.Paper p join SM_ScientificProduct.AuthorPaper ap on p.paper_id = ap.paper_id
										group by p.paper_id) as a on p.paper_id = a.paper_id
                            where rp.status_id in (4, 6) and rp.type = @type and ah.is_reseacher = @reseacher";
            List<WaitDecisionPaper> list = db.Database.SqlQuery<WaitDecisionPaper>(sql, new SqlParameter("type", type), new SqlParameter("reseacher", reseacher)).ToList();
            return list;
        }

        public List<WaitDecisionPaper> getListWwaitDecision2(string type, int reseacher)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select p.name, p.journal_name, po.name as 'author_name', pro.mssv_msnv, o.office_abbreviation, a.note, rp.request_id, p.paper_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join [SM_Request].BaseRequest br on rp.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on o.office_id = po.office_id
								join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
								join (select p.paper_id, count(ap.people_id) as 'note'
										from SM_ScientificProduct.Paper p join SM_ScientificProduct.AuthorPaper ap on p.paper_id = ap.paper_id
										group by p.paper_id) as a on p.paper_id = a.paper_id
                            where rp.status_id in (4, 7) and rp.type = @type and ah.is_reseacher = @reseacher";
            List<WaitDecisionPaper> list = db.Database.SqlQuery<WaitDecisionPaper>(sql, new SqlParameter("type", type), new SqlParameter("reseacher", reseacher)).ToList();
            return list;
        }

        public List<Paper_Appendix_1> getListAppendix1_2(string type, int reseacher)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select ah.name as 'author_name', ah.mssv_msnv, o.office_abbreviation, p.name, p.journal_name, a.sum, b.sumFE, p.paper_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
	                            join [General].Office o on ah.office_id = o.office_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join (select p.paper_id, count(ap.people_id) as 'sum'
			                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
			                            group by p.paper_id) as a on p.paper_id = a.paper_id
	                            join (select p.paper_id, sum(case when ah.mssv_msnv is null then 0 else 1 end) as 'sumFE'
			                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
			                            join [SM_ScientificProduct].Author ah on ah.people_id = ap.people_id
			                            group by p.paper_id) as b on p.paper_id = b.paper_id
                            where rp.status_id in (4, 6, 7) and rp.type = @type and ah.is_reseacher = @reseacher
                            order by ah.name";
            List<Paper_Appendix_1> list = db.Database.SqlQuery<Paper_Appendix_1>(sql, new SqlParameter("type", type), new SqlParameter("reseacher", reseacher)).ToList();
            return list;
        }

        public List<Paper_Apendix_3> getListAppendix3_4(string type, int reseacher)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select ah.name, ah.mssv_msnv, o.office_abbreviation, case when sum(ap.money_reward_in_decision) is null then 0 else sum(ap.money_reward_in_decision) end as 'sum_money', ah.identification_file_link
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
	                            join [General].Office o on ah.office_id = o.office_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
                            where rp.status_id in (4, 6, 7) and rp.type = @type and ah.is_reseacher = @reseacher
                            group by ah.name, ah.mssv_msnv, o.office_abbreviation, ah.identification_file_link
                            having sum(ap.money_reward_in_decision) > 0
                            order by ah.name";
            List<Paper_Apendix_3> list = db.Database.SqlQuery<Paper_Apendix_3>(sql, new SqlParameter("type", type), new SqlParameter("reseacher", reseacher)).ToList();
            return list;
        }

        public List<string> getLstEmailAuthor(int reseacher)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            string sql = @"select distinct ah.email
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
	                            join [General].Office o on ah.office_id = o.office_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
                            where rp.status_id in (4, 6, 7) and ah.is_reseacher = @reseacher";
            List<string> list = db.Database.SqlQuery<string>(sql, new SqlParameter("reseacher", reseacher)).ToList();
            return list;
        }
    }
}
