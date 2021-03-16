using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Comment
{
    public class CommentRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<DetailComment> getComment(int id)
        {
            List<DetailComment> list = new List<DetailComment>();
            string sql = @"select cb.*, po.email
                            from [Comment].CommentBase cb join [Comment].CommentRequest cr on cb.comment_id = cr.comment_id
	                            join [SM_Request].BaseRequest br on cr.request_id = br.request_id
	                            join [General].People po on cb.people_id = po.people_id
                            where cr.request_id = @id";
            list = db.Database.SqlQuery<DetailComment>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }
    }
}
