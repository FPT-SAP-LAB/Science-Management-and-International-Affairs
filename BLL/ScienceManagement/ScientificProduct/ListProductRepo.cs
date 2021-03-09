using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.ScientificProduct
{
    public class ListProductRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListProduct_JournalPaper> getList()
        {
            List<ListProduct_JournalPaper> list = new List<ListProduct_JournalPaper>();
            list = db.Database.SqlQuery<ListProduct_JournalPaper>(@"select p.name,b.author, p.journal_name, case when a.quality is null then p.[index] else a.quality end as 'quality', p.vol, p.page, p.link_doi, p.link_scholar
                from [SM_ScientificProduct].Paper p
	                join (select p.paper_id, STRING_AGG(c.name, ',') AS 'quality'
			                from [SM_ScientificProduct].Paper p left join [SM_ScientificProduct].PaperWithCriteria pc on p.paper_id = pc.paper_id
				                left join [SM_ScientificProduct].PaperCriteria c on pc.criteria_id = c.criteria_id
			                group by p.paper_id) as a on p.paper_id = a.paper_id
	                join(select p.paper_id, STRING_AGG(po.name, ',') AS 'author'
			                from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
				                join [General].People po on ap.people_id = po.people_id
			                group by p.paper_id) as b on p.paper_id = b.paper_id
                where p.paper_type_id = 1").ToList();
            return list;
        }
    }
}
