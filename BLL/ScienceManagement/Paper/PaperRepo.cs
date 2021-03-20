using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
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
            item = db.Database.SqlQuery<DetailPaper>(sql, new SqlParameter("id", id)).FirstOrDefault();
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

        public List<AuthorInfo> getAuthorPaper(string id)
        {
            List<AuthorInfo> list = new List<AuthorInfo>();
            string sql = @"select po.*, tl.name as 'title_name', ct.name as 'contract_name', ap.money_reward, o.office_abbreviation, f.link, pro.bank_branch, pro.bank_number, pro.mssv_msnv, pro.tax_code, pro.identification_number, pro.office_id, pc.contract_id, t.title_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [General].People po on ap.people_id = po.people_id
	                            join [SM_Researcher].PeopleTitle pt on po.people_id = pt.people_id
	                            join [SM_MasterData].Title t on pt.title_id = t.title_id
	                            join [Localization].TitleLanguage tl on t.title_id = tl.title_id
	                            join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            join [SM_MasterData].ContractType ct on pc.contract_id = ct.contract_id
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on pro.office_id = o.office_id
	                            join [General].[File] f on pro.identification_file_id = f.file_id
                            where p.paper_id = @id";
            list = db.Database.SqlQuery<AuthorInfo>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }

        public ENTITIES.Paper addPaper(ENTITIES.Paper item)
        {
            try
            {
                db.Papers.Add(item);
                db.SaveChanges();
                string sql = @"select p.*
                                from [SM_ScientificProduct].Paper p
                                where p.name = @name and p.publish_date = @date";
                ENTITIES.Paper p = db.Database.SqlQuery<ENTITIES.Paper>(sql, new SqlParameter("name", item.name), new SqlParameter("date", item.publish_date)).FirstOrDefault();
                return p;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public BaseRequest addBaseRequest(string account_id)
        {
            BaseRequest b = new BaseRequest
            {
                account_id = 10,
                created_date = DateTime.Today
            };
            db.BaseRequests.Add(b);
            db.SaveChanges();
            string sql = @"select b.*
                            from [SM_Request].BaseRequest b 
                            where b.account_id = @id
                            order by b.created_date desc";
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
            catch
            {
                return "ff";
            }
        }

        public string addAuthor(List<AddAuthor> list, string paper_id)
        {
            try
            {
                List<string> listMail = db.Database.SqlQuery<string>("select email from [General].People").ToList();
                string listmail = "";
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
                            Profile pro = (from a in db.Profiles join b in db.People on a.people_id equals b.people_id
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
                List<AuthorInfo> listAuthor = db.Database.SqlQuery<AuthorInfo>(sql, listParam.ToArray()).ToList();
                foreach (var item in listAuthor)
                {
                    AuthorPaper ap = new AuthorPaper
                    {
                        people_id = item.people_id,
                        paper_id = Int32.Parse(paper_id),
                        current_mssv_msnv = item.mssv_msnv
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
    }
}
