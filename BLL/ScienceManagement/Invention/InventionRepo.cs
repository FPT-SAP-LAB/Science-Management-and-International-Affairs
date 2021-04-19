using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Invention;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            string sql = @"select i.*, it.name as 'type_name', ri.reward_type, ri.total_reward, ri.request_id, f.link as 'link_file', it.name as 'type_name', ri.status_id
                           from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].InventionType it on i.type_id = it.invention_type_id
	                            join [SM_ScientificProduct].RequestInvention ri on i.invention_id = ri.invention_id
	                            join [General].[File] f on f.file_id = i.file_id
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

        public List<InventionType> getType()
        {
            List<InventionType> list = db.InventionTypes.ToList();
            return list;
        }

        public List<AuthorInfoWithNull> getAuthor(string id, string lang)
        {
            List<AuthorInfoWithNull> list = new List<AuthorInfoWithNull>();
            string sql = @"select ah.people_id, ah.name, ah.email,ah.office_id, ah.bank_branch, ah.bank_number,ah.tax_code, ah.identification_number,ah.mssv_msnv, ah.contract_id, title.name as 'title_name', ct.name as 'contract_name', o.office_abbreviation, o.office_id as 'office_id_string', ah.title_id as 'title_id_string', case when ah.is_reseacher is null then cast(0 as bit) else ah.is_reseacher end as 'is_reseacher', ai.money_reward
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].AuthorInvention ai on i.invention_id = ai.invention_id
								join [SM_ScientificProduct].Author ah on ai.people_id = ah.people_id
								left join (select ah.people_id, tl.name
				                            from [SM_ScientificProduct].Author ah join [Localization].TitleLanguage tl on ah.title_id = tl.title_id
					                            join [Localization].Language l on tl.language_id = l.language_id
				                            where l.language_name = @lang) as title on ah.people_id = title.people_id
	                            left join [SM_MasterData].ContractType ct on ah.contract_id = ct.contract_id
	                            left join [General].Office o on ah.office_id = o.office_id
                            where i.invention_id = @id";
            list = db.Database.SqlQuery<AuthorInfoWithNull>(sql, new SqlParameter("id", id), new SqlParameter("lang", lang)).ToList();
            foreach (var item in list)
            {
                item.title_string = item.title_name;
                if (item.is_reseacher == true) item.title_string += ", Nghiên cứu viên";
                if (item.title_id_string != null) item.title_id = item.title_id_string.Value;
            }
            return list;
        }

        public List<string> getAuthorEmail()
        {
            string sql = @"select distinct ah.email
                            from [SM_ScientificProduct].Invention p join [SM_ScientificProduct].AuthorInvention ap on p.invention_id = ap.invention_id
	                            join [SM_ScientificProduct].Author ah on ap.people_id = ah.people_id
	                            join [SM_ScientificProduct].RequestInvention rp on p.invention_id = rp.invention_id
                            where rp.status_id in (4, 6, 7)";
            List<string> list = db.Database.SqlQuery<string>(sql).ToList();
            return list;
        }

        public string deleteRequest(int id)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                RequestInvention rp = db.RequestInventions.Where(x => x.request_id == id).FirstOrDefault();
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

        public string changeStatus(DetailInvention inven)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                RequestInvention ri = db.RequestInventions.Where(x => x.request_id == inven.request_id).FirstOrDefault();
                ri.status_id = 5;
                db.SaveChanges();
                dbc.Commit();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                dbc.Rollback();
                return "ff";
            }
        }

        public string changeStatusManager(DetailInvention inven)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                RequestInvention ri = db.RequestInventions.Where(x => x.request_id == inven.request_id).FirstOrDefault();
                ri.status_id = 3;
                db.SaveChanges();
                dbc.Commit();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                dbc.Rollback();
                return "ff";
            }
        }

        public int addInven_refactor(ENTITIES.Invention inven, string type, List<Country> listCountry, List<AddAuthor> author, File fl, Account acc)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                InventionType ck = db.InventionTypes.Where(x => x.name == type).FirstOrDefault();
                if (ck == null)
                {
                    InventionType ip = new InventionType
                    {
                        name = type
                    };
                    db.InventionTypes.Add(ip);
                    db.SaveChanges();
                }
                inven.type_id = ck.invention_type_id;
                db.Inventions.Add(inven);
                db.SaveChanges();

                BaseRequest b = new BaseRequest
                {
                    account_id = acc.account_id,
                    created_date = DateTime.Today
                };
                db.BaseRequests.Add(b);
                db.SaveChanges();

                RequestInvention ri = new RequestInvention
                {
                    request_id = b.request_id,
                    status_id = 3,
                    invention_id = inven.invention_id,
                    reward_type = "2"
                };
                db.RequestInventions.Add(ri);
                db.SaveChanges();

                foreach (var item in listCountry)
                {
                    InventionCountry ic = new InventionCountry()
                    {
                        invention_id = inven.invention_id,
                        country_id = item.country_id
                    };
                    db.InventionCountries.Add(ic);
                }
                db.SaveChanges();

                List<Author> listAuthor = new List<Author>();
                foreach (var item in author)
                {
                    Author temp = new Author
                    {
                        name = item.name,
                        email = item.email
                    };
                    if (item.office_id != 0)
                    {
                        temp.office_id = item.office_id;
                        temp.bank_number = item.bank_number;
                        temp.bank_branch = item.bank_branch;
                        temp.tax_code = item.tax_code;
                        temp.identification_number = item.identification_number;
                        temp.mssv_msnv = item.mssv_msnv;
                        temp.is_reseacher = item.is_reseacher;
                        temp.title_id = item.title_id;
                        temp.contract_id = item.contract_id;
                    }
                    db.Authors.Add(temp);
                    listAuthor.Add(temp);
                }
                db.SaveChanges();

                foreach (var item in listAuthor)
                {
                    AuthorInvention ai = new AuthorInvention
                    {
                        people_id = item.people_id,
                        invention_id = inven.invention_id,
                        money_reward = 0
                    };
                    db.AuthorInventions.Add(ai);
                }
                db.SaveChanges();

                dbc.Commit();
                return inven.invention_id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                return 0;
            }
        }

        public string uploadDecision(DateTime date_format, int file_id, string number, string file_drive_id)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                Decision decision = new Decision
                {
                    valid_date = date_format,
                    file_id = file_id,
                    decision_number = number
                };
                db.Decisions.Add(decision);

                List<WaitDecisionInven> wait = getListWaitDecision();
                foreach (var item in wait)
                {
                    RequestDecision request = new RequestDecision
                    {
                        request_id = item.request_id,
                        decision_id = decision.decision_id
                    };
                    db.RequestDecisions.Add(request);
                    RequestInvention rc = db.RequestInventions.Where(x => x.request_id == item.request_id).FirstOrDefault();
                    rc.status_id = 2;
                }

                foreach (var item in wait)
                {
                    BaseRequest br = db.BaseRequests.Where(x => x.request_id == item.request_id).FirstOrDefault();
                    br.finished_date = DateTime.Now;
                    db.Entry(br).State = EntityState.Modified;
                }

                db.SaveChanges();
                dbc.Commit();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                GoogleDriveService.DeleteFile(file_drive_id);
                return "ff";
            }
        }

        public string addFile(File f)
        {
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

        public InventionType addInvenType(string name)
        {
            try
            {
                InventionType ck = db.InventionTypes.Where(x => x.name == name).FirstOrDefault();
                if (ck == null)
                {
                    InventionType ip = new InventionType
                    {
                        name = name
                    };
                    db.InventionTypes.Add(ip);
                    db.SaveChanges();
                    return ip;
                }
                else
                {
                    db.SaveChanges();
                    return ck;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ENTITIES.Invention addInven(ENTITIES.Invention inven)
        {
            try
            {
                db.Inventions.Add(inven);
                db.SaveChanges();
                return inven;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string addAuthor(List<AddAuthor> list, int invention_id)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                List<Author> listAuthor = new List<Author>();
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
                        author.contract_id = item.contract_id;
                    }
                    db.Authors.Add(author);
                    listAuthor.Add(author);
                }
                db.SaveChanges();

                foreach (var item in listAuthor)
                {
                    AuthorInvention ai = new AuthorInvention
                    {
                        people_id = item.people_id,
                        invention_id = invention_id,
                        money_reward = 0
                    };
                    db.AuthorInventions.Add(ai);
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

        public string addInvenRequest(BaseRequest br, ENTITIES.Invention inven)
        {
            try
            {
                RequestInvention ri = new RequestInvention
                {
                    request_id = br.request_id,
                    status_id = 3,
                    invention_id = inven.invention_id,
                    reward_type = "2"
                };
                db.RequestInventions.Add(ri);
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string editInven(ENTITIES.Invention inven)
        {
            try
            {
                ENTITIES.Invention i = db.Inventions.Where(x => x.invention_id == inven.invention_id).FirstOrDefault();
                i.name = inven.name;
                i.no = inven.no;
                i.type_id = inven.type_id;
                i.date = inven.date;
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string editRequest(string id, string type)
        {
            try
            {
                int request_id = Int32.Parse(id);
                RequestInvention ri = db.RequestInventions.Where(x => x.request_id == request_id).FirstOrDefault();
                ri.reward_type = type;
                ri.status_id = 3;
                //db.Entry(ri).State = EntityState.Modified;
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string updateAuthor(int id, List<AddAuthor> people)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                List<AddAuthor> list = new List<AddAuthor>();
                //string sql = @"delete from [SM_ScientificProduct].AuthorInvention where invention_id = @id";
                //db.Database.ExecuteSqlCommand(sql, new SqlParameter("id", id));
                //addAuthor(people, id);
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
                addAuthor(list, id);
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dbc.Rollback();
                return "ff";
            }
        }

        public List<PendingInvention_Manager> getListPending()
        {
            string sql = @"select i.name, acc.email, br.created_date, i.invention_id, ri.status_id
                           from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].RequestInvention ri on i.invention_id = ri.invention_id
	                            join [SM_Request].BaseRequest br on ri.request_id = br.request_id
	                            join [General].Account acc on acc.account_id = br.account_id
                           where ri.status_id = 3 or ri.status_id = 5";
            List<PendingInvention_Manager> list = db.Database.SqlQuery<PendingInvention_Manager>(sql).ToList();
            return list;
        }

        public string updateRewardInven(DetailInvention item)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                RequestInvention ri = db.RequestInventions.Where(x => x.request_id == item.request_id).FirstOrDefault();
                ri.total_reward = item.total_reward;
                ri.status_id = 4;
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

        public string updateAuthorReward(DetailInvention inven, List<AuthorInfoWithNull> people)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                foreach (var item in people)
                {
                    AuthorInvention ai = db.AuthorInventions
                                            .Where(x => x.invention_id == inven.invention_id)
                                            .Where(x => x.people_id == item.people_id)
                                            .FirstOrDefault();
                    ai.money_reward = item.money_reward;
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

        public List<WaitDecisionInven> getListWaitDecision()
        {
            string sql = @"select i.name, it.name as 'type_name', po.name as 'author_name', pro.mssv_msnv, o.office_abbreviation, count(ai.people_id) as 'note', ri.request_id, i.invention_id
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].AuthorInvention ai on i.invention_id = ai.invention_id
	                            join [SM_ScientificProduct].RequestInvention ri on i.invention_id = ri.invention_id
	                            join [SM_Request].BaseRequest br on ri.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on po.office_id = o.office_id
	                            join [SM_ScientificProduct].InventionType it on i.type_id = it.invention_type_id
                            where ri.status_id = 4
                            group by i.name, it.name, po.name, pro.mssv_msnv, o.office_abbreviation, ri.request_id, i.invention_id";
            List<WaitDecisionInven> list = db.Database.SqlQuery<WaitDecisionInven>(sql).ToList();
            return list;
        }

        public string deleteFileCM(string fileid)
        {
            try
            {
                GoogleDriveService.DeleteFile(fileid);
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public List<Invention_Appendix_1> getListAppendix1()
        {
            string sql = @"select ah.name as 'author_name', ah.mssv_msnv, o.office_abbreviation, it.name as 'type_name', i.no, i.name
                            from SM_ScientificProduct.Invention i join SM_ScientificProduct.AuthorInvention ai on i.invention_id = ai.invention_id
	                            join SM_ScientificProduct.Author ah on ah.people_id = ai.people_id
	                            join SM_ScientificProduct.InventionType it on i.type_id = it.invention_type_id
	                            join General.Office o on o.office_id = ah.office_id
	                            join SM_ScientificProduct.RequestInvention ri on ri.invention_id = i.invention_id
                            where ri.status_id = 4
                            order by ah.name";
            List<Invention_Appendix_1> list = db.Database.SqlQuery<Invention_Appendix_1>(sql).ToList();
            return list;
        }

        public List<Invention_Appendix_2> getListAppendix2()
        {
            string sql = @"select ah.name as 'author_name', ah.mssv_msnv, o.office_abbreviation, ai.money_reward
                            from SM_ScientificProduct.Invention i join SM_ScientificProduct.AuthorInvention ai on i.invention_id = ai.invention_id
	                            join SM_ScientificProduct.Author ah on ah.people_id = ai.people_id
	                            join SM_ScientificProduct.InventionType it on i.type_id = it.invention_type_id
	                            join General.Office o on o.office_id = ah.office_id
	                            join SM_ScientificProduct.RequestInvention ri on ri.invention_id = i.invention_id
                            where ri.status_id = 4
                            order by ah.name";
            List<Invention_Appendix_2> list = db.Database.SqlQuery<Invention_Appendix_2>(sql).ToList();
            return list;
        }

        public List<CustomCountry> getListCountryEdit(int id)
        {
            string sql = @"select i.invention_id, c.country_id, case when i.invention_id is null then cast(0 as bit) else cast(1 as bit) end as 'selected', c.country_name
                            from SM_ScientificProduct.Invention i join SM_ScientificProduct.InventionCountry ic on i.invention_id = ic.invention_id
	                            right join General.Country c on ic.country_id = c.country_id
                            where i.invention_id = @id or i.invention_id is null";
            List<CustomCountry> list = db.Database.SqlQuery<CustomCountry>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }
    }
}
