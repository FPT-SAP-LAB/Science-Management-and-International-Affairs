using ENTITIES;
using ENTITIES.CustomModels.Datatable;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Request
{
    public class RequestRepo
    {
        public static BaseServerSideData<PendingPaper_Manager> GetListHistory(BaseDatatable baseDatatable, string name_search)
        {
            try
            {
                using (ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities())
                {
                    if (name_search == null) name_search = "";
                    db.Configuration.LazyLoadingEnabled = false;
                    string sql = @"select p.name, a.email, br.created_date, p.paper_id, rp.status_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join [SM_Request].BaseRequest br on rp.request_id = br.request_id
	                            join [General].Account a on br.account_id = a.account_id
                            where rp.status_id = 2 and p.name like @name
                            ORDER BY " + baseDatatable.SortColumnName + " " + baseDatatable.SortDirection +
                            " OFFSET " + baseDatatable.Start + " ROWS FETCH NEXT " + baseDatatable.Length + " ROWS ONLY";
                    List<PendingPaper_Manager> internalUnits = db.Database.SqlQuery<PendingPaper_Manager>(sql, new SqlParameter("name", "%" + name_search + "%")).ToList();
                    string sql_count = @"select count(*)
                                        from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
                                        where rp.status_id = 2 and p.name like @name";
                    int recordsTotal = db.Database.SqlQuery<int>(sql_count, new SqlParameter("name", "%" + name_search + "%")).FirstOrDefault();
                    return new BaseServerSideData<PendingPaper_Manager>(internalUnits, recordsTotal);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}