using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Invention;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
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

        public List<AuthorInfo> getAuthor(string id)
        {
            List<AuthorInfo> list = new List<AuthorInfo>();
            string sql = @"select po.*, tl.name as 'title_name', ct.name as 'contract_name', ai.money_reward, o.office_abbreviation, f.link, pro.bank_branch, pro.bank_number, pro.mssv_msnv, pro.tax_code, pro.identification_number, pro.office_id, pc.contract_id, t.title_id
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].AuthorInvention ai on i.invention_id = ai.invention_id
	                            join [General].People po on ai.people_id = po.people_id
	                            join [SM_Researcher].PeopleTitle pt on po.people_id = pt.people_id
	                            join [SM_MasterData].Title t on pt.title_id = t.title_id
	                            join [Localization].TitleLanguage tl on t.title_id = tl.title_id
	                            join [SM_Researcher].PeopleContract pc on po.people_id = pc.people_id
	                            join [SM_MasterData].ContractType ct on pc.contract_id = ct.contract_id
	                            join [General].Profile pro on pro.people_id = po.people_id
	                            join [General].Office o on pro.office_id = o.office_id
	                            join [General].[File] f on pro.identification_file_id = f.file_id
                            where i.invention_id = @id";
            list = db.Database.SqlQuery<AuthorInfo>(sql, new SqlParameter("id", id)).ToList();
            return list;
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
                InventionType ip = new InventionType
                {
                    name = name
                };
                db.InventionTypes.Add(ip);
                db.SaveChanges();
                return ip;
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
    }
}
