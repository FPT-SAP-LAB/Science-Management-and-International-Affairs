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
        public List<ListProduct_OnePerson> getList(DataSearch item)
        {
            List<ListProduct_OnePerson> list = new List<ListProduct_OnePerson>();
            string sql = @"select p.name, p.date_request, psl.name, p.status_id, p.paper_id
                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
	                            join [SM_ScientificProduct].PaperStatus ps on p.status_id = ps.status_id
	                            join [Localization].PaperStatusLanguage psl on ps.status_id = psl.status_id
                            where ap.people_id = @id and ap.author_request = 1";
            if (item.nameS != null && item.nameS != " ") sql += " and p.name like @name"; else item.nameS = " ";
            sql += " union ";
            sql += @"select i.name, i.date_request, psl.name, i.status, i.invention_id
                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].AuthorInvention ai on i.invention_id = ai.invention_id
	                            join [SM_ScientificProduct].PaperStatus ps on i.status = ps.status_id
	                            join [Localization].PaperStatusLanguage psl on ps.status_id = psl.status_id
                            where ai.people_id = 1 and ai.author_request = 1";
            if (item.nameS != null && item.nameS != " ") sql += " and i.name like @name"; else item.nameS = " ";
            sql += " order by status_id desc";
            list = db.Database.SqlQuery<ListProduct_OnePerson>(sql
                , new SqlParameter("id", 1)
                , new SqlParameter("name", "%"+item.nameS+"%")).ToList();
            return list;
        }
    }
}
