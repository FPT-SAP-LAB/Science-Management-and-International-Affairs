using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
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
            string sql = @"select po.*, o.office_abbreviation, ct.contract_id, t.title_id, rc.total_reward, pro.bank_branch, pro.bank_number, pro.mssv_msnv, pro.tax_code, pro.identification_number
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

        public string addCitaion(List<ENTITIES.Citation> citation) { 
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
    }
}
