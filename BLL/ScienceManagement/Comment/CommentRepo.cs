using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.ScienceManagement.Comment
{
    public class CommentRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<DetailComment> GetComment(int request_id)
        {
            if (request_id <= 0 || request_id == int.MaxValue)
                return new List<DetailComment>();
            List<DetailComment> list = (from a in db.CommentBases
                                        join b in db.Accounts on a.account_id equals b.account_id
                                        where a.BaseRequest.request_id == request_id
                                        orderby a.date descending
                                        select new DetailComment
                                        {
                                            Content = a.content,
                                            Date = a.date,
                                            Email = b.email
                                        }).ToList();
            return list;
        }
        public AlertModal<string> AddComment(int request_id, int account_id, string content, int role_id, bool is_manager, string path)
        {
            if (request_id <= 0 || request_id == int.MaxValue)
                return new AlertModal<string>(false, "Đề nghị không tồn tại");
            if (account_id <= 0 || account_id == int.MaxValue)
                return new AlertModal<string>(false, "Tài khoản không tồn tại");
            List<int> manager_account_id = new List<int> { 2, 3 };
            if (string.IsNullOrWhiteSpace(content))
                return new AlertModal<string>(false, "Nội dung không được bỏ trống");
            BaseRequest request = db.BaseRequests.Find(request_id);
            if (request == null)
                return new AlertModal<string>(false, "Đề nghị không tồn tại");
            if (request.account_id != account_id && !(manager_account_id.Contains(role_id)))
                return new AlertModal<string>(false, "Bạn không có quyền bình luận vào đề nghị này");
            if (request.finished_date != null)
                return new AlertModal<string>(false, "Đề nghị đã kết thúc");

            NotificationRepo notificationRepo = new NotificationRepo(db);
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    request.CommentBases.Add(new CommentBase()
                    {
                        account_id = account_id,
                        content = content.Trim(),
                        date = DateTime.Now
                    });
                    string notification_id = null;
                    if (path.Contains("ConferenceSponsor"))
                    {
                        if (is_manager)
                            notification_id = notificationRepo.AddByAccountID(request.account_id, 1, "/ConferenceSponsor/Detail?id=" + request.request_id, false).ToString();
                        else
                            notification_id = JsonConvert.SerializeObject(notificationRepo.AddByRightID(new List<int> { 22, 34 }, 1, "/ConferenceSponsor/Detail?id=" + request.request_id, true));
                    }
                    else if (path.Contains("Paper"))
                    {
                        int id = GetPaperID(request.request_id);
                        if (is_manager)
                            notification_id = notificationRepo.AddByAccountID(request.account_id, 1, "/Paper/Edit?id=" + id, false).ToString();
                        else
                            notification_id = JsonConvert.SerializeObject(notificationRepo.AddByRightID(new List<int> { 16, 17 }, 1, "/Paper/Detail?id=" + id, true));
                    }
                    else if (path.Contains("Invention"))
                    {
                        int id = GetInvenID(request.request_id);
                        if (is_manager)
                            notification_id = notificationRepo.AddByAccountID(request.account_id, 1, "/Invention/Edit?id=" + id, false).ToString();
                        else
                            notification_id = JsonConvert.SerializeObject(notificationRepo.AddByRightID(new List<int> { 16, 17 }, 1, "/Invention/Detail?id=" + id, true));
                    }
                    else if (path.Contains("Citation"))
                    {
                        if (is_manager)
                            notification_id = notificationRepo.AddByAccountID(request.account_id, 1, "/Citation/Edit?id=" + request.request_id, false).ToString();
                        else
                            notification_id = JsonConvert.SerializeObject(notificationRepo.AddByRightID(new List<int> { 16, 17 }, 1, "/Citation/Detail?id=" + request.request_id, true));
                    }
                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(notification_id, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                }
            }
            return new AlertModal<string>(false);
        }

        public int GetPaperID(int requestID)
        {
            RequestPaper rp = db.RequestPapers.Where(x => x.request_id == requestID).FirstOrDefault();
            return rp.paper_id;
        }

        public int GetInvenID(int requestID)
        {
            RequestInvention rp = db.RequestInventions.Where(x => x.request_id == requestID).FirstOrDefault();
            return rp.invention_id;
        }
    }
}
