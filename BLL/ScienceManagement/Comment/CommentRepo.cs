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
        public AlertModal<string> AddComment(int request_id, int account_id, string content, int role_id, bool is_manager)
        {
            NotificationRepo notificationRepo = new NotificationRepo();

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
                    string notification_id;
                    if (is_manager)
                        notification_id = notificationRepo.AddByAccountID(request.account_id, 1, "/ConferenceSponsor/Detail?id=" + request.request_id).ToString();
                    else
                        notification_id = JsonConvert.SerializeObject(notificationRepo.AddByRightID(new List<int> { 22, 34 }, 1, "/ConferenceSponsor/Detail?id=" + request.request_id));
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
    }
}
