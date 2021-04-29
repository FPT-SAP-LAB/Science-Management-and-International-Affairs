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

        public AlertModal<string> EditRequestCitation(List<ENTITIES.Citation> citation, AddAuthor addAuthor, int account_id, int request_id)
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

                    Author author = EditAuthor(addAuthor);

                    db.Citations.RemoveRange(br.RequestCitation.Citations);
                    br.RequestCitation.Citations = citation;

                    RequestCitation rc = br.RequestCitation;
                    rc.people_id = author.people_id;
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

        private Author EditAuthor(AddAuthor addAuthor)
        {
            if (addAuthor == null)
                return null;
            SupportClass.TrimProperties(addAuthor);
            if (addAuthor.name == null || addAuthor.email == null)
                return null;

            try
            {
                Author author = db.Authors.Where(x => x.people_id == addAuthor.people_id).FirstOrDefault();
                author.name = addAuthor.name;
                author.email = addAuthor.email;
                if (addAuthor.office_id == 0 || addAuthor.office_id == null)
                {
                    author.office_id = null;
                }
                else
                {
                    author.office_id = addAuthor.office_id;
                    author.bank_number = addAuthor.bank_number;
                    author.bank_branch = addAuthor.bank_branch;
                    author.tax_code = addAuthor.tax_code;
                    author.identification_number = addAuthor.identification_number;
                    author.mssv_msnv = addAuthor.mssv_msnv;
                    author.is_reseacher = addAuthor.is_reseacher;
                    author.title_id = addAuthor.title_id;
                    author.contract_id = 1;
                    author.identification_file_link = addAuthor.identification_file_link;
                }
                db.SaveChanges();
                return author;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
