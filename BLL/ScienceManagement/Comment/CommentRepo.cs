using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                        select new DetailComment
                                        {
                                            Content = a.content,
                                            Date = a.date,
                                            Email = b.email
                                        }).ToList();
            return list;
        }
        public AlertModal<string> AddComment(int request_id, int account_id, string content)
        {
            if (String.IsNullOrWhiteSpace(content))
                return new AlertModal<string>(false, "Nội dung không được bỏ trống");
            BaseRequest request = db.BaseRequests.Where(x => x.account_id == account_id && x.request_id == request_id).FirstOrDefault();
            if (request == null)
                return new AlertModal<string>(false, "Đề nghị không tồn tại");
            if (request.finished_date != null)
                return new AlertModal<string>(false, "Đề nghị đã kết thúc");
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                CommentBase Comment = new CommentBase()
                {
                    account_id = account_id,
                    content = content.Trim(),
                    date = DateTime.Now
                };
                request.CommentBases.Add(Comment);
                db.SaveChanges();
                transaction.Commit();
            }
            return new AlertModal<string>(true);
        }
    }
}
