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
            string sql = @"select i.*, it.name as 'type_name', ri.reward_type, ri.total_reward, ri.request_id, f.link as 'link_file'
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

        public List<AuthorInfoWithNull> getAuthor(string id)
        {
            List<AuthorInfoWithNull> list = new List<AuthorInfoWithNull>();
            string sql = @"select po.*, tl.name as 'title_name', ct.name as 'contract_name', ai.money_reward, o.office_abbreviation, f.link, pro.bank_branch, pro.bank_number, pro.mssv_msnv, pro.tax_code, pro.identification_number, pro.office_id as 'office_id_string', pc.contract_id, t.title_id, pro.is_reseacher
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].AuthorInvention ai on i.invention_id = ai.invention_id
	                            join [General].People po on ai.people_id = po.people_id
	                            left join [SM_Researcher].PeopleTitle pt on po.people_id = pt.people_id
	                            left join [SM_MasterData].Title t on pt.title_id = t.title_id
	                            left join [Localization].TitleLanguage tl on t.title_id = tl.title_id
	                            left join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            left join [SM_MasterData].ContractType ct on pc.contract_id = ct.contract_id
	                            left join [General].Profile pro on pro.people_id = po.people_id
	                            left join [General].Office o on pro.office_id = o.office_id
	                            left join [General].[File] f on pro.identification_file_id = f.file_id
                            where i.invention_id = @id";
            list = db.Database.SqlQuery<AuthorInfoWithNull>(sql, new SqlParameter("id", id)).ToList();
            return list;
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
                dbc.Rollback();
                return "ff";
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
            try
            {
                PaperRepo pr = new PaperRepo();
                List<string> listMail = db.Database.SqlQuery<string>("select email from [General].People").ToList();
                string listmail = "";
                string tempSql = "";
                List<SqlParameter> listParam1 = new List<SqlParameter>();
                int count = 1;
                foreach (var item in list)
                {
                    if (!listMail.Contains(item.email))
                    {
                        int peopleid = pr.addPeople(item.name, item.email);
                        if (item.office_abbreviation != "Khác")
                        {
                            item.people_id = peopleid;
                            pr.addProfile(item);
                        }
                    }
                    else
                    {
                        Person p = db.People.Where(x => x.email == item.email).FirstOrDefault();
                        p.name = item.name;
                        p.phone_number = item.phone_number;
                        if (item.office_abbreviation != "Khác")
                        {
                            Profile pro = (from a in db.Profiles
                                           join b in db.People on a.people_id equals b.people_id
                                           where b.email == item.email
                                           select a).FirstOrDefault();
                            pro.bank_branch = item.bank_branch;
                            pro.bank_number = item.bank_number;
                            pro.tax_code = item.tax_code;
                            pro.identification_number = item.identification_number;
                            pro.office_id = item.office_id;
                            pro.mssv_msnv = item.mssv_msnv;

                            tempSql += " update [SM_Researcher].PeopleContract set contract_id = @contract" + count + " where people_id = @people" + count;
                            SqlParameter tempParam1 = new SqlParameter("@contract" + count, item.contract_id);
                            listParam1.Add(tempParam1);

                            tempSql += " delete from [SM_Researcher].PeopleTitle where people_id = @people" + count + " insert into [SM_Researcher].PeopleTitle values (@people" + count + ", @title" + count + ")";
                            SqlParameter tempParam2 = new SqlParameter("@title" + count, item.title_id);
                            listParam1.Add(tempParam2);

                            SqlParameter tempParam3 = new SqlParameter("@people" + count, pro.people_id);
                            listParam1.Add(tempParam3);
                        }
                    }
                    listmail += "," + item.email;
                    count++;
                }
                db.SaveChanges();
                db.Database.ExecuteSqlCommand(tempSql, listParam1.ToArray());
                listmail = listmail.Substring(1);
                string[] mail = listmail.Split(',');
                String strAppend = "";
                List<SqlParameter> listParam = new List<SqlParameter>();
                for (int i = 0; i < mail.Length; i++)
                {
                    SqlParameter param = new SqlParameter("@idParam" + i, mail[i]);
                    listParam.Add(param);
                    string paramName = "@idParam" + i;
                    strAppend += paramName + ",";
                }
                strAppend = strAppend.ToString().Remove(strAppend.LastIndexOf(","), 1);
                string sql = @"select po.people_id, pro.mssv_msnv
                            from [General].People po left outer join [General].Profile pro on po.people_id = pro.people_id
                            where po.email in (" + strAppend + ")";
                List<AuthorInfo> listAuthor = db.Database.SqlQuery<AuthorInfo>(sql, listParam.ToArray()).ToList();
                foreach (var item in listAuthor)
                {
                    AuthorInvention ap = new AuthorInvention
                    {
                        people_id = item.people_id,
                        invention_id = invention_id,
                        current_mssv_msnv = item.mssv_msnv,
                        money_reward = item.money_reward
                    };
                    db.AuthorInventions.Add(ap);
                }
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string addInvenRequest(BaseRequest br, ENTITIES.Invention inven, string type)
        {
            try
            {
                RequestInvention ri = new RequestInvention
                {
                    request_id = br.request_id,
                    status_id = 3,
                    invention_id = inven.invention_id,
                    reward_type = type
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
                i.country_id = inven.country_id;
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
            try
            {
                string sql = @"delete from [SM_ScientificProduct].AuthorInvention where invention_id = @id";
                db.Database.ExecuteSqlCommand(sql, new SqlParameter("id", id));
                addAuthor(people, id);
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public List<PendingInvention_Manager> getListPending()
        {
            string sql = @"select i.name, acc.email, br.created_date, i.invention_id
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].RequestInvention ri on i.invention_id = ri.invention_id
	                            join [SM_Request].BaseRequest br on ri.request_id = br.request_id
	                            join [General].Account acc on acc.account_id = br.account_id
                            where ri.status_id = 3";
            List<PendingInvention_Manager> list = db.Database.SqlQuery<PendingInvention_Manager>(sql).ToList();
            return list;
        }

        public string updateRewardInven(DetailInvention item)
        {
            try
            {
                RequestInvention ri = db.RequestInventions.Where(x => x.request_id == item.request_id).FirstOrDefault();
                ri.total_reward = item.total_reward;
                ri.status_id = 4;
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string updateAuthorReward(DetailInvention inven, List<AuthorInfoWithNull> people)
        {
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
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public List<WaitDecisionInven> getListWaitDecision()
        {
            string sql = @"select i.name, it.name as 'type_name', po.name as 'author_name', pro.mssv_msnv, o.office_abbreviation, count(ai.people_id) as 'note', ri.request_id
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].AuthorInvention ai on i.invention_id = ai.invention_id
	                            join [SM_ScientificProduct].RequestInvention ri on i.invention_id = ri.invention_id
	                            join [SM_Request].BaseRequest br on ri.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on pro.office_id = o.office_id
	                            join [SM_ScientificProduct].InventionType it on i.type_id = it.invention_type_id
                            where ri.status_id = 4
                            group by i.name, it.name, po.name, pro.mssv_msnv, o.office_abbreviation, ri.request_id";
            List<WaitDecisionInven> list = db.Database.SqlQuery<WaitDecisionInven>(sql).ToList();
            return list;
        }
    }
}
