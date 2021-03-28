using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Citation
{
    public class CitationRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListOnePerson_Citation> GetList(int id)
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
            string sql = @"select po.*, o.office_abbreviation, ct.contract_id, t.title_id, rc.total_reward, pro.bank_branch, pro.bank_number, pro.mssv_msnv, pro.tax_code, pro.identification_number, ct.name as 'contract_name'
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

        public AuthorInfo addAuthor(List<AddAuthor> list)
        {
            try
            {
                PaperRepo pr = new PaperRepo();
                List<string> listMail = db.Database.SqlQuery<string>("select email from [General].People").ToList();
                string listmail = "";
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
                        }
                    }
                    listmail += "," + item.email;
                }
                db.SaveChanges();
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
                AuthorInfo Author = db.Database.SqlQuery<AuthorInfo>(sql, listParam.ToArray()).FirstOrDefault();
                return Author;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string addCitationRequest(BaseRequest br, AuthorInfo author)
        {
            try
            {
                RequestCitation rc = new RequestCitation
                {
                    request_id = br.request_id,
                    status_id = 3,
                    people_id = author.people_id,
                    current_mssv_msnv = author.mssv_msnv
                };
                db.RequestCitations.Add(rc);
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string addCitaion(List<ENTITIES.Citation> citation)
        {
            try
            {
                foreach (var item in citation)
                {
                    db.Citations.Add(item);
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

        public string addRequestHasCitation(List<ENTITIES.Citation> citation, BaseRequest br)
        {
            try
            {
                string sql = "";
                List<SqlParameter> listParam = new List<SqlParameter>();
                for (int i = 0; i < citation.Count; i++)
                {
                    sql += "insert into [SM_Citation].RequestHasCitation values (@request, @citation" + i + ") \n";
                    SqlParameter param = new SqlParameter("@citation" + i, citation[i].citation_id);
                    listParam.Add(param);
                }
                SqlParameter param2 = new SqlParameter("@request", br.request_id);
                listParam.Add(param2);
                db.Database.ExecuteSqlCommand(sql, listParam.ToArray());
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string editCitation(List<ENTITIES.Citation> citation, List<ENTITIES.Citation> newcitation, string request_id)
        {
            try
            {
                string sql = "";
                List<SqlParameter> listParam = new List<SqlParameter>();
                int id = Int32.Parse(request_id);
                BaseRequest br = db.BaseRequests.Where(x => x.request_id == id).FirstOrDefault();
                for (int i = 0; i < citation.Count; i++)
                {
                    sql += "delete from [SM_Citation].RequestHasCitation where citation_id = @citation" + i + " and request_id = @request \n";
                    SqlParameter param = new SqlParameter("@citation" + i, citation[i].citation_id);
                    listParam.Add(param);
                }
                SqlParameter param2 = new SqlParameter("@request", br.request_id);
                listParam.Add(param2);
                db.Database.ExecuteSqlCommand(sql, listParam.ToArray());
                foreach (var item in citation)
                {
                    db.Citations.Remove(db.Citations.Single(s => s.citation_id == item.citation_id));
                }
                db.SaveChanges();
                addCitaion(newcitation);
                addRequestHasCitation(newcitation, br);
                RequestCitation rc = db.RequestCitations.Where(x => x.request_id == br.request_id).FirstOrDefault();
                rc.status_id = 3;
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public List<PendingCitation_manager> getListPending()
        {
            string sql = @"select acc.email, br.created_date, br.request_id
                            from [SM_Citation].RequestCitation rc join [SM_Request].BaseRequest br on rc.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
                            where rc.status_id = 3";
            List<PendingCitation_manager> list = db.Database.SqlQuery<PendingCitation_manager>(sql).ToList();
            return list;
        }

        public Nullable<int> getTotalReward(string id)
        {
            int request_id = Int32.Parse(id);
            RequestCitation item = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
            return item.total_reward;
        }

        public string updateReward(string id, string total)
        {
            DbContextTransaction dbc = db.Database.BeginTransaction();
            try
            {
                int request_id = Int32.Parse(id);
                string temp = total.Replace(",", "");
                int reward = Int32.Parse(temp);
                RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                rc.total_reward = reward;
                rc.status_id = 4;
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

        public List<WaitDecisionCitation> getListWait()
        {
            string sql = @"select po.name, o.office_abbreviation, pro.mssv_msnv, rc.total_reward, SUM(c.COUNT) as 'sum'
                            from [SM_Citation].RequestCitation rc join [SM_Request].BaseRequest br on rc.request_id = br.request_id
	                            join [General].Account acc on acc.account_id = br.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on pro.office_id = o.office_id
	                            join [SM_Citation].RequestHasCitation rhc on rc.request_id = rhc.request_id
	                            join [SM_Citation].Citation c on rhc.citation_id = c.citation_id
                            where rc.status_id = 4
                            group by po.name, o.office_abbreviation, pro.mssv_msnv, rc.total_reward";
            List<WaitDecisionCitation> list = db.Database.SqlQuery<WaitDecisionCitation>(sql).ToList();
            return list;
        }
    }
}
