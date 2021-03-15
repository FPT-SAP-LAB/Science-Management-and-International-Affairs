using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Citation
{
    public class CitationRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListOnePerson_Citation> GetList(string id)
        {
            string sql = @"select STRING_AGG(c.source, ',') AS 'source',SUM(c.count) as 'count', rc.date_request, rc.status_id, rc.request_id
                            from [SM_Citation].Citation c join [SM_Citation].RequestHasCitation rhc on c.citation_id = rhc.citation_id
	                            join [SM_Citation].RequestCitation rc on rhc.request_id = rc.request_id
                            where rc.people_id = @id
                            group by rc.date_request, rc.status_id,  rc.request_id";
            List<ListOnePerson_Citation> list = new List<ListOnePerson_Citation>();
            list = db.Database.SqlQuery<ListOnePerson_Citation>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }
    }
}
