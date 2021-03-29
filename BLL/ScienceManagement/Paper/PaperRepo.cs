using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            item = db.Database.SqlQuery<DetailPaper>(sql, new SqlParameter("id", Int32.Parse(id))).FirstOrDefault();
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

        public List<AuthorInfoWithNull> getAuthorPaper(string id)
        {
            List<AuthorInfoWithNull> list = new List<AuthorInfoWithNull>();
            string sql = @"select po.*, tl.name as 'title_name', ct.name as 'contract_name', ap.money_reward, o.office_abbreviation, f.link, pro.bank_branch, pro.bank_number, pro.mssv_msnv, pro.tax_code, pro.identification_number, pro.office_id as 'office_id_string', pc.contract_id, t.title_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [General].People po on ap.people_id = po.people_id
	                            left join [SM_Researcher].PeopleTitle pt on po.people_id = pt.people_id
	                            left join [SM_MasterData].Title t on pt.title_id = t.title_id
	                            left join [Localization].TitleLanguage tl on t.title_id = tl.title_id
	                            left join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            left join [SM_MasterData].ContractType ct on pc.contract_id = ct.contract_id
	                            left join [General].Profile pro on po.people_id = pro.people_id
	                            left join [General].Office o on pro.office_id = o.office_id
	                            left join [General].[File] f on pro.identification_file_id = f.file_id
                            where p.paper_id = @id";
            list = db.Database.SqlQuery<AuthorInfoWithNull>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }

        public ENTITIES.Paper addPaper(DetailPaper item)
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
                    paper_type_id = item.paper_type_id
                };
                db.Papers.Add(paper);
                db.SaveChanges();
                string sql = @"select p.*
                                from [SM_ScientificProduct].Paper p
                                where p.name = @name and p.publish_date = @date
                                order by p.paper_id desc";
                ENTITIES.Paper p = db.Database.SqlQuery<ENTITIES.Paper>(sql, new SqlParameter("name", item.name), new SqlParameter("date", item.publish_date)).FirstOrDefault();
                return p;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public BaseRequest addBaseRequest(int account_id)
        {
            BaseRequest b = new BaseRequest
            {
                account_id = account_id,
                created_date = DateTime.Today
            };
            db.BaseRequests.Add(b);
            db.SaveChanges();
            string sql = @"select b.*
                            from [SM_Request].BaseRequest b 
                            where b.account_id = @id
                            order by b.request_id desc";
            BaseRequest ba = db.Database.SqlQuery<BaseRequest>(sql, new SqlParameter("id", account_id)).FirstOrDefault();
            return ba;
        }

        public string addRequestPaper(int request_id, RequestPaper r)
        {
            try
            {
                r.request_id = request_id;
                r.status_id = 3;
                db.RequestPapers.Add(r);
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string addAuthor(List<AddAuthor> list, string paper_id)
        {
            try
            {
                List<string> listMail = db.Database.SqlQuery<string>("select email from [General].People").ToList();
                string listmail = "";
                string tempSql = "";
                List<SqlParameter> listParam1 = new List<SqlParameter>();
                int count = 1;
                foreach (var item in list)
                {
                    if (!listMail.Contains(item.email))
                    {
                        int peopleid = addPeople(item.name, item.email);
                        if (item.office_abbreviation != "Khác")
                        {
                            item.people_id = peopleid;
                            addProfile(item);
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

                            tempSql += " update [SM_Researcher].PeopleTitle set title_id = @title" + count + " where people_id = @people" + count;
                            SqlParameter tempParam2 = new SqlParameter("@title" + count, item.contract_id);
                            listParam1.Add(tempParam2);

                            SqlParameter tempParam3 = new SqlParameter("@people" + count, item.people_id);
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
                    AuthorPaper ap = new AuthorPaper
                    {
                        people_id = item.people_id,
                        paper_id = Int32.Parse(paper_id),
                        current_mssv_msnv = item.mssv_msnv,
                        money_reward = item.money_reward
                    };
                    db.AuthorPapers.Add(ap);
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

        public string uploadDecision(DateTime date_format1, int file_id1, string number1, string file_drive_id1,
                                     DateTime date_format2, int file_id2, string number2, string file_drive_id2)
        {
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

                Decision decision2 = new Decision
                {
                    valid_date = date_format2,
                    file_id = file_id2,
                    decision_number = number2
                };
                db.Decisions.Add(decision2);
                db.SaveChanges();

                List<WaitDecisionPaper> listTN = getListWwaitDecision("Trongnuoc");
                List<WaitDecisionPaper> listQT = getListWwaitDecision("Quocte");
                foreach (var item in listTN)
                {
                    RequestDecision request = new RequestDecision
                    {
                        request_id = item.request_id,
                        decision_id = decision.decision_id
                    };
                    db.RequestDecisions.Add(request);

                    RequestDecision request2 = new RequestDecision
                    {
                        request_id = item.request_id,
                        decision_id = decision2.decision_id
                    };
                    db.RequestDecisions.Add(request2);

                    RequestPaper rc = db.RequestPapers.Where(x => x.request_id == item.request_id).FirstOrDefault();
                    rc.status_id = 2;
                }
                foreach (var item in listQT)
                {
                    RequestDecision request = new RequestDecision
                    {
                        request_id = item.request_id,
                        decision_id = decision.decision_id
                    };
                    db.RequestDecisions.Add(request);

                    RequestDecision request2 = new RequestDecision
                    {
                        request_id = item.request_id,
                        decision_id = decision2.decision_id
                    };
                    db.RequestDecisions.Add(request2);

                    RequestPaper rc = db.RequestPapers.Where(x => x.request_id == item.request_id).FirstOrDefault();
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
                GoogleDriveService.DeleteFile(file_drive_id1);
                GoogleDriveService.DeleteFile(file_drive_id2);
                return "ff";
            }
        }

        public int addPeople(string name, string mail)
        {
            Person p = new Person
            {
                name = name,
                email = mail
            };
            db.People.Add(p);
            db.SaveChanges();
            string sql = @"select people_id
                            from [General].People
                            where email = @mail";
            int result = db.Database.SqlQuery<int>(sql, new SqlParameter("mail", mail)).FirstOrDefault();
            return result;
        }

        public void addProfile(AddAuthor a)
        {
            Profile pro = new Profile
            {
                people_id = a.people_id,
                bank_number = a.bank_number,
                bank_branch = a.bank_branch,
                tax_code = a.tax_code,
                identification_number = a.identification_number,
                office_id = a.office_id,
                mssv_msnv = a.mssv_msnv
            };
            db.Profiles.Add(pro);
            db.SaveChanges();
        }

        public string addCriteria(List<CustomCriteria> criteria, string paper_id)
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
                foreach (var item in list)
                {
                    foreach (var cri in criteria)
                    {
                        if (cri.name == item.name)
                        {
                            item.link = cri.link;
                        }
                    }
                    PaperWithCriteria pwc = new PaperWithCriteria
                    {
                        paper_id = Int32.Parse(paper_id),
                        criteria_id = item.criteria_id,
                        link = item.link
                    };
                    db.PaperWithCriterias.Add(pwc);
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

        public string updatePaper(string paper_id, ENTITIES.Paper paper)
        {
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
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string updateRequest(RequestPaper item)
        {
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

                if (item.type == "Trongnuoc")
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM [SM_ScientificProduct].RequestPaper where paper_id = @id", new SqlParameter("id", item.paper_id));
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

        public string updateCriteria(List<CustomCriteria> criteria, string paper_id)
        {
            try
            {
                db.Database.ExecuteSqlCommand("delete from [SM_ScientificProduct].PaperWithCriteria where paper_id = @id", new SqlParameter("id", paper_id));
                string mess = addCriteria(criteria, paper_id);
                return mess;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string updateAuthor(List<AddAuthor> people, string paper_id)
        {
            try
            {
                db.Database.ExecuteSqlCommand("delete from[SM_ScientificProduct].AuthorPaper where paper_id = @id", new SqlParameter("id", paper_id));
                string mess = addAuthor(people, paper_id);
                return mess;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public List<PendingPaper_Manager> listPending()
        {
            List<PendingPaper_Manager> list = new List<PendingPaper_Manager>();
            string sql = @"select p.name, a.email, br.created_date, p.paper_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join [SM_Request].BaseRequest br on rp.request_id = br.request_id
	                            join [General].Account a on br.account_id = a.account_id
                            where rp.status_id = 3";
            list = db.Database.SqlQuery<PendingPaper_Manager>(sql).ToList();
            return list;
        }

        public string updateRewardPaper(DetailPaper paper)
        {
            try
            {
                RequestPaper p = db.RequestPapers.Where(x => x.request_id == paper.request_id).FirstOrDefault();
                p.total_reward = paper.total_reward;
                p.status_id = 4;
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string updateAuthorReward(DetailPaper paper, List<AuthorInfoWithNull> people)
        {
            try
            {
                foreach (var item in people)
                {
                    AuthorPaper ap = db.AuthorPapers
                                        .Where(x => x.paper_id == paper.paper_id)
                                        .Where(x => x.people_id == item.people_id)
                                        .FirstOrDefault();
                    ap.money_reward = item.money_reward;
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

        public List<WaitDecisionPaper> getListWwaitDecision(string type)
        {
            string sql = @"select p.name, p.company, po.name as 'author_name', pro.mssv_msnv, o.office_abbreviation, count(ap.people_id) as 'note', rp.request_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join [SM_Request].BaseRequest br on rp.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on o.office_id = pro.office_id
                            where rp.status_id = 4 and rp.type = @type
                            group by p.name, p.company, po.name, pro.mssv_msnv, o.office_abbreviation, rp.request_id";
            List<WaitDecisionPaper> list = db.Database.SqlQuery<WaitDecisionPaper>(sql, new SqlParameter("type", type)).ToList();
            return list;
        }

        public List<Paper_Appendix_1> getListAppendix1_2(string type)
        {
            string sql = @"select po.name as 'author_name', pro.mssv_msnv, o.office_abbreviation, p.name, p.company, a.sum, b.sumFE, p.paper_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [General].People po on ap.people_id = po.people_id
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on pro.office_id = o.office_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join (select p.paper_id, count(ap.people_id) as 'sum'
			                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
			                            group by p.paper_id) as a on p.paper_id = a.paper_id
	                            join (select p.paper_id, count(ap.people_id) as 'sumFE'
			                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
				                            join [General].Profile pro on ap.people_id = pro.people_id
			                            group by p.paper_id) as b on p.paper_id = b.paper_id
                            where rp.status_id = 4 and rp.type = @type
                            order by po.name";
            List<Paper_Appendix_1> list = db.Database.SqlQuery<Paper_Appendix_1>(sql, new SqlParameter("type", type)).ToList();
            return list;
        }

        public List<Paper_Apendix_3> getListAppendix3_4(string type)
        {
            string sql = @"select po.name, pro.mssv_msnv, o.office_abbreviation, sum(ap.money_reward) as 'sum_money'
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [General].People po on ap.people_id = po.people_id
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on pro.office_id = o.office_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
                            where rp.status_id = 4 and rp.type = @type
                            group by po.name, pro.mssv_msnv, o.office_abbreviation
                            order by po.name";
            List<Paper_Apendix_3> list = db.Database.SqlQuery<Paper_Apendix_3>(sql, new SqlParameter("type", type)).ToList();
            return list;
        }
    }
}
