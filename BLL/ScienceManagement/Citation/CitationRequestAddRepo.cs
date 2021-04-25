using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BLL.ScienceManagement.Citation
{
    public class CitationRequestAddRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();

        public AlertModal<int> AddRequestCitation(List<ENTITIES.Citation> citation, AddAuthor addAuthor, int account_id)
        {
            Support.SupportClass.TrimProperties(addAuthor);
            if (addAuthor.name == null)
                return new AlertModal<int>(false, "Tên người tạo đề nghị không được bỏ trống");
            if (addAuthor.email == null)
                return new AlertModal<int>(false, "Email người tạo đề nghị không được bỏ trống");

            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    Author author = AddAuthor(addAuthor);

                    db.Citations.AddRange(citation);
                    db.SaveChanges();

                    BaseRequest b = new BaseRequest
                    {
                        account_id = account_id,
                        created_date = DateTime.Today,
                    };
                    db.BaseRequests.Add(b);
                    db.SaveChanges();

                    RequestCitation rc = new RequestCitation
                    {
                        request_id = b.request_id,
                        citation_status_id = 3,
                        people_id = author.people_id,
                        Citations = citation
                    };
                    db.RequestCitations.Add(rc);
                    db.SaveChanges();

                    dbc.Commit();

                    return new AlertModal<int>(b.request_id, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return new AlertModal<int>(false);
                }
            }
        }

        private Author AddAuthor(AddAuthor item)
        {
            Author author = new Author
            {
                name = item.name,
                email = item.email
            };

            if (item.office_id != 0)
            {
                author.office_id = item.office_id;
                author.bank_number = item.bank_number;
                author.bank_branch = item.bank_branch;
                author.tax_code = item.tax_code;
                author.identification_number = item.identification_number;
                author.mssv_msnv = item.mssv_msnv;
                author.is_reseacher = item.is_reseacher;
                author.title_id = item.title_id;
                author.contract_id = item.contract_id;
                author.identification_file_link = item.identification_file_link;
            }
            db.Authors.Add(author);
            db.SaveChanges();
            return author;
        }
    }
}
