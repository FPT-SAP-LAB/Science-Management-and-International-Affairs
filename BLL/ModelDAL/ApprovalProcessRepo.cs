using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelDAL
{
    public class ApprovalProcessRepo
    {
        public static ApprovalProcess Add(int account_id, DateTime create_date, int? position_id, int request_id, string comment)
        {
            ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
            return Add(db, account_id, create_date, position_id, request_id, comment);
        }
        public static ApprovalProcess Add(ScienceAndInternationalAffairsEntities db, int account_id, DateTime create_date, int? position_id, int request_id, string comment)
        {
            ApprovalProcess process = new ApprovalProcess
            {
                account_id = account_id,
                created_date = create_date,
                position_id = position_id,
                request_id = request_id,
                comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim()
            };
            db.ApprovalProcesses.Add(process);
            db.SaveChanges();
            return process;
        }
    }
}
