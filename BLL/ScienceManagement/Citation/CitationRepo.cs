using BLL.ModelDAL;
using BLL.Support;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.ScienceManagement.Citation
{
    public class CitationRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListOnePerson_Citation> GetList(int account_id)
        {
            if (account_id <= 0 || account_id == int.MaxValue)
                return null;

            List<ListOnePerson_Citation> list = (from a in db.RequestCitations
                                                 join b in db.BaseRequests on a.request_id equals b.request_id
                                                 where b.account_id == account_id
                                                 select new ListOnePerson_Citation
                                                 {
                                                     count = a.Citations.Select(x => x.count).Sum(),
                                                     created_date = b.created_date.Value,
                                                     note = a.citation_status_id + "_" + a.request_id,
                                                     request_id = a.request_id,
                                                     TypeNames = (from c in db.CitationTypes
                                                                  join d in db.Citations on c.citation_type_id equals d.citation_type_id
                                                                  where d.request_id == a.request_id
                                                                  select c.citation_type_name).Distinct().ToList(),
                                                     status_id = a.citation_status_id
                                                 }).ToList();
            list.ForEach(x => x.source = string.Join(", ", x.TypeNames));
            return list;
        }

        public int GetStatus(string id)
        {
            if (!int.TryParse(id, out int request_id) || request_id <= 0 || request_id == int.MaxValue)
                return 0;
            try
            {
                RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                return rc.citation_status_id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public AuthorInfo GetAuthor(string id)
        {
            if (!int.TryParse(id, out int request_id) || request_id <= 0 || request_id == int.MaxValue)
                return null;

            string sql = @"select ah.name, ah.email, o.office_abbreviation, ah.contract_id, ah.title_id, rc.total_reward, ah.bank_branch, ah.bank_number, ah.mssv_msnv, ah.tax_code, ah.identification_number, ct.name as 'contract_name', case when ah.is_reseacher is null then cast(0 as bit) else ah.is_reseacher end as 'is_reseacher', ah.identification_file_link, ah.people_id
                            from [SM_Citation].Citation c
	                            join [SM_Citation].RequestCitation rc on c.request_id = rc.request_id
	                            join [SM_ScientificProduct].Author ah on rc.people_id = ah.people_id
	                            join [General].Office o on o.office_id = ah.office_id
	                            join [SM_MasterData].ContractType ct on ah.contract_id = ct.contract_id
                            where rc.request_id = @id";
            AuthorInfo item = db.Database.SqlQuery<AuthorInfo>(sql, new SqlParameter("id", request_id)).FirstOrDefault();
            return item;
        }

        public List<string> GetAuthorEmail()
        {
            string sql = @"select distinct ah.email
                            from SM_Citation.RequestCitation rc join SM_ScientificProduct.Author ah on rc.people_id = ah.people_id
                            where rc.citation_status_id in (4, 6, 7)";
            List<string> list = db.Database.SqlQuery<string>(sql).ToList();
            return list;
        }

        public RequestCitation GetRequestCitation(string id)
        {
            if (!int.TryParse(id, out int request_id) || request_id <= 0 || request_id == int.MaxValue)
                return null;

            RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
            return rc;
        }

        public string DeleteRequest(int request_id)
        {
            if (request_id <= 0 || request_id == int.MaxValue)
                return "ff";

            try
            {
                RequestCitation rp = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                rp.citation_status_id = 1;
                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public string ChangeStatus(string id)
        {
            if (!int.TryParse(id, out int request_id) || request_id <= 0 || request_id == int.MaxValue)
                return "ff";
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                    rc.citation_status_id = 5;

                    Account account = rc.BaseRequest.Account;
                    NotificationRepo nr = new NotificationRepo(db);
                    int notification_id = nr.AddByAccountID(account.account_id, 4, "/Citation/Edit?id=" + request_id, false);

                    db.SaveChanges();
                    dbc.Commit();
                    return notification_id.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return "ff";
                }
            }
        }

        public List<CustomCitation> GetCitation(string id)
        {
            if (!int.TryParse(id, out int request_id) || request_id <= 0 || request_id == int.MaxValue)
                return null;

            string sql = @"select c.*, ct.citation_type_name as 'source'
                            from SM_Citation.Citation c join SM_Citation.CitationType ct on c.citation_type_id = ct.citation_type_id
                            where c.request_id = @id";
            List<CustomCitation> list = db.Database.SqlQuery<CustomCitation>(sql, new SqlParameter("id", id)).ToList();

            return list;
        }

        public List<PendingCitation_manager> GetListPending()
        {
            string sql = @"select acc.email, br.created_date, br.request_id, rc.citation_status_id
                           from [SM_Citation].RequestCitation rc join [SM_Request].BaseRequest br on rc.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
                           where rc.citation_status_id = 3 or  rc.citation_status_id = 5 or rc.citation_status_id = 8";
            List<PendingCitation_manager> list = db.Database.SqlQuery<PendingCitation_manager>(sql).ToList();
            return list;
        }

        public string UpdateReward(string id, string total)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    int request_id = int.Parse(id);
                    string temp = total.Replace(",", "");
                    int reward = int.Parse(temp);
                    RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                    rc.total_reward = reward;
                    rc.citation_status_id = 4;
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

        public List<WaitDecisionCitation> GetListWait()
        {
            string sql = @"select po.name, o.office_abbreviation, pro.mssv_msnv, rc.total_reward, SUM(c.COUNT) as 'sum', rc.request_id
                            from [SM_Citation].RequestCitation rc join [SM_Request].BaseRequest br on rc.request_id = br.request_id
	                            join [General].Account acc on acc.account_id = br.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on po.office_id = o.office_id
	                            join [SM_Citation].Citation c on rc.request_id = c.request_id
                            where rc.citation_status_id = 4
                            group by po.name, o.office_abbreviation, pro.mssv_msnv, rc.total_reward, rc.request_id";
            List<WaitDecisionCitation> list = db.Database.SqlQuery<WaitDecisionCitation>(sql).ToList();
            return list;
        }

        public string UploadDecision(DateTime date, int file_id, string number, string file_drive_id)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    Decision decision = new Decision
                    {
                        valid_date = date,
                        file_id = file_id,
                        decision_number = number
                    };
                    db.Decisions.Add(decision);
                    db.SaveChanges();

                    List<WaitDecisionCitation> wait = GetListWait();
                    foreach (var item in wait)
                    {
                        RequestDecision request = new RequestDecision
                        {
                            request_id = item.request_id,
                            decision_id = decision.decision_id
                        };
                        db.RequestDecisions.Add(request);
                        RequestCitation rc = db.RequestCitations.Where(x => x.request_id == item.request_id).FirstOrDefault();
                        rc.citation_status_id = 2;
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

        public List<Citation_Appendix_1> GetListAppendix1()
        {
            string sql = @"select ah.name, ah.mssv_msnv, o.office_abbreviation, a.sum_scopus, b.sum_scholar
                            from SM_ScientificProduct.Author ah
	                            join(select ah.people_id, sum(c.count) as 'sum_scopus'
	                            from SM_Citation.Citation c 
		                            join SM_Citation.RequestCitation rc on c.request_id = rc.request_id
		                            join SM_ScientificProduct.Author ah on rc.people_id = ah.people_id
	                            where rc.citation_status_id = 4 and c.citation_type_id = 2
	                            group by ah.people_id) as a on ah.people_id = a.people_id
	                            join(select ah.people_id, sum(c.count) as 'sum_scholar'
	                            from SM_Citation.Citation c 
		                            join SM_Citation.RequestCitation rc on c.request_id = rc.request_id
		                            join SM_ScientificProduct.Author ah on rc.people_id = ah.people_id
	                            where (rc.citation_status_id = 4) and (c.citation_type_id = 1)
	                            group by ah.people_id) as b on ah.people_id = b.people_id
	                            join General.Office o on ah.office_id = o.office_id
                            order by ah.name";
            List<Citation_Appendix_1> list = db.Database.SqlQuery<Citation_Appendix_1>(sql).ToList();
            return list;
        }

        public List<Citation_Appendix_2> GetListAppendix2()
        {
            string sql = @"select ah.name, ah.mssv_msnv, o.office_abbreviation, rc.total_reward
                            from SM_Citation.RequestCitation rc join SM_ScientificProduct.Author ah on rc.people_id = ah.people_id
	                            join General.Office o on o.office_id = ah.office_id
                            where rc.citation_status_id = 4";
            List<Citation_Appendix_2> list = db.Database.SqlQuery<Citation_Appendix_2>(sql).ToList();
            return list;
        }
    }
}
