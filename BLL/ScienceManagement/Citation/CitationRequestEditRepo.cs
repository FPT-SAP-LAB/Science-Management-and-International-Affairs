using BLL.Support;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BLL.ScienceManagement.Citation
{
    public class CitationRequestEditRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public AlertModal<string> EditRequestCitation(List<ENTITIES.Citation> citation, int account_id, int request_id)
        {
            if (citation == null || citation.Count == 0)
                return new AlertModal<string>(false, "Thiếu thông tin trích dẫn");

            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    BaseRequest br = db.BaseRequests.Where(x => x.request_id == request_id).FirstOrDefault();
                    if (br == null || br.RequestCitation == null)
                        return new AlertModal<string>(false, "Đề nghị không tồn tại");

                    if (br.account_id != account_id)
                        return new AlertModal<string>(false, "Đề nghị này không phải của bạn");

                    db.Citations.RemoveRange(br.RequestCitation.Citations);
                    br.RequestCitation.Citations = citation;

                    RequestCitation rc = br.RequestCitation;
                    rc.citation_status_id = 3;

                    db.SaveChanges();
                    trans.Commit();
                    return new AlertModal<string>(true, "Chỉnh sửa thành công");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                    return new AlertModal<string>(false, "Có lỗi xảy ra");
                }
            }
        }
    }
}
