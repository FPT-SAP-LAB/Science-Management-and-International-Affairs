using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

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

        public RequestCitation GetRequestCitation(string id)
        {
            if (!int.TryParse(id, out int request_id) || request_id <= 0 || request_id == int.MaxValue)
                return null;
            db.Configuration.LazyLoadingEnabled = false;
            RequestCitation rc = db.RequestCitations.Find(request_id);
            return rc;
        }

        public string DeleteRequest(int request_id)
        {
            if (request_id <= 0 || request_id == int.MaxValue)
                return "ff";

            try
            {
                RequestCitation rp = db.RequestCitations.Find(request_id);
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

        public AlertModal<string> UpdateReward(string id, string total)
        {
            if (total == null)
                return new AlertModal<string>(false, "Số tiền không hợp lệ");
            try
            {
                int request_id = int.Parse(id);
                string temp = total.Replace(",", "");
                int.TryParse(temp, out int reward);
                if (reward <= 0)
                    return new AlertModal<string>(false, "Số tiền không hợp lệ");

                RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                rc.total_reward = reward;
                rc.citation_status_id = 4;
                db.SaveChanges();

                return new AlertModal<string>(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new AlertModal<string>(false);
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

        public AlertModal<string> UploadDecision(HttpPostedFileBase file, string number, string date)
        {
            string file_drive_id = null;
            DateTime finish = DateTime.Now;

            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    if (!DateTime.TryParseExact(date, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime valid_date))
                        return new AlertModal<string>(false, "Ngày có hiệu lực không hợp lệ");

                    string name = "QD_" + number + "_" + date;

                    List<int> acceptedStatusIds = new List<int>() { 4, 6, 7 };

                    var requests = (from a in db.RequestCitations
                                    join b in db.BaseRequests on a.request_id equals b.request_id
                                    join c in db.Accounts on b.account_id equals c.account_id
                                    where acceptedStatusIds.Contains(a.citation_status_id)
                                    select new
                                    {
                                        a.request_id,
                                        RequestCitation = a,
                                        BaseRequest = b,
                                        c.email
                                    }).ToList();

                    if (requests.Count == 0)
                        return new AlertModal<string>(false, "Chưa có đề nghị nào để cập nhật quyết định");

                    List<string> listE = requests.Select(x => x.email).Distinct().ToList();

                    Google.Apis.Drive.v3.Data.File f = GoogleDriveService.UploadDecisionFile(file, name, listE);
                    file_drive_id = f.Id;

                    Decision decision = new Decision
                    {
                        valid_date = valid_date,
                        File = new File
                        {
                            link = f.WebViewLink,
                            file_drive_id = f.Id,
                            name = name
                        },
                        decision_number = number
                    };
                    db.Decisions.Add(decision);
                    db.SaveChanges();

                    foreach (var item in requests)
                    {
                        db.RequestDecisions.Add(new RequestDecision
                        {
                            request_id = item.request_id,
                            decision_id = decision.decision_id
                        });
                        item.RequestCitation.citation_status_id = 2;
                        item.BaseRequest.finished_date = finish;
                    }

                    db.SaveChanges();
                    dbc.Commit();
                    return new AlertModal<string>(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    if (file_drive_id != null)
                        GoogleDriveService.DeleteFile(file_drive_id);
                    return new AlertModal<string>(false);
                }
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
