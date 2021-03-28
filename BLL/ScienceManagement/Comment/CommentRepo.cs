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
        public AlertModal<string> AddComment(int request_id, int account_id, string content, int role_id)
        {
            List<int> manager_account_id = new List<int> { 2, 3 };
            if (String.IsNullOrWhiteSpace(content))
                return new AlertModal<string>(false, "Nội dung không được bỏ trống");
            BaseRequest request = db.BaseRequests.Find(request_id);
            if (request == null)
                return new AlertModal<string>(false, "Đề nghị không tồn tại");
            if (request.account_id != account_id && !(manager_account_id.Contains(role_id)))
                return new AlertModal<string>(false, "Bạn không có quyền bình luận vào đề nghị này");
            if (request.finished_date != null)
                return new AlertModal<string>(false, "Đề nghị đã kết thúc");
            request.CommentBases.Add(new CommentBase()
            {
                account_id = account_id,
                content = content.Trim(),
                date = DateTime.Now
            });
            db.SaveChanges();
            return new AlertModal<string>(true);
        }
    }
}
