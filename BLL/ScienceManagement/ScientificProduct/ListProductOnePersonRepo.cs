using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.ScientificProduct
{
    public class ListProductOnePersonRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListProduct_OnePerson> getList(DataSearch item, string id)
        {
            List<ListProduct_OnePerson> list = new List<ListProduct_OnePerson>();
            string sql = @"select p.name, br.created_date,rp.status_id, p.paper_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
	                            join [SM_Request].BaseRequest br on rp.request_id = br.request_id
                            where br.account_id = @id";
            if (item.nameS != null && item.nameS != " ") sql += " and p.name like @name"; else item.nameS = " ";
            sql += " order by status_id desc";
            list = db.Database.SqlQuery<ListProduct_OnePerson>(sql
                , new SqlParameter("id", id)
                , new SqlParameter("name", "%" + item.nameS + "%")).ToList();
            return list;
        }

        public List<ListProduct_OnePerson> getListInven(DataSearch item, string id)
        {
            List<ListProduct_OnePerson> list = new List<ListProduct_OnePerson>();
            string sql = @"select i.name, br.created_date, ri.status_id, i.invention_id as 'paper_id'
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].RequestInvention ri on i.invention_id = ri.invention_id
	                            join [SM_Request].BaseRequest br on ri.request_id = br.request_id
                            where br.account_id = @id";
            if (item.nameS != null && item.nameS != " ") sql += " and i.name like @name"; else item.nameS = " ";
            sql += " order by status_id desc";
            list = db.Database.SqlQuery<ListProduct_OnePerson>(sql
                , new SqlParameter("id", id)
                , new SqlParameter("name", "%" + item.nameS + "%")).ToList();
            return list;
        }
    }
}
