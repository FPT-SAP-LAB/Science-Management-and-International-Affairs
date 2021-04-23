using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.ScienceManagement.ScientificProduct
{
    public class ListProductRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListProduct_JournalPaper> getList(DataSearch item)
        {
            List<ListProduct_JournalPaper> list = new List<ListProduct_JournalPaper>();
            string sql = @"select p.name,b.author, p.journal_name, case when a.quality is null then p.[index] else a.quality end as 'quality', p.vol, p.page, p.link_doi, p.link_scholar
                            from [SM_ScientificProduct].Paper p
	                            join (select p.paper_id, STRING_AGG(c.name, ',') AS 'quality'
			                            from [SM_ScientificProduct].Paper p left join [SM_ScientificProduct].PaperWithCriteria pc on p.paper_id = pc.paper_id
				                            left join [SM_ScientificProduct].PaperCriteria c on pc.criteria_id = c.criteria_id
			                            group by p.paper_id) as a on p.paper_id = a.paper_id
	                            join(select p.paper_id, STRING_AGG(po.name, ',') AS 'author'
			                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
				                            join [SM_ScientificProduct].Author po on ap.people_id = po.people_id
			                            group by p.paper_id) as b on p.paper_id = b.paper_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
                            where p.paper_type_id = 1 and rp.status_id in (2, 6, 7)";
            if (item.nameS != null && item.nameS != " ") sql += " and p.name like @name"; else item.nameS = " ";
            if (item.monthS != null && item.monthS != " ") sql += " and month(p.publish_date) = @month"; else item.monthS = " ";
            if (item.yearS != null && item.yearS != " ") sql += " and year(p.publish_date) = @year"; else item.yearS = " ";
            sql += " order by p.publish_date desc";
            list = db.Database.SqlQuery<ListProduct_JournalPaper>(sql
                , new SqlParameter("name", "%" + item.nameS + "%")
                , new SqlParameter("month", item.monthS)
                , new SqlParameter("year", item.yearS)).ToList();
            return list;
        }

        public List<ListProduct_ConferencePaper> getList2(DataSearch item)
        {
            string sql = @"select p.name,b.author, p.journal_name,  case when a.quality is null then p.[index] else a.quality end as 'quality', p.vol, p.page, p.link_doi, p.link_scholar
                            from [SM_ScientificProduct].Paper p
	                            join (select p.paper_id, STRING_AGG(c.name, ',') AS 'quality'
			                            from [SM_ScientificProduct].Paper p left join [SM_ScientificProduct].PaperWithCriteria pc on p.paper_id = pc.paper_id
				                            left join [SM_ScientificProduct].PaperCriteria c on pc.criteria_id = c.criteria_id
			                            group by p.paper_id) as a on p.paper_id = a.paper_id
	                            join(select p.paper_id, STRING_AGG(po.name, ',') AS 'author'
			                            from [SM_ScientificProduct].Paper p join [SM_ScientificProduct].AuthorPaper ap on p.paper_id = ap.paper_id
				                            join [SM_ScientificProduct].Author po on ap.people_id = po.people_id
			                            group by p.paper_id) as b on p.paper_id = b.paper_id
	                            join [SM_ScientificProduct].RequestPaper rp on p.paper_id = rp.paper_id
                            where p.paper_type_id = 2 and rp.status_id in (2, 6, 7)";
            if (item.nameS != null && item.nameS != " ") sql += " and p.name like @name"; else item.nameS = " ";
            if (item.monthS != null && item.monthS != " ") sql += " and month(p.publish_date) = @month"; else item.monthS = " ";
            if (item.yearS != null && item.yearS != " ") sql += " and year(p.publish_date) = @year"; else item.yearS = " ";
            sql += " order by p.publish_date desc";
            List<ListProduct_ConferencePaper> list = new List<ListProduct_ConferencePaper>();
            list = db.Database.SqlQuery<ListProduct_ConferencePaper>(sql
                , new SqlParameter("name", "%" + item.nameS + "%")
                , new SqlParameter("month", item.monthS)
                , new SqlParameter("year", item.yearS)).ToList();
            return list;
        }

        public List<ListProdcut_Inven> getListInven(DataSearch item)
        {
            string sql = @"select i.name, a.author, it.name as 'name_shtt', i.date, i.no
                            from [SM_ScientificProduct].Invention i
	                            join(select	i.invention_id, STRING_AGG(po.name, ',') AS 'author'
			                            from [SM_ScientificProduct].Invention i join [SM_ScientificProduct].AuthorInvention ai on i.invention_id = ai.invention_id
				                             join [SM_ScientificProduct].Author po on ai.people_id = po.people_id
			                            group by i.invention_id) as a on i.invention_id = a.invention_id
	                            join [SM_ScientificProduct].InventionType it on i.type_id = it.invention_type_id
	                            join [SM_ScientificProduct].RequestInvention ri on i.invention_id = ri.invention_id
                            where 1=1 and ri.status_id in (2, 6)";
            if (item.nameS != null && item.nameS != " ") sql += " and i.name like @name"; else item.nameS = " ";
            if (item.monthS != null && item.monthS != " ") sql += " and month(i.date) = @month"; else item.monthS = " ";
            if (item.yearS != null && item.yearS != " ") sql += " and year(i.date) = @year"; else item.yearS = " ";
            sql += " order by i.date desc";
            List<ListProdcut_Inven> list = new List<ListProdcut_Inven>();
            list = db.Database.SqlQuery<ListProdcut_Inven>(sql
                , new SqlParameter("name", "%" + item.nameS + "%")
                , new SqlParameter("month", item.monthS)
                , new SqlParameter("year", item.yearS)).ToList();
            return list;
        }
    }
}
